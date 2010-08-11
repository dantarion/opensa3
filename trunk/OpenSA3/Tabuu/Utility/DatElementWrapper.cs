﻿using System;
using System.Collections;
using System.Linq;
using OpenSALib3.DatHandler;

namespace Tabuu.Utility {
    public class DatElementWrapper : Be.Windows.Forms.IByteProvider {
        private readonly DatElement _datelement;
        private bool _changed;

        public DatElementWrapper(DatElement datElement) {
            _datelement = datElement;
        }

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
            get { return _datelement.Length; }
        }

        public event EventHandler Changed;
        public event EventHandler LengthChanged;

        public unsafe byte ReadByte(long index) {
            return *(byte*)(_datelement.RootFile.Address + _datelement.FileOffset + (uint)index);
        }

        public System.Drawing.Color GetByteColor(long index) {
            var ele = SearchForDatElement(_datelement, _datelement.FileOffset + index);
            return ele != null ? ele.Color : System.Drawing.Color.Transparent;
        }

        private static DatElement SearchForDatElement(IEnumerable node, long index) {
            DatElement found = null;
            foreach (var searchableChild in node.OfType<IEnumerable>()) {
                found = SearchForDatElement(searchableChild, index); //Search child nodes
                if (found != null)
                    break; //If we find it, stop searching
            }
            if (found != null)
                return found; //Return the found result
            //If its not in any of the children, maybe this is it!
            var asDatElement = node as DatElement;
            if (asDatElement == null) //If its not a DatElement though, the search failed
                return null;
            return new DatElementWrapper(asDatElement).IsInDatElement(index)
                ? asDatElement //But if it is, and we are in there, we found it!
                : null;        //But if its not we failed


        }

        private bool IsInDatElement(long index) {
            return index >= _datelement.FileOffset && index < _datelement.FileOffset + Length;
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

        #region Scripting Usability Functions
        public unsafe int ReadInt(int offset) {
            return *(bint*)(_datelement.RootFile.Address + _datelement.FileOffset + offset);
        }

        public unsafe short ReadShort(int offset) {
            return *(bshort*)(_datelement.RootFile.Address + _datelement.FileOffset + offset);
        }

        public unsafe float ReadFloat(int offset) {
            return *(bfloat*)(_datelement.RootFile.Address + _datelement.FileOffset + offset);
        }
        #endregion
    }
}