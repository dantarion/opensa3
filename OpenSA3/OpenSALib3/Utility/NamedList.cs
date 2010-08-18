using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
namespace OpenSALib3.Utility {
    public class NamedList<T> : List<T> {
        [Browsable(true)]
        public String Name { get; set; }

        public NamedList(String n)
        {
            Name = n;
        }
        
        public override string ToString() {
            return Name;
        }

        [Browsable(true)]
        public new int Count
        {
            get { return base.Count; }
        }
    }
}