using System;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;
using OpenSALib3.PSA;
namespace OpenSALib3.Moveset
{
    public class DataCommonSection :DatSection {
#pragma warning disable 169 //'Field ____ is never used'
#pragma warning disable 649 //'Field ____ is never assigned';
        struct Header
        {
            public bint Unknown0;
            public bint Unknown1;
            public bint Unknown2;
            public bint Unknown3;
            public bint ActionsStart;
            public bint Actions2Start;
            public bint Unknown6;
            public bint Unknown7;
            public bint Unknown8;
            public bint Unknown9;
            public bint Unknown10;
            public bint Unknown11;
            public bint Unknown12;
            public bint Unknown13;
            public bint Unknown14;
            public bint Unknown15;
            public bint Unknown16;
            public bint Unknown17;
            public bint Unknown18;
            public bint Unknown19;
            public bint Unknown20;
            public bint Unknown21;
            public bint Unknown22;
            public bint Unknown23;
            public bint Unknown24;
            public bint Unknown25;
        }
#pragma warning restore 169 //'Field ____ is never used'
#pragma warning restore 649 //'Field ____ is never assigned';
        private Header _header;
        public unsafe DataCommonSection(DatElement parent, int offset, int stringbase)
            : base(parent,offset, stringbase)
        {
            _header = *(Header*)(RootFile.Address + DataOffset);
        }
        public override void Parse()
        {
            var count = 0;
            var subroutines = new NamedList(this, "Subroutines");
            var actions = new NamedList(this, "Actions");
            for (int i = _header.ActionsStart; i < _header.Actions2Start; i += 4)
                actions[count] = new CommandList(actions, i, String.Format("{0:X03}",count++), subroutines);
            count = 0;
            var actions2 = new NamedList(this, "Actions2");
            for (int i = _header.Actions2Start; i < _header.Unknown7; i += 4)
                actions2[count] = new CommandList(actions2, i, String.Format("{0:X03}",count++), subroutines);
            AddByName(actions);
            AddByName(actions2);
            AddByName(subroutines);
        }
    }
}
