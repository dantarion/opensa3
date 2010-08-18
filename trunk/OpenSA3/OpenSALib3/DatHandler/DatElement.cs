using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using OpenSALib3.Utility;
using System.Activities.Presentation.PropertyEditing;
namespace OpenSALib3.DatHandler
{
    public abstract class DatElement : IEnumerable
    {
        #region Node Structure
        /* Every Node has a parent */
        [Browsable(false)]

        public DatElement Parent { get; private set; }
        [Browsable(false)]
        /* And a way to access the root file*/
        public virtual DatFile RootFile
        {
            get { return Parent is DatFile ? (DatFile)Parent : Parent.RootFile; }
        }
        public IEnumerable this[string name]
        {
            get
            {
                if (name != null)
                    return _dictionary[name];
                return null;
            }
            set
            {
                if (name != null)
                    _dictionary[name] = value;
                else
                    _dictionary["_auto" + (i++)] = value;
            }
        }
        public IEnumerable this[int index]
        {
            get { return _dictionary["_indexed" + index]; }
            set { _dictionary["_indexed" + index] = value; }
        }
        private int i = 0;
        /* Hidden list of children */
        protected Dictionary<string, IEnumerable> _dictionary = new Dictionary<string, IEnumerable>();
        #endregion
        /* Printable Path 
         TODO: Make it so that identical paths don't exist..i.e Hurtbox#0,Hurtbox# etc
         */
        [Browsable(true)]
        [Category("Element")]
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
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int FileOffset { get; private set; }
        /* The physical length of this element, NOT its childen */
        [Category("Element")]
        [Browsable(true)]
        [ReadOnly(true)]
        public int Length { get; internal set; }

        /* Used for random colors*/
        private static readonly Random Random = new Random();

        protected DatElement(DatElement parent, int fileoffset)
        {
            Length = 4;
            Parent = parent;
            FileOffset = fileoffset;
            if (parent != null)/*This is so DatFiles can be instantiated */
                _address = RootFile.Address + FileOffset;
            Color = Color.FromArgb(255 / 2, Random.Next(255), Random.Next(255), Random.Next(255));
        }

        public override string ToString()
        {
            return Name ?? "NullName";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
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