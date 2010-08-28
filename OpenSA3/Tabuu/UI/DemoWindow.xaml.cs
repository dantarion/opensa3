using System.Windows;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using OpenSALib3.DatHandler;
using System.Collections.Generic;
using OpenSALib3.PSA;
using OpenSALib3;
namespace Tabuu.UI
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("import clr");
            sb.AppendLine(@"clr.AddReference(""OpenSALib3"")");
            sb.AppendLine("from OpenSALib3.CommandReceiver import *");
            intelliBox1.Text = sb.ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //set reference to CommandReceiver class.
            //CommandReceiver._commandList=～～～～～
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            ScriptSource source = engine.CreateScriptSourceFromString(intelliBox1.Text);
            source.Execute(scope);
            //actual conversation between python to C# will use CommandList class, and Parameter class
        }


    }
}
