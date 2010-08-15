using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.PSA
{
    public enum AnimationFlags : byte
    {
        None = 0,
        NoOutTransition = 1,
        Loop = 2,
        MovesCharacter = 4,
        Unknown1 = 8,
        Unknown2 = 16,
        Unknown3 = 32,
        TransitionOutFromStart = 64,
        Unknown4 = 128
    }
    class SubactionFlags : DatElement
    {
        struct Data
        {
            public byte InTransitionTime;
            public AnimationFlags Flags;
            public byte filler;
            public byte filler2;
            public bint AnimationStringOffset;
        }
        private Data data;
        public byte InTransitionTime { get { return data.InTransitionTime; } set { data.InTransitionTime = value; } }
        public AnimationFlags Flags { get { return data.Flags; } set { data.Flags = value; } }
        private string _string;
        public unsafe String AnimationName
        {
            get
            {
                return _string;
            }
        }
        public unsafe SubactionFlags(DatElement parent, uint offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Name = AnimationName;
            Length = 8;
            if (data.AnimationStringOffset > 0)
                _string = RootFile.ReadString(data.AnimationStringOffset);
            else
                _string =  "N/A";
        }
    }
}
