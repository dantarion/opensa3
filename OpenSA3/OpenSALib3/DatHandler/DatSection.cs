using System;
using System.ComponentModel;
using OpenSALib3.Moveset;

namespace OpenSALib3.DatHandler {

    public struct DatSectionHeader
    {
        public buint DataOffset;
        public bint StringOffset;
    }
    public unsafe class DatSection : DatElement
    {
        private DatSectionHeader _header;
        public static DatSection Factory(DatElement parent, uint offset, int stringBase)
        {
            var ds = new DatSection(parent, offset, stringBase);
            return ds.Name == "data" ? new MovesetSection(parent, offset, stringBase) : ds;
        }
        [Browsable(false)]
        public int StringOffset {
            get { return _header.StringOffset; }
            set { _header.StringOffset = value; }
        }
        public uint DataOffset
        {
            get { return _header.DataOffset; }
            set { _header.DataOffset = value; }
        }
        public uint DataLength
        {
            get;
            set;
        }
        protected DatSection(DatElement parent, uint offset, int stringbase)
            : base(parent, offset)
        {
            _header = *(DatSectionHeader*)Address;
            Length = 8;
            Name = RootFile.ReadString(stringbase + StringOffset);
        }
    }
}