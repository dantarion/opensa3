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
            return Name;
        }
        public uint FileOffset { get; protected set; }
        public uint Length { get; set; }
        public Color Color { get; set; }
        protected DatElement(DatElement parent) {
            Parent = parent;
            var rnd = new Random();
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
