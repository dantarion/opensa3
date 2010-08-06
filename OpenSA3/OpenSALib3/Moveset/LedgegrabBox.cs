using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using System.ComponentModel;
namespace OpenSALib3.Moveset
{
    public class LedgegrabBox : DatElement
    {
        struct Data
        {
            public bfloat X;
            public bfloat Y;
            public bfloat Width;
            public bfloat Height;
        }
        private Data _data;
        [Category("LedgegrabBox")]
        public float X
        {
            get { return _data.X; }
            set { _data.X = value; }
        }
        [Category("LedgegrabBox")]
        public float Y
        {
            get { return _data.Y; }
            set { _data.Y = value; }
        }
        [Category("LedgegrabBox")]
        public float Width
        {
            get { return _data.Width; }
            set { _data.Width = value; }
        }
        [Category("LedgegrabBox")]
        public float Height
        {
            get { return _data.Height; }
            set { _data.Height = value; }
        }
        public unsafe LedgegrabBox(DatElement parent, uint offset)
            : base(parent, offset)
        {
             Name = "LedgegrabBox";
            _data = *(Data*)(RootFile.Address + offset);
        }
    }
}
