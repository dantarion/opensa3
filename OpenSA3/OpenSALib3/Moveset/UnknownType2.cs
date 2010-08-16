using System;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{
    class UnknownElement : DatElement
    {
        public UnknownElement(DatElement parent, uint offset, String name, uint length)
            : base(parent, offset)
        {
            Name = name;
            Length = length;
        }
    }
}
