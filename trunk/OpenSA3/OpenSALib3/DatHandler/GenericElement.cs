using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSALib3.DatHandler
{
    public class GenericElement<T> : DatElement
    {
        public Type Type
        {
            get { return typeof(T); }
        }
        public unsafe GenericElement(DatElement parent, uint fileOffset,String name)
            : base(parent, fileOffset)
        {
            Length = 4;
            Name = name;
            intval = *(bint*)Address;
            floatval = *(bfloat*)Address;
        }
        private bint intval;
        private bfloat floatval;
        public unsafe object Value
        {
            get
            {
                if (Type == typeof(float))
                    return (float)floatval;
                return (int)intval;
            }
            set
            {
                if (Type == typeof(float))
                    floatval = Convert.ToSingle(value);
                else
                    intval = Convert.ToInt32(value);
            }
        }
    }
}
