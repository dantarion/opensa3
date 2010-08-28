using System;
using System.Diagnostics;
using System.Drawing;
using OpenSALib3.DatHandler;
using System.ComponentModel;
using OpenSALib3.Utility;

namespace OpenSALib3.PSA
{

    public class Command : DatElement
    {
        public static NamedList ReadCommands(DatElement parent, int offset, NamedList subroutinelist)
        {
            
            var count = 0;
            var list = new NamedList(parent,String.Format("@0x{0:X}", offset));
            if (offset == -1)
                return list;
            try
            {
                var command = new Command(parent, offset);
                while (command.Module != 0 || command.ID != 0)
                {
                    offset += 8;
                    list[count++] = command;
                    command = new Command(parent, offset);
                }
                list[count] = command;
            }
            catch (Exception exception)
            {
                Debug.Fail(exception.ToString());
            }
            //Search for subroutines
            foreach (Command c in list)
            {
                var suboff = -1;
                if (c.Module == 0x00 && (c.ID == 0x07 || c.ID == 0x09) && subroutinelist != null)
                {
                    var paramOffset = c._data.ParameterOffset + 4;
                    var ext = c.RootFile.IsExternal(paramOffset);
                    if (ext == null)
                        suboff = ((Parameter)c[0]).RawData;
                    else
                        c.Name += " - " + ext;
                    if (suboff > 0 && list[0].FileOffset != suboff)
                    {
                        subroutinelist[suboff] = (ReadCommands(parent, suboff, subroutinelist));
                    }
                }
                else
                    if (c.Module == 0x0D && c.ID == 0x00 && subroutinelist != null)
                    {
                        suboff = ((Parameter)c[1]).RawData;
                        if (suboff > 0)
                            subroutinelist[suboff] = (ReadCommands(parent, suboff, subroutinelist));
                    }
                    else if (c.Module == 0x00 && c.ID == 0x09 && subroutinelist != null)
                    {
                        suboff = ((Parameter)c[0]).RawData;
                        if (suboff > 0)
                            subroutinelist[suboff] = ReadCommands(parent, suboff, subroutinelist);
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
        [Category("Command")]
        public byte Module
        {
            get { return _data.Module; }
            set { _data.Module = value; NotifyChanged("Module"); NotifyChanged("Name"); }
        }
        [Category("Command")]
        public byte ID
        {
            get { return _data.ID; }
            set { _data.ID = value; NotifyChanged("ID"); NotifyChanged("Name"); }
        }

        private Data _data;
        public unsafe Command(DatElement parent, int offset)
            : base(parent, offset)
        {
            base.TreeColor = null;
            _data = *(Data*)(base.Address);
            Length = 8;
            Color = Color.Blue;
            PSANames.LoadData();
            var shortcommand = string.Format("{0:X02}{1:X02}", Module, ID);
            PSANames.EventData eventdata;
            PSANames.EventNames.TryGetValue(shortcommand, out eventdata);
            base.Name = eventdata == null 
                ? String.Format("{0:X02}{1:X02}{2:X02}{3:X02}", Module, ID, _data.ParameterCount, _data.Unknown)
                : eventdata.Name;

            for (var i = 0; i < _data.ParameterCount; i++)
            {
                var param = Parameter.Factory(this, (_data.ParameterOffset + i * 8));
                this[i] = param;
                if (eventdata != null && eventdata.ParamNames.Count > i)
                    param.Name = eventdata.ParamNames[i];
            }
        }
        #region FrameData Properties
        [Browsable(true)]
        public int Frame
        {
            get
            {
                var list = Parent;
                var frame = 0;
                foreach (Command cur in list)
                {
                    if (cur.Module == 0 && cur.ID == 1)
                        frame += (int)((ScalarParameter) cur[0]).Value;
                    if (cur.Module == 0 && cur.ID == 2)
                        frame = (int)((ScalarParameter) cur[0]).Value;
                    if (cur == this)
                        return frame;
                }
                return -1;
            }
        }
        public struct HitBoxData
        {
            public float X;
            public float Y;
            public float Z;

        }
        #endregion
    }

}
