﻿#pragma warning disable 649 //'Field ____ is never assigned'
using System;
using System.Collections.Generic;
using OpenSALib3.DatHandler;
using System.ComponentModel;

namespace OpenSALib3.Moveset
{
    class MultiJumpData : DatElement
    {
        private Data _data;

         struct Data
        {
            public bfloat Unknown1;
            public bfloat Unknown2;
            public bfloat Unknown3;
            public bfloat HorizontalBoost;
            public bint HopListOffset;
            public bint UnknownListOffset;
            public bint TurnFrames;
        }
        [Browsable(true), Category("MultiJumpData")]
         public float Unknown1 { get { return _data.Unknown1; } set { _data.Unknown1 = value; NotifyChanged("Unknown1"); } }
        [Browsable(true), Category("MultiJumpData")]
        public float Unknown2 { get { return _data.Unknown2; } set { _data.Unknown2 = value; NotifyChanged("Unknown2"); } }
        [Browsable(true), Category("MultiJumpData")]
        public float HorizontalBoost { get { return _data.HorizontalBoost; } set { _data.HorizontalBoost = value; NotifyChanged("HorizontalBoost"); } }
        [Browsable(true), Category("MultiJumpData")]
        public float Unknown3 { get { return _data.Unknown3; } set { _data.Unknown3 = value; NotifyChanged("Unknown3"); } }
        [Browsable(true), Category("MultiJumpData")]
        public int TurnFrames { get { return _data.TurnFrames; } set { _data.TurnFrames = value; NotifyChanged("TurnFrames"); } }
        private readonly List<GenericElement<float>> _hoplist = new List<GenericElement<float>>();
        private readonly List<GenericElement<float>> _unknownlist = new List<GenericElement<float>>();
        public unsafe MultiJumpData(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)base.Address;
            Name = "MultiJumpData";
            Length = 7*4;
            for (int i = _data.HopListOffset; i < FileOffset; i += 4)
                _hoplist.Add(new GenericElement<float>(this, i, "Unknown"));
            this["HopList"] = _hoplist;
            for (int i = _data.UnknownListOffset; i < _data.HopListOffset; i += 4)
                _unknownlist.Add(new GenericElement<float>(this, i, "Unknown"));
            this["UnknownList"] = _unknownlist;


        }
    }
}
