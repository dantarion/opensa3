using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{
    public class BoneRef : DatElement
    {
        unsafe struct BoneData
        {
            public bint boneindex;
        }
        private BoneData _boneData;
        public int Index
        {
            get { return _boneData.boneindex; } set{ _boneData.boneindex = value; }
        }
        public string BoneName
        {
            get { return RootFile.getBoneName(Index); }
        }
        public unsafe BoneRef(DatElement parent, uint offset, String name)
            : base(parent, offset)
        {
             Name = name;
            _boneData = *(BoneData*)(RootFile.Address + offset);
        }
        public override string ToString()
        {
            return Name + ": " + BoneName;
        }
    }
}
