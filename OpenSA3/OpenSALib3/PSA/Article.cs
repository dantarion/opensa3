#pragma warning disable 649 //'Field ____ is never assigned'
using System;
using System.Collections.Generic;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;
using System.ComponentModel;
using System.Activities.Presentation.PropertyEditing;

namespace OpenSALib3.PSA
{
    public class Article : DatElement
    {
        private Data _data;

        struct Data
        {
            public bint Unknown1;
            public bint Unknown2;
            public bint BoneID;
            public bint ActionFlagsStart;
            public bint SubactionFlagsStart;
            public bint ActionsStart;
            public bint SubactionMainStart;
            public bint SubactionGFXStart;
            public bint SubactionSFXStart;
            public bint OtherStart;
            public bint UnknownD1;
            public bint UnknownD2;
            public bint UnknownD3;
            public bint DataOffset;
        }
        public int Unknown1
        {
            get { return _data.Unknown1; }
            set { _data.Unknown1 = value; }
        }
        public int Unknown2
        {
            get { return _data.Unknown2; }
            set { _data.Unknown2 = value; }
        }
        [Browsable(true)]
        public string BoneName
        {
            get { return RootFile.GetBoneName(BoneID); }
        }
        public int BoneID
        {
            get { return _data.BoneID; }
            set { _data.BoneID = value; }
        }
        public int UnknownD1
        {
            get { return _data.UnknownD1; }
            set { _data.UnknownD1 = value; }
        }
        public int UnknownD2
        {
            get { return _data.UnknownD2; }
            set { _data.UnknownD2 = value; }
        }
        public int UnknownD3
        {
            get { return _data.UnknownD3; }
            set { _data.UnknownD3 = value; }
        }
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public int DataOffset
        {
            get { return _data.UnknownD3; }
            set { _data.UnknownD3 = value; }
        }
        public unsafe Article(DatElement parent, int offset)
            : base(parent, offset)
        {
            var _actionflags = new NamedList(this, "Action Flags");
            NamedList _actions = new NamedList(this, "Actions");
            NamedList _subactionflags = new NamedList(this, "Flags");
            NamedList _subactionmain = new NamedList(this, "Main");
            NamedList _subactiongfx = new NamedList(this, "GFX");
            NamedList _subactionsfx = new NamedList(this, "SFX");
            NamedList _subactionother = new NamedList(this, "OTHER");
            NamedList _subroutines = new NamedList(this, "Subroutines");
            _data = *(Data*)Address;
            Name = "Article";
            Length = 4 * 14;
            if (
                _data.SubactionFlagsStart < 1 ||
                _data.ActionsStart > RootFile.Length || _data.ActionsStart % 4 != 0 ||
                _data.SubactionFlagsStart > RootFile.Length || _data.SubactionFlagsStart % 4 != 0 ||
                _data.SubactionGFXStart > RootFile.Length || _data.SubactionGFXStart % 4 != 0 ||
                _data.SubactionSFXStart > RootFile.Length || _data.SubactionSFXStart % 4 != 0 ||
                _data.OtherStart > RootFile.Length || _data.OtherStart % 4 != 0
                )
                throw new Exception("Not actually an Article, lol");
            var actions = 0;
            var subactions = (FileOffset - _data.SubactionFlagsStart) / 8;
            if (_data.ActionFlagsStart > 0)
                actions = (_data.ActionsStart - _data.ActionFlagsStart) / 0x10;
            if (_data.SubactionFlagsStart > 0 && _data.SubactionMainStart > 0)
                subactions = (_data.SubactionMainStart - _data.SubactionFlagsStart) / 0x8;
            if (subactions > 0x1000 || actions > 0x1000)
                throw new Exception("Not actually a Article, lol");
            //Parse
            var count = 0;
            for (var i = 0; i < actions; i++)
            {
                _actionflags[i] = new ActionFlags(this, _data.ActionFlagsStart, i);
                _actions[i] = new CommandList(this, _data.ActionsStart + i * 4, "Action " + String.Format("0x{0:X}", i), _subroutines);
            }
            for (var i = 0; i < subactions; i++)
                _subactionflags[i] = new SubactionFlags(this, _data.SubactionFlagsStart + i * 8);
            count = 0;
            if (_data.SubactionMainStart > 0)
                for (var i = 0; i < subactions; i++)
                    _subactionmain[i] = new CommandList(this, _data.SubactionMainStart + i * 4, "Subaction Main " + String.Format("0x{0:X}", i), _subroutines);
            if (_data.SubactionGFXStart > 0)
                for (var i = 0; i < subactions; i++)
                    _subactiongfx[i] = new CommandList(this, _data.SubactionGFXStart + i * 4, "Subaction GFX " + String.Format("0x{0:X}", i), _subroutines);
            if (_data.SubactionSFXStart > 0)
                for (var i = 0; i < subactions; i++)
                    _subactionsfx[i] = new CommandList(this, _data.SubactionSFXStart + i * 4, "Subaction SFX " + String.Format("0x{0:X}", i), _subroutines);
            if (_data.DataOffset != 0)
            {
                //var datalen = new  GenericElement<int>(this,_data.DataOffset,"DataLength");
                //this["DataLength"] = datalen;
                //this["Data"] = new UnknownElement(this, _data.DataOffset + 4, "Data", (int)datalen.Value);
            }
            //if (_data.OtherStart > 0)
            //    for (var i = 0; i < subactions; i++)
            //        _subactionother.Add(new CommandList(this, _data.OtherStart + i * 4, "Subaction Other " + String.Format("0x{0:X}", count++), _subroutines));


            this["Action Flags"] = _actionflags;
            this["Actions"] = _actions;
            this["Subaction Flags"] = _subactionflags;
            this["Subaction Main"] = _subactionmain;
            this["Subaction GFX"] = _subactiongfx;
            this["Subaction SFX"] = _subactionsfx;
            this["Subaction Other"] = _subactionother;
            this["Subroutines Flags"] = _subroutines;
        }
    }
}
