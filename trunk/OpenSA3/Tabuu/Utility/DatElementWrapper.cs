using System;
using System.Collections;
using System.Linq;
using OpenSALib3.DatHandler;
using System.Collections.Generic;
namespace Tabuu.Utility {
    public class DatElementWrapper : Be.Windows.Forms.IByteProvider {
        private readonly DatElement _datelement;
        private bool _changed;
        readonly bool _contentmode;
        public DatElementWrapper(DatElement datElement, bool contentmode = false) {
            _datelement = datElement;
            _contentmode = contentmode;
        }
        private readonly Dictionary<long, System.Drawing.Color> _colorCache = new Dictionary<long, System.Drawing.Color>();
        #region IByteProvider
        public void ApplyChanges() { }

        public void DeleteBytes(long index, long length) {
            throw new NotImplementedException();
        }

        public bool HasChanges() {
            return _changed;
        }

        public void InsertBytes(long index, byte[] bs) {
            throw new NotImplementedException();
        }

        public long Length {
            get { return _contentmode ? ((DatSection) _datelement).DataLength: _datelement.Length; }
        }

#pragma warning disable 067 //The event '____' is never used
        public event EventHandler Changed;
        public event EventHandler LengthChanged;
#pragma warning restore 067

        public unsafe byte ReadByte(long index) {
            var offset = _contentmode ? ((DatSection) _datelement).DataOffset : _datelement.FileOffset;
            return *(byte*)(_datelement.RootFile.Address + offset + (uint)index);
        }

        public System.Drawing.Color GetByteColor(long index) {

            long offset = _contentmode ? ((DatSection) _datelement).DataOffset : _datelement.FileOffset;
            offset += index;
            offset -= offset % 4;
            if (!_colorCache.ContainsKey(offset))
            {
                var ele = SearchForDatElement(_datelement, offset);
                _colorCache[offset] =  ele != null ? ele.Color : System.Drawing.Color.Transparent;
            }
            return _colorCache[offset];
        }

        private static DatElement SearchForDatElement(IEnumerable node, long index) {
            foreach (var found in
                node.OfType<IEnumerable>().Select(searchableChild => SearchForDatElement(searchableChild, index)).Where(found => found != null)) {
                return found; //If we find it, stop searching>	Tabuu.exe!Tabuu.Utility.DatElementWrapper.SearchForDatElement(System.Collections.IEnumerable node, long index) Line 59 + 0x70 bytes	C#
            }
            //If its not in any of the children, maybe this is it!
            var asDatElement = node as DatElement;
            if (asDatElement == null) //If its not a DatElement though, the search failed
                return null;
            return IsIndexInDatElement(index,asDatElement)
                ? asDatElement //But if it is, and we are in there, we found it!
                : null;        //But if its not we failed


        }
        private static bool IsIndexInDatElement(long index, DatElement element)
        {
            return element.FileOffset <= index && index < element.FileOffset + element.Length;
        }

        public bool SupportsDeleteBytes() {
            return false;
        }

        public bool SupportsInsertBytes() {
            return false;
        }

        public bool SupportsWriteByte() {
            return true;
        }

        public unsafe void WriteByte(long index, byte value) {
            var src = (byte*)(_datelement.RootFile.Address + _datelement.FileOffset + (uint)index);
            *src = value;
            _changed = true;
            _datelement.RootFile.Changed = true;
        }
        #endregion

    }
}