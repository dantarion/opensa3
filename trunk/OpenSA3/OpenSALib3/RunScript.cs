using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3;
using IronPython;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using Microsoft.Scripting.Hosting;
namespace OpenSALib3
{
    public class RunScript
    {
        private static ScriptRuntime runtime;
        private static ScriptScope scope;
        private static ScriptEngine engine   = IronPython.Hosting.Python.CreateEngine();
        private static StringBuilder sb = new StringBuilder();
        public static void ResetScope()
        {
            scope = null;
        }
        public static void LoadAssmbly(System.Reflection.Assembly asm)
        {
            engine.Runtime.LoadAssembly(asm);
            scope = null;
        }
        public static void SetVar(String str, object obj)
        {
            setupScope();
            scope.SetVariable(str, obj);
        }
        public static void setupScope()
        {
           if (scope == null)
                {
                    
                    
                    runtime = engine.Runtime; 
                    runtime.LoadAssembly(typeof(DatFile).Assembly);
                    runtime.LoadAssembly(typeof(ResourceNode).Assembly);
                                   
                    scope = engine.CreateScope();
                }
        }
        public static String FromString(String str, DatFile file)
        {
            try
            {
                setupScope();
                var ms = new MemoryStream();
                sb.Clear();
                runtime.IO.SetOutput(ms, new StringWriter(sb));
                scope.SetVariable("currentDat", file);
                var script = engine.CreateScriptSourceFromString(str);
                
                script.Execute(scope);
                ms.Flush();
                return sb.ToString();
            }
            catch (Exception err)
            {
                return err.Message+"\n";
            }
        }

    }
}
