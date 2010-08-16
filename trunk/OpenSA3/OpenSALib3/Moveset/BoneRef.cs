using System;
using System.ComponentModel;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset {
    public sealed class BoneRef : DatElement {
        private bint _index;

        public int BoneIndex
        {
            get { return _index; }
            set { _index = value; }
        }
        [Browsable(true)]
        public string BoneName {
            get { return RootFile.GetBoneName(BoneIndex); }
        }

        public unsafe BoneRef(DatElement parent, int offset, String name)
            : base(parent, offset) {

            Name = name;
            _index = *(bint*) (RootFile.Address + offset);
        }

        public override string ToString() {
            return Name + ": " + BoneName;
        }
    }
}