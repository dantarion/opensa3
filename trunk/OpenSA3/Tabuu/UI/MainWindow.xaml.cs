using System.Linq;
using System.Windows;
using System.Windows.Input;
using BrawlLib.IO;
using OpenSALib3;
using OpenSALib3.DatHandler;
using Tabuu.Utility;
using OpenSALib3.PSA;
using System.Windows.Threading;
using System.Threading;
using System;
using System.ComponentModel;

namespace Tabuu.UI
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        
        /* Context Menu Commands */
        public static RoutedCommand HexOpenCommand = new RoutedCommand();
        public static RoutedCommand HexOpenContentCommand = new RoutedCommand();
        public static RoutedCommand ModelOpenCommand = new RoutedCommand();
        public static RoutedCommand ExamineCommand = new RoutedCommand();
        public static RoutedCommand LoadModelCommand = new RoutedCommand();
        public static RoutedCommand LoadAnimationCommand = new RoutedCommand();
        public static RoutedCommand SaveFileCommand = new RoutedCommand();
        public static RoutedCommand SaveFileAsCommand = new RoutedCommand();
        public static RoutedCommand CloseFileCommand = new RoutedCommand();
        /* Instance */
        public static ModelViewerWindow ModelViewer;

        public MainWindow()
        {
            InitializeComponent();
            /* Command Bindings for Context Menu commands */
            CommandBindings.Add(new CommandBinding(HexOpenCommand, HexOpenCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(HexOpenContentCommand, HexOpenContentCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(ModelOpenCommand, ModelOpenCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(CloseFileCommand, CloseFileCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(SaveFileCommand, SaveFileCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(LoadModelCommand, LoadModelCommandExecuted, AlwaysExecute));
            CommandBindings.Add(new CommandBinding(LoadAnimationCommand, LoadAnimationCommandExecuted, AlwaysExecute));
            /* Load Tabuu Assembly for scripting*/
            RunScript.LoadAssembly(typeof(DatElementWrapper).Assembly);
            /* Fixes rightclicking on TreeView nodes*/
            Focus();
        }
        #region Menu Commands
        /* Open File*/
        private void MenuItemClickOpen(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            LoadFilename(dialog.FileName);
        }
        /* Open /fighter/ */
        private void MenuItemClickOpen2(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            foreach (var filename in Constants.FilePaths.Where(filename => System.IO.File.Exists(dialog.SelectedPath + @"\" + filename)))
            {
                Thread.Sleep(0);
                LoadFilename(dialog.SelectedPath + @"\" + filename);
                ProgressBar.Value++;
                ProgressBar.InvalidateVisual();
            }
        }
        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            RunScript.SetVar("loadedFiles", TreeView.Items);
            new ScriptWindow().Show();
        }
        #endregion
        
        #region Context Menu Commands
        private static void LoadModelCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            var d = (DatFile)e.Parameter;
            (sender as MainWindow).LoadModel(d, dialog.FileName);
        }
        private static void LoadAnimationCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            var d = (DatFile)e.Parameter;
            (sender as MainWindow).LoadAnimations(d, dialog.FileName);

        }
        private static void HexOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Parameter.ToString();
            var d = (DatElement)e.Parameter;
            new HexViewWindow(new DatElementWrapper(d), d.Path).Show();
        }
        private static void HexOpenContentCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Parameter.ToString();
            var d = (DatElement)e.Parameter;
            new HexViewWindow(new DatElementWrapper(d, true), d.Path).Show();
        }
        private static void ModelOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ModelViewer != null)
                ModelViewer.Close();
            e.Parameter.ToString();
            var d = (DatFile)e.Parameter;
            if (d.Model != null)
            {
                ModelViewer = new ModelViewerWindow(d);
                ModelViewer.Show();
            }
        }
        private void CloseFileCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Parameter.ToString();
            var d = (DatFile)e.Parameter;
            if (d.IsChanged)
            {
                var result = MessageBox.Show("Unsaved changes detected! Are you sure you want to close this file?",
                                             "Close file?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;
            }
            d.Node.RootNode.Dispose();
            //d.Node.RootNode.Merge();
            TreeView.Items.Remove(e.Parameter);
        }
        private static void SaveFileCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Parameter.ToString();
            var d = (DatFile)e.Parameter;
            if (d.IsChanged)
            {
                var result = MessageBox.Show("This will overrite the file! Are you sure you want to save this file?",
                                             "Save file?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;
            }
            var filename = d.Node.RootNode.FilePath;
            d.Node.Rebuild(true);
            d.Node.RootNode.Merge();
            d.Node.RootNode.Export(filename);
            d.MarkClean();
        }

        private static void AlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        
        #region Functions
        private void LoadFilename(string s)
        {
            try
            {
                var fp = FileMap.FromFile(s, FileMapProtect.ReadWrite);
                var ds = new BrawlLib.SSBB.ResourceNodes.DataSource(fp);
                var rs = BrawlLib.SSBB.ResourceNodes.NodeFactory.FromSource(null, ds);
                var list = rs.FindChildrenByType(".", BrawlLib.SSBB.ResourceNodes.ResourceType.ARCEntry).Where(x => x.Name.Contains("MiscData"));
                foreach (var node in list.Where(x => DatFile.IsDatFile(x)))
                {
                    var df = DatFile.FromNode(node);
                    TreeView.Items.Add(df);
                    if (AutoLoadResources.IsChecked.Value)
                    {
                        if (System.IO.File.Exists(s.Replace(".pac", "00.pac")))
                            df.LoadModel(s.Replace(".pac", "00.pac"));
                        if (System.IO.File.Exists(s.Replace(".pac", "Motion.pac")))
                            df.LoadAnimations(s.Replace(".pac", "Motion.pac"));
                        if (System.IO.File.Exists(s.Replace(".pac", "MotionEtc.pac")))
                            df.LoadAnimations(s.Replace(".pac", "MotionEtc.pac"));
                    }

                }
                rs.Rebuild(true);
                rs.Merge();
            }
            catch (System.IO.IOException err)
            {
                StatusLabel.Content = err.Message;
            }
        }
        public void LoadModel(DatFile d, string s)
        {
            d.LoadModel(s);
            TreeView.Items.Refresh();
        }
        public void LoadAnimations(DatFile d, string s)
        {
            d.LoadAnimations(s);
            TreeView.Items.Refresh();
        }
        #endregion

        #region Events
        private void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (ModelViewer != null && e.NewValue is OpenSALib3.PSA.SubactionFlags)
            {
                SubactionFlags flags = (SubactionFlags)e.NewValue;
                if (flags.RootFile.Animations.ContainsKey(flags.AnimationName))
                {
                    ModelViewer.ChangeAnimation(flags.RootFile.Animations[flags.AnimationName]);
                }
            }
            else if (ModelViewer != null && e.NewValue is OpenSALib3.PSA.Command)
            {
                Command c = (Command)e.NewValue;
                var flags = (c.Parent.Parent["Flags"] as SubactionFlags);
                if (flags != null && flags.RootFile.Animations.ContainsKey(flags.AnimationName))
                {
                    ModelViewer.ChangeAnimation(flags.RootFile.Animations[flags.AnimationName]);
                    ModelViewer.SetFrame(c.Frame);
                    ModelViewer.Stop();
                }

            }
        }
        #endregion
    }
}