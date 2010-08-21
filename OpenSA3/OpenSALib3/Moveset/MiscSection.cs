using System;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset
{

    public sealed class MiscSection : DatElement
    {
#pragma warning disable 169 //'Field ____ is never used'
#pragma warning disable 649 //'Field ____ is never assigned';
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


        public unsafe MiscSection(DatElement parent, int fileOffset)
            : base(parent, fileOffset)
        {

            Name = "Misc";
            Length = 4 * 19;
            _header = *(Header*)(Address);

            //tmp vars
            NamedList _section1 = new NamedList(this, "Section1");
            NamedList _hurtboxes = new NamedList(this, "Hurtboxes");
            _hurtboxes.TreeColor = System.Windows.Media.Brushes.Orange;
            NamedList _unknowntype1List = new NamedList(this, "UnknownT");
            NamedList _ledgegrabboxes = new NamedList(this, "LedgegrabBoxes");
            _hurtboxes.TreeColor = System.Windows.Media.Brushes.Orange;
            NamedList _boneref2 = new NamedList(this, "BoneRef");
            MultiJumpData _multijumpdata = null;
            GlideData _glidedata = null;
            CrawlData _crawldata = null;
            TetherData _tetherdata = null;
            SoundData _sounddata = null;
            /* TODO: Figure out proper length of section */
            for (var i = 0; i < 14; i++)
                _section1[i] = new GenericElement<int>(this, _header.Section1 + i * 4, "Unknown");
            for (var i = 0; i < +_header.HurtBoxCount; i++)
                _hurtboxes[i] = new Hurtbox(this, _header.HurtBoxOffset + i * 4 * 5);
            for (var i = 0; i < +_header.UnknownSectionCount; i++)
                _unknowntype1List[i] = new UnknownType1(this, _header.UnknownSectionOffset + i * 4 * 8);
            for (var i = 0; i < +_header.LedgegrabCount; i++)
                _ledgegrabboxes[i] = new LedgegrabBox(this, _header.LedgegrabOffset + i * 4 * 4);
            var unknownX = new NamedList(this,"UnknownX");
            for (var i = 0; i < _header.UnknownSection2Count; i++)
                unknownX[i] = new UnknownElement(this, _header.UnknownSection2Offset + i * 32, "UnknownX", 32);

            for (int i = _header.BoneRef2Offset; i < _header.BoneRef2Offset + 4 * 14; i += 4)
                _boneref2[null] = (new BoneRef(this, i, "BoneRef2"));

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
            AddByName(_section1);
            AddByName(_hurtboxes);
            AddByName(_unknowntype1List);
            AddByName(_ledgegrabboxes);
            AddByName(unknownX);
            AddByName(_boneref2);
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