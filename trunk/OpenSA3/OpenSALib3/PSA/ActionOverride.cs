using System;
using OpenSALib3.DatHandler;

namespace OpenSALib3.PSA
{
    class ActionOverride : DatElement
    {
        private Data _data;

         struct Data
        {
            public bint SubactionID;
            public bint CommandListOffset;
        }
        public int SubactionID
        {
            get { return _data.SubactionID; }
            set { _data.SubactionID = value; }
        }
        public unsafe ActionOverride(DatElement parent, uint offset)
            : base(parent, offset)
        {
            _data = *(Data*)Address;
            Name = "ActionOverride";
            Length = 8;
            if(_data.CommandListOffset > 0)
            Children = Command.ReadCommands(this, (uint)_data.CommandListOffset,null);

        }
    }
}
