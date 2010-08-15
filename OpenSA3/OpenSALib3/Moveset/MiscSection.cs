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
            public buint UnknownSection4Offset;
            public buint UnknownSection5Offset;
            public buint MultiJumpOffset;
            public buint GlideOffset;
            public buint CrawlOffset;
            public buint UnknownSection9Offset;
            public buint TetherOffset;
            public buint UnknownSection12Offset;
            public buint UnknownSection13Offset;
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
        private MultiJumpData _multijumpdata;
        private GlideData _glidedata;
        private CrawlData _crawldata;
        private TetherData _tetherdata;
        public unsafe MiscSection(DatElement parent, uint fileoffset)
            : base(parent, fileoffset)
        {
            Length = 4 * 19;
            Name = "Misc";
            _header = *(Header*)(Address);
            /* TODO: Figure out proper length of section */
            for (uint i = 0; i < 10; i++)
                _section1.Add(new GenericElement<int>(this, _header.Section1 + i * 4, "Unknown"));
            for (uint i = 0; i < +_header.HurtBoxCount; i++)
                _hurtboxes.Add(new Hurtbox(this, _header.HurtBoxOffset + i * 4 * 5));
            for (uint i = 0; i < +_header.UnknownSectionCount; i++)
                _unknowntype1List.Add(new UnknownType1(this, _header.UnknownSectionOffset + i * 4 * 8));
            for (uint i = 0; i < +_header.LedgegrabCount; i++)
                _ledgegrabboxes.Add(new LedgegrabBox(this, _header.LedgegrabOffset + i * 4 * 4));
            for (uint i = _header.BoneRef2Offset; i < _header.BoneRef2Offset + 4 * 10; i += 4)
                _boneref2.Add(new BoneRef(this, i, "Unknown"));
            if (_header.MultiJumpOffset != 0)
                _multijumpdata = new MultiJumpData(this, _header.MultiJumpOffset);
            if (_header.GlideOffset != 0)
                _glidedata = new GlideData(this, _header.GlideOffset);
            if (_header.CrawlOffset != 0)
                _crawldata = new CrawlData(this, _header.CrawlOffset);
            if (_header.TetherOffset != 0)
                _tetherdata = new TetherData(this, _header.TetherOffset);
            //Setup Tree Structure
            _children.Add(new NamedList(_section1, "Section1"));
            _children.Add(new NamedList(_hurtboxes, "Hurtboxes"));
            _children.Add(new NamedList(_unknowntype1List, "UnknownType1"));
            _children.Add(new NamedList(_ledgegrabboxes, "LedgegrabBoxes"));
            _children.Add(new NamedList(_boneref2, "BoneRef"));
            _children.Add(_multijumpdata);
            _children.Add(_glidedata);
            _children.Add(_crawldata);
            _children.Add(_tetherdata);
            (_children as List<IEnumerable>).RemoveAll(x => x == null);
        }
    }
}