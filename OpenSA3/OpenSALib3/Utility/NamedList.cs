using System;
using System.Collections;

namespace OpenSALib3.Utility {
    public class NamedList : IEnumerable {
        private readonly IEnumerable _list;
        public String Name { get; set; }

        public NamedList(IEnumerable l, String n) {
            _list = l;
            Name = n;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _list.GetEnumerator();
        }

        public override string ToString() {
            return Name;
        }
    }
}