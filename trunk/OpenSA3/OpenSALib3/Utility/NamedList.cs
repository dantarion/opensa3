using System;
using System.Collections;
using System.ComponentModel;
namespace OpenSALib3.Utility {
    public class NamedList : IEnumerable {
        private readonly ICollection _list;
        [Browsable(true)]
        public String Name { get; set; }

        public NamedList(ICollection l, String n)
        {
            _list = l;
            Name = n;
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return _list.GetEnumerator();
        }
        
        public override string ToString() {
            return Name;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        [Browsable(true)]
        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return this; }
        }
    }
}