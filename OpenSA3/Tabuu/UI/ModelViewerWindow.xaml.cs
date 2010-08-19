using System.Windows.Forms;
using OpenSALib3.DatHandler;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Threading;
using System;
namespace Tabuu.UI
{
    /// <summary>
    /// Interaction logic for ModelViewerWindow.xaml
    /// </summary>
    /// 

    public partial class ModelViewerWindow {
        private readonly ModelPanel _mc = new ModelPanel();
        private readonly MDL0Node model;
        public int frame = 0;
        private CHR0Node curentAnimation;
        DispatcherTimer timer = new DispatcherTimer();
        public ModelViewerWindow(DatFile d)
        {
            InitializeComponent();
            windowsFormsHost1.Child = _mc;
            model = d.Model;
            BrawlLib.OpenGL.GLContext.Attach(_mc);
          
            _mc.AddTarget(d.Model);
            _mc.AddReference(d.Node);
            timer.Tick += NextFrame;
            timer.Interval = new System.TimeSpan(166667);
        }
        public void NextFrame(object sender, EventArgs e)
        {
            model.ApplyCHR(curentAnimation, ++frame);
            if (frame > curentAnimation.FrameCount)
                frame = 0;
            slider.Value = frame;
            _mc.Invalidate();
        }
        public void ChangeAnimation(CHR0Node animation)
        {
            label.Content = animation.Name;
            slider.Maximum = animation.FrameCount;
            slider.Value = 0;
            frame = 0;
            curentAnimation = animation;
            timer.Start();
        }
    }
}
