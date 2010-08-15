using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.PSA
{
    class ActionOverride : DatElement
    {
        private Data data;
        unsafe struct Data
        {
            public bint SubactionID;
            public bint CommandListOffset;
        }
        public int SubactionID
        {
            get { return data.SubactionID; }
            set { data.SubactionID = value; }
        }
        public unsafe ActionOverride(DatElement parent, uint offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Name = "ActionOverride";
            Length = 8;
            if(data.CommandListOffset > 0)
            _children = Command.ReadCommands(this, (uint)data.CommandListOffset,null);

        }
    }
}
