using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Moveset
{
    class UnknownElement : DatElement
    {
        public unsafe UnknownElement(DatElement parent, int offset, String name, int length)
            : base(parent, offset)
        {
            Name = name;
            Length = length;
        }
    }
}
