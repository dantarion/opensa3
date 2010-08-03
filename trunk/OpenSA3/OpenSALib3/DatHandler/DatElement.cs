using System.ComponentModel;

namespace OpenSALib3.DatHandler {
    public class FileSpan {
        public int StartOffset;
        public int Length;
    }
    abstract public class DatElement {
        [Browsable(false)]
        public DatFile RootFile { get { return Parent is DatFile ? Parent as DatFile : Parent.RootFile; } }
        [Browsable(false)]
        public DatElement Parent { get; set; }

        [Browsable(false)]
        public FileSpan FileSpan { get; set; }

        protected DatElement(DatElement parent) {
            Parent = parent;
            FileSpan = new FileSpan();
        }
    }
}
