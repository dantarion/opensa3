﻿using System;
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
    class SubactionFlags : DatElement {

#pragma warning disable 169 //'Field ____ is never used'
#pragma warning disable 649 //'Field ____ is never assigned';
        struct Data
        {
            public byte InTransitionTime;
            public AnimationFlags Flags;
            public byte Filler;
            public byte Filler2;
            public bint AnimationStringOffset;
        }
#pragma warning restore 169 //'Field ____ is never used'
#pragma warning restore 649 //'Field ____ is never assigned';
        private Data _data;
        public byte InTransitionTime { get { return _data.InTransitionTime; } set { _data.InTransitionTime = value; } }
        public AnimationFlags Flags { get { return _data.Flags; } set { _data.Flags = value; } }
        public int AnimationStringOffset { get { return _data.AnimationStringOffset; } set { _data.AnimationStringOffset = value; } }
        private readonly string _string;
        public String AnimationName
        {
            get
            {
                return _string;
            }
        }
        public unsafe SubactionFlags(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)base.Address;
            Length = 8;
            _string = _data.AnimationStringOffset > 0 ? RootFile.ReadString(_data.AnimationStringOffset) : "N/A";
            Name = AnimationName;
        }
    }
}