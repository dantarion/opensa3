using System;
using System.Drawing;
using OpenSALib3.DatHandler;

namespace OpenSALib3.PSA
{
    public enum ParameterType {
        Value = 0,
        Scalar = 1,
        Offset = 2,
        Boolean = 3,
        Variable = 5,
        Requirement = 6,
    }
    public class Parameter : DatElement
    {
        struct Data
        {
            public ParameterType Type;
            public bint rawData;
        }
        private Data _data;
        public int RawData
        {
            get { return _data.rawData; }
        }
        public unsafe Parameter(DatElement parent, uint offset)
            : base(parent, offset)
        {
            _data = *(Data*)Address;
            Length = 8;
            Name = "Parameter";
            Color = Color.BlueViolet;
        }
    }
}
