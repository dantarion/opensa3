﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using System.Diagnostics;
namespace OpenSALib3.PSA
{

    public class Command : DatElement
    {
        public static List<Command> ReadCommands(DatElement parent, int offset, List<List<Command>> subroutinelist)
        {
 
            var list = new List<Command>();
            if (offset == -1)
                return list;
            Command command = new Command(parent, offset);
            while (command.Module != 0 || command.ID != 0)
            {
                offset += 8;
                list.Add(command);
                command = new Command(parent, offset);
            }
            list.Add(command);

            //Search for subroutines
            foreach (Command c in list)
            {
                int suboff = -1;
                if (c.Module == 0x00 && (c.ID == 0x07||c.ID ==0x09) && subroutinelist != null)
                {
                    int ParamOffset = c.data.ParameterOffset+ 4;
                    var ext = c.RootFile.isExternal(ParamOffset);
                    if (!ext)
                        suboff = (c[0] as Parameter).RawData;
                    if (suboff > 0 && list[0].FileOffset != suboff && !subroutinelist.Exists(x => x[0].FileOffset == suboff))
                    {
                        subroutinelist.Add(ReadCommands(parent, suboff, subroutinelist));
                    }
                }
                else
                    if (c.Module == 0x0D && c.ID == 0x00 && subroutinelist != null)
                    {
                        suboff = (c[1] as Parameter).RawData;
                        if (suboff > 0 && !subroutinelist.Exists(x => x[0].FileOffset == suboff))
                            subroutinelist.Add(ReadCommands(parent, suboff, subroutinelist));
                    }
            }
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
            get { return data.Module; }
            set { data.Module = value; }
        }
        public byte ID
        {
            get { return data.ID; }
            set { data.ID = value; }
        }

        private Data data;
        public unsafe Command(DatElement parent, int offset)
            : base(parent, offset)
        {
            data = *(Data*)(Address);
            Length = 8;
            Color = System.Drawing.Color.Blue;
            Name = String.Format("{0:X02}{1:X02}{2:X02}{3:X02}", Module, ID, data.ParameterCount, data.Unknown);
            if (Module == 0xFF)
                Debug.Fail("Command Module 0xFF");
            for (int i = 0; i < data.ParameterCount; i++)
                this[i] = new Parameter(this, (int)(data.ParameterOffset + i * 8));
        }
    }

}
