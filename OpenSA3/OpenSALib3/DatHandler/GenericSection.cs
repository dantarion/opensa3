using System;

namespace OpenSALib3.DatHandler
{
    public class GenericSection : DatSection
    {
        public GenericSection(DatElement parent, int offset, VoidPtr stringPtr)
            : base(parent, offset, stringPtr)
        {
            
        }
        public override void Parse()
        {
            this.AddByName(new UnknownElement(this, DataOffset, "Data", DataLength));
        }
    }
}
