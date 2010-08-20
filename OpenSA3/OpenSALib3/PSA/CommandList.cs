using System;
using System.Collections.Generic;
using System.Drawing;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.PSA
{
    class CommandList : DatElement
    {
        readonly bint _offset;
        public unsafe CommandList(DatElement parent, int offset, string name, NamedList subroutines)
            : base(parent, offset)
        {
            Name = name;
            _offset = *(bint*)(base.Address);
            if(_offset > 0)
                foreach (Command cl in Command.ReadCommands(this, _offset, subroutines))
                    this[null] = cl;
            Color = Color.CadetBlue;
        }
    }
}
