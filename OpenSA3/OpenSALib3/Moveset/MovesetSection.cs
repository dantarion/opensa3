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
            public bint SubactionFlagsStart;
            public bint Unknown1;//UnknownB
            public bint AttributeStart;
            public bint SSEAttributeStart;
            public bint MiscSectionOffset;
            public bint Unknown5Start;//UnknownA
            public bint Unknown6Start;//UnknownC
            public bint Unknown7;
            public bint Unknown8;
            public bint ActionsStart;
            public bint Actions2Start;
            public bint Unknown11;
            public bint SubactionMainStart;
            public bint SubactionGFXStart;
            public bint SubactionSFXStart;
            public bint SubactionOtherStart;
            public bint Unknown16;
            public bint Unknown17;
            public bint Unknown18;
            public bint Unknown19;
            public bint Unknown20;
            public bint Unknown21;
            public bint Unknown22;//Bone References?
            public bint Unknown23;
            public bint Unknown24;
            public bint Unknown25;
            public bint Unknown26;//Entry Article
            public bint Unknown27;
            public bint Unknown28;
            public bint Unknown29;
            public bint Unknown30;
            public bint Unknown31;
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

        private MiscSection _miscsection;

        [Browsable(false)]
        public MiscSection MiscSection
        {
            get { return _miscsection; }
        }
        private List<UnknownElement> _unknowna = new List<UnknownElement>();
        private List<UnknownElement> _unknownb = new List<UnknownElement>();
        private List<UnknownElement> _unknownc = new List<UnknownElement>();
        private List<UnknownElement> _unknowne = new List<UnknownElement>();
        private List<CommandList> _actions = new List<CommandList>();
        private List<CommandList> _actions2 = new List<CommandList>();
        private List<SubactionFlags> _subactionflags = new List<SubactionFlags>();
        private List<CommandList> _subactionmain = new List<CommandList>();
        private List<CommandList> _subactiongfx = new List<CommandList>();
        private List<CommandList> _subactionsfx = new List<CommandList>();
        private List<CommandList> _subactionother = new List<CommandList>();
        private List<Article> _articles = new List<Article>();
        private List<List<Command>> _subroutines = new List<List<Command>>();
        public unsafe MovesetSection(DatElement parent, int offset, VoidPtr stringPtr)
            : base(parent, offset, stringPtr)
        {
            _header = *(MovesetHeader*)(RootFile.Address + DataOffset);

        }
        public override void Parse()
        {
            _miscsection = new MiscSection(this, _header.MiscSectionOffset);
            for (int i = _header.AttributeStart; i < _header.SSEAttributeStart; i += 4)
                _attributes.Add(new Attribute(this, i));
            for (int i = _header.SSEAttributeStart; i < _header.Unknown5Start; i += 4)
                _sseattributes.Add(new Attribute(this, i));

            for (int i = _header.Unknown5Start; i < _header.Unknown7; i += 16)
                _unknowna.Add(new UnknownElement(this, i, "UnknownA", 16));
            for (int i = _header.Unknown6Start; i < _header.ActionsStart; i += 16)
                _unknownc.Add(new UnknownElement(this, i, "UnknownC", 16));
            for (int i = _header.Unknown7; i < _header.Unknown7 + 8 * (_unknowna.Count+_unknownc.Count); i += 8)
                _unknowne.Add(new UnknownElement(this, i, "UnknownE", 8));
            for (int i = _header.Unknown1; i < _header.Unknown19; i += 8)
                _unknownb.Add(new UnknownElement(this, i, "UnknownB", 8));

            var unknownd = new UnknownElement(this, _header.Unknown8, "UnknownD", 8);
            int count = 0x112;
            for (int i = _header.ActionsStart; i < _header.Actions2Start; i += 4)
                _actions.Add(new CommandList(this, i, "Action " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _header.Actions2Start; i < _header.Unknown11; i += 4)
                _actions2.Add(new CommandList(this, i, "Action2 " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _header.SubactionFlagsStart; i < _header.SubactionMainStart; i += 8)
                _subactionflags.Add(new SubactionFlags(this, i));
            var stringChunk = new UnknownElement(this, _subactionflags[0].AnimationStringOffset, "SubactionStrings", _header.SubactionFlagsStart - _subactionflags[0].AnimationStringOffset);
            count = 0;
            for (int i = _header.SubactionMainStart; i < _header.SubactionGFXStart; i += 4)
                _subactionmain.Add(new CommandList(this, i, "Subaction Main " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _header.SubactionGFXStart; i < _header.SubactionSFXStart; i += 4)
                _subactiongfx.Add(new CommandList(this, i, "Subaction GFX " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _header.SubactionSFXStart; i < _header.SubactionOtherStart; i += 4)
                _subactionsfx.Add(new CommandList(this, i, "Subaction SFX " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _header.SubactionOtherStart; i < _header.SubactionOtherStart + _subactiongfx.Count * 4; i += 4)
                _subactionother.Add(new CommandList(this, i, "Subaction Other " + String.Format("0x{0:X}", count++), _subroutines));

            if (_header.Unknown26 > 0)
                _articles.Add(new Article(this, _header.Unknown26));
            for (int i = DataOffset + 36 * 4; i < DataOffset + DataLength; i += 4)
            {
                Article art = null;
                try
                {
                    //art = new Article(this, RootFile.ReadInt(i));
                    //_articles.Add(art);
                }
                catch (Exception err)
                {
                }

            }
            List<UnknownElement> unknownV = new List<UnknownElement>();
            for(int i = _header.Unknown16; i < _header.Unknown18; i +=0x1c)
                unknownV.Add(new UnknownElement(this,i,"UnknownV",0x1c));
            List<GenericElement<int>> unknownAO = new List<GenericElement<int>>();
            for (int i = _header.Unknown11; i < _header.Unknown11+4*0x116; i += 0x4)
                unknownAO.Add(new GenericElement<int>(this, i, "UnknownV"));
            //Setup Tree Structure
            Children.Add(new UnknownElement(this, DataOffset, "Header", 32 * 4));
            Children.Add(new NamedList(Attributes, "Attributes"));
            Children.Add(new NamedList(SSEAttributes, "SSE Attributes"));
            Children.Add(new NamedList(_unknowna, "UnknownA(Global Action Flag?)"));
            Children.Add(new NamedList(_unknownc, "UnknownC(Local Action Flag2?)"));
            Children.Add(new NamedList(_unknowne, "UnknownE(Action Flag2?)"));
            Children.Add(new NamedList(unknownV, "UnknownV"));
            Children.Add(new NamedList(unknownAO, "UnknownAO"));
            //Children.Add(new NamedList(_unknownb, "UnknownB(Local Action Flag?)"));


            Children.Add(unknownd);
            Children.Add(MiscSection);
            Children.Add(new NamedList(_actions, "Actions"));
            Children.Add(new NamedList(_actions2, "Actions2?"));
            Children.Add(new NamedList(_subactionflags, "Subaction Flags"));
            Children.Add(stringChunk);
            Children.Add(new NamedList(_subactionmain, "Subaction Main"));
            Children.Add(new NamedList(_subactiongfx, "Subaction GFX"));
            Children.Add(new NamedList(_subactionsfx, "Subaction SFX"));
            Children.Add(new NamedList(_subactionother, "Subaction Other"));
            Children.Add(new NamedList(_subroutines, "Subroutines"));
            Children.Add(new NamedList(_articles, "Articles"));
        }

    }

}