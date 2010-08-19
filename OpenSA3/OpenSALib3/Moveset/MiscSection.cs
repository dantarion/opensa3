using System;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset
{

    public sealed class MiscSection : DatElement {
#pragma warning disable 169 //'Field ____ is never used'
#pragma warning disable 649 //'Field ____ is never assigned';
        /* TODO: Cleanup */
        private struct Header
        {
            public bint Section1;
            public bint HurtBoxOffset;
            public bint HurtBoxCount;
            public bint UnknownSectionOffset;//Used
            public bint UnknownSectionCount;
            public bint LedgegrabOffset;
            public bint LedgegrabCount;
            public bint UnknownSection2Offset;
            public bint UnknownSection2Count;
            public bint BoneRef2Offset;
            public bint UnknownSection3Offset;
            public bint UnknownSection4Offset;
            public bint UnknownSection5Offset;
            public bint MultiJumpOffset;
            public bint GlideOffset;
            public bint CrawlOffset;
            public bint UnknownSection9Offset;
            public bint TetherOffset;
            public bint UnknownSection12Offset;
            public bint UnknownSection13Offset;
        }
#pragma warning restore 169 //'Field ____ is never used'
#pragma warning restore 649 //'Field ____ is never assigned';

        private readonly Header _header;
        private readonly NamedList<GenericElement<int>> _section1 = new NamedList<GenericElement<int>>("Section1");
        private readonly NamedList<Hurtbox> _hurtboxes = new NamedList<Hurtbox>("Hurtboxes");
        private readonly NamedList<UnknownType1> _unknowntype1List = new NamedList<UnknownType1>("UnknownT");
        private readonly NamedList<LedgegrabBox> _ledgegrabboxes = new NamedList<LedgegrabBox>("LedgegrabBoxes");
        private readonly NamedList<BoneRef> _boneref2 = new NamedList<BoneRef>("BoneRef");
        private readonly MultiJumpData _multijumpdata;
        private readonly GlideData _glidedata;
        private readonly CrawlData _crawldata;
        private readonly TetherData _tetherdata;
        private readonly SoundData _sounddata;

        public unsafe MiscSection(DatElement parent, int fileOffset)
            : base(parent, fileOffset)
        {
            Name = "Misc";
            Length = 4 * 19;
            _header = *(Header*)(Address);
            /* TODO: Figure out proper length of section */
            for (var i = 0; i < 14; i++)
                _section1.Add(new GenericElement<int>(this, _header.Section1 + i * 4, "Unknown"));
            for (var i = 0; i < +_header.HurtBoxCount; i++)
                _hurtboxes.Add(new Hurtbox(this, _header.HurtBoxOffset + i * 4 * 5));
            for (var i = 0; i < +_header.UnknownSectionCount; i++)
                _unknowntype1List.Add(new UnknownType1(this, _header.UnknownSectionOffset + i * 4 * 8));
            for (var i = 0; i < +_header.LedgegrabCount; i++)
                _ledgegrabboxes.Add(new LedgegrabBox(this, _header.LedgegrabOffset + i * 4 * 4));
            var unknownX = new NamedList<UnknownElement>("UnknownX");
            for (var i = 0; i < _header.UnknownSection2Count; i++)
                unknownX.Add(new UnknownElement(this, _header.UnknownSection2Offset + i * 32, "UnknownX", 32));
            for (int i = _header.BoneRef2Offset; i < _header.BoneRef2Offset + 4 * 14; i += 4)
                _boneref2.Add(new BoneRef(this, i, "BoneRef2"));
            if (_header.MultiJumpOffset != 0)
                _multijumpdata = new MultiJumpData(this, _header.MultiJumpOffset);
            if (_header.GlideOffset != 0)
                _glidedata = new GlideData(this, _header.GlideOffset);
            if (_header.CrawlOffset != 0)
                _crawldata = new CrawlData(this, _header.CrawlOffset);
            if (_header.TetherOffset != 0)
                _tetherdata = new TetherData(this, _header.TetherOffset);
            if (_header.UnknownSection4Offset != 0)
                _sounddata = new SoundData(this, _header.UnknownSection4Offset);
            //LastOne
            var unknownY = new UnknownElement(this, _header.UnknownSection9Offset, "UnknownY", 8);
            var count = unknownY.ReadInt(4);
            for (var i = 0; i < count; i++)
            {
                var offele = new GenericElement<int>(unknownY, unknownY.ReadInt(0) + i * 4, "Entry");
                offele[null] = new UnknownElement(offele, (int)offele.Value, "Data", 24);
                unknownY[null] = offele;
            }
            //LastOne
            var unknownZ = new UnknownElement(this, _header.UnknownSection12Offset, "UnknownY2", 8);
            count = unknownZ.ReadInt(4) + 1;
            for (var i = 0; i < count; i++)
            {
                var offele = new GenericElement<int>(unknownZ, unknownZ.ReadInt(0) + i * 4, "Entry");
                offele[null] = new UnknownElement(offele, (int)offele.Value, "Data", 24);
                unknownZ[null] = offele;
            }
            var unknowno = new UnknownElement(this, _header.UnknownSection3Offset, "UnknownO", 24);
            for (var i = unknowno.ReadInt(16); i < unknowno.FileOffset; i += 4)
                unknowno[null] = new GenericElement<int>(unknowno, i, "Unknown");
            //Setup Tree Structure
            AddNamedList(_section1);
            AddNamedList(_hurtboxes);
            AddNamedList(_unknowntype1List);
            AddNamedList(_ledgegrabboxes);
            AddNamedList(unknownX);
            AddNamedList(_boneref2);
            this["UnknownY"] = unknownY;
            this["UnknownZ"] = unknownZ;
            this["UnknownO"] = unknowno;
            if (_multijumpdata != null)
                this["JumpData"] = _multijumpdata;
            if (_glidedata != null)
                this["GlideData"] = _glidedata;
            if (_crawldata != null)
                this["CrawlData"] = _crawldata;
            if (_tetherdata != null)
                this["TetherData"] = _tetherdata;
            if (_sounddata != null)
                this["SoundData"] = _sounddata;
        }
    }
}