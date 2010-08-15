using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;
namespace OpenSALib3.Moveset
{
    class MultiJumpData : DatElement
    {
        private Data data;
        unsafe struct Data
        {
            public bfloat Unknown1;
            public bfloat Unknown2;
            public bfloat Unknown3;
            public bfloat HorizontalBoost;
            public bint HopListOffset;
            public bint UnknownListOffset;
            public bint TurnFrames;
        }
        public float Unknown1 { get { return data.Unknown1; } set { data.Unknown1 = value; } }
        public float Unknown2 { get { return data.Unknown2; } set { data.Unknown2 = value; } }
        public float HorizontalBoost { get { return data.HorizontalBoost; } set { data.HorizontalBoost = value; } }
        public float Unknown3 { get { return data.Unknown3; } set { data.Unknown3 = value; } }
        public int TurnFrames { get { return data.TurnFrames; } set { data.TurnFrames = value; } }
        private List<GenericElement<float>> _hoplist = new List<GenericElement<float>>();
        private List<GenericElement<float>> _unknownlist = new List<GenericElement<float>>();
        public unsafe MultiJumpData(DatElement parent, uint offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Name = "MultiJumpData";
            Length = 7*4;
            for (int i = data.HopListOffset; i < FileOffset; i += 4)
                _hoplist.Add(new GenericElement<float>(this, (uint)i, "Unknown"));
            _children.Add(new NamedList(_hoplist, "HopList"));
            for (int i = data.UnknownListOffset; i < data.HopListOffset; i += 4)
                _unknownlist.Add(new GenericElement<float>(this, (uint)i, "Unknown"));
            _children.Add(new NamedList(_unknownlist,"UnknownList"));


        }
    }
}
