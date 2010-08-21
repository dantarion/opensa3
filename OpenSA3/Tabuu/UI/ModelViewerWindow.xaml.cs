using System.Windows.Forms;
using OpenSALib3.DatHandler;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Threading;
using System;
using BrawlLib.OpenGL;
using OpenSALib3.Moveset;
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
        GLContext ctx;
        public ModelViewerWindow(DatFile d)
        {
            InitializeComponent();
            windowsFormsHost1.Child = _mc;
            model = d.Model;

            ctx = BrawlLib.OpenGL.GLContext.Attach(_mc);
            foreach (BoneRef boneref in d.GetDefaultHiddenBones())
            {
                int tmp = 0;
                var bone = d.Model.FindChild(boneref.BoneName, true);
                (bone as MDL0BoneNode)._render = false;
            }
            _mc.AddTarget(d.Model);
            _mc.AddReference(d.Node);
            var bone2 = (MDL0BoneNode)d.Model.FindChild("TopN", true);

            _mc.Rotate(0, -90);
            _mc.Translate(bone2.BoxMax._x/2, bone2.BoxMax._y/2, 30);
            
            d.Model.RenderBones = false;
            d.Model.RenderWireframe = false;
            timer.Tick += NextFrame;
            timer.Interval = new System.TimeSpan(166667);
        }
        public void Stop()
        {
            timer.Stop();
            slider.IsEnabled = true;
        }
        public void Play()
        {
            timer.Start();
            slider.IsEnabled = false;
        }
        public void SetFrame(int i)
        {
            frame = i;
            model.ApplyCHR(curentAnimation, frame);
            slider.Value = i;
            _mc.Invalidate();
        }
        public void NextFrame(object sender, EventArgs e)
        {
            if (curentAnimation == null)
                return;
            if (frame > curentAnimation.FrameCount)
            {
                frame = 0;
                if (!Loop.IsChecked.Value)
                    Stop();

            }
            SetFrame(++frame);
  
        }
        public void ChangeAnimation(CHR0Node animation)
        {
            Stop();
            label.Content = animation.Name;
            slider.Maximum = animation.FrameCount;
            slider.Value = 1;
            frame = 1;
            curentAnimation = animation;
            Play();
        }

        private void slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            SetFrame((int)slider.Value);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Play();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Stop();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.Detach(ctx);
            ctx.Release();
        }
    }
}
