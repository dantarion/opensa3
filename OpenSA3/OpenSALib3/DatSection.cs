using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenSALib3.Moveset;
namespace OpenSALib3
{
    unsafe struct DatSectionHeader
    {
        public buint DataOffset;
        public  buint StringOffset;
    }
    public unsafe class DatSection : DatElement
    {
        public static DatSection Factory(VoidPtr ptr, VoidPtr stringPtr,DatElement parent)
        {
            var ds = new DatSection(ptr,stringPtr, parent);
            if (ds.Name == "data")
                return new MovesetSection(ptr,stringPtr, parent);
            return ds;
        }
        private DatSectionHeader header;

        public uint DataOffset { get { return header.DataOffset; } set { header.DataOffset = value; } }
        [Browsable(false)]
        public uint StringOffset { get { return header.StringOffset; } set { header.StringOffset = value; } }

        public String Name { get; set; }
        public uint Length { get; set; }
        public override string ToString()
        {
            return Name;
        }
        protected DatSection(VoidPtr ptr, VoidPtr stringPtr, DatElement parent)
            : base(parent)
        {
            header = *(DatSectionHeader*)ptr;
            Name = new String((sbyte*)stringPtr + StringOffset);
            FileSpan.StartOffset = ptr - RootFile.Address;
            FileSpan.Length = 8;
        }
    }
}
