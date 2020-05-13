using bui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace bui.GUI.ImageControls
{
    public partial class PanelKeyboard : ImageControl, IBitmapable
    {
        public event Action<int> OnDigit;
        public event Action OnDot;
        public event Action OnBackspace;
        ButtonImage[] buttons;
        public PanelKeyboard()
        {
            InitializeComponent();
            buttons = new ButtonImage[]
            {
                buttonImage0,
                buttonImage1,
                buttonImage2,
                buttonImage3,
                buttonImage4,
                buttonImage5,
                buttonImage6,
                buttonImage7,
                buttonImage8,
                buttonImage9,
            };
            float[] feqs = { 220, 246.94f, 261.63f, 293.66f, 329.63f, 349.23f, 415.3f, 440f, 493.88f, 523.26f, 587.32f, 659.26f };
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Images = new List<Bitmap> { Resources.btn_short_empty, Resources.btn_short_empty_pressed };
                int n = i;
                buttons[i].SetText($"{i}", Color.White);
                buttons[i].OnClick += (s, e) =>
                {
                    OnDigit?.Invoke(n);
                    this.Play($"-f {feqs[n]} -l 100");
                };
            }
            buttonImageBackspace.Images = new List<Bitmap> { Resources.btn_short_empty, Resources.btn_short_empty_pressed };
            buttonImageBackspace.SetText($"◀", Color.White);
            buttonImageBackspace.OnClick += (s, e) =>
            {
                this.Play($"-f {feqs[11]} -l 100");
                OnBackspace?.Invoke();
            };
            buttonImageDot.Images = new List<Bitmap> { Resources.btn_short_empty, Resources.btn_short_empty_pressed };
            buttonImageDot.SetText($".", Color.White);
            buttonImageDot.OnClick += (s, e) =>
            {
                this.Play($"-f {feqs[10]} -l 100");
                OnDot?.Invoke();
            };
        }

        public override Image ActiveBitmap
        {
            get
            {
                using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    foreach (var b in buttons)
                    {
                        if (b.IsDirty) g.DrawImageUnscaled(b.ActiveBitmap, b.Location);
                    }
                    if (buttonImageBackspace.IsDirty)
                        g.DrawImageUnscaled(buttonImageBackspace.ActiveBitmap, buttonImageBackspace.Location);
                    if (buttonImageDot.IsDirty)
                        g.DrawImageUnscaled(buttonImageDot.ActiveBitmap, buttonImageDot.Location);
                }
                IsDirty = false;
                return bmpActive;
            }
        }
        /*internal void Play(string v)
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.FileName = "beep";
                    myProcess.StartInfo.Arguments = v;
                    myProcess.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }*/
    }
}
