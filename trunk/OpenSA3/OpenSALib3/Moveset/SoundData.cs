#pragma warning disable 649 //'Field ____ is never assigned'
using System;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{
    class SoundData : DatElement
    {
        private readonly Data _data;

        struct Data
        {
            public bint  StartOffset;
            public bint  ListCount;
        }
        public unsafe SoundData(DatElement parent, int offset)
            : base(parent, offset)
        {
            TreeColor = System.Windows.Media.Brushes.Orange;
            _data = *(Data*)base.Address;
            Name = "SoundData";
            Length = 8;
            for (var i = 0; i < _data.ListCount; i++)
            {
                this[i] = new SoundDataList(parent, _data.StartOffset + i * 8);
            }
        }
    }
    class SoundDataList : DatElement
    {
        private readonly Data _data;

        struct Data
        {
            public bint StartOffset;
            public bint ListCount;
        }
        public unsafe SoundDataList(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)base.Address;
            Name = "SoundDataList";
            Length = 8;
            for (var i = 0; i < _data.ListCount; i++)
            {
                this[i] = new GenericElement<int>(this, _data.StartOffset + i * 4, "SFX?");
            }
        }
    }
}
