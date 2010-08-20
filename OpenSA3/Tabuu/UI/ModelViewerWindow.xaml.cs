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
            if (frame > curentAnimation.FrameCount)
            {
                frame = 1;
                if (!Loop.IsChecked.Value)
                    Stop();

            }
            SetFrame(frame++);
  
        }
        public void ChangeAnimation(CHR0Node animation)
        {
            //timer.Stop();
            label.Content = animation.Name;
            slider.Maximum = animation.FrameCount;
            slider.Value = 1;
            frame = 1;
            curentAnimation = animation;
            timer.Start();
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
    }
}
