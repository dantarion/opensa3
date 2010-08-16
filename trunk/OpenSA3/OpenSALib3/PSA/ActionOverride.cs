using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using System.ComponentModel;
using OpenSALib3.Utility;
using System.Activities.Presentation.PropertyEditing;
namespace OpenSALib3.PSA
{
    public class ActionOverride : DatElement
    {
        private Data data;
        unsafe struct Data
        {
            public bint ActionID;
            public bint CommandListOffset;
        }
        [Category("Override")]
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int ActionID
        {
            get { return data.ActionID; }
            set { data.ActionID = value; }
        }
        [Category("Override")]
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int CommandListOffset
        {
            get { return data.CommandListOffset; }
            set { data.CommandListOffset = value; }
        }
        public unsafe ActionOverride(DatElement parent, int offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Name = "ActionOverride";
            Length = 8;
            if(data.CommandListOffset > 0)
            Children = Command.ReadCommands(this, (int)data.CommandListOffset,null);

        }
    }
}
