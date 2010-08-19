﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.PSA
{
    class CommandList : DatElement
    {
        bint _offset;
        public unsafe CommandList(DatElement parent, int offset, string name, List<List<Command>> subroutines)
            : base(parent, offset)
        {
            Name = name;
            _offset = *(bint*)(Address);
            if(_offset > 0)
                foreach (Command cl in Command.ReadCommands(this, _offset, subroutines))
                    this[null] = cl;
            Color = System.Drawing.Color.CadetBlue;
        }
    }
}