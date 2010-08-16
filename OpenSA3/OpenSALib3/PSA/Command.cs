using System;
using System.Collections.Generic;
using System.Drawing;
using OpenSALib3.DatHandler;

namespace OpenSALib3.PSA
{

    public class Command : DatElement
    {
        public static List<Command> ReadCommands(DatElement parent, uint offset, Dictionary<int, List<Command>> subroutinelist)
        {
            var list = new List<Command>();
            var command = new Command(parent, offset);
            while (command.Module != 0 || command.ID != 0)
            {
                offset += 8;
                list.Add(command);
                command = new Command(parent, offset);
            }
            list.Add(command);

            //Search for subroutines
            /*
            foreach (Command c in list)
            {
                if (c.Module == 0x00 && c.ID == 0x07)
                    if (c.RootFile.References.Count(x => x.DataOffset.Equals(c.data.ParameterOffset + 4)) == 0)
                        if ((c._children[0] as Parameter).RawData > 0)
                            subroutinelist[(c._children[0] as Parameter).RawData] = (ReadCommands(parent, (uint)(c._children[0] as Parameter).RawData, subroutinelist));
                if (c.Module == 0x0D && c.ID == 0x00)
                    subroutinelist[(c._children[1] as Parameter).RawData] = (ReadCommands(parent, (uint)(c._children[1] as Parameter).RawData, subroutinelist));
            }
             */
            return list;
        }

        struct Data
        {
            public byte Module;
            public byte ID;
            public byte ParameterCount;
            public byte Unknown;
            public bint ParameterOffset;
        }
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
        public unsafe Command(DatElement parent, uint offset)
            : base(parent, offset)
        {
            _data = *(Data*)(Address);
            Length = 8;
            Color = Color.Blue;
            Name = String.Format("{0:X02}{1:X02}{2:X02}{3:X02}", Module, ID, _data.ParameterCount, _data.Unknown);
            if (Module != 0 || ID != 0)
                for (var i = 0; i < _data.ParameterCount; i++)
                    Children.Add(new Parameter(this, (uint)(_data.ParameterOffset + i * 8)));
        }
    }
}
