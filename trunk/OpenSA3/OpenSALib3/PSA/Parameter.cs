using System;
using System.ComponentModel;
#pragma warning disable 649 //'Field ____ is never assigned'
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
      private struct Data
        {
            public bint Type;
            public bint RawData;
        }
        private Data _data;
        [Category("Parameter")]
        public ParameterType Type
        {
            get { return (ParameterType)(int)_data.Type; }
        }
        [Category("Parameter")]
        public int RawData
        {
            get { return _data.RawData; }
        }
        public unsafe Parameter(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)base.Address;
            Length = 8;
            if ((int)(Type) > 6)
                throw new Exception("Improper Parameter");
            Name = Type + " Parameter";
            Color = Color.BlueViolet;
        }
    }
}
