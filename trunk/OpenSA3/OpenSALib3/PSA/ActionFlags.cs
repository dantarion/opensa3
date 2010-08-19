using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
using System.ComponentModel;
using OpenSALib3.Utility;
using System.Activities.Presentation.PropertyEditing;
namespace OpenSALib3.PSA
{
    public static class Extensions
    {
        public static buint RotateLeft(this buint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static buint RotateRight(this buint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }
        public static buint rlwinm(this buint value, int rot, uint maskleft, uint maskright)
        {
            buint mask = 1;
            for (buint i = maskleft; i < maskright; i++)
                mask += (32 - i);

            return value.RotateLeft(rot) & mask;
        }
    }
    public class ActionFlags : DatElement
    {
        struct Data
        {
            public bint Unknown1;
            public bint Unknown2;
            public bint Unknown3;
            public buint Flags;
        }



        private Data _data;
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
        [Editor(typeof(HexPropertyEditor), typeof(PropertyValueEditor))]
        public uint RawFlags
        {
            get { return _data.Flags; }
            set { _data.Flags = value; }
        }
        [Browsable(true)]
        public uint UnknownFlag1
        {
            get
            {
                return _data.Flags.RotateLeft(24) & 0xF;
            }
        }
        [Browsable(true)]
        public uint StageCollision
        {
            get
            {
                return _data.Flags.rlwinm(8, 28, 31);
            }
        }
        [Browsable(true)]
        public uint Ledgegrab
        {
            get
            {
                return _data.Flags.rlwinm(11, 29, 31);
            }
        }
        [Browsable(true)]
        public uint UnknownFlag2
        {
            get
            {
                return _data.Flags.rlwinm(11, 0, 8).RotateLeft(24);
            }
        }
        [Browsable(true)]
        public uint Floating
        {
            get
            {
                return _data.Flags.rlwinm(20, 31, 31);
            }
        }
        public unsafe ActionFlags(DatElement parent, int offset, int index)
            : base(parent, offset)
        {
            _data = *(Data*)Address;
            Name = String.Format("{0:X03}",index);
            Length = 16;
        }
    }
}
