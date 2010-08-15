using System.Windows.Forms;
using OpenSALib3.DatHandler;
namespace Tabuu
{
    /// <summary>
    /// Interaction logic for ModelViewerWindow.xaml
    /// </summary>
    /// 

    public partial class ModelViewerWindow {
        private readonly ModelPanel _mc = new ModelPanel();
        public ModelViewerWindow(DatFile d)
        {
            InitializeComponent();
            windowsFormsHost1.Child = _mc;

            BrawlLib.OpenGL.GLContext.Attach(_mc);
            _mc.AddTarget(d.Model);
        }
    }
}
