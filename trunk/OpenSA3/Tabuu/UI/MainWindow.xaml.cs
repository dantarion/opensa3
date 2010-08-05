using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenSALib3;
using OpenSALib3.DatHandler;
using Tabuu.Utility;
using BrawlLib.IO;
using System.Windows.Data;
using OpenSALib3.Moveset;
namespace Tabuu.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static RoutedCommand HexOpenCommand = new RoutedCommand();
        public static RoutedCommand ExamineCommand = new RoutedCommand();
        public static RoutedCommand LoadBoneCommand = new RoutedCommand();
        public static RoutedCommand SaveFileCommand = new RoutedCommand();
        public static RoutedCommand SaveFileAsCommand = new RoutedCommand();
        public static RoutedCommand CloseFileCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(HexOpenCommand, HexOpenCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(CloseFileCommand, CloseFileCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(SaveFileCommand, SaveFileCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(LoadBoneCommand, LoadBoneCommandExecuted, AlwaysExecute));

            RunScript.LoadAssembly(typeof(DatElementWrapper).Assembly);
            Focus();

        }

        private void MenuItemClickOpen(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            loadFilename(dialog.FileName);

        }
        private void MenuItemClickOpen2(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            string[] paths = {  @"captain\FitCaptain.pac",
                                @"dedede\FitDedede.pac",
                                @"diddy\FitDiddy.pac",
                                @"donkey\FitDonkey.pac",
                                @"falco\FitFalco.pac",
                                @"fox\FitFox.pac",
                                @"gamewatch\FitGameWatch.pac",
                                @"ganon\FitGanon.pac",
                                @"gkoopa\FitGKoopa.pac",
                                @"ike\FitIke.pac",
                                @"kirby\FitKirby.pac",
                                @"koopa\FitKoopa.pac",
                                @"link\FitLink.pac",
                                @"lucario\FitLucario.pac",
                                @"lucas\FitLucas.pac",
                                @"luigi\FitLuigi.pac",
                                @"mario\FitMario.pac",
                                @"marth\FitMarth.pac",
                                @"metaknight\FitMetaknight.pac",
                                @"ness\FitNess.pac",
                                @"peach\FitPeach.pac",
                                @"pikachu\FitPikachu.pac",
                                @"pikmin\FitPikmin.pac",
                                @"pit\FitPit.pac",
                                @"pokefushigisou\FitPokeFushigisou.pac",
                                @"pokelizardon\FitPokeLizardon.pac",
                                @"poketrainer\FitPokeTrainer.pac",
                                @"pokezenigame\FitPokeZenigame.pac",
                                @"popo\FitPopo.pac",
                                @"purin\FitPurin.pac",
                                @"robot\FitRobot.pac",
                                @"samus\FitSamus.pac",
                                @"sheik\FitSheik.pac",
                                @"snake\FitSnake.pac",
                                @"sonic\FitSonic.pac",
                                @"szerosuit\FitSZerosuit.pac",
                                @"toonlink\FitToonLink.pac",
                                @"wario\FitWario.pac",
                                @"warioman\FitWarioMan.pac",
                                @"wolf\FitWolf.pac",
                                @"yoshi\FitYoshi.pac",
                                @"zakoball\FitZakoBall.pac",
                                @"zakoboy\FitZakoBoy.pac",
                                @"zakochild\FitZakoChild.pac",
                                @"zakogirl\FitZakoGirl.pac",
                                @"zelda\FitZelda.pac"};
            foreach(string filename in paths)
                if(System.IO.File.Exists(dialog.SelectedPath+@"\" +filename))
                    loadFilename(dialog.SelectedPath + @"\" + filename);

        }
        private void loadFilename(string s)
        {
            var fp = FileMap.FromFile(s, FileMapProtect.ReadWrite);
            var ds = new BrawlLib.SSBB.ResourceNodes.DataSource(fp);
            var rs = BrawlLib.SSBB.ResourceNodes.NodeFactory.FromSource(null, ds);
            TreeView.Items.Add(DatFile.FromNode(rs));
        }
        private void LoadBoneCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            var d = e.Parameter as DatFile;
            d.readBoneNames(dialog.FileName);
        }
        private void HexOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Parameter.ToString();
            var d = e.Parameter as DatElement;
            new HexViewWindow(new DatElementWrapper(d), d.Name).Show();
        }
        private void CloseFileCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Parameter.ToString();
            var d = e.Parameter as DatFile;
            if (d.Changed == true)
            {
                var result = MessageBox.Show("Unsaved changes detected! Are you sure you want to close this file?", "Close file?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;
            }
            d._node.Rebuild(true);
            d._node.RootNode.Merge();
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
        private void AlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            RunScript.SetVar("loadedFiles", TreeView.Items);
            new ScriptWindow().Show();
        }

        private void ViewPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewPanel.Children.Clear();
            if (ViewPanel.DataContext is OpenSALib3.Utility.NamedList<Attribute>)
            {
                DataGrid dg = new DataGrid();
                dg.ItemsSource = ViewPanel.DataContext as System.Collections.IEnumerable;
                dg.AutoGenerateColumns = true;
                dg.CanUserSortColumns = false;
                dg.CanUserResizeRows = false;
                DataGridTextColumn col1 = new DataGridTextColumn();
                col1.Header = "Name";
                col1.Binding = new Binding(".");
                dg.Columns.Add(col1);

                DataGridTextColumn col2 = new DataGridTextColumn();
                col2.Header = "Value";
                col2.Binding = new Binding("Value");
                dg.Columns.Add(col2);

                dg.Margin = new Thickness(0);
                ViewPanel.Children.Add(dg);
            }
            if (ViewPanel.DataContext is OpenSALib3.Utility.NamedList<int>)
            {
                DataGrid dg = new DataGrid();
                dg.ItemsSource = ViewPanel.DataContext as System.Collections.IEnumerable;
                dg.AutoGenerateColumns = true;
                dg.CanUserSortColumns = false;
                dg.CanUserResizeRows = false;
                DataGridTextColumn col1 = new DataGridTextColumn();
                col1.Header = "Value";
                col1.Binding = new Binding(".");
                dg.Columns.Add(col1);
                dg.Margin = new Thickness(0);
                ViewPanel.Children.Add(dg);
            }
        }
    }
}

