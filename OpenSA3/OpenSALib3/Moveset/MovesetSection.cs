﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using OpenSALib3.DatHandler;
using OpenSALib3.PSA;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset
{
    public sealed class MovesetSection : DatSection
    {
#pragma warning disable 169 //'Field ____ is never used'
#pragma warning disable 649 //'Field ____ is never assigned'
        private struct MovesetHeader
        {
            public bint SubactionFlagsStart;
            public bint Unknown1;//UnknownB
            public bint AttributeStart;
            public bint SSEAttributeStart;
            public bint MiscSectionOffset;
            public bint CommonActionFlagsStart;//UnknownA
            public bint ActionFlagsStart;//UnknownC
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
#pragma warning restore 169 //'Field ____ is never used'
#pragma warning restore 649 //'Field ____ is never assigned'

        private readonly MovesetHeader _header;

        [Browsable(false)]
        public MiscSection MiscSection { get; private set; }


        public unsafe MovesetSection(DatElement parent, int offset, VoidPtr stringPtr)
            : base(parent, offset, stringPtr)
        {
            _header = *(MovesetHeader*)(RootFile.Address + DataOffset);

        }
        public override void Parse()
        {
            MiscSection = new MiscSection(this, _header.MiscSectionOffset);

            var _attributes = new NamedList(this, "Attributes");
            _attributes.TreeColor = null;
            var _sseattributes = new NamedList(this, "SSEAttributes");
            _sseattributes.TreeColor = System.Windows.Media.Brushes.Orange;
            var _commonactionflags = new NamedList(this, "Common Action Flags");
            _commonactionflags.TreeColor = System.Windows.Media.Brushes.Orange;
            var _actionflags = new NamedList(this, "Special Action Flags");
            _actionflags.TreeColor = System.Windows.Media.Brushes.Orange;
            var _actions = new NamedList(this, "Actions");
            _actions.TreeColor = null;
            var _actions2 = new NamedList(this, "Actions2");
            _actions2.TreeColor = System.Windows.Media.Brushes.Orange;
            var _subactions = new NamedList(this, "Subactions");
            _subactions.TreeColor = null;
            var _unknownb = new NamedList(this, "UnknownB");
            var _unknowne = new NamedList(this, "Action ???");
            _unknowne.TreeColor = null;
            var _articles = new NamedList(this, "Article");
            var _subroutines = new NamedList(this, "Subroutines");
            _subroutines.TreeColor = null;

            /* Attributes */
            var count = 0;
            for (int i = _header.AttributeStart; i < _header.SSEAttributeStart; i += 4)
                _attributes[count++] =new Attribute(this, i);
            count = 0;
            for (int i = _header.SSEAttributeStart; i < _header.CommonActionFlagsStart; i += 4)
                _sseattributes[count++] = new Attribute(this, i);

            /* Action Interrupts */
            var _actioninterrupts = new UnknownElement(this, _header.Unknown8, "Action Interrupts", 8);
            _actioninterrupts.TreeColor = null;
            count = 0;
            for (var i = RootFile.ReadInt(_actioninterrupts.FileOffset); i < RootFile.ReadInt(_actioninterrupts.FileOffset) + RootFile.ReadInt(_actioninterrupts.FileOffset + 4) * 4; i += 4)
            {
                var tmp = new GenericElement<int>(_actioninterrupts, i, String.Format("0x{0:X03}", count++));
                var str = RootFile.IsExternal(i);
                if (str != null)
                    tmp.Name += " - " + str;
                _actioninterrupts[i] = tmp;
            }

            /* Action Flags */
            count = 0;
            for (int i = _header.CommonActionFlagsStart; i < _header.Unknown7; i += 16)
                _commonactionflags[count] = new ActionFlags(this, i, count++);
            for (int i = _header.ActionFlagsStart; i < _header.ActionsStart; i += 16)
                _actionflags[count] = new ActionFlags(this, i, count++);
            count = 0;
            for (int i = _header.Unknown7; i < _header.Unknown7 + 8 * (_commonactionflags.Count + _actionflags.Count); i += 8)
                _unknowne[null] = new UnknownElement(this, i, String.Format("0x{0:X03}", count++), 8);
            for (int i = _header.Unknown1; i < _header.Unknown19; i += 8)
                _unknownb[null] = new UnknownElement(this, i, "UnknownB", 8);

            /* Action _Pre */
            var _actionpre = new NamedList(this, "Action_Pre");
            _actionpre.TreeColor = null;
            count = 0;
            for (int i = _header.Unknown11; i < _header.Unknown11 + 4 * _unknowne.Count; i += 0x4)
            {

                var tmp = new GenericElement<int>(_actionpre, i, String.Format("0x{0:X03}", count++));
                var str = RootFile.IsExternal(i);
                if (str != null)
                    tmp.Name += " - " + str;
                _actionpre[null] = tmp;
            }

            /* Actions */
            count = 0x112;
            for (int i = _header.ActionsStart; i < _header.Actions2Start; i += 4)
                _actions[count] = new CommandList(this, i, "Action " + String.Format("0x{0:X03}", count++), _subroutines);
            count = 0x112;
            for (int i = _header.Actions2Start; i < _header.Unknown11; i += 4)
                _actions2[count] = new CommandList(this, i, "Action2 " + String.Format("0x{0:X03}", count++), _subroutines);
            count = 0;

            /* Subaction Flags */
            int first = int.MaxValue;
            int last = 0;
            for (int i = _header.SubactionFlagsStart; i < _header.SubactionMainStart; i += 8)
            {
                var subactiongroup = new UnknownElement(this, -1, String.Format("0x{0:X03}", count), 0);
                _subactions[count++] = subactiongroup;
                var flags = new SubactionFlags(subactiongroup, i);
                if (flags.AnimationStringOffset != 0)
                    first = Math.Min(first, flags.AnimationStringOffset);
                last = Math.Max(last, flags.AnimationStringOffset + flags.AnimationName.Length);
                subactiongroup["Flags"] = flags;
                subactiongroup.Name += " - " + flags.AnimationName;
            }
            /* Subaction Strings */
            var size = _header.SubactionFlagsStart - first;
            var stringChunk = new UnknownElement(this, first, "SubactionStrings", size);
            stringChunk.TreeColor = null;

            /* Subaction Main,GFX,SFX,Other */
            count = 0;
            for (int i = _header.SubactionMainStart; i < _header.SubactionGFXStart; i += 4)
                (_subactions[count] as DatElement)["Main"] = new CommandList(_subactions[count++], i, "Main ", _subroutines);
            count = 0;
            for (int i = _header.SubactionGFXStart; i < _header.SubactionSFXStart; i += 4)
                (_subactions[count] as DatElement)["GFX"] = new CommandList(_subactions[count++], i, "GFX", _subroutines);
            count = 0;
            for (int i = _header.SubactionSFXStart; i < _header.SubactionOtherStart; i += 4)
                (_subactions[count] as DatElement)["SFX"] = new CommandList(_subactions[count++], i, "SFX ", _subroutines);
            var othercount = count;
            count = 0;
            for (int i = _header.SubactionOtherStart; i < _header.SubactionOtherStart + othercount * 4; i += 4)
                (_subactions[count] as DatElement)["Other"] = new CommandList(_subactions[count++], i, "Other ", _subroutines);

            //Model Display
            var unknownK = new UnknownElement(this, _header.Unknown1, "Model Display", 16);
            for (var i = 0; i < unknownK.ReadInt(12); i++)
                unknownK[i] = new UnknownElement(unknownK, unknownK.ReadInt(8) + i * 8, "Stats", 8);
            count = unknownK.ReadInt(4);
            for (var i = 0; i < 2; i++)
            {
                //For each offset...
                var offele = new GenericElement<int>(unknownK, unknownK.ReadInt(0) + i * 4, i == 0 ? "Hidden" : "Visible");
                if ((int)offele.Value == 0)
                {
                    unknownK.AddByName(offele);
                    continue;
                }
                for (var c = 0; c < count; c++)
                {
                    //Each element needs is children too
                    var inele = new UnknownElement(offele, (int)offele.Value + c * 8, "Entry", 8);
                    var incount = inele.ReadInt(4);
                    for (var j = 0; j < incount; j++)
                    {
                        var ininele = new UnknownElement(inele, inele.ReadInt(0) + j * 8, "Entry", 8);
                        var inincount = ininele.ReadInt(4);
                        for (var m = 0; m < inincount; m++)
                        {
                            var inininele = new BoneRef(ininele, ininele.ReadInt(0) + m * 4, "");
                            ininele[m] = inininele;
                        }
                        inele[j] = ininele;
                    }
                    offele[c] = inele;
                }
                unknownK.AddByName(offele);
            }
            count = 0;

            var unknowno = new UnknownElement(this, _header.Unknown19, "UnknownO", 24);
            for (var i = unknowno.ReadInt(20); i < unknowno.FileOffset; i += 4)
                unknowno[null] = new GenericElement<int>(unknowno, i, String.Format("0x{0:X03}",count++));
            var bonerefs = new NamedList(this, "BoneRefs");
            for (int i = _header.Unknown18; i < MiscSection.ReadInt(4 * 9); i += 4)
                bonerefs[null] = (new BoneRef(this, i, "Unknown"));
            if (_header.Unknown26 > 0)
                _articles[null] = (new Article(this, _header.Unknown26));

            var headerExtension = new UnknownElement(this, DataOffset + 31 * 4, "HeaderEXT", DataLength - 31 * 4);
            for (var i = headerExtension.FileOffset; i < headerExtension.FileOffset + Math.Min(headerExtension.Length,0x40); i += 4)
            {
                Article art = null;
                try
                {
                    art = new Article(this, RootFile.ReadInt(i));
                    _articles[null] = art;
                }
                catch (Exception)
                {
                }
            }
            var unknownV = new NamedList(this, "UnknownV");
            for (int i = _header.Unknown16; i < _header.Unknown18; i += 0x1c)
                unknownV[null] = new UnknownElement(this, i, "UnknownV", 0x1c);
            //TODO: Make Unknown letters make sense...

            //Setup Tree Structure
            this["Header"] = new UnknownElement(this, DataOffset, "Header", 31 * 4);
            this["HeaderEXT"] = headerExtension;
            AddByName(_attributes);
            AddByName(_sseattributes);
            
            AddByName(unknownV);          
            AddByName(unknownK);
            this["UnknownO"] = unknowno;       
            this["MiscSection"] = MiscSection;
            this["BoneRefs"] = bonerefs;
            AddByName(_unknowne);
            AddByName(_actioninterrupts);
            AddByName(_actionpre);
            AddByName(_commonactionflags);
            AddByName(_actionflags);
            AddByName(_actions);
            AddByName(_actions2);
            AddByName(_subactions);
            this["String Chunk"] = stringChunk;
            this.AddByName(_subroutines);
            AddByName(_articles);
        }

    }

}