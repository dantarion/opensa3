using System;
using System.ComponentModel;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{
    public sealed class Hurtbox : DatElement
    {
        private struct HurtboxData
        {
            public bint BoneIndex;
            public bfloat X;
            public bfloat Y;
            public bfloat Width;
            public bfloat Height;
        }

        private HurtboxData _data;

        [Category("Hurtbox")]
        public int BoneIndex
        {
            get { return _data.BoneIndex; }
            set { _data.BoneIndex = value; }
        }

        [Category("Hurtbox")]
        public float X
        {
            get { return _data.X; }
            set { _data.X = value; }
        }

        [Category("Hurtbox")]
        public float Y
        {
            get { return _data.Y; }
            set { _data.Y = value; }
        }

        [Category("Hurtbox")]
        public float Width
        {
            get { return _data.Width; }
            set { _data.Width = value; }
        }

        [Category("Hurtbox")]
        public float Height
        {
            get { return _data.Height; }
            set { _data.Height = value; }
        }

        [Category("Hurtbox")]
        [Browsable(true)]
        public string BoneName
        {
            get { return RootFile.GetBoneName(BoneIndex); }
        }

        public unsafe Hurtbox(DatElement parent, uint offset)
            : base(parent, offset)
        {
            Length = 5 * 4;
            Name = "Hurtbox";
            _data = *(HurtboxData*)(RootFile.Address + offset);
        }
    }
}