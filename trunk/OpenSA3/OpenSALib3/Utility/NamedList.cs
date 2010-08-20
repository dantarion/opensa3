using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Utility {
    public class NamedList : DatHandler.DatElement{

        public NamedList(DatElement parent,String n) : base(parent,-1)
        {
            Length = 0;
            Name = n;
        }
        
        [Browsable(true)]
        public new int Count
        {
            get { return Dictionary.Count; }
        }
    }
}