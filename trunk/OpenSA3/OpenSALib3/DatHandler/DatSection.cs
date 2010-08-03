using System;
using System.ComponentModel;
using OpenSALib3.Moveset;

namespace OpenSALib3.DatHandler {
    struct DatSectionHeader {
        public buint DataOffset;
        public buint StringOffset;
    }
    public unsafe class DatSection : DatElement {
        public static DatSection Factory(VoidPtr ptr, VoidPtr stringPtr, DatElement parent) {
            var ds = new DatSection(ptr, stringPtr, parent);
            return ds.Name == "data" ? new MovesetSection(ptr, stringPtr, parent) : ds;
        }
        private DatSectionHeader _header;

        public uint DataOffset { get { return _header.DataOffset; } set { _header.DataOffset = value; } }
        [Browsable(false)]
        public uint StringOffset { get { return _header.StringOffset; } set { _header.StringOffset = value; } }

        public String Name { get; set; }
        public uint Length { get; set; }
        public override string ToString() {
            return Name;
        }
        protected DatSection(VoidPtr ptr, VoidPtr stringPtr, DatElement parent) : base(parent) {
            _header = *(DatSectionHeader*)ptr;
            Name = new String((sbyte*)stringPtr + StringOffset);
            FileSpan.StartOffset = ptr - RootFile.Address;
            FileSpan.Length = 8;
        }
    }
}
