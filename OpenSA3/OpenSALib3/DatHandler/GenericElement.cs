using System;
using System.Collections;

namespace OpenSALib3.DatHandler
{
    public class GenericElement<T> : DatElement
    {
        public Type Type
        {
            get { return typeof(T); }
        }
        public unsafe GenericElement(DatElement parent, int fileOffset, String name)
            : base(parent, fileOffset)
        {
            Length = 4;
            Name = name;
            _intval = *(bint*)base.Address;
            _floatval = *(bfloat*)base.Address;
        }
        private bint _intval;
        private bfloat _floatval;
        public object Value
        {
            get
            {
                if (Type == typeof(float))
                    return (float)_floatval;
                return (int)_intval;
            }
            set
            {
                if (Type == typeof(float))
                    _floatval = Convert.ToSingle(value);
                else
                    _intval = Convert.ToInt32(value);
            }
        }
    }
}
