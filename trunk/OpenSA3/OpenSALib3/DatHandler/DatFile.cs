using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
namespace OpenSALib3.DatHandler {
    struct DatFileHeader {
        public buint FileSize;
        public buint DataChunkSize;
        public buint OffsetCount;
        public buint SectionCount;
        public buint ReferenceCount;
        public buint Unknown1;
        public buint Unknown2;
        public buint Unknown3;
    }
    public static class SizeOf {
        public const uint
            Header = 0x20;
    }
    public unsafe class DatFile : DatElement {
        public static DatFile FromFile(string filename) {
            var node = NodeFactory.FromFile(null, filename);
            return new DatFile(node.Children[0]);
        }

        private DatFileHeader _header;
        private VoidPtr _dataChunk;
        private ResourceNode _node;
        public List<DatSection> Sections { get; private set; }
        public List<DatSection> References { get; private set; }
        public uint FileSize { get { return _header.FileSize - SizeOf.Header; } }
        public String Filename { get; set; }
        [Browsable(false)]
        public VoidPtr Address { get; internal set; }

        public override string ToString() {
            return Filename;
        }
        protected DatFile(ResourceNode node) : base(null) {
            Sections = new List<DatSection>();
            References = new List<DatSection>();
            _node = node;
            Filename = node.RootNode.Name;
            _header = *(DatFileHeader*)node.WorkingUncompressed.Address;
            if (_header.FileSize != node.WorkingUncompressed.Length)
                throw new Exception("This is not a valid moveset file");

            Address = node.WorkingUncompressed.Address + 0x20;
            var section = Address + _header.DataChunkSize + _header.OffsetCount * 4;
            var stringptr = section + (_header.SectionCount + _header.ReferenceCount) * 8;
            //Parse sections
            for (var i = 0; i < _header.SectionCount; i++) {
                var s = DatSection.Factory(section, stringptr, this);
                Sections.Add(s);
                section += 8;
            }
            ComputeLengths(Sections);
            //Parse References
            for (var i = 0; i < _header.ReferenceCount; i++) {
                var s = DatSection.Factory(section, stringptr, this);
                References.Add(s);
                section += 8;
            }
        }
        private void ComputeLengths(IEnumerable<DatSection> sections) {
            var sorted = sections.OrderBy(x => x.DataOffset).ToList();
            for (var i = 0; i < sorted.Count; i++)
                if (i < sorted.Count - 1)
                    sorted[i].Length = sorted[i + 1].DataOffset - sorted[i].DataOffset;
                else sorted[i].Length = _header.DataChunkSize - sorted[i].DataOffset;
        }
    }
}
