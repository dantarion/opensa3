using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using OpenSALib3.DatHandler;

namespace OpenSALib3
{
    public static class RunScript
    {
        private static ScriptRuntime _runtime;
        private static ScriptScope _scope;
        private static readonly ScriptEngine Engine = Python.CreateEngine();
        private static readonly StringBuilder SB = new StringBuilder();

        public static void ResetScope()
        {
            _scope = null;
        }

        public static void LoadAssembly(Assembly asm)
        {
            Engine.Runtime.LoadAssembly(asm);
            _scope = null;
        }

        public static void SetVar(String key, object value)
        {
            SetupScope();
            _scope.SetVariable(key, value);
        }

        public static void SetupScope()
        {
            if (_scope != null) return;
            _runtime = Engine.Runtime;
            var EntryAssembly = Assembly.GetEntryAssembly();
            foreach (AssemblyName Name in EntryAssembly.GetReferencedAssemblies())
                _runtime.LoadAssembly(Assembly.LoadWithPartialName(Name.FullName));
            _runtime.LoadAssembly(typeof(DatFile).Assembly);
            _runtime.LoadAssembly(typeof(ResourceNode).Assembly);
            _scope = Engine.CreateScope();
        }

        public static String FromString(String script, DatFile file)
        {
            try
            {
                SetupScope();
                var ms = new MemoryStream();
                SB.Clear();
                _runtime.IO.SetOutput(ms, new StringWriter(SB, new CultureInfo("en-US")));
                _scope.SetVariable("currentDat", file);
                var compiledscript = Engine.CreateScriptSourceFromString(script);
                compiledscript.Execute(_scope);
                ms.Flush();
                return SB.ToString();
            }
            catch (Exception err)
            {
                return err.Message + "\n";
            }
        }
    }
}