﻿using System;
using OpenSALib3.DatHandler;
using System.ComponentModel;

namespace OpenSALib3.Moveset
{
    class CrawlData : DatElement
    {
        private Data _data;

         struct Data
        {
            public bfloat ForwardAccel;
            public bfloat BackwardAccel;
        }
        [Browsable(true), Category("CrawlData")]
        public float ForwardAcceleration
        {
            get { return _data.ForwardAccel; }
            set { _data.ForwardAccel = value; }
        }
        [Browsable(true), Category("CrawlData")]
        public float BackwardAcceleration
        {
            get { return _data.BackwardAccel; }
            set { _data.BackwardAccel = value; }
        }
        public unsafe CrawlData(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)base.Address;
            Name = "CrawlData";
            Length = 8;
        }
    }
}