using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace OpenSALib3.Utility
{
    public static class PSANames
    {
        public static bool Loaded { get; private set; }
        public static Dictionary<string, EventData> EventNames = new Dictionary<string, EventData>();
        public static Dictionary<int, string> ReqNames = new Dictionary<int, string>();
        public class EventData
        {
            public String Name;
            public String Description;
            public List<string> ParamNames = new List<string>();
        }
        public static String VersionString()
        {
            return "OpenSALib3 Version " + Assembly.GetExecutingAssembly().GetName().Version;
        }
        public static void LoadData()
        {
            if (Loaded)
                return;
            
            var t = new StringReader(Properties.Resources.Events);
            while (t.Peek() != -1)
            {
                var hex = t.ReadLine();
                var data = new EventData {Name = t.ReadLine(), Description = t.ReadLine()};
                var varname = t.ReadLine();
                if (varname == null) break;
                while (varname.Length > 0)
                {
                    data.ParamNames.Add(varname);
                    varname = t.ReadLine();
                }
                EventNames.Add(hex, data);
            }
            t = new StringReader(Properties.Resources.Requirements);
            var i = 0;
            while (t.Peek() != -1)
            {
                ReqNames.Add(i, t.ReadLine());
                i++;
            }
            Loaded = true;
        }
        public static String ConsoleHeader(String title)
        {
            return String.Format(
                "##########################################" + "\n" +
                "# {0}"                                      + "\n" + 
                "##########################################"        ,
                title);
        }
    }
}
