using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using OpenSALib3.Utility;

namespace OpenSALib3.DatHandler
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DatFileHeader
    {
        public buint FileSize;
        public buint DataChunkSize;
        public buint OffsetCount;
        public buint SectionCount;
        public buint ReferenceCount;
        public buint Unknown1;
        public buint Unknown2;
        public buint Unknown3;
    }

    public unsafe class DatFile : DatElement
    {
        #region Static Methods
        public static DatFile FromFile(string filename)
        {
            var node = NodeFactory.FromFile(null, filename);
            return FromNode(node);
        }

        public static DatFile FromNode(ResourceNode node)
        {
            var file =
                new DatFile(node.Children[0])
                {
                    Filename = node.OriginalSource.Map.FilePath
                };
            return file;
        }
        #endregion

        public MDL0Node Model { get; private set; }
        private Dictionary<int, string> _boneNames;
        public void LoadModel(string filename)
        {
            try
            {
                _boneNames = new Dictionary<int, string>();
                var node = NodeFactory.FromFile(null, filename);
                Model = node.Children[0].Children[0].Children[0] as MDL0Node;
                Debug.Assert(Model != null);
                foreach (MDL0BoneNode innernode in Model.FindChildrenByType("", ResourceType.MDL0Bone))
                {
                    //Debug.Assert(_boneNames.ContainsKey(innernode.BoneIndex));
                    _boneNames[innernode.BoneIndex] = innernode.Name;
                }
            }
            catch (Exception)
            {
                Model = null;
                _boneNames = null;
            }
        }
        public string GetBoneName(int boneIndex)
        {
            if (_boneNames == null)
                return "No Model Ref Loaded";
            return !_boneNames.ContainsKey(boneIndex) ? "????" : _boneNames[boneIndex];
        }

        private DatFileHeader _header;
        private VoidPtr _dataChunk;
        private DataSource _source;
        public ResourceNode Node;

        [Browsable(false)]
        public List<DatSection> Sections { get; private set; }

        [Browsable(false)]
        public List<DatSection> References { get; private set; }

        [Category("File")]
        [ReadOnly(true)]
        [Browsable(true)]
        public bool Changed { get; set; }

        [Category("File")]
        [ReadOnly(true)]
        [Browsable(true)]
        public String Filename { get; set; }

        [Browsable(false)]
        public override VoidPtr Address
        {
            get { return Node.WorkingSource.Address + Marshal.SizeOf(_header); }
        }

        [Browsable(false)]
        public override DatFile RootFile
        {
            get { return this; }
        }

        protected DatFile(ResourceNode node)
            : base(null, 0)
        {
            _source = node.OriginalSource;
            Node = node;
            _header = *(DatFileHeader*)node.WorkingUncompressed.Address;
            if (_header.FileSize != node.WorkingUncompressed.Length)
                throw new Exception("This is not a valid moveset file");
            Sections = new List<DatSection>();
            References = new List<DatSection>();
            Name = node.RootNode.Name;
            Length = (uint)(_header.FileSize - Marshal.SizeOf(_header));
            Changed = false;
            Color = Color.Transparent;
            //Start Parse
            var section = _header.DataChunkSize + _header.OffsetCount * 4;
            var stringBase =(int) (section + (_header.SectionCount + _header.ReferenceCount) * 8);
            //Parse sections
            for (var i = 0; i < _header.SectionCount; i++)
            {
                var s = DatSection.Factory(this, section, stringBase);
                Sections.Add(s);
                section += 8;
            }
            ComputeDataLengths(Sections);
            //Parse References
            for (var i = 0; i < _header.ReferenceCount; i++)
            {
                var s = DatSection.Factory(this, section, stringBase);
                References.Add(s);
                section += 8;
            }
            //Setup Tree Structure
            Children.Add(new NamedList(Sections, "Sections"));
            Children.Add(new NamedList(References, "References"));

        }

        private void ComputeDataLengths(IEnumerable<DatSection> sections)
        {
            var sorted = sections.OrderBy(x => x.DataOffset).ToList();
            for (var i = 0; i < sorted.Count; i++)
                if (i < sorted.Count - 1)
                    sorted[i].DataLength = sorted[i + 1].DataOffset - sorted[i].DataOffset;
                else sorted[i].DataLength = _header.DataChunkSize - sorted[i].DataOffset;
        }
        public string Report()
        {
            var usagedata = new List<UsageData>();
            CollectUsage(this,usagedata);
            usagedata.AddRange(_sectionUsageData);
            var sorted = usagedata.OrderBy(x => x.Offset);
            var usage = sorted.Sum(ug => ug.Length);
            var sb = new StringBuilder();
            sb.AppendFormat("Total Size:{0}\n",Length);
            sb.AppendFormat("Bytes Parsed:{0}\n", usage);
            sb.AppendFormat("% Complete:{0}\n", (float)usage / Length);

            var lastdata = 0;
            foreach (var ug in sorted)
            {
                
                sb.AppendFormat("{0:X08} - {1:8} - {2:25}", ug.Offset, ug.Length,ug.ID);
                if (ug.Offset == lastdata)
                    sb.AppendFormat(" MATCH\n");
                else if (ug.Offset < lastdata)
                    sb.AppendFormat("OVERLAP\n");
                else
                    sb.AppendFormat(" HOLE - {0:X08}\n", ug.Offset - lastdata);
                lastdata = ug.Offset + ug.Length;

            }
            return sb.ToString();
        }
        public struct UsageData
        {
            public int Offset;
            public int Length;
            public string ID;
        }
        public void CollectUsage(IEnumerable element,List<UsageData> list)
        {
            foreach (IEnumerable child in element)
            {
                CollectUsage(child, list);//Get the child usage
                var de = child as DatElement;//If the child is actually a DatElement        
                if(de == null)
                    continue;
                UsageData ud;
                ud.Offset = (int)de.FileOffset;
                ud.Length = (int)de.Length;
                ud.ID = de.Path + " " + de.GetType().Name;
                list.Add(ud);
            }
        }

        private readonly List<UsageData> _sectionUsageData = new List<UsageData>();
        public string ReadString(int offset)
        {
            var s = new String((sbyte*)(Address + offset));
            UsageData ud;
            ud.Offset = offset;
            ud.Length = s.Length + 1;
            ud.ID = "String";
            if(_sectionUsageData.Count(x=>x.Offset == offset) == 0)
                _sectionUsageData.Add(ud);
            return s;
        }
    }
}