using System;
using System.Collections.Generic;
using System.Drawing;
using OpenSALib3.DatHandler;

namespace OpenSALib3.PSA
{
    class CommandList : DatElement
    {
        readonly bint _offset;
        public unsafe CommandList(DatElement parent, int offset, string name, List<List<Command>> subroutines)
            : base(parent, offset)
        {
            Name = name;
            _offset = *(bint*)(base.Address);
            if(_offset > 0)
                foreach (var cl in Command.ReadCommands(this, _offset, subroutines))
                    this[null] = cl;
            Color = Color.CadetBlue;
        }
    }
}
