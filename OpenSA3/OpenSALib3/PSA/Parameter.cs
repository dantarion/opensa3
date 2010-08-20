using System;
using System.ComponentModel;
#pragma warning disable 649 //'Field ____ is never assigned'
using System.Drawing;
using OpenSALib3.DatHandler;

namespace OpenSALib3.PSA
{
    public enum ParameterType
    {
        Value = 0,
        Scalar = 1,
        Offset = 2,
        Boolean = 3,
        Variable = 5,
        Requirement = 6,
    }

    public class ValueParameter : Parameter
    {
        [Category("Parameter")]
        public int Value
        {
            get { return _data.RawData; }
            set { _data.RawData = value; NotifyChanged("Value"); }
        }
        public ValueParameter(DatElement parent, int offset)
            : base(parent, offset)
        {
        }
    }
    public class ScalarParameter : Parameter
    {
        [Category("Parameter")]
        public float Value
        {
            get { return (float)(int)_data.RawData / 60000; }
            set { _data.RawData = (int)value * 60000; NotifyChanged("Value"); }
        }

        public ScalarParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
    }
    public class OffsetParameter : Parameter
    {
        [Category("Parameter")]
        public int Value
        {
            get { return _data.RawData; }
            set { _data.RawData = value; NotifyChanged("Value"); }
        }
        public OffsetParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
    }
    public class BooleanParameter : Parameter
    {
        public BooleanParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
    }
    public class VariableParameter : Parameter
    {
        public VariableParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
    }
    public class RequirementParameter : Parameter
    {
        public string RequirementName
        {
            get
            {
                string name;
                Utility.PSANames.ReqNames.TryGetValue(RawData, out name);
                return name != null ? name : "Unknown";
            }
        }
        public RequirementParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
    }
    public class Parameter : DatElement
    {
        public static Parameter Factory(DatElement parent, int offset)
        {
            var parameter = new Parameter(parent, offset);
            switch (parameter.Type)
            {
                case ParameterType.Value:
                    return new ValueParameter(parent, offset);
                case ParameterType.Scalar:
                    return new ScalarParameter(parent, offset);
                case ParameterType.Offset:
                    return new OffsetParameter(parent, offset);
                case ParameterType.Variable:
                    return new VariableParameter(parent, offset);
                case ParameterType.Requirement:
                    return new RequirementParameter(parent, offset);
            }

            return parameter;
        }
        protected struct Data
        {
            public bint Type;
            public bint RawData;
        }
        protected Data _data;
        [Category("Parameter")]
        public ParameterType Type
        {
            get { return (ParameterType)(int)_data.Type; }
        }
        [Category("Parameter"), Browsable(true)]
        public int RawData
        {
            get { return _data.RawData; }
        }
        protected unsafe Parameter(DatElement parent, int offset)
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
