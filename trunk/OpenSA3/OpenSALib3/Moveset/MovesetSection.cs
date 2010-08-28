using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
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
            public bint ModelVisibilityStart;//UnknownB
            public bint AttributeStart;
            public bint SSEAttributeStart;
            public bint MiscSectionOffset;
            public bint CommonActionFlagsStart;//UnknownA
            public bint ActionFlagsStart;//UnknownC
            public bint Unknown7;
            public bint Unknown8;
            public bint ActionsStart;
            public bint Actions2Start;
            public bint ActionPreStart;
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
            public bint EntryArticleStart;
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

            var attributes = new NamedList(this, "Attributes") {TreeColor = null};
            var sseAttributes = new NamedList(this, "SSEAttributes") {TreeColor = Brushes.Orange};
            var commonActionFlags = new NamedList(this, "Common Action Flags") {TreeColor = Brushes.Orange};
            var actionFlags = new NamedList(this, "Special Action Flags") {TreeColor = Brushes.Orange};
            var actions = new NamedList(this, "Actions") {TreeColor = null};
            var actions2 = new NamedList(this, "Actions2") {TreeColor = Brushes.Orange};
            var subactions = new NamedList(this, "Subactions") {TreeColor = null};
            var unknownB = new NamedList(this, "UnknownB");
            var unknownE = new NamedList(this, "Action ???") {TreeColor = null};
            var articles = new NamedList(this, "Article");
            var subroutines = new NamedList(this, "Subroutines") {TreeColor = null};
            /* Attributes */
            var count = 0;
            for (int i = _header.AttributeStart; i < _header.SSEAttributeStart; i += 4)
                attributes[count++] = new Attribute(attributes, i);
            count = 0;
            for (int i = _header.SSEAttributeStart; i < _header.CommonActionFlagsStart; i += 4)
                sseAttributes[count++] = new Attribute(sseAttributes, i);

            /* Action Interrupts */
            var actionInterrupts = new UnknownElement(this, _header.Unknown8, "Action Interrupts", 8) {TreeColor = null};
            count = 0;
            for (var i = RootFile.ReadInt(actionInterrupts.FileOffset); i < RootFile.ReadInt(actionInterrupts.FileOffset) + RootFile.ReadInt(actionInterrupts.FileOffset + 4) * 4; i += 4)
            {
                var tmp = new GenericElement<int>(actionInterrupts, i, String.Format("0x{0:X03}", count++));
                var str = RootFile.IsExternal(i);
                if (str != null)
                    tmp.Name += " - " + str;
                actionInterrupts[i] = tmp;
            }

            /* Action Flags */
            count = 0;
            for (int i = _header.CommonActionFlagsStart; i < _header.Unknown7; i += 16)
                commonActionFlags[count] = new ActionFlags(commonActionFlags, i, count++);
            for (int i = _header.ActionFlagsStart; i < _header.ActionsStart; i += 16)
                actionFlags[count] = new ActionFlags(actionFlags, i, count++);
            count = 0;
            for (int i = _header.Unknown7; i < _header.Unknown7 + 8 * (commonActionFlags.Count + actionFlags.Count); i += 8)
                unknownE[null] = new UnknownElement(unknownE, i, String.Format("0x{0:X03}", count++), 8);
            for (int i = _header.ModelVisibilityStart; i < _header.Unknown19; i += 8)
                unknownB[null] = new UnknownElement(unknownB, i, "UnknownB", 8);

            /* Action _Pre */
            var actionPre = new NamedList(this, "Action_Pre") {TreeColor = null};
            count = 0;
            for (int i = _header.ActionPreStart; i < _header.ActionPreStart + 4 * unknownE.Count; i += 0x4)
            {

                var tmp = new GenericElement<int>(actionPre, i, String.Format("0x{0:X03}", count++));
                var str = RootFile.IsExternal(i);
                if (str != null)
                    tmp.Name += " - " + str;
                actionPre[null] = tmp;
            }

            /* Actions */
            count = 0x112;
            for (int i = _header.ActionsStart; i < _header.Actions2Start; i += 4)
                actions[count] = new CommandList(actions, i, "Action " + String.Format("0x{0:X03}", count++), subroutines);
            count = 0x112;
            for (int i = _header.Actions2Start; i < _header.ActionPreStart; i += 4)
                actions2[count] = new CommandList(actions2, i, "Action2 " + String.Format("0x{0:X03}", count++), subroutines);
            count = 0;

            /* Subaction Flags */
            var first = int.MaxValue;
            var last = 0;
            for (int i = _header.SubactionFlagsStart; i < _header.SubactionMainStart; i += 8)
            {
                var subactiongroup = new UnknownElement(this, -1, String.Format("0x{0:X03}", count), 0);
                subactions[count++] = subactiongroup;
                var flags = new SubactionFlags(subactiongroup, i);
                if (flags.AnimationStringOffset != 0)
                    first = Math.Min(first, flags.AnimationStringOffset);
                last = Math.Max(last, flags.AnimationStringOffset + flags.AnimationName.Length);
                subactiongroup["Flags"] = flags;
                subactiongroup.Name += " - " + flags.AnimationName;
            }
            /* Subaction Strings */
            var size = _header.SubactionFlagsStart - first;
            var stringChunk = new UnknownElement(this, first, "SubactionStrings", size) {TreeColor = null};
            /* Subaction Main,GFX,SFX,Other */
            count = 0;
            for (int i = _header.SubactionMainStart; i < _header.SubactionGFXStart; i += 4)
                subactions[count]["Main"] = new CommandList(subactions[count++], i, "Main ", subroutines);
            count = 0;
            for (int i = _header.SubactionGFXStart; i < _header.SubactionSFXStart; i += 4)
                subactions[count]["GFX"] = new CommandList(subactions[count++], i, "GFX", subroutines);
            count = 0;
            for (int i = _header.SubactionSFXStart; i < _header.SubactionOtherStart; i += 4)
                subactions[count]["SFX"] = new CommandList(subactions[count++], i, "SFX ", subroutines);
            var othercount = count;
            count = 0;
            for (int i = _header.SubactionOtherStart; i < _header.SubactionOtherStart + othercount * 4; i += 4)
                subactions[count]["Other"] = new CommandList(subactions[count++], i, "Other ", subroutines);

            //Model Display
            var unknownK = new UnknownElement(this, _header.ModelVisibilityStart, "Model Display", 16);
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
            var bonerefs = new NamedList(this, "Smash Ball Bones?");
            for (int i = _header.Unknown18; i < MiscSection.ReadInt(4 * 9); i += 4)
                bonerefs[null] = (new BoneRef(bonerefs, i, "Unknown"));
            if (_header.EntryArticleStart > 0)
                articles[null] = (new Article(articles, _header.EntryArticleStart));

            var headerExtension = new UnknownElement(this, DataOffset + 31 * 4, "HeaderEXT", DataLength - 31 * 4);
            for (var i = headerExtension.FileOffset; i < headerExtension.FileOffset + Math.Min(headerExtension.Length,0x40); i += 4)
            {
                try {
                    var art = new Article(articles, RootFile.ReadInt(i));
                    articles[null] = art;
                } catch (Exception exception) {
                    Debug.Fail(exception.ToString());
                }
            }
            var unknownV = new NamedList(this, "UnknownV");
            for (int i = _header.Unknown16; i < _header.Unknown18; i += 0x1c)
                unknownV[null] = new UnknownElement(unknownV, i, "UnknownV", 0x1c);
            //TODO: Make Unknown letters make sense...

            //Setup Tree Structure
            this["Header"] = new UnknownElement(this, DataOffset, "Header", 31 * 4);
            this["HeaderEXT"] = headerExtension;
            AddByName(attributes);
            AddByName(sseAttributes);
            
            AddByName(unknownV);          
            AddByName(unknownK);
            this["UnknownO"] = unknowno;       
            this["MiscSection"] = MiscSection;
            this["BoneRefs"] = bonerefs;
            AddByName(unknownE);
            AddByName(actionInterrupts);
            AddByName(actionPre);
            AddByName(commonActionFlags);
            AddByName(actionFlags);
            AddByName(actions);
            AddByName(actions2);
            AddByName(subactions);
            this["String Chunk"] = stringChunk;
            AddByName(subroutines);
            AddByName(articles);
        }

    }

}