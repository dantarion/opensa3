using System;
using System.Collections;
using System.Collections.Generic;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset
{

    public sealed class MiscSection : DatElement
    {
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

        private readonly Header _header;
        private readonly List<GenericElement<int>> _section1 = new NamedList<GenericElement<int>>("Section1");
        private readonly List<Hurtbox> _hurtboxes = new NamedList<Hurtbox>("Hurtboxes");
        private readonly List<UnknownType1> _unknowntype1List = new NamedList<UnknownType1>("UnknownT");
        private readonly List<LedgegrabBox> _ledgegrabboxes = new NamedList<LedgegrabBox>("LedgegrabBoxes");
        private readonly List<BoneRef> _boneref2 = new NamedList<BoneRef>("BoneRef");
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
            for (int i = 0; i < 14; i++)
                _section1.Add(new GenericElement<int>(this, _header.Section1 + i * 4, "Unknown"));
            for (int i = 0; i < +_header.HurtBoxCount; i++)
                _hurtboxes.Add(new Hurtbox(this, _header.HurtBoxOffset + i * 4 * 5));
            for (int i = 0; i < +_header.UnknownSectionCount; i++)
                _unknowntype1List.Add(new UnknownType1(this, _header.UnknownSectionOffset + i * 4 * 8));
            for (int i = 0; i < +_header.LedgegrabCount; i++)
                _ledgegrabboxes.Add(new LedgegrabBox(this, _header.LedgegrabOffset + i * 4 * 4));
            var _unknownx = new NamedList<UnknownElement>("UnknownX");
            for (int i = 0; i < _header.UnknownSection2Count; i++)
                _unknownx.Add(new UnknownElement(this, _header.UnknownSection2Offset + i * 32, "UnknownX", 32));
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
            var _unknowny = new UnknownElement(this, _header.UnknownSection9Offset, "UnknownY",8);
            int count = _unknowny.ReadInt(4);
            for (int i = 0; i < count; i++)
            {
                var offele = new GenericElement<int>(_unknowny, _unknowny.ReadInt(0) + i * 4, "Entry");
                offele[null] =new UnknownElement(offele, (int)offele.Value, "Data", 24);
                _unknowny[null] =offele;
            }
            //LastOne
            var _unknownz = new UnknownElement(this, _header.UnknownSection12Offset, "UnknownY2", 8);
            count = _unknownz.ReadInt(4)+1;
            for (int i = 0; i < count; i++)
            {
                var offele = new GenericElement<int>(_unknownz, _unknownz.ReadInt(0) + i * 4, "Entry");
                offele[null] =new UnknownElement(offele, (int)offele.Value, "Data", 24);
                _unknownz[null] =offele;
            }
            var unknowno = new UnknownElement(this, _header.UnknownSection3Offset, "UnknownO", 24);
            for (int i = unknowno.ReadInt(16); i < unknowno.FileOffset; i += 4)
                unknowno[null] =new GenericElement<int>(unknowno, i, "Unknown");
            //Setup Tree Structure
            this["Section1"] = _section1;
            this["Hurtboxes"] = _hurtboxes;
            this["UnknownType1"] = _unknowntype1List;
            this["LedgegrabBoxes"] = _ledgegrabboxes;
            this["UnkonwnX"] = _unknownx;
            this["BoneRef"] = _boneref2;
            this["UnknownY"] = _unknowny;
            this["UnknownZ"] = _unknownz;
            this["UnknownO"] = unknowno;
            this["JumpData"] = _multijumpdata;
            this["GlideData"] = _glidedata;
            this["CrawlData"] = _crawldata;
            this["TetherData"] = _tetherdata;
            this["SoundData"] = _sounddata;
        }
    }
}