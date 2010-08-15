using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.PSA
{
    public enum ParameterType : int
    {
        VALUE = 0,
        SCALAR = 1,
        OFFSET = 2,
        BOOLEAN = 3,
        VARIABLE = 5,
        REQUIREMENT = 6,
    }
    public class Parameter : DatElement
    {
        struct Data
        {
            public ParameterType Type;
            public bint rawData;
        }
        private Data data;
        public int RawData
        {
            get { return data.rawData; }
        }
        public unsafe Parameter(DatElement parent, uint offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Length = 8;
            Name = "Parameter";
            Color = System.Drawing.Color.BlueViolet;
        }
    }
}
