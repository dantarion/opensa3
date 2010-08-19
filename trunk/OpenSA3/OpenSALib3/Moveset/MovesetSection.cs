﻿using System;
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
        private readonly NamedList<Attribute> _attributes = new NamedList<Attribute>("Attributes");
        private readonly NamedList<Attribute> _sseattributes = new NamedList<Attribute>("SSEAttributes");
        private MiscSection _miscsection;

        [Browsable(false)]
        public MiscSection MiscSection
        {
            get { return _miscsection; }
        }
        private NamedList<ActionFlags> _unknowna = new NamedList<ActionFlags>("Action Flags");
        private NamedList<CommandList> _actions = new NamedList<CommandList>("Actions");
        private NamedList<CommandList> _actions2 = new NamedList<CommandList>("Actions2");
        private NamedList<UnknownElement> _subactions = new NamedList<UnknownElement>("Subactions");
        private NamedList<UnknownElement> _unknownb = new NamedList<UnknownElement>("UnknownB");
        private NamedList<UnknownElement> _unknownc = new NamedList<UnknownElement>("UnknownC");
        private NamedList<UnknownElement> _unknowne = new NamedList<UnknownElement>("UnknownE");
        private NamedList<Article> _articles = new NamedList<Article>("Article");
        private NamedList<List<Command>> _subroutines = new NamedList<List<Command>>("Subroutines");

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

            int count = 0;
            for (int i = _header.Unknown5Start; i < _header.Unknown7; i += 16)
                _unknowna.Add(new ActionFlags(this,i,count++));
            for (int i = _header.Unknown6Start; i < _header.ActionsStart; i += 16)
                _unknownc.Add(new UnknownElement(this, i, "UnknownC", 16));
            for (int i = _header.Unknown7; i < _header.Unknown7 + 8 * (_unknowna.Count + _unknownc.Count); i += 8)
                _unknowne.Add(new UnknownElement(this, i, "UnknownE", 8));
            for (int i = _header.Unknown1; i < _header.Unknown19; i += 8)
                _unknownb.Add(new UnknownElement(this, i, "UnknownB", 8));

            var unknownd = new UnknownElement(this, _header.Unknown8, "UnknownD", 8);
            for (int i = RootFile.ReadInt(unknownd.FileOffset); i < RootFile.ReadInt(unknownd.FileOffset) + RootFile.ReadInt(unknownd.FileOffset + 4) * 4; i += 4)
                unknownd[i] =new GenericElement<int>(unknownd, i, "UnknownDEntry");
            count = 0x112;
            for (int i = _header.ActionsStart; i < _header.Actions2Start; i += 4)
                _actions.Add(new CommandList(this, i, "Action " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _header.Actions2Start; i < _header.Unknown11; i += 4)
                _actions2.Add(new CommandList(this, i, "Action2 " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _header.SubactionFlagsStart; i < _header.SubactionMainStart; i += 8)
            {
                var subactiongroup = new UnknownElement(this, -1, String.Format("0x{0:X03}", count), 0);
                _subactions.Add(subactiongroup);
                 var flags = new SubactionFlags(subactiongroup, i);
                 subactiongroup["Flags"] = flags;
                 subactiongroup.Name += " - " + flags.AnimationName;
            }
            //var stringChunk = new UnknownElement(this, _subactionflags[0].AnimationStringOffset, "SubactionStrings", _subactionflags[_subactionflags.Count - 1].AnimationStringOffset + _subactionflags[_subactionflags.Count - 1].Name.Length+1 - _subactionflags[0].AnimationStringOffset);
            count = 0;
            for (int i = _header.SubactionMainStart; i < _header.SubactionGFXStart; i += 4)
                _subactions[count]["Main"] = new CommandList(_subactions[count], i, "Main ", _subroutines);
            count = 0;
            for (int i = _header.SubactionGFXStart; i < _header.SubactionSFXStart; i += 4)
                _subactions[count]["GFX"] = new CommandList(_subactions[count], i,"GFX",_subroutines);
            count = 0;
            for (int i = _header.SubactionSFXStart; i < _header.SubactionOtherStart; i += 4)
                _subactions[count]["SFX"] = new CommandList(_subactions[count], i, "SFX ", _subroutines);
            int othercount = count;
            count = 0;
            for (int i = _header.SubactionOtherStart; i < _header.SubactionOtherStart + othercount * 4; i += 4)
                _subactions[count]["Other"] = new CommandList(_subactions[count], i, "Other ", _subroutines);

            //LastOne
            var _unknownk = new UnknownElement(this, _header.Unknown1, "UnknownKTree", 16);
            for (int i = 0; i < _unknownk.ReadInt(12); i++)
                _unknownk[i] =new UnknownElement(_unknownk, _unknownk.ReadInt(8) + i * 8, "Stats", 8);
            count = _unknownk.ReadInt(4);
            for (int i = 0; i < 2; i++)
            {
                //For each offset...
                var offele = new GenericElement<int>(_unknownk, _unknownk.ReadInt(0) + i * 4, "Offset");
                if ((int)offele.Value == 0)
                {
                    _unknownk[i] =offele;
                    continue;
                }
                for (int c = 0; c < count; c++)
                {
                    //Each element needs is children too
                    var inele = new UnknownElement(offele, (int)offele.Value+c*8, "Entry", 8);
                    var incount = inele.ReadInt(4);
                    for (int j = 0; j < incount; j++)
                    {
                        var ininele = new UnknownElement(inele, inele.ReadInt(0) + j * 8, "Entry", 8);
                        var inincount = ininele.ReadInt(4);
                        for (int m = 0; m < inincount; m++)
                        {
                            var inininele = new GenericElement<int>(ininele, ininele.ReadInt(0) + m * 4, "Value");
                            ininele[m] =inininele;
                        }
                        inele[j] =ininele;
                    }
                    offele[c] =inele;
                }
                _unknownk[i] =offele;
            }
            var unknowno = new UnknownElement(this, _header.Unknown19, "UnknownO", 24);
            for (int i = unknowno.ReadInt(20); i < unknowno.FileOffset; i += 4)
                unknowno[null] =new GenericElement<int>(unknowno, i, "Unknown");
            var bonerefs = new NamedList<BoneRef>("BoneRefs");
            for (int i = _header.Unknown18; i < MiscSection.ReadInt(4 * 9); i += 4)
                bonerefs.Add(new BoneRef(this, i, "Unknown"));
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
            UnknownElement headerExtension = new UnknownElement(this, DataOffset + 31 * 4, "HeaderEXT", DataLength - 31 * 4);
            var unknownV = new NamedList<UnknownElement>("UnknownV");
            for (int i = _header.Unknown16; i < _header.Unknown18; i += 0x1c)
                unknownV.Add(new UnknownElement(this, i, "UnknownV", 0x1c));
            var unknownAO = new NamedList<GenericElement<int>>("UnknownAO");
            for (int i = _header.Unknown11; i < _header.Unknown11 + 4 * _unknowne.Count; i += 0x4)
                unknownAO.Add(new GenericElement<int>(this, i, "UnknownAO"));

            //Setup Tree Structure
            this["Header"] = new UnknownElement(this, DataOffset, "Header", 31 * 4);
            this["HeaderEXT"] = headerExtension;
            this.AddNamedList(_attributes);
            this.AddNamedList(_sseattributes);
            
            this.AddNamedList(_unknownc);
            this.AddNamedList( _unknowne);
            this.AddNamedList( unknownV);
            this.AddNamedList(unknownAO);
            this["UnknownK"] =_unknownk;
            this["UnknownO"] = unknowno;

            this["UnknownD"] = unknownd;
            this["MiscSection"] = MiscSection;
            this["BoneRefs"] = bonerefs;
            this.AddNamedList(_unknowna);
            this.AddNamedList(_actions);
            this.AddNamedList( _actions2);    
            this.AddNamedList( _subactions);
            //this["String Chunk"] = stringChunk;
            this.AddNamedList(_subroutines);
            this.AddNamedList( _articles);
        }

    }

}