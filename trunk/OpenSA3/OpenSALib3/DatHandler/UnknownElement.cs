using System;

namespace OpenSALib3.DatHandler
{
    public class UnknownElement : DatElement
    {
        public UnknownElement(DatElement parent, int offset, String name, int length)
            : base(parent, offset)
        {
            Name = name;
            Length = length;
        }

    }
}
