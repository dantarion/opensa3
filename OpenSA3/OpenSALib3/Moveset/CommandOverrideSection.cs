using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using OpenSALib3.PSA;
namespace OpenSALib3.Moveset
{
    class CommandOverrideSection : DatSection
    {
        public unsafe CommandOverrideSection(DatElement parent, int offset, VoidPtr stringPtr)
            : base(parent, offset, stringPtr)
        {
        
        }
        public override void Parse()
        {
            var o = DataOffset;
            var ao = new ActionOverride(this, o);
            while (ao.ActionID > 0)
            {
                Children.Add(ao);
                o += 8;
                ao = new ActionOverride(this, o);
            }
            Children.Add(ao); 
        }
    }
}
