using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using OpenSALib3.Utility;
namespace OpenSALib3.DatHandler
{
    struct DatFileHeader
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
    public static class SizeOf
    {
        public const uint
            Header = 0x20;
    }
    public unsafe class DatFile : DatElement
    {
        public static DatFile FromFile(string filename)
        {
            var node = NodeFactory.FromFile(null, filename);
            return FromNode(node);
        }
        public static DatFile FromNode(ResourceNode node)
        {
            var file =  new DatFile(node.Children[0]);
            file.Filename = node.OriginalSource.Map.FilePath;
            return file;
        }

        private DatFileHeader _header;
        private VoidPtr _dataChunk;
        private DataSource source;
        public ResourceNode _node;
        public List<DatSection> Sections { get; private set; }
        public List<DatSection> References { get; private set; }
        public bool Changed { get; set; }
        public String Filename { get; set; }
        [Browsable(false)]
        public VoidPtr Address { get { return _node.WorkingSource.Address+SizeOf.Header; } }

        public override DatFile RootFile
        {
            get { return this; }
        }
        protected DatFile(ResourceNode node)
            : base(null)
        {
            source = node.OriginalSource;
            _node = node;
            _header = *(DatFileHeader*)node.WorkingUncompressed.Address;
            if (_header.FileSize != node.WorkingUncompressed.Length)
                throw new Exception("This is not a valid moveset file");

            Sections = new List<DatSection>();
            References = new List<DatSection>();
            Name = node.RootNode.Name;
            FileOffset = 0;
            Length = _header.FileSize - SizeOf.Header;
            Changed = false;
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

    class DatFileEnumerator : IEnumerator
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
                        return new NamedList<DatSection>(_file.Sections, "Sections");
                    case 1:
                        return new NamedList<DatSection>(_file.References, "References");
                }
                return null;
            }
        }

        public bool MoveNext()
        {
            _i++;
            return _i <= 1;
        }

        public void Reset()
        {
            _i = -1;
        }
    }
}
