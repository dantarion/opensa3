﻿using System;
using OpenSALib3.DatHandler;
using System.ComponentModel;
using System.Activities.Presentation.PropertyEditing;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset {
    public sealed class UnknownType1 : DatElement {
        private struct Data {
            public bfloat Float1;
            public bfloat Float2;
            public bfloat Float3;
            public bfloat Float4;
            public bfloat Float5;
            public bfloat Float6;
            public bfloat Float7;
            public int Flags;
        }

        public float Float1 {
            get { return _data.Float1; }
            set { _data.Float1 = value; }
        }

        public float Float2 {
            get { return _data.Float2; }
            set { _data.Float2 = value; }
        }

        public float Float3 {
            get { return _data.Float3; }
            set { _data.Float3 = value; }
        }

        public float Float4 {
            get { return _data.Float4; }
            set { _data.Float4 = value; }
        }

        public float Float5 {
            get { return _data.Float5; }
            set { _data.Float5 = value; }
        }

        public float Float6 {
            get { return _data.Float6; }
            set { _data.Float6 = value; }
        }

        public float Float7 {
            get { return _data.Float7; }
            set { _data.Float7 = value; }
        }
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int Flags {
            get { return _data.Flags; }
            set { _data.Flags = value; }
        }

        private Data _data;

        public unsafe UnknownType1(DatElement parent, int offset)
            : base(parent, offset) {
            Name = "UnknownType1";
            Length = 4 * 8;
            _data = *(Data*) (RootFile.Address + offset);
        }
    }
}