using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace OpenSALib3.Utility
{

    class NamedList<T> : IEnumerable
    {
        private readonly List<T> _list;
        public String Name { get; set; }
        public NamedList(List<T> l, String n)
        {
            _list = l;
            Name = n;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        public override string ToString()
        {
            return Name;
        }
    }

}
