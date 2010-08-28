using System;
using System.Activities.Presentation.PropertyEditing;
using System.ComponentModel;
using OpenSALib3.Moveset;
using OpenSALib3.Utility;

namespace OpenSALib3.DatHandler {

    public struct DatSectionHeader
    {
        public bint DataOffset;
        public bint StringOffset;
    }
    public unsafe abstract class DatSection : DatElement
    {
        private DatSectionHeader _header;
        public static DatSection Factory(DatElement parent, int offset, int stringBase)
        {
            var ds = new GenericSection(parent, offset, stringBase);
            if (ds.Name == "data")
                return new MovesetSection(parent, offset, stringBase);
            if (ds.Name == "dataCommon")
                return new DataCommonSection(parent, offset, stringBase);
            if (ds.Name.StartsWith("statusAnimCmdDisguiseList"))
                return new CommandOverrideSection(parent, offset, stringBase);
            if (ds.Name.StartsWith("statusAnimCmdExitDisguiseList"))
                return new CommandOverrideSection(parent, offset, stringBase);
            if (ds.Name.Contains("AnimCmd"))
                return new CommandSection(parent, offset, stringBase);
            return ds;
        }
        public abstract void Parse();
        [Browsable(false)]
        public int StringOffset {
            get { return _header.StringOffset; }
            set { _header.StringOffset = value; }
        }
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        [Category("Section")]
        public int DataOffset
        {
            get { return _header.DataOffset; }
            set { _header.DataOffset = value; }
        }
        [Category("Section")]
        public int DataLength
        {
            get;
            set;
        }
        protected DatSection(DatElement parent, int offset, int stringbase)
            : base(parent, offset)
        {
            base.TreeColor = null;
            _header = *(DatSectionHeader*)base.Address;
            Length = 8;
            base.Name = RootFile.ReadString(stringbase + StringOffset);
        }
    }
}