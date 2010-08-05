using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using System.Collections;
using OpenSALib3.Utility;
namespace OpenSALib3.Moveset
{
    public class MiscSection : DatElement
    {
        unsafe struct Header
        {
            public buint Section1;
            public buint HurtBoxOffset;
            public buint HurtBoxCount;
            public buint UnknownSectionOffset;
            public buint UnknownSectionCount;
            public buint LedgegrabOffset;
            public buint LedgegrabCount;
            public buint UnknownSection2Offset;
            public buint UnknownSection2Count;
            public buint BoneRef2Offset;
            public buint UnknownSection3Offset;
            /* TODO: Finish this section. should be 20 entries here */
        }
        private Header _header;
        private List<int> _section1 = new List<int>();
        public List<int> Section { get { return _section1; } }
        private List<Hurtbox> _hurtboxes = new List<Hurtbox>();
        public List<Hurtbox> Hurtboxes { get { return _hurtboxes; } }

        unsafe public MiscSection(DatElement parent, uint fileoffset)
            : base(parent, fileoffset)
        {
            Length = 4 * 20;
            Name = "Misc";
            _header = *(Header*)(RootFile.Address + FileOffset);
            for (uint i = 0; i < 0x0A; i++)
                _section1.Add(*(bint*)(RootFile.Address + _header.Section1 + i * 4));
            for (uint i = 0; i < +_header.HurtBoxCount; i++)
                _hurtboxes.Add(new Hurtbox(this,_header.HurtBoxOffset + i * 4 * 5));

        }
        public override IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        class Enumerator : IEnumerator
        {
            private readonly MiscSection _file;
            private int _i = -1;
            public Enumerator(MiscSection f)
            {
                _file = f;
            }

            public object Current
            {
                get
                {
                    switch (_i)
                    {
                        case 0:
                            return _file._header;
                        case 1:
                            return new NamedList<int>(_file._section1, "Section1");
                        case 2:
                            return new NamedList<Hurtbox>(_file._hurtboxes, "Hurtboxes");

                    }
                    return null;
                }
            }

            public bool MoveNext()
            {
                _i++;
                return _i <= 2;
            }

            public void Reset()
            {
                _i = -1;
            }
        }
    }
}
