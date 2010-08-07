using System;
using System.Collections;
using System.Collections.Generic;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset {
    public sealed class MiscSection : DatElement {
        private struct Header {
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
            //TODO: Finish this section. should be 20 entries here
        }

        private readonly Header _header;
        private readonly List<int> _section1 = new List<int>();

        public List<int> Section {
            get { return _section1; }
        }

        private readonly List<Hurtbox> _hurtboxes = new List<Hurtbox>();

        public List<Hurtbox> Hurtboxes {
            get { return _hurtboxes; }
        }

        private readonly List<UnknownType1> _unknowntype1List = new List<UnknownType1>();

        public List<UnknownType1> UnknownType1List {
            get { return _unknowntype1List; }
        }

        private readonly List<LedgegrabBox> _ledgegrabboxes = new List<LedgegrabBox>();

        public List<LedgegrabBox> LedgegrabBoxes {
            get { return _ledgegrabboxes; }
        }

        public unsafe MiscSection(DatElement parent, uint fileoffset)
            : base(parent, fileoffset) {
            Length = 4*20;
            Name = "Misc";
            _header = *(Header*) (RootFile.Address + FileOffset);
            for (uint i = 0; i < 0x0A; i++)
                _section1.Add(*(bint*) (RootFile.Address + _header.Section1 + i*4));
            for (uint i = 0; i < +_header.HurtBoxCount; i++)
                _hurtboxes.Add(new Hurtbox(this, _header.HurtBoxOffset + i*4*5));
            for (uint i = 0; i < +_header.UnknownSectionCount; i++)
                _unknowntype1List.Add(new UnknownType1(this, _header.UnknownSectionOffset + i*4*8));
            for (uint i = 0; i < +_header.LedgegrabCount; i++)
                _ledgegrabboxes.Add(new LedgegrabBox(this, _header.LedgegrabOffset + i*4*4));
        }

        public override IEnumerator GetEnumerator() {
            return new Enumerator(this);
        }

        private class Enumerator : IEnumerator {
            private readonly MiscSection _file;
            private int _i = -1;

            public Enumerator(MiscSection f) {
                _file = f;
            }

            public object Current {
                get {
                    switch (_i) {
                        case 0:
                            return _file._header;
                        case 1:
                            return new NamedList<int>(_file._section1, "Section1");
                        case 2:
                            return new NamedList<Hurtbox>(_file._hurtboxes, "Hurtboxes");
                        case 3:
                            return new NamedList<UnknownType1>(_file._unknowntype1List, "UnknownType1");
                        case 4:
                            return new NamedList<LedgegrabBox>(_file._ledgegrabboxes, "LedgegrabBoxes");
                    }
                    return null;
                }
            }

            public bool MoveNext() {
                _i++;
                return _i <= 4;
            }

            public void Reset() {
                _i = -1;
            }
        }
    }
}