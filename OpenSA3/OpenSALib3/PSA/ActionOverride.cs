using System;
using System.Activities.Presentation.PropertyEditing;
using System.ComponentModel;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.PSA
{
    public class ActionOverride : DatElement
    {
        private Data _data;

         struct Data
        {
            public bint ActionID;
            public bint CommandListOffset;
        }
        [Category("Override")]
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int ActionID
        {
            get { return _data.ActionID; }
            set { _data.ActionID = value; }
        }
        [Category("Override")]
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int CommandListOffset
        {
            get { return _data.CommandListOffset; }
            set { _data.CommandListOffset = value; }
        }
        public unsafe ActionOverride(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)base.Address;
            Name = "ActionOverride";
            Length = 8;
            if (_data.CommandListOffset > 0)
                foreach (DatElement cl in Command.ReadCommands(this, _data.CommandListOffset, null))
                    this[null] = cl;


        }
    }
}
