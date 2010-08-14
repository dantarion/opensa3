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
using System.Windows.Shapes;
using System.Windows.Forms;
using OpenSALib3.DatHandler;
namespace Tabuu
{
    /// <summary>
    /// Interaction logic for ModelViewerWindow.xaml
    /// </summary>
    /// 

    public partial class ModelViewerWindow : Window
    {
        private ModelPanel mc = new ModelPanel();
        public ModelViewerWindow(DatFile d)
        {
            InitializeComponent();
            windowsFormsHost1.Child = mc;

            BrawlLib.OpenGL.GLContext.Attach(mc);
            mc.AddTarget(d.Model);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
