using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using System.ComponentModel;
using System.Diagnostics;
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
            public bint type;
            public bint rawData;
        }
        private Data data;
        [Category("Parameter")]
        public ParameterType Type
        {
            get { return (ParameterType)(int)data.type; }
        }
        [Category("Parameter")]
        public int RawData
        {
            get { return data.rawData; }
        }
        public unsafe Parameter(DatElement parent, int offset)
            : base(parent, offset)
        {
            data = *(Data*)Address;
            Length = 8;
            if ((int)(this.Type) > 6)
                throw new Exception("Improper Parameter");
            Name = Type + " Parameter";
            Color = System.Drawing.Color.BlueViolet;
        }
    }
}
