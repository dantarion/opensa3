using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.Moveset;
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
            Children.Add(new UnknownElement(this, DataOffset, "Data", DataLength));
        }
    }
}
