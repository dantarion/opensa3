using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenSALib3;
using OpenSALib3.DatHandler;
using Tabuu.Utility;
using BrawlLib.IO;
namespace Tabuu.UI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public static RoutedCommand HexOpenCommand = new RoutedCommand();
        public static RoutedCommand SaveFileCommand = new RoutedCommand();
        public static RoutedCommand SaveFileAsCommand = new RoutedCommand();
        public static RoutedCommand CloseFileCommand = new RoutedCommand();

        public MainWindow() {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(HexOpenCommand, HexOpenCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(CloseFileCommand, CloseFileCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(SaveFileCommand, SaveFileCommandExecuted, AlwaysExecute));

            RunScript.LoadAssmbly(typeof(DatElementWrapper).Assembly);
            Focus();

        }

        private void MenuItemClickOpen(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            
            var fp = FileMap.FromFile(dialog.FileName, FileMapProtect.ReadWrite);
            var ds = new BrawlLib.SSBB.ResourceNodes.DataSource(fp);
            var rs = BrawlLib.SSBB.ResourceNodes.NodeFactory.FromSource(null,ds);
            TreeView.Items.Add(DatFile.FromNode(rs));
        }

        private void HexOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            e.Parameter.ToString();
            var d = e.Parameter as DatElement;
            new HexViewWindow(new DatElementWrapper(d),d.Name).Show();
        }
        private void CloseFileCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            e.Parameter.ToString();
            var d = e.Parameter as DatFile;
            if (d.Changed == true)
            {
                var result = MessageBox.Show("Unsaved changes detected! Are you sure you want to close this file?", "Close file?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;
            }
            TreeView.Items.Remove(e.Parameter);
        }
        private void SaveFileCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Parameter.ToString();
            var d = e.Parameter as DatFile;
            if (d.Changed == true)
            {
                var result = MessageBox.Show("This will overrite the file! Are you sure you want to save this file?", "Save file?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;
            } 
            var filename = d._node.RootNode.OriginalSource.Map.FilePath;

            d._node.Rebuild(true);
            d._node.RootNode.Merge();        
            d._node.RootNode.Export(filename);
        }
        private void AlwaysExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {

            RunScript.SetVar("loadedFiles", TreeView.Items);
            new ScriptWindow().Show();
        }
    }
}

