using System;
using System.ComponentModel;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Utility {
    public class NamedList : DatElement{

        public NamedList(DatElement parent,String name) : base(parent,-1)
        {
            Length = 0;
            base.Name = name;
        }
        
        [Browsable(true)]
        public int Count
        {
            get { return Dictionary.Count; }
        }
    }
}