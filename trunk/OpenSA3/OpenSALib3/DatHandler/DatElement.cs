using System;
using System.Activities.Presentation.PropertyEditing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using OpenSALib3.Utility;

namespace OpenSALib3.DatHandler
{
    public abstract class DatElement : IEnumerable, INotifyPropertyChanged
    {
        #region Node Structure
        /* Every Node has a parent */
        [Browsable(false)]
        public DatElement Parent { get; private set; }
        [Browsable(false)]
        /* And a way to access the root file*/
        public DatFile RootFile
        {
            get {
                if (this is DatFile) return (DatFile) this;
                return Parent.RootFile;
            }
        }
        private int _autoIndex;
        public IEnumerable this[string name]
        {
            get
            {
                if (name != null)
                if (Dictionary.ContainsKey(name))
                    return Dictionary[name];
                return null;
            }
            set
            {
                if (name != null)
                    Dictionary[name] = value;
                else
                    Dictionary["_auto" + (_autoIndex++)] = value;
            }
        }
        public IEnumerable this[int index]
        {
            get { var key = "_indexed" + index; 
                return Dictionary.ContainsKey(key) ? Dictionary[key] : null;
            }
            set { Dictionary["_indexed" + index] = value; }
        }
        public void AddNamedList(INamed list)
        {
            this[list.Name] = list;
        }
        /* Hidden list of children */
        protected Dictionary<string, IEnumerable> Dictionary = new Dictionary<string, IEnumerable>();
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
        public string Name { get; internal set; }
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
            //PropertyChanged += new PropertyChangedEventHandler(MarkDirty);
        }
        [ReadOnly(true), Browsable(true)]
        private bool _ischanged;
        public bool IsChanged
        {
            get { return _ischanged; }
            private set {
                _ischanged = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChanged"));
            }
        }
        public void MarkClean()
        {
            _ischanged = false;
            foreach (IEnumerable el in this)
            {
                if (el is DatElement)
                    ((DatElement)el).MarkClean();
                el._ischanged = false;
            }

        }
        public void MarkDirty(object sender, PropertyChangedEventArgs e)
        {
            if(Parent != null)
                Parent.MarkDirty(sender,e);
            IsChanged = true;
        }
        public override string ToString()
        {
            return Name ?? "NullName";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dictionary.Values.GetEnumerator();
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}