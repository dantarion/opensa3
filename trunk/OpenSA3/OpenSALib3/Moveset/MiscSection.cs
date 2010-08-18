using System;
using System.Collections;
using System.Collections.Generic;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset
{

    public sealed class MiscSection : DatElement
    {
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
        private readonly List<GenericElement<int>> _section1 = new List<GenericElement<int>>();

        public List<GenericElement<int>> Section
        {
            get { return _section1; }
        }

        private readonly List<Hurtbox> _hurtboxes = new List<Hurtbox>();

        public List<Hurtbox> Hurtboxes
        {
            get { return _hurtboxes; }
        }

        private readonly List<UnknownType1> _unknowntype1List = new List<UnknownType1>();

        public List<UnknownType1> UnknownType1List
        {
            get { return _unknowntype1List; }
        }

        private readonly List<LedgegrabBox> _ledgegrabboxes = new List<LedgegrabBox>();

        public List<LedgegrabBox> LedgegrabBoxes
        {
            get { return _ledgegrabboxes; }
        }
        private readonly List<BoneRef> _boneref2 = new List<BoneRef>();

        public List<BoneRef> BoneRef2
        {
            get { return _boneref2; }
        }
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
            var _unknownx = new List<UnknownElement>();
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
                offele.Children.Add(new UnknownElement(offele, (int)offele.Value, "Data", 24));
                _unknowny.Children.Add(offele);
            }
            //LastOne
            var _unknownz = new UnknownElement(this, _header.UnknownSection12Offset, "UnknownY2", 8);
            count = _unknownz.ReadInt(4)+1;
            for (int i = 0; i < count; i++)
            {
                var offele = new GenericElement<int>(_unknownz, _unknownz.ReadInt(0) + i * 4, "Entry");
                offele.Children.Add(new UnknownElement(offele, (int)offele.Value, "Data", 24));
                _unknownz.Children.Add(offele);
            }
            var unknowno = new UnknownElement(this, _header.UnknownSection3Offset, "UnknownO", 24);
            for (int i = unknowno.ReadInt(16); i < unknowno.FileOffset; i += 4)
                unknowno.Children.Add(new GenericElement<int>(unknowno, i, "Unknown"));
            //Setup Tree Structure
            Children.Add(new NamedList(_section1, "Section1"));
            Children.Add(new NamedList(_hurtboxes, "Hurtboxes"));
            Children.Add(new NamedList(_unknowntype1List, "UnknownType1"));
            Children.Add(new NamedList(_ledgegrabboxes, "LedgegrabBoxes"));
            Children.Add(new NamedList(_unknownx, "UnkonwnX"));
            Children.Add(new NamedList(_boneref2, "BoneRef"));
            Children.Add(_unknowny);
            Children.Add(_unknownz);
            Children.Add(unknowno);
            Children.Add(_multijumpdata);
            Children.Add(_glidedata);
            Children.Add(_crawldata);
            Children.Add(_tetherdata);
            Children.Add(_sounddata);
            (Children as List<IEnumerable>).RemoveAll(x => x == null);
        }
    }
}