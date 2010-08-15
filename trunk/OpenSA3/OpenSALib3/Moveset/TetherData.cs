using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Moveset
{  
    class TetherData : DatElement
    {
        private Data data;
        unsafe struct Data
        {
            public bint HangFrameCount;
            public bfloat Unknown;
        }
        public int HangFrameCount
        {
            get { return data.HangFrameCount; }
            set { data.HangFrameCount = value; }
        }
        public float Unknown
        {
            get { return data.Unknown; }
            set { data.Unknown = value; }
        }
        public unsafe TetherData(DatElement parent, uint offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Name = "TetherData";
            Length = 8;
        }
    }
}
