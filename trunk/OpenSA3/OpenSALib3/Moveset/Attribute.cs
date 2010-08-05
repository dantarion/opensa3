using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Moveset
{
    public class Attribute : DatElement
    {
        struct Metadata
        {
            public string Name;
            public string Description;
            public Type Type;
        }
        static Dictionary<uint,Metadata> DB;
        static void SetupDB()
        {
            if (DB != null)
                return;
            DB = new Dictionary<uint, Metadata>();
            var sr = new StringReader(OpenSALib3.Properties.Resources.Attributes);
            while (sr.Peek() != -1)
            {
                var line1 = sr.ReadLine();
                var am = new Attribute.Metadata();
                
                if (line1.StartsWith("*"))
                {
                    line1 = line1.Substring(1);
                    am.Type = typeof(int);
                }
                else
                    am.Type = typeof(float);
                var offset = uint.Parse(line1.Substring(2, 3), System.Globalization.NumberStyles.HexNumber);
                if(line1.Length > 6)
                    am.Name = line1.Substring(6);
                else
                    am.Name = String.Format("0x{0:X03}", offset);
                am.Description = sr.ReadLine();
                DB[offset] = am;
                sr.ReadLine();
                sr.ReadLine();
            }
        }
        public Attribute(DatElement parent,uint FileOffset) : base(parent,FileOffset)
        {
            Length = 4;
            SetupDB();
            if (DB.ContainsKey(FileOffset))
                Name = DB[FileOffset].Name;
            else Name = String.Format("0x{0:X03}", FileOffset);
        }
        public string Description
        {
            get { return DB.ContainsKey(FileOffset) ? DB[FileOffset].Description : "Unknown"; }
        }
        public Type Type
        {
            get { return DB.ContainsKey(FileOffset) ? DB[FileOffset].Type : typeof(float); }
        }
        public unsafe object Value
        {
            get { 

                if(this.Type == typeof(float))
                    return (float)*(bfloat*)(RootFile.Address+FileOffset);
                return (int)*(bint*)(RootFile.Address + FileOffset);  
            }
            set
            {
                if (this.Type == typeof(float))
                    *(bfloat*)(RootFile.Address + FileOffset) = Convert.ToSingle(value);
                else
                    *(bint*)(RootFile.Address + FileOffset) = Convert.ToInt32(value);  
            }
        }
    }
}
