using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using OpenSALib3.DatHandler;

namespace OpenSALib3.PSA
{

    public class Command : DatElement
    {
        public static List<Command> ReadCommands(DatElement parent, int offset, List<List<Command>> subroutinelist)
        {
 
            var list = new List<Command>();
            if (offset == -1)
                return list;
            var command = new Command(parent, offset);
            while (command.Module != 0 || command.ID != 0)
            {
                offset += 8;
                list.Add(command);
                command = new Command(parent, offset);
            }
            list.Add(command);

            //Search for subroutines
            foreach (var c in list)
            {
                var suboff = -1;
                if (c.Module == 0x00 && (c.ID == 0x07||c.ID ==0x09) && subroutinelist != null)
                {
                    var paramOffset = c._data.ParameterOffset+ 4;
                    var ext = c.RootFile.IsExternal(paramOffset);
                    if (!ext)
                        suboff = ((Parameter) c[0]).RawData;
                    if (suboff > 0 && list[0].FileOffset != suboff && !subroutinelist.Exists(x => x[0].FileOffset == suboff))
                    {
                        subroutinelist.Add(ReadCommands(parent, suboff, subroutinelist));
                    }
                }
                else
                    if (c.Module == 0x0D && c.ID == 0x00 && subroutinelist != null)
                    {
                        suboff = ((Parameter) c[1]).RawData;
                        if (suboff > 0 && !subroutinelist.Exists(x => x[0].FileOffset == suboff))
                            subroutinelist.Add(ReadCommands(parent, suboff, subroutinelist));
                    }
            }
            return list;
        }
#pragma warning disable 169 //'Field ____ is never used'
#pragma warning disable 649 //'Field ____ is never assigned';
        struct Data
        {
            public byte Module;
            public byte ID;
            public byte ParameterCount;
            public byte Unknown;
            public bint ParameterOffset;
        }
#pragma warning restore 169 //'Field ____ is never used'
#pragma warning restore 649 //'Field ____ is never assigned';
        public byte Module
        {
            get { return _data.Module; }
            set { _data.Module = value; }
        }
        public byte ID
        {
            get { return _data.ID; }
            set { _data.ID = value; }
        }

        private Data _data;
        public unsafe Command(DatElement parent, int offset)
            : base(parent, offset)
        {
            _data = *(Data*)(base.Address);
            Length = 8;
            Color = Color.Blue;
            Name = String.Format("{0:X02}{1:X02}{2:X02}{3:X02}", Module, ID, _data.ParameterCount, _data.Unknown);
            if (Module == 0xFF)
                Debug.Fail("Command Module 0xFF");
            for (var i = 0; i < _data.ParameterCount; i++)
                this[i] = new Parameter(this, (_data.ParameterOffset + i * 8));
        }
    }

}
