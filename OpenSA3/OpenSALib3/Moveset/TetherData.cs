using System;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{  
    class TetherData : DatElement
    {
        private Data _data;

         struct Data
        {
            public bint HangFrameCount;
            public bfloat Unknown;
        }
        public int HangFrameCount
        {
            get { return _data.HangFrameCount; }
            set { _data.HangFrameCount = value; }
        }
        public float Unknown
        {
            get { return _data.Unknown; }
            set { _data.Unknown = value; }
        }
        public unsafe TetherData(DatElement parent, uint offset)
            : base(parent, offset)
        {
            _data = *(Data*)Address;
            Name = "TetherData";
            Length = 8;
        }
    }
}
