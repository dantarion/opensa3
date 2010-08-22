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
using OpenSALib3.Moveset;

namespace OpenSALib3.DatHandler
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DatFileHeader
    {
        public bint FileSize;
        public bint DataChunkSize;
        public bint OffsetCount;
        public bint SectionCount;
        public bint ReferenceCount;
        public bint Unknown1;
        public bint Unknown2;
        public bint Unknown3;
    }

    public unsafe class DatFile : DatElement
    {
        #region Static Methods
        public static bool IsDatFile(ResourceNode node)
        {
            return node.WorkingUncompressed.Length == *(bint*)node.WorkingUncompressed.Address;

        }
        public static DatFile FromFile(string filename)
        {
            var node = NodeFactory.FromFile(null, filename);
            return FromNode(node);
        }

        public static DatFile FromNode(ResourceNode node)
        {
            var file =
                new DatFile(node)
                {
                    Filename = node.RootNode.FilePath
                };
            return file;
        }
        #endregion

        public MDL0Node Model { get; private set; }
        public Dictionary<string, CHR0Node> Animations { get; private set; }
        private Dictionary<int, string> _boneNames;
        public List<int> OffsetList = new List<int>();
        public void LoadModel(string filename)
        {
            try
            {
                _boneNames = new Dictionary<int, string>();
                var node = NodeFactory.FromFile(null, filename);
                Model = (MDL0Node)node.FindChildrenByType("", ResourceType.MDL0).First();
                Debug.Assert(Model != null);
                foreach (MDL0BoneNode innernode in Model.FindChildrenByType("", ResourceType.MDL0Bone))
                {
                    _boneNames[innernode.BoneIndex] = innernode.Name;
                }

            }
            catch (Exception)
            {
                Model = null;
                _boneNames = null;
            }
        }
        public void LoadAnimations(string filename)
        {
            try
            {
                Animations = new Dictionary<string, CHR0Node>();
                var node = NodeFactory.FromFile(null, filename);
                foreach (CHR0Node innernode in node.FindChildrenByType("", ResourceType.CHR0))
                {
                    Animations[innernode.Name] = innernode;
                }
                node.Merge();
            }
            catch (Exception)
            {
                Animations = null;
            }
        }
        public List<BoneRef> GetDefaultHiddenBones()
        {


            var list = new List<BoneRef>();
            var modelvis = this["Sections"]["data"]["Model Display"]["Hidden"];
            foreach (DatElement de in modelvis)
            {

                var first = true;
                foreach (DatElement dee in de)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    

                    foreach (BoneRef val in dee)
                    {
                        list.Add(val);
                    }
                }
            }

            return list;
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
        private readonly UnknownElement _sections;
        public UnknownElement Sections { get { return _sections; } }

        private readonly UnknownElement _references;
        [Browsable(false)]
        public UnknownElement References { get { return _references; } }
        [Category("File")]
        [ReadOnly(true)]
        [Browsable(true)]
        public String Filename { get; set; }

        [Browsable(false)]
        public override VoidPtr Address
        {
            get { return Node.WorkingSource.Address + Marshal.SizeOf(_header); }
        }

        private readonly Dictionary<int,DatSection> _external = new Dictionary<int,DatSection>();
        public string IsExternal(int offset)
        {
            bool result = _external.ContainsKey(offset);
            if (result)
            {
                _external[offset].TreeColor = System.Windows.Media.Brushes.Green;
                return _external[offset].Name;
            }
            return null;
        }
        protected DatFile(ResourceNode node)
            : base(null, 0)
        {
            TreeColor = null;
            _source = node.OriginalSource;
            Node = node;
            _header = *(DatFileHeader*)node.WorkingUncompressed.Address;
            if (_header.FileSize != node.WorkingUncompressed.Length)
                throw new Exception("This is not a valid moveset file");
            Name = node.TreePathAbsolute;
            Length = (_header.FileSize - Marshal.SizeOf(_header));
            Color = Color.Transparent;
            //Start Parse
            var section = _header.DataChunkSize + _header.OffsetCount * 4;
            var stringBase = (section + (_header.SectionCount + _header.ReferenceCount) * 8);
            var section2 = section + _header.SectionCount * 8;
            //Parse References FIRST
            _references = new UnknownElement(this, -1, "References", 0);
            _references.TreeColor = null;
            for (var i = 0; i < _header.ReferenceCount; i++)
            {
                var s = DatSection.Factory(References, section2, stringBase);
                
                References[s.Name] = s;
                var tmp = s.DataOffset;
                do
                {
                    _external[tmp] = s;
                    tmp = ReadInt(tmp);
                }
                while (tmp != -1);
                section2 += 8;
            }
            //Parse sections
            _sections = new UnknownElement(this, -1, "Sections", 0);
            _sections.TreeColor = null;
            for (var i = 0; i < _header.SectionCount; i++)
            {
                var s = DatSection.Factory(Sections, section, stringBase);
                s.TreeColor = null;
                Sections[s.Name] = s;
                section += 8;
            }
            var offsetchunk = new UnknownElement(this, _header.DataChunkSize, "OffsetChunk", _header.OffsetCount * 4);
            for (int i = 0; i < _header.OffsetCount; i++)
                OffsetList.Add(ReadInt(_header.DataChunkSize + i * 4));
            offsetchunk.TreeColor = null;
            ComputeDataLengths(Sections);
            foreach (DatSection s in Sections)
                s.Parse();
            var stringChunk = new UnknownElement(this, stringBase, "StringChunk", _header.FileSize - stringBase);
            stringChunk.TreeColor = null;

            //Setup Tree Structure
            this["Sections"] = Sections;
            this["References"] = References;
            this["OffsetChunk"] = offsetchunk;
            this["StringChunk"] = stringChunk;

        }

        private void ComputeDataLengths(DatElement sections)
        {

            var sorted = sections.OfType<DatSection>().OrderBy(x => (x).DataOffset).ToList();
            for (var i = 0; i < sorted.Count; i++)
            {
                if (i < sorted.Count - 1)
                    sorted[i].DataLength = sorted[i + 1].DataOffset - sorted[i].DataOffset;
                else sorted[i].DataLength = _header.DataChunkSize - sorted[i].DataOffset;
                sorted[i].DataLength += sorted[i].DataLength % 4;
            }
        }
        public string Report(bool onlyholes = false)
        {
            var usagedata = new List<UsageData>();
            foreach (var child in Dictionary.Values)
                CollectUsage(child, ref usagedata);
            usagedata.AddRange(_susagedata);
            usagedata.Sort();
            var usage = usagedata.Sum(ug => ug.Length);
            var sb = new StringBuilder(0x150000);
            sb.AppendFormat("{0}\n", Name);
            sb.AppendFormat("Total Size:{0}\n", Length);
            sb.AppendFormat("Bytes Parsed:{0}\n", usage);
            sb.AppendFormat("% Complete:{0}\n", (float)usage / (Length + 0x20));

            var lastdata = 0;
            foreach (var ug in usagedata)
            {
                if (ug.Offset == -1)
                    continue;
                if (ug.Offset < lastdata)
                    sb.AppendFormat("OVERLAP\n");
                else if (ug.Offset > lastdata)
                    sb.AppendFormat("{1:X08} HOLE - {0:X08}\n", ug.Offset - lastdata, lastdata);
                if (!onlyholes)
                    sb.AppendFormat("@0x{0:X08} - len {1:X08} - {2,25}\n", ug.Offset, ug.Length, ug.ID);
                lastdata = ug.Offset + ug.Length;

            }
            return sb.ToString();
        }
        public struct UsageData : IComparable<UsageData>
        {
            public int Offset;
            public int Length;
            public string ID;

            public int CompareTo(UsageData other)
            {
                return Offset - other.Offset;
            }
        }
        public void CollectUsage(DatElement element, ref List<UsageData> list)
        {
            foreach (DatElement child in element)
                CollectUsage(child, ref list);//Get the child usage
            var de = (DatElement)element;
            if (de == null) return;
            if (list.Exists(x => x.Offset == de.FileOffset))
                return;
            UsageData ud;

            ud.Offset = de.FileOffset;
            ud.Length = de.Length;
            ud.ID = de.Path + " " + de.GetType().Name;
            
                list.Add(ud);
        }

        readonly List<UsageData> _susagedata = new List<UsageData>();
        public string ReadString(int offset)
        {
            if (offset > Length)
                throw new Exception("Invalid Offset");
            var s = new String((sbyte*)(Address + offset));
            UsageData ud;
            ud.Offset = offset;
            ud.Length = s.Length + 1;
            ud.Length += ud.Length % 8;
            ud.ID = "String";
            // if (susagedata.Count(x => x.offset == offset) == 0)
            //     susagedata.Add(ud);
            return s;
        }
    }
}