using System;
using System.ComponentModel;
#pragma warning disable 649 //'Field ____ is never assigned'
using System.Drawing;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;
using System.Activities.Presentation.PropertyEditing;

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
        public ValueParameter(DatElement parent,int type,int rawdata):base(parent,type,rawdata)
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
        public ScalarParameter(DatElement parent, int type, int rawdata)
            : base(parent, type, rawdata)
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
        public OffsetParameter(DatElement parent, int type, int rawdata)
            : base(parent, type, rawdata)
        {
        }
    }
    public class BooleanParameter : Parameter
    {
        public BooleanParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
        public BooleanParameter(DatElement parent, int type, int rawdata)
            : base(parent, type, rawdata)
        {
        }
    }
    public class VariableParameter : Parameter
    {
        public VariableParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
        public VariableParameter(DatElement parent, int type, int rawdata)
            : base(parent, type, rawdata)
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
                PSANames.ReqNames.TryGetValue(RawData, out name);
                return name ?? "Unknown";
            }
        }
        public RequirementParameter(DatElement parent, int offset)
            : base(parent, offset)
        {

        }
        public RequirementParameter(DatElement parent, int type, int rawdata)
            : base(parent, type, rawdata)
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
        protected Data _data; //TODO
        [Category("Parameter")]
        public ParameterType Type
        {
            get { return (ParameterType)(int)_data.Type; }
        }
        [Category("Parameter"), Browsable(true)]
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int RawData
        {
            get { return _data.RawData; }
        }
        protected unsafe Parameter(DatElement parent, int offset)
            : base(parent, offset)
        {
            base.TreeColor = null;
            _data = *(Data*)base.Address;
            Length = 8;
            if ((int)(Type) > 6)
                throw new Exception("Improper Parameter");
            base.Name = Type + " Parameter";
            Color = Color.BlueViolet;
        }
        public unsafe Parameter(DatElement parent, int type, int rawData):base(parent,0)
        {
            TreeColor = null;
            _data.Type = type;
            _data.RawData = rawData;
            Name = Type + "Parameter";
            Color = Color.BlueViolet;
        }
    }
}
