﻿using System;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset {
    public sealed class BoneRef : DatElement {
        private bint _index;

        public int Index {
            get { return _index; }
            set { _index = value; }
        }

        public string BoneName {
            get { return RootFile.GetBoneName(Index); }
        }

        public unsafe BoneRef(DatElement parent, uint offset, String name)
            : base(parent, offset) {
            Name = name;
            _index = *(bint*) (RootFile.Address + offset);
        }

        public override string ToString() {
            return Name + ": " + BoneName;
        }
    }
}