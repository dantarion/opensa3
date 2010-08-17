using System;
using OpenSALib3.DatHandler;
using OpenSALib3.Utility;
using System.Collections.Generic;
using OpenSALib3.Moveset;
namespace OpenSALib3.PSA
{
    public class Article : DatElement
    {
        private Data _data;

        struct Data
        {
            public bint Unknown1;
            public bint Unknown2;
            public bint Unknown3;
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
            public bint UnknownD4;
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
        public int Unknown3
        {
            get { return _data.Unknown3; }
            set { _data.Unknown3 = value; }
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
        public int UnknownD4
        {
            get { return _data.UnknownD4; }
            set { _data.UnknownD4 = value; }
        }
        private List<CommandList> _actions = new List<CommandList>();
        private List<SubactionFlags> _subactionflags = new List<SubactionFlags>();
        private List<CommandList> _subactionmain = new List<CommandList>();
        private List<CommandList> _subactiongfx = new List<CommandList>();
        private List<CommandList> _subactionsfx = new List<CommandList>();
        private List<CommandList> _subactionother = new List<CommandList>();
        private Dictionary<int, List<Command>> _subroutines = new Dictionary<int, List<Command>>();
        public unsafe Article(MovesetSection parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)Address;
            Name = "Article";
            Length = 4 * 10;

            int actions = 0;
            int subactions = (FileOffset-_data.SubactionFlagsStart)/8;
            if (_data.ActionFlagsStart > 0)
                actions = (_data.ActionsStart - _data.ActionFlagsStart) / 0x10;
            if (_data.SubactionFlagsStart > 0 && _data.SubactionMainStart > 0)
                subactions = (_data.SubactionMainStart - _data.SubactionFlagsStart) / 0x8;

            
            //Parse
            int count = 0;
            for (int i = 0; i < actions; i++)
                _actions.Add(new CommandList(this,_data.ActionsStart+ i*4, "Action " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = 0; i < subactions; i++)
                _subactionflags.Add(new SubactionFlags(this, _data.SubactionFlagsStart+i*8));
            count = 0;
            if(_data.SubactionMainStart > 0)
            for (int i = 0; i < subactions; i++)
                _subactionmain.Add(new CommandList(this, _data.SubactionMainStart + i * 4, "Subaction Main " + String.Format("0x{0:X}", count++), _subroutines));
            if (_data.SubactionGFXStart > 0)
                for (int i = 0; i < subactions; i++)
                    _subactiongfx.Add(new CommandList(this, _data.SubactionGFXStart + i * 4, "Subaction GFX " + String.Format("0x{0:X}", count++), _subroutines));
            if (_data.SubactionSFXStart > 0)
                for (int i = 0; i < subactions; i++)
                   _subactionsfx.Add(new CommandList(this, _data.SubactionSFXStart + i * 4, "Subaction SFX " + String.Format("0x{0:X}", count++), _subroutines));
            if (_data.OtherStart > 0)
                for (int i = 0; i < subactions; i++)
                    _subactionother.Add(new CommandList(this, _data.OtherStart + i * 4, "Subaction Other " + String.Format("0x{0:X}", count++), _subroutines));
            /*
            count = 0;
            for (int i = _data.SubactionMainStart; i < _data.SubactionGFXStart; i += 4)
                _subactionmain.Add(new CommandList(this, i, "Subaction Main " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _data.SubactionGFXStart; i < _data.SubactionSFXStart; i += 4)
                _subactiongfx.Add(new CommandList(this, i, "Subaction GFX " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _data.SubactionSFXStart; i < _data.SubactionOtherStart; i += 4)
                _subactionsfx.Add(new CommandList(this, i, "Subaction SFX " + String.Format("0x{0:X}", count++), _subroutines));
            count = 0;
            for (int i = _data.SubactionOtherStart; i < _data.SubactionOtherStart + _subactiongfx.Count * 4; i += 4)
                _subactionother.Add(new CommandList(this, i, "Subaction Other " + String.Format("0x{0:X}", count++), _subroutines));
            //Tree
             * */
            Children.Add(new NamedList(_actions, "Actions"));
            Children.Add(new NamedList(_subactionflags, "Subaction Flags"));
            Children.Add(new NamedList(_subactionmain, "Subaction Main"));
            Children.Add(new NamedList(_subactiongfx, "Subaction GFX"));
            Children.Add(new NamedList(_subactionsfx, "Subaction SFX"));
            Children.Add(new NamedList(_subactionother, "Subaction Other"));
            Children.Add(new NamedList(_subroutines.Values, "Subroutines"));
        }
    }
}
