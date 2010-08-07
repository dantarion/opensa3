using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset {
    public class Attribute : DatElement {
        #region Static functions
        private struct Metadata {
            public string Name;
            public string Description;
            public Type Type;
        }

        private static Dictionary<uint, Metadata> _db;

        private static void SetupDB() {
            if (_db != null)
                return;
            _db = new Dictionary<uint, Metadata>();
            var sr = new StringReader(Properties.Resources.Attributes);
            while (sr.Peek() != -1) {
                var line1 = sr.ReadLine();
                Debug.Assert(line1 != null);
                var am = new Metadata();
                if (line1.StartsWith("*")) {
                    line1 = line1.Substring(1);
                    am.Type = typeof (int);
                } else
                    am.Type = typeof (float);
                var offset = uint.Parse(line1.Substring(2, 3), System.Globalization.NumberStyles.HexNumber);
                am.Name = line1.Length > 6
                    ? line1.Substring(6)
                    : String.Format("0x{0:X03}", offset);
                am.Description = sr.ReadLine();
                _db[offset] = am;
                sr.ReadLine();
                sr.ReadLine();
            }
        }
        #endregion

        public Attribute(DatElement parent, uint fileOffset) : base(parent, fileOffset) {
            Length = 4;
            SetupDB();
            Name = _db.ContainsKey(fileOffset)
                 ? _db[fileOffset].Name
                 : String.Format("0x{0:X03}", fileOffset);
        }

        [Category("Attribute")]
        public string Description {
            get { return _db.ContainsKey(FileOffset) ? _db[FileOffset].Description : "Unknown"; }
        }

        [Category("Attribute")]
        public Type Type {
            get { return _db.ContainsKey(FileOffset) ? _db[FileOffset].Type : typeof (float); }
        }

        [Category("Attribute")]
        public unsafe object Value {
            get {
                if (Type == typeof (float))
                    return (float) *(bfloat*) (RootFile.Address + FileOffset);
                return (int) *(bint*) (RootFile.Address + FileOffset);
            }
            set {
                if (Type == typeof (float))
                    *(bfloat*) (RootFile.Address + FileOffset) = Convert.ToSingle(value);
                else
                    *(bint*) (RootFile.Address + FileOffset) = Convert.ToInt32(value);
            }
        }
    }
}