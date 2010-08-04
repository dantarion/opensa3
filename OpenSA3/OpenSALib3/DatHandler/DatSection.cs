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
        [Browsable(false)]
        public uint StringOffset { get { return _header.StringOffset; } set { _header.StringOffset = value; } }

        protected DatSection(VoidPtr ptr, VoidPtr stringPtr, DatElement parent) : base(parent) {
            _header = *(DatSectionHeader*)ptr;
            Name = new String((sbyte*)stringPtr + StringOffset);
            FileOffset = _header.DataOffset;
        }
    }
}
