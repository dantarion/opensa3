using System;
using System.ComponentModel;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset {
    public sealed class BoneRef : DatElement {
        private bint _index;
        [Browsable(true), Category("BoneRef")]
        public int BoneIndex
        {
            get { return _index; }
            set { _index = value; NotifyChanged("BoneIndex"); NotifyChanged("Name"); }
        }
        [Browsable(true),Category("BoneRef")]
        public string BoneName {
            get { return RootFile.GetBoneName(BoneIndex); }
        }

        public unsafe BoneRef(DatElement parent, int offset, String name)
            : base(parent, offset) {
            TreeColor = null;
            base.Name = name;
            _index = *(bint*) (RootFile.Address + offset);
        }

        public override String Name {
            get
            {
                return base.Name + ": " + BoneName;
            }
        }
    }
}