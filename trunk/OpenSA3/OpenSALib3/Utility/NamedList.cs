using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace OpenSALib3.Utility
{
    public class NamedList<T> : IEnumerable, IEnumerable<T>
    {
        private readonly IEnumerable<T> _list;
        public String Name { get; set; }
        public NamedList(IEnumerable<T> l, String n)
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

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

}
