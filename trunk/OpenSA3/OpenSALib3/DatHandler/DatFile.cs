using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

        private MDL0Node _model = null;
        public MDL0Node Model
        {
           get{ return _model;}
        }
        private Dictionary<int, string> _boneNames;
        public void LoadModel(string filename)
        {
            try
            {
                _boneNames = new Dictionary<int, string>();
                var node = NodeFactory.FromFile(null, filename);
                _model = node.Children[0].Children[0].Children[0] as MDL0Node;
                foreach (MDL0BoneNode innernode in _model.FindChildrenByType("", ResourceType.MDL0Bone))
                {
                    //Debug.Assert(_boneNames.ContainsKey(innernode.BoneIndex));
                    _boneNames[innernode.BoneIndex] = innernode.Name;
                }
            }
            catch (Exception)
            {
                _model = null;
                _boneNames = null;
            }
        }
        public string GetBoneName(int boneIndex)
        {
            if (_boneNames == null)
                return "No Model Ref Loaded";
            if (!_boneNames.ContainsKey(boneIndex))
                return "????";
            return _boneNames[boneIndex];
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
            Color = System.Drawing.Color.Transparent;
            //Start Parse
            var section = Address + _header.DataChunkSize + _header.OffsetCount * 4;
            var stringptr = section + (_header.SectionCount + _header.ReferenceCount) * 8;
            //Parse sections
            for (var i = 0; i < _header.SectionCount; i++)
            {
                var s = DatSection.Factory(section, stringptr, this);
                Sections.Add(s);
                section += 8;
            }
            ComputeLengths(Sections);
            //Parse References
            for (var i = 0; i < _header.ReferenceCount; i++)
            {
                var s = DatSection.Factory(section, stringptr, this);
                References.Add(s);
                section += 8;
            }
        }

        private void ComputeLengths(IEnumerable<DatSection> sections)
        {
            var sorted = sections.OrderBy(x => x.FileOffset).ToList();
            for (var i = 0; i < sorted.Count; i++)
                if (i < sorted.Count - 1)
                    sorted[i].Length = sorted[i + 1].FileOffset - sorted[i].FileOffset;
                else sorted[i].Length = _header.DataChunkSize - sorted[i].FileOffset;
        }

        public override IEnumerator GetEnumerator()
        {
            return new DatFileEnumerator(this);
        }
    }

    internal class DatFileEnumerator : IEnumerator
    {
        private readonly DatFile _file;
        private int _i = -1;

        public DatFileEnumerator(DatFile f)
        {
            _file = f;
        }

        public object Current
        {
            get
            {
                switch (_i)
                {
                    case 0:
                        return _file.Sections[0];
                    case 1:
                        return new NamedList<DatSection>(_file.Sections, "Sections");
                    case 2:
                        return new NamedList<DatSection>(_file.References, "References");
                }
                return null;
            }
        }

        public bool MoveNext()
        {
            _i++;
            return _i <= 2;
        }

        public void Reset()
        {
            _i = -1;
        }
    }
}