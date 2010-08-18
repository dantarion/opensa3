using System;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{
    class SoundData : DatElement
    {
        private Data _data;

        struct Data
        {
            public bint  StartOffset;
            public bint  ListCount;
        }
        public unsafe SoundData(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)Address;
            Name = "SoundData";
            Length = 8;
            for (int i = 0; i < _data.ListCount; i++)
            {
                this[i] = new SoundDataList(parent, _data.StartOffset + i * 8);
            }
        }
    }
    class SoundDataList : DatElement
    {
        private Data _data;

        struct Data
        {
            public bint StartOffset;
            public bint ListCount;
        }
        public unsafe SoundDataList(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)Address;
            Name = "SoundDataList";
            Length = 8;
            for (int i = 0; i < _data.ListCount; i++)
            {
                this[i] = new GenericElement<int>(this, _data.StartOffset + i * 4, "SFX?");
            }
        }
    }
}
