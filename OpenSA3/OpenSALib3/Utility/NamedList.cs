using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenSALib3.Utility {
    public class NamedList<T> : List<T>,INamed {
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
    public interface INamed :IEnumerable
    {
        String Name
        {
            get;
        }
    }
}