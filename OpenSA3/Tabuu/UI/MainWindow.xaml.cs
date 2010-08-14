using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BrawlLib.IO;
using OpenSALib3;
using OpenSALib3.DatHandler;
using OpenSALib3.Moveset;
using Tabuu.Utility;

namespace Tabuu.UI {
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public static RoutedCommand HexOpenCommand = new RoutedCommand();
        public static RoutedCommand ModelOpenCommand = new RoutedCommand();
        public static RoutedCommand ExamineCommand = new RoutedCommand();
        public static RoutedCommand LoadModelCommand = new RoutedCommand();
        public static RoutedCommand SaveFileCommand = new RoutedCommand();
        public static RoutedCommand SaveFileAsCommand = new RoutedCommand();
        public static RoutedCommand CloseFileCommand = new RoutedCommand();

        public MainWindow() {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(HexOpenCommand, HexOpenCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(ModelOpenCommand, ModelOpenCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(CloseFileCommand, CloseFileCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(SaveFileCommand, SaveFileCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(LoadModelCommand, LoadModelCommandExecuted, AlwaysExecute));
            RunScript.LoadAssembly(typeof(DatElementWrapper).Assembly);
            Focus();
        }

        private void MenuItemClickOpen(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            LoadFilename(dialog.FileName);
        }

        private void MenuItemClickOpen2(object sender, RoutedEventArgs e) {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            string[] paths = {
                                 @"captain\FitCaptain.pac",
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
                                 @"zelda\FitZelda.pac"
                             };
            foreach (var filename in paths.Where(filename => System.IO.File.Exists(dialog.SelectedPath + @"\" + filename)))
                LoadFilename(dialog.SelectedPath + @"\" + filename);
        }

        private void LoadFilename(string s) {
            var fp = FileMap.FromFile(s, FileMapProtect.ReadWrite);
            var ds = new BrawlLib.SSBB.ResourceNodes.DataSource(fp);
            var rs = BrawlLib.SSBB.ResourceNodes.NodeFactory.FromSource(null, ds);
            TreeView.Items.Add(DatFile.FromNode(rs));
        }

        private static void LoadModelCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            var d = (DatFile)e.Parameter;
            d.LoadModel(dialog.FileName);
        }

        private static void HexOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            e.Parameter.ToString();
            var d = (DatElement)e.Parameter;
            new HexViewWindow(new DatElementWrapper(d), d.Name).Show();
        }
        private static void ModelOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

            e.Parameter.ToString();
            var d = (DatFile)e.Parameter;
            if(d.Model != null)
            new ModelViewerWindow(d).Show();
        }

        private void CloseFileCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            e.Parameter.ToString();
            var d = (DatFile)e.Parameter;
            if (d.Changed) {
                var result = MessageBox.Show("Unsaved changes detected! Are you sure you want to close this file?",
                                             "Close file?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;
            }
            d.Node.RootNode.Dispose();
            //d.Node.RootNode.Merge();
            TreeView.Items.Remove(e.Parameter);
        }

        private static void SaveFileCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            e.Parameter.ToString();
            var d = (DatFile)e.Parameter;
            if (d.Changed) {
                var result = MessageBox.Show("This will overrite the file! Are you sure you want to save this file?",
                                             "Save file?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;
            }
            var filename = d.Node.RootNode.OriginalSource.Map.FilePath;
            d.Node.Rebuild(true);
            d.Node.RootNode.Merge();
            d.Node.RootNode.Export(filename);
        }

        private static void AlwaysExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void MenuItemClick(object sender, RoutedEventArgs e) {
            RunScript.SetVar("loadedFiles", TreeView.Items);
            new ScriptWindow().Show();
        }

        private void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) { }
    }
}