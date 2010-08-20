using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace OpenSALib3.Utility
{
    static class PSANames
    {
        public static bool loaded = false;
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
        public static void loadData()
        {
            if (loaded)
                return;
            
            var t = new StringReader(Properties.Resources.Events);
            while (t.Peek() != -1)
            {
                string hex = t.ReadLine();
                EventData data = new EventData();
                data.Name = t.ReadLine();
                data.Description = t.ReadLine();
                string varname = t.ReadLine();
                while (varname.Length > 0)
                {
                    data.ParamNames.Add(varname);
                    varname = t.ReadLine();
                }
                EventNames.Add(hex, data);
            }
            t = new StringReader(Properties.Resources.Requirements);
            int i = 0;
            while (t.Peek() != -1)
            {
                ReqNames.Add(i, t.ReadLine());
                i++;
            }
            loaded = true;
        }
        public static String consoleHeader(String title)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("##########################################");
            sb.AppendLine(String.Format("# {0}", title));
            sb.AppendLine("##########################################");
            return sb.ToString();
        }
    }
}
