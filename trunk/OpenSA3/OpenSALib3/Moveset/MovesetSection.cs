using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset {
    public sealed class MovesetSection : DatSection {
        /* TODO
         private const int SUBACTION_FLAG_START = 0;
        private const int SOMETHING = 1;
        private const int ATTRIBUTE_START = 2;
        private const int SSE_ATTRIBUTE_START = 3;
        private const int ATTRIBUTE2_START = 4;
        private const int ACTION_OFFSETS_START = 9;
        private const int HIDDEN_ACTION_START = 10;
        private const int HIDDEN_ACTION_END = 11;
        private const int SUBACTION_MAIN_OFFSETS_START = 12;
        private const int SUBACTION_GFX_OFFSETS_START = 13;
        private const int SUBACTION_SFX_OFFSETS_START = 14;
        private const int SUBACTION_OTHER_OFFSETS_START = 15;
         */

        private struct MovesetHeader {
            public buint SubactionFlagsStart;
            public buint Unknown1;
            public buint AttributeStart;
            public buint SSEAttributeStart;
            public buint MiscSectionOffset;
            public buint Unknown5;
            public buint Unknown6;
            public buint Unknown7;
            public buint Unknown8;
            public buint ActionsStart;
            public buint Actions2Start;
            public buint Unknown11;
            public buint SubactionMainStart;
            public buint SubactionGFXStart;
            public buint SubactionSFXStart;
            public buint SubactionOtherStart;
        }

        private readonly MovesetHeader _header;
        private readonly List<Attribute> _attributes = new List<Attribute>();

        [Browsable(false)]
        public List<Attribute> Attributes {
            get { return _attributes; }
        }

        private readonly List<Attribute> _sseattributes = new List<Attribute>();

        [Browsable(false)]
        public List<Attribute> SSEAttributes {
            get { return _sseattributes; }
        }

        private readonly MiscSection _miscsection;

        [Browsable(false)]
        public MiscSection MiscSection {
            get { return _miscsection; }
        }

        public unsafe MovesetSection(VoidPtr ptr, VoidPtr stringPtr, DatElement parent)
            : base(ptr, stringPtr, parent) {
            _header = *(MovesetHeader*) (RootFile.Address + FileOffset);
            _miscsection = new MiscSection(this, _header.MiscSectionOffset);
            for (uint i = _header.AttributeStart; i < _header.SSEAttributeStart; i += 4)
                _attributes.Add(new Attribute(this, i));
            for (uint i = _header.SSEAttributeStart; i < _header.Unknown5; i += 4)
                _sseattributes.Add(new Attribute(this, i));
        }

        public override IEnumerator GetEnumerator() {
            return new MovesetSectionEnumerator(this);
        }
    }

    internal class MovesetSectionEnumerator : IEnumerator {
        private readonly MovesetSection _file;
        private int _i = -1;

        public MovesetSectionEnumerator(MovesetSection f) {
            _file = f;
        }

        public object Current {
            get {
                switch (_i) {
                    case 0:
                        return new NamedList<Attribute>(_file.Attributes, "Attributes");
                    case 1:
                        return new NamedList<Attribute>(_file.SSEAttributes, "SSE Attributes");
                    case 2:
                        return _file.MiscSection;
                }
                return null;
            }
        }

        public bool MoveNext() {
            _i++;
            return _i <= 2;
        }

        public void Reset() {
            _i = -1;
        }
    }
}