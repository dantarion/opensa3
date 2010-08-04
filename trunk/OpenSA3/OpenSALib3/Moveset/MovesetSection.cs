using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{
    public class MovesetSection : DatSection
    {
        /*
         private const int SUBACTION_FLAG_START = 0;
        private const int SOMETHING = 1;
        private const int ATTRIBUTE_START = 2;
        private const int SSE_ATTRIBUTE_START = 3;
        private const int ATTRIBUTE2_START = 4;
        private const int ACTION_OFFSETS_START = 9;
        private const int HIDDEN_ACTION_START = 10;
        private const int HIDDEN_ACTION_END = 11;
        private const int SUBACTION_MAIN_OFFSETS_START = 12;
        private const int SUBACTION_GFX_OFFSETS_START = 13;
        private const int SUBACTION_SFX_OFFSETS_START = 14;
        private const int SUBACTION_OTHER_OFFSETS_START = 15;
         */
        public unsafe struct MovesetHeader 
        {
            public uint SubactionFlagsStart;
            public uint Unknown1;
            public uint AttributeStart;
            public uint Attribute2Start;
            public uint Attribute3Start;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint ActionsStart;
            public uint Actions2Start;
            public uint Unknown11;
            public uint SubactionMainStart;
            public uint SubactionGFXStart;
            public uint SubactionSFXStart;
            public uint SubactionOtherStart;
        }
        private MovesetHeader header;
        public unsafe MovesetSection(VoidPtr ptr, VoidPtr stringPtr, DatElement parent)
            : base(ptr, stringPtr, parent)
        {
            header = *(MovesetHeader*)(RootFile.Address + FileOffset);

        }
    }
}
