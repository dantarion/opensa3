﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using System.ComponentModel;
namespace OpenSALib3.Moveset
{
    public class Hurtbox : DatElement
    {
        unsafe struct HurtboxData
        {
            public bint boneindex;
            public bfloat X;
            public bfloat Y;
            public bfloat Width;
            public bfloat Height;
        }
        private HurtboxData _data;
        [Category("Hurtbox")]
        public int BoneIndex
        {
            get { return _data.boneindex; }
            set { _data.boneindex = value; }
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
        public string BoneName
        {
            get { return RootFile.getBoneName(BoneIndex); }
        }
        public unsafe Hurtbox(DatElement parent, uint offset)
            : base(parent, offset)
        {
             Name = "Hurtbox";
            _data = *(HurtboxData*)(RootFile.Address + offset);
        }
    }
}
