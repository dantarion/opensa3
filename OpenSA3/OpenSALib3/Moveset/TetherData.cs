using System;
using OpenSALib3.DatHandler;
using System.ComponentModel;

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
        [Browsable(true), Category("TetherData")]
        public int HangFrameCount
        {
            get { return _data.HangFrameCount;  }
            set { _data.HangFrameCount = value; NotifyChanged("HangFrameCount"); }
        }
        [Browsable(true), Category("TetherData")]
        public float Unknown
        {
            get { return _data.Unknown; }
            set { _data.Unknown = value; NotifyChanged("Unknown"); }
        }
        public unsafe TetherData(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)base.Address;
            Name = "TetherData";
            Length = 8;
        }
    }
}
