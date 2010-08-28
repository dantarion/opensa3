using System;

namespace OpenSALib3.DatHandler
{
    public class UnknownElement : DatElement
    {
        public UnknownElement(DatElement parent, int offset, String name, int length)
            : base(parent, offset)
        {
            base.Name = name;
            Length = length;
        }

    }
}
