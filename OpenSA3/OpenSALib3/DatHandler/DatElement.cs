using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace OpenSALib3.DatHandler {
    public abstract class DatElement : IEnumerable
    {
        #region Node Structure
        /* Every Node has a parent */
        [Browsable(false)]
        
        public DatElement Parent { get; private set; }
        [Browsable(false)]
        /* And a way to access the root file*/
        public virtual DatFile RootFile {
            get { return Parent is DatFile ? (DatFile)Parent : Parent.RootFile; }
        }
        /* Hidden list of children */
        protected IList Children = new List<IEnumerable>();
        #endregion
        /* Printable Path 
         TODO: Make it so that identical paths don't exist..i.e Hurtbox#0,Hurtbox# etc
         */
        [Browsable(true)]
        public String Path
        {
            get
            {
                if (Parent != null)
                    return Parent.Path + "/" + Name;
                return Name;
            }
        }
        /* Pointer to the beginning of this element */
        private readonly VoidPtr _address;
        [Browsable(false)]
        public virtual VoidPtr Address
        {
            get { return _address; }
        }
        /* Color for HexView Display */
        [Browsable(false)]
        public Color Color { get; set; }
        /* Name */
        [Category("Element")]
        [Browsable(true)]
        public string Name { get; protected set; }
        /* The offset inside the RootFile */
        [Category("Element")]
        [Browsable(true)]
        public uint FileOffset { get; private set; }
        /* The physical length of this element, NOT its childen */
        [Category("Element")]
        [ReadOnly(true)]
        public uint Length { get; internal set; }

        /* Used for random colors*/
        private static readonly Random Random = new Random();

        protected DatElement(DatElement parent, uint fileoffset) {
            Length = 4;
            Parent = parent;
            FileOffset = fileoffset;
            if(parent != null)/*This is so DatFiles can be instantiated */
                _address = RootFile.Address + FileOffset;
            Color = Color.FromArgb(255/2, Random.Next(255), Random.Next(255), Random.Next(255));
        }

        public override string ToString() {
            return Name ?? "NullName";
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Children.GetEnumerator();
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