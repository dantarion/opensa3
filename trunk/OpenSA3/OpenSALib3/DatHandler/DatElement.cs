using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace OpenSALib3.DatHandler {
    public abstract class DatElement : IEnumerable {
        [Browsable(false)]
        public virtual DatFile RootFile {
            get { return Parent is DatFile ? (DatFile)Parent : Parent.RootFile; }
        }
        private VoidPtr _address;
        [Browsable(false)]
        public virtual VoidPtr Address
        {
            get { return _address; }
        }
        [Browsable(false)]
        public DatElement Parent { get; set; }

        [Browsable(true)]
        public String Path
        {
            get
            {
                if(Parent != null)
                    return Parent.Path + "/" + Name;
                return Name;
            }
        }

        [Browsable(false)]
        public Color Color { get; set; }

        [Category("Element")]
        [Browsable(true)]
        public string Name { get; protected set; }

        [Category("Element")]
        [Browsable(true)]
        public uint FileOffset { get; protected set; }

        [Category("Element")]
        [ReadOnly(true)]
        public uint Length { get; set; }

        private static readonly Random Random = new Random();

        protected DatElement(DatElement parent, uint fileoffset) {
            Length = 4;
            Parent = parent;
            FileOffset = fileoffset;
            _address = RootFile.Address + FileOffset;
            Color = Color.FromArgb(255/2, Random.Next(255), Random.Next(255), Random.Next(255));
        }

        public override string ToString() {
            return Name ?? "NullName";
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public virtual IEnumerator GetEnumerator() {
            return new List<object>().GetEnumerator();
        }
        #region ScriptingFunctions
        public unsafe byte ReadByte(int offset)
        {
            return *(byte*)(RootFile.Address + FileOffset + offset);
        }
        public unsafe short ReadShort(int offset)
        {
            return *(bshort*)(RootFile.Address + FileOffset + offset);
        }
        public unsafe int ReadInt(int offset)
        {
            return *(bint*)(RootFile.Address + FileOffset + offset);
        }
        public unsafe float ReadFloat(int offset)
        {
            return *(bfloat*)(RootFile.Address + FileOffset + offset);
        }
        #endregion
    }
}