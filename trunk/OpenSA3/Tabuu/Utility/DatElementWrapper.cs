using System;
using System.Collections.Generic;
using System.Collections;
using BrawlLib.SSBB.ResourceNodes;
using OpenSALib3.DatHandler;

namespace Tabuu.Utility
{
    public class DatElementWrapper : Be.Windows.Forms.IByteProvider
    {

        private DatElement _datelement;
        bool changed = false;
        public DatElementWrapper(DatElement datElement)
        {
            _datelement = datElement;
        }
        #region IByteProvider
        public void ApplyChanges()
        {
        }
        public void DeleteBytes(long index, long length)
        {
            throw new NotImplementedException();
        }

        public bool HasChanges()
        {
            return changed;
        }

        public void InsertBytes(long index, byte[] bs)
        {
            throw new NotImplementedException();
        }

        public long Length
        {
            get { return _datelement.Length; }
        }

        public event EventHandler Changed;
        public event EventHandler LengthChanged;

        public unsafe byte ReadByte(long index)
        {
            return *(byte*)(_datelement.RootFile.Address + _datelement.FileOffset + (uint)index);
        }
        public System.Drawing.Color GetByteColor(long index)
        {
            DatFile file = _datelement as DatFile;
            if (file != null)
                foreach (DatSection section in file.Sections)
                    if (index >= section.FileOffset && index < section.FileOffset + section.Length)
                        return section.Color;
            return System.Drawing.Color.Transparent;
        }
        public bool SupportsDeleteBytes()
        {
            return false;
        }

        public bool SupportsInsertBytes()
        {
            return false;
        }

        public bool SupportsWriteByte()
        {
            return true;
        }

        public unsafe void WriteByte(long index, byte value)
        {
            byte* src = (byte*)(_datelement.RootFile.Address + _datelement.FileOffset + (uint)index);
            (*src) = value;
            changed = true;
            _datelement.RootFile.Changed = true;
        }
        #endregion
        #region Scripting Usability Functions
        public unsafe int ReadInt(int offset)
        {
            return *(bint*)(_datelement.RootFile.Address + _datelement.FileOffset + offset);
        }
        public unsafe short ReadShort(int offset)
        {
            return *(bshort*)(_datelement.RootFile.Address + _datelement.FileOffset + offset);
        }
        public unsafe float ReadFloat(int offset)
        {
            return *(bfloat*)(_datelement.RootFile.Address + _datelement.FileOffset + offset);
        }
        #endregion
    }



}
