using System;
using OpenSALib3.DatHandler;
using OpenSALib3.PSA;

namespace OpenSALib3.Moveset
{
    class CommandOverrideSection : DatSection
    {
        public CommandOverrideSection(DatElement parent, int offset, VoidPtr stringPtr)
            : base(parent, offset, stringPtr)
        {
        
        }
        public override void Parse()
        {
            var o = DataOffset;
            var ao = new ActionOverride(this, o);
            while (ao.ActionID > 0)
            {
                this[null] = ao;
                o += 8;
                ao = new ActionOverride(this, o);
            }
            this[null] = ao; 
        }
    }
}
