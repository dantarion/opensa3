using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
namespace OpenSALib3
{
    unsafe struct DatFileHeader
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
        public static DatFile fromFile(string filename)
        {
            var node = NodeFactory.FromFile(null, filename);
            return new DatFile(node.Children[0]);
        }

        private DatFileHeader header;
        private VoidPtr dataChunk;
        private ResourceNode node;

        private List<DatSection> sections = new List<DatSection>();
        public List<DatSection> Sections { get { return sections; } }
        private List<DatSection> references = new List<DatSection>();
        public List<DatSection> References { get { return references; } }

        public uint FileSize { get { return header.FileSize - 0x20; } }
        public String Filename { get; set; }
        [Browsable(false)]
        public VoidPtr Address { get; internal set; }
        
        public override string ToString()
        {
            return Filename;
        }
        protected DatFile(ResourceNode node) : base(null)
        {
            this.node = node;
            Filename = node.RootNode.Name;
            header = *(DatFileHeader*)node.WorkingUncompressed.Address;
            if (header.FileSize != node.WorkingUncompressed.Length)
                throw new Exception("This is not a valid moveset file");

            Address = node.WorkingUncompressed.Address + 0x20;
            var section = Address + header.DataChunkSize + header.OffsetCount * 4;
            var stringptr = section + (header.SectionCount + header.ReferenceCount) * 8;
            //Parse sections
            for (int i = 0; i < header.SectionCount; i++)
            {
                var s = DatSection.Factory(section,stringptr,this);
                Sections.Add(s);
                section += 8;
            }
            ComputeLengths(Sections);
            //Parse References
            for (int i = 0; i < header.ReferenceCount; i++)
            {
                var s = DatSection.Factory(section,stringptr, this);
                References.Add(s);
                section += 8;
            }
        }
        private void ComputeLengths(List<DatSection> sections)
        {
            var sorted = sections.OrderBy(x => x.DataOffset).ToList();
            for (int i = 0; i < sorted.Count; i++)
                if (i < sorted.Count - 1)
                    sorted[i].Length = sorted[i + 1].DataOffset - sorted[i].DataOffset;
                else sorted[i].Length = header.DataChunkSize - sorted[i].DataOffset;
        }
    }
}
