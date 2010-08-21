using System;
using System.ComponentModel;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset {
    public sealed class LedgegrabBox : DatElement {
        private struct Data {
            public bfloat X;
            public bfloat Y;
            public bfloat Width;
            public bfloat Height;
        }

        private Data _data;

        [Category("LedgegrabBox")]
        public float X {
            get { return _data.X; }
            set { _data.X = value; NotifyChanged("X"); }
        }

        [Category("LedgegrabBox")]
        public float Y {
            get { return _data.Y; }
            set { _data.Y = value; NotifyChanged("Y"); }
        }

        [Category("LedgegrabBox")]
        public float Width {
            get { return _data.Width; }
            set { _data.Width = value; NotifyChanged("Width"); }
        }

        [Category("LedgegrabBox")]
        public float Height {
            get { return _data.Height; }
            set { _data.Height = value; NotifyChanged("Height"); }
        }

        public unsafe LedgegrabBox(DatElement parent, int offset)
            : base(parent, offset) {
                Length = 4 * 4;
                TreeColor = null;
            Name = "LedgegrabBox";
            _data = *(Data*) (Address);
        }
    }
}