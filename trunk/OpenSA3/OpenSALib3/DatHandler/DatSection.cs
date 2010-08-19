﻿using System;
using System.ComponentModel;
using OpenSALib3.Moveset;

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
            if (ds.Name.StartsWith("statusAnimCmdDisguiseList"))
                return new CommandOverrideSection(parent, offset, stringBase);
            if (ds.Name.StartsWith("statusAnimCmdExitDisguiseList"))
                return new CommandOverrideSection(parent, offset, stringBase);
            return ds;
        }
        public abstract void Parse();
        [Browsable(false)]
        public int StringOffset {
            get { return _header.StringOffset; }
            set { _header.StringOffset = value; }
        }
        [Editor(typeof(OpenSALib3.Utility.HexPropertyEditor), typeof(System.Activities.Presentation.PropertyEditing.PropertyValueEditor))]
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
            _header = *(DatSectionHeader*)Address;
            Length = 8;
            Name = RootFile.ReadString(stringbase + StringOffset);
        }
    }
}