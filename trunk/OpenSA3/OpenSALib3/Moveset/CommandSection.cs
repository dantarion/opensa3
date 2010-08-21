using System;
using OpenSALib3.DatHandler;
using OpenSALib3.PSA;

namespace OpenSALib3.Moveset
{
    class CommandSection : DatSection
    {
        public CommandSection(DatElement parent, int offset, VoidPtr stringPtr)
            : base(parent, offset, stringPtr)
        {
            TreeColor = null;
        }
        public override void Parse()
        {
            var sr = new Utility.NamedList(this, "Subroutines");
            this["Script"] = Command.ReadCommands(this, DataOffset, sr);
            AddByName(sr);
        }
    }
}
