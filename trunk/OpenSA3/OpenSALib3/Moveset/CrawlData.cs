using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Moveset
{
    class CrawlData : DatElement
    {
        private Data data;
        unsafe struct Data
        {
            public bfloat ForwardAccel;
            public bfloat BackwardAccel;
        }
        public float ForwardAcceleration
        {
            get { return data.ForwardAccel; }
            set { data.ForwardAccel = value; }
        }
        public float BackwardAcceleration
        {
            get { return data.BackwardAccel; }
            set { data.BackwardAccel = value; }
        }
        public unsafe CrawlData(DatElement parent, uint offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Name = "CrawlData";
            Length = 8;
        }
    }
}
