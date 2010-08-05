using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
namespace OpenSALib3.DatHandler {
    abstract public class DatElement : IEnumerable {
        [Browsable(false)]
        public virtual DatFile RootFile { get { return Parent is DatFile ? Parent as DatFile : Parent.RootFile; } }
        [Browsable(false)]
        public DatElement Parent { get; set; }

        public String Name { get; set; }

        public override string ToString()
        {
            return Name != null ? Name :"NullName" ;
        }
        public uint FileOffset { get; protected set; }
        public uint Length { get; set; }
        public Color Color { get; set; }
        private static Random rnd = new Random();
        protected DatElement(DatElement parent,uint fileoffset) {
            Parent = parent;
            FileOffset = fileoffset;
            Color = Color.FromArgb(255/2,rnd.Next(255), rnd.Next(255), rnd.Next(255));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public virtual IEnumerator GetEnumerator()
        {
            return new List<object>().GetEnumerator();
        }
    }
}
