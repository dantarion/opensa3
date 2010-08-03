using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenSALib3;
using Tabuu.Utility;
namespace Tabuu
{
    namespace UI
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
        {
            public static RoutedCommand HexOpenCommand = new RoutedCommand();
            public static RoutedCommand CloseFileCommand = new RoutedCommand();

            public MainWindow()
            {
                InitializeComponent();
                CommandBindings.Add(new CommandBinding(HexOpenCommand, HexOpenCommand_Executed, AlwaysExecute));
                CommandBindings.Add(new CommandBinding(CloseFileCommand, CloseFileCommand_Executed, AlwaysExecute));
                RunScript.LoadAssmbly(typeof(DatWrapper).Assembly);
                Focus();

            }

            private void MenuItem_ClickOpen(object sender, RoutedEventArgs e)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                if (!dialog.ShowDialog().Value)
                    return;
                TreeView.Items.Add(DatWrapper.fromFile(dialog.FileName));
            }

            private void HexOpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                e.Parameter.ToString();
                DatSection d = e.Parameter as DatSection;
                new HexViewWindow(d.RootFile as DatWrapper, d.DataOffset, d.Name,d.Length).Show();
            }
            private void CloseFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                e.Parameter.ToString();
                DatSection d = e.Parameter as DatSection;

                TreeView.Items.Remove(e.Parameter);
            }

            private void AlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
            }

            private void MenuItem_Click(object sender, RoutedEventArgs e)
            {

                RunScript.SetVar("loadedFiles", TreeView.Items);
                new ScriptWindow().Show();
            }
        }
    }
}
