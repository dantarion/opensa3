using System;
using System.Collections.Generic;
using System.Collections;
using BrawlLib.SSBB.ResourceNodes;
using OpenSALib3.DatHandler;

namespace Tabuu.Utility {
    public class DatWrapper : DatFile, IEnumerable, Be.Windows.Forms.IByteProvider {
        public new static DatWrapper FromFile(string filename) {
            var node = NodeFactory.FromFile(null, filename);
            return new DatWrapper(node.Children[0]);
        }
        protected DatWrapper(ResourceNode s)
            : base(s) {
        }
        public IEnumerator GetEnumerator() {
            return new DatEnemerator(this);
        }
        #region IByteProvider
        public void ApplyChanges() {
            throw new NotImplementedException();
        }

        public event EventHandler Changed {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public void DeleteBytes(long index, long length) {
            throw new NotImplementedException();
        }

        public bool HasChanges() {
            throw new NotImplementedException();
        }

        public void InsertBytes(long index, byte[] bs) {
            throw new NotImplementedException();
        }

        public long Length {
            get { return FileSize; }
        }

        public event EventHandler LengthChanged {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public unsafe byte ReadByte(long index) {
            return *(byte*)(Address + (uint)index);
        }

        public bool SupportsDeleteBytes() {
            throw new NotImplementedException();
        }

        public bool SupportsInsertBytes() {
            throw new NotImplementedException();
        }

        public bool SupportsWriteByte() {
            return true;
        }

        public unsafe void WriteByte(long index, byte value) {
            *(byte*)(Address + (uint)index) = value;
        }
        #endregion
        #region Scripting Usability Functions
        public unsafe int ReadInt32(int offset) {
            return *(bint*)(Address + offset);
        }
        public unsafe short ReadInt16(int offset) {
            return *(bshort*)(Address + offset);
        }
        public unsafe float ReadSingle(int offset) {
            return *(bfloat*)(Address + offset);
        }
        #endregion
    }
    class NamedList<T> : IEnumerable {
        private readonly List<T> _list;
        public String Name { get; set; }
        public NamedList(List<T> l, String n) {
            _list = l;
            Name = n;
        }
        public IEnumerator GetEnumerator() {
            return _list.GetEnumerator();
        }
        public override string ToString() {
            return Name;
        }
    }
    class DatEnemerator : IEnumerator {
        private readonly DatFile _file;
        public int i = -1;
        public DatEnemerator(DatFile f) {
            _file = f;
        }

        public object Current {
            get {
                switch (i) {
                    case 0:
                        return new NamedList<DatSection>(_file.Sections, "Sections");
                    case 1:
                        return new NamedList<DatSection>(_file.References, "References");
                }
                return null;
            }
        }

        public bool MoveNext() {
            i++;
            return i <= 1;
        }

        public void Reset() {
            i = -1;
        }
    }

}
