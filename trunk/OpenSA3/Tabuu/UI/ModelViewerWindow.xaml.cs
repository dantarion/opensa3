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
        public int frame = 0;
        private CHR0Node curentAnimation;
        DispatcherTimer timer = new DispatcherTimer();
        GLContext ctx;
        DatFile datfile;
        public ModelViewerWindow(DatFile d)
        {
            this.datfile = d;
            InitializeComponent();
            windowsFormsHost1.Child = _mc;

            ctx = BrawlLib.OpenGL.GLContext.Attach(_mc);
            foreach (BoneRef boneref in d.GetDefaultHiddenBones())
            {
                int tmp = 0;
                var bone = d.Model.FindChild(boneref.BoneName, true);
                (bone as MDL0BoneNode)._render = false;
            }
            //Hitbox Rendering
            foreach (MDL0BoneNode bone in d.Model.FindChildrenByType(".",ResourceType.MDL0Bone))
            {
                if(bone.Name == "HipN")
                    ;// bone.CustomRenderEvent += RenderLedgegrab;
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
        /*
        public void RenderLedgegrab(object sender, EventArgs e)
        {
            var ledgegrab = datfile.getLedgegrabBox();
            MDL0BoneNode bone = (MDL0BoneNode)sender;
            bone.RenderBox(ctx, ledgegrab.X, ledgegrab.Y, ledgegrab.Width, ledgegrab.Height);
            //bone.RenderSphere(ctx, 4);
        }
         
        public void RenderHitboxes(object sender, EventArgs e)
        {
            MDL0BoneNode bone = (MDL0BoneNode)sender;
            bone.RenderSphere(ctx, 4);
        }
        */
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
            datfile.Model.ApplyCHR(curentAnimation, frame);
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
            datfile.Model.Detach(ctx);
            ctx.Release();
        }
    }
}
