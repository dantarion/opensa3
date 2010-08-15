using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;
using System.Runtime.InteropServices;
using OpenSALib3.PSA;
namespace OpenSALib3.Moveset
{
    public sealed class MovesetSection : DatSection
    {
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

        private struct MovesetHeader
        {
            public buint SubactionFlagsStart;
            public buint Unknown1;//UnknownB
            public buint AttributeStart;
            public buint SSEAttributeStart;
            public buint MiscSectionOffset;
            public buint Unknown5Start;//UnknownA
            public buint Unknown6Start;//UnknownC
            public buint Unknown7;
            public buint Unknown8;
            public buint ActionsStart;
            public buint Actions2Start;
            public buint Unknown11;
            public buint SubactionMainStart;
            public buint SubactionGFXStart;
            public buint SubactionSFXStart;
            public buint SubactionOtherStart;
            public buint Unknown16;
            public buint Unknown17;
            public buint Unknown18;
            public buint Unknown19;
            public buint Unknown20;
            public buint Unknown21;
            public buint Unknown22;//Bone References?
            public buint Unknown23;
            public buint Unknown24;
            public buint Unknown25;
            public buint Unknown26;//Entry Article
            public buint Unknown27;
        }

        private readonly MovesetHeader _header;
        private readonly List<Attribute> _attributes = new List<Attribute>();

        [Browsable(false)]
        public List<Attribute> Attributes
        {
            get { return _attributes; }
        }

        private readonly List<Attribute> _sseattributes = new List<Attribute>();

        [Browsable(false)]
        public List<Attribute> SSEAttributes
        {
            get { return _sseattributes; }
        }

        private readonly MiscSection _miscsection;

        [Browsable(false)]
        public MiscSection MiscSection
        {
            get { return _miscsection; }
        }
        private List<UnknownElement> _unknowna = new List<UnknownElement>();
        private List<UnknownElement> _unknownb = new List<UnknownElement>();
        private List<UnknownElement> _unknownc = new List<UnknownElement>();
        private List<ActionOverride> _overrides = new List<ActionOverride>();
        private List<ActionOverride> _overrides2 = new List<ActionOverride>();
        private List<CommandList> _actions = new List<CommandList>();
        private List<CommandList> _actions2 = new List<CommandList>();
        private List<SubactionFlags> _subactionflags = new List<SubactionFlags>();
        private List<CommandList> _subactionmain = new List<CommandList>();
        private List<CommandList> _subactiongfx = new List<CommandList>();
        private List<CommandList> _subactionsfx = new List<CommandList>();
        private List<CommandList> _subactionother = new List<CommandList>();
        private Dictionary<int, List<Command>> _subroutines = new Dictionary<int, List<Command>>();
        public unsafe MovesetSection(DatElement parent, uint offset, VoidPtr stringPtr)
            : base(parent, offset, stringPtr)
        {
            _header = *(MovesetHeader*)(RootFile.Address + DataOffset);
            _miscsection = new MiscSection(this, _header.MiscSectionOffset);
            for (uint i = _header.AttributeStart; i < _header.SSEAttributeStart; i += 4)
                _attributes.Add(new Attribute(this, i));
            for (uint i = _header.SSEAttributeStart; i < _header.Unknown5Start; i += 4)
                _sseattributes.Add(new Attribute(this, i));

            for (uint i = _header.Unknown5Start; i < _header.Unknown7; i += 16)
                _unknowna.Add(new UnknownElement(this, i, "UnknownA", 16));
            for (uint i = _header.Unknown1; i < _header.Unknown19; i += 8)
                _unknownb.Add(new UnknownElement(this, i, "UnknownB", 8));
            for (uint i = _header.Unknown6Start; i < _header.ActionsStart; i += 16)
                _unknownc.Add(new UnknownElement(this, i, "UnknownC", 16));
            var unknownd = new UnknownElement(this, _header.Unknown8, "UnknownD", 8);
            //PSA Stuffs
            if (_header.Unknown20 != 0)
            {
                var o = _header.Unknown20;
                var ao = new ActionOverride(this, o);
                while (ao.SubactionID > 0)
                {
                    _overrides.Add(ao);
                    o += 8;
                    ao = new ActionOverride(this, o);
                }
                _overrides.Add(ao);
            }
            if (_header.Unknown21 != 0)
            {
                var o = _header.Unknown21;
                var ao = new ActionOverride(this, o);
                while (ao.SubactionID > 0)
                {
                    _overrides2.Add(ao);
                    o += 8;
                    ao = new ActionOverride(this, o);
                }
                _overrides2.Add(ao);
            }
            for (uint i = _header.ActionsStart; i < _header.Actions2Start; i += 4)
                _actions.Add(new CommandList(this, i, _subroutines));
            for (uint i = _header.Actions2Start; i < _header.Unknown11; i += 4)
                _actions2.Add(new CommandList(this, i, _subroutines));
            for (uint i = _header.SubactionFlagsStart; i < _header.SubactionMainStart; i += 8)
                _subactionflags.Add(new SubactionFlags(this, i));
            for (uint i = _header.SubactionMainStart; i < _header.SubactionGFXStart; i += 4)
                _subactionmain.Add(new CommandList(this, i, _subroutines));
            for (uint i = _header.SubactionGFXStart; i < _header.SubactionSFXStart; i += 4)
                _subactiongfx.Add(new CommandList(this, i, _subroutines));
            for (uint i = _header.SubactionSFXStart; i < _header.SubactionOtherStart; i += 4)
                _subactionsfx.Add(new CommandList(this, i, _subroutines));
            for (uint i = _header.SubactionOtherStart; i < _header.SubactionOtherStart + _subactiongfx.Count * 4; i += 4)
                _subactionother.Add(new CommandList(this, i, _subroutines));

            //Setup Tree Structure
            _children.Add(new NamedList(Attributes, "Attributes"));
            _children.Add(new NamedList(SSEAttributes, "SSE Attributes"));
            _children.Add(new NamedList(_unknowna, "UnknownA(Action Flag?)"));
            _children.Add(new NamedList(_unknownb, "UnknownB(Action Flag?)"));
            _children.Add(new NamedList(_unknownc, "UnknownC(Action?)"));
            _children.Add(unknownd);
            _children.Add(MiscSection);
            _children.Add(new NamedList(_overrides, "Overrides"));
            _children.Add(new NamedList(_overrides2, "Overrides2"));
            _children.Add(new NamedList(_actions, "Actions"));
            _children.Add(new NamedList(_actions2, "Actions2?"));
            var subactions = new List<IEnumerable>();
            for (int i = 0; i < _subactionflags.Count; i++)
            {
                var subaction = new List<IEnumerable>();
                subaction.Add(new NamedList(_subactionflags[i], "Flags"));
                subaction.Add(new NamedList(_subactionmain[i], "Main"));
                subaction.Add(new NamedList(_subactiongfx[i], "GFX"));
                subaction.Add(new NamedList(_subactionsfx[i], "SFX"));
                subaction.Add(new NamedList(_subactionother[i], "Other"));
                subactions.Add(new NamedList(subaction, string.Format("{0:x03}", i)));
            }
            _children.Add(new NamedList(subactions, "Subactions"));
            _children.Add(new NamedList(_subroutines.Values, "Subroutines"));
        }

    }

}