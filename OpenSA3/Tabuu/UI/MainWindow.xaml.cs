using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenSALib3;
using OpenSALib3.DatHandler;
using Tabuu.UI.UI;
using Tabuu.Utility;
using Tabuu.Utility.Utility;

namespace Tabuu.UI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public static RoutedCommand HexOpenCommand = new RoutedCommand();
        public static RoutedCommand CloseFileCommand = new RoutedCommand();

        public MainWindow() {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(HexOpenCommand, HexOpenCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(CloseFileCommand, CloseFileCommandExecuted, AlwaysExecute));
            RunScript.LoadAssmbly(typeof(DatWrapper).Assembly);
            Focus();

        }

        private void MenuItemClickOpen(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            TreeView.Items.Add(DatWrapper.FromFile(dialog.FileName));
        }

        private void HexOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            e.Parameter.ToString();
            var d = e.Parameter as DatSection;
            new HexViewWindow(d.RootFile as DatWrapper, d.DataOffset, d.Name, d.Length).Show();
        }
        private void CloseFileCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            e.Parameter.ToString();
            var d = e.Parameter as DatSection;

            TreeView.Items.Remove(e.Parameter);
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

