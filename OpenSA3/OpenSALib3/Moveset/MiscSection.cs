using System;
using System.Windows.Media;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;

namespace OpenSALib3.Moveset {

    public sealed class MiscSection : DatElement {
#pragma warning disable 169 //'Field ____ is never used'
#pragma warning disable 649 //'Field ____ is never assigned';
        private struct Header {
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
            : base(parent, fileOffset) {

            Name = "Misc";
            Length = 4 * 19;
            _header = *(Header*)(Address);

            //tmp vars
            var section1 = new NamedList(this, "Section1");
            var hurtboxes = new NamedList(this, "Hurtboxes") { TreeColor = Brushes.Orange };
            var unknowntype1List = new NamedList(this, "UnknownT");
            var ledgegrabboxes = new NamedList(this, "LedgegrabBoxes");
            var boneref2 = new NamedList(this, "BoneRef");
            var multijumpdata = (MultiJumpData)null;
            var glideData = (GlideData)null;
            var crawlData = (CrawlData)null;
            var tetherData = (TetherData)null;
            var soundData = (SoundData)null;
            /* TODO: Figure out proper length of section */
            for (var i = 0; i < 14; i++)
                section1[i] = new GenericElement<int>(this, _header.Section1 + i * 4, "Unknown");
            for (var i = 0; i < +_header.HurtBoxCount; i++)
                hurtboxes[i] = new Hurtbox(this, _header.HurtBoxOffset + i * 4 * 5);
            for (var i = 0; i < +_header.UnknownSectionCount; i++)
                unknowntype1List[i] = new UnknownType1(this, _header.UnknownSectionOffset + i * 4 * 8);
            for (var i = 0; i < +_header.LedgegrabCount; i++)
                ledgegrabboxes[i] = new LedgegrabBox(this, _header.LedgegrabOffset + i * 4 * 4);
            var unknownX = new NamedList(this, "UnknownX");
            for (var i = 0; i < _header.UnknownSection2Count; i++)
                unknownX[i] = new UnknownElement(this, _header.UnknownSection2Offset + i * 32, "UnknownX", 32);

            for (int i = _header.BoneRef2Offset; i < _header.BoneRef2Offset + 4 * 14; i += 4)
                boneref2[null] = (new BoneRef(this, i, "BoneRef2"));

            if (_header.MultiJumpOffset != 0)
                multijumpdata = new MultiJumpData(this, _header.MultiJumpOffset);
            if (_header.GlideOffset != 0)
                glideData = new GlideData(this, _header.GlideOffset);
            if (_header.CrawlOffset != 0)
                crawlData = new CrawlData(this, _header.CrawlOffset);
            if (_header.TetherOffset != 0)
                tetherData = new TetherData(this, _header.TetherOffset);
            if (_header.UnknownSection4Offset != 0)
                soundData = new SoundData(this, _header.UnknownSection4Offset);

            //LastOne
            var unknownY = new UnknownElement(this, _header.UnknownSection9Offset, "UnknownY", 8);
            var count = unknownY.ReadInt(4);
            for (var i = 0; i < count; i++) {
                var offele = new GenericElement<int>(unknownY, unknownY.ReadInt(0) + i * 4, "Entry");
                offele[null] = new UnknownElement(offele, (int)offele.Value, "Data", 24);
                unknownY[null] = offele;
            }
            //LastOne
            var unknownZ = new UnknownElement(this, _header.UnknownSection12Offset, "UnknownY2", 8);
            count = unknownZ.ReadInt(4) + 1;
            for (var i = 0; i < count; i++) {
                var offele = new GenericElement<int>(unknownZ, unknownZ.ReadInt(0) + i * 4, "Entry");
                offele[null] = new UnknownElement(offele, (int)offele.Value, "Data", 24);
                unknownZ[null] = offele;
            }
            var unknowno = new UnknownElement(this, _header.UnknownSection3Offset, "UnknownO", 24);
            for (var i = unknowno.ReadInt(16); i < unknowno.FileOffset; i += 4)
                unknowno[null] = new GenericElement<int>(unknowno, i, "Unknown");
            //Setup Tree Structure
            AddByName(section1);
            AddByName(hurtboxes);
            AddByName(unknowntype1List);
            AddByName(ledgegrabboxes);
            AddByName(unknownX);
            AddByName(boneref2);
            this["UnknownY"] = unknownY;
            this["UnknownZ"] = unknownZ;
            this["UnknownO"] = unknowno;
            if (multijumpdata != null)
                this["JumpData"] = multijumpdata;
            if (glideData != null)
                this["GlideData"] = glideData;
            if (crawlData != null)
                this["CrawlData"] = crawlData;
            if (tetherData != null)
                this["TetherData"] = tetherData;
            if (soundData != null)
                this["SoundData"] = soundData;
        }
    }
}