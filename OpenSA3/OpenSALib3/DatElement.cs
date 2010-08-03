using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace OpenSALib3
{
    public class FileSpan
    {
        public int StartOffset;
        public int Length;
    }
    abstract public class DatElement
    {
        [Browsable(false)]
        public DatFile RootFile {
            get{if(Parent is DatFile) return Parent as DatFile; return Parent.RootFile;
        }}
        [Browsable(false)]
        public DatElement Parent { get;set;}
        private FileSpan filespan;
        [Browsable(false)]
        public FileSpan FileSpan { get; set; }
        public DatElement(DatElement parent)
        {
            this.Parent = parent;
            this.FileSpan = new FileSpan();
        }


    }
}
