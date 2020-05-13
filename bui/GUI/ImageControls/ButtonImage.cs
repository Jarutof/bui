using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bui.Properties;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;

namespace bui.GUI
{
    public partial class ButtonImage : ImageControl, IBitmapable, IPressable
    {
        /*private Point location;
        private Bitmap bmpBack;
        private Bitmap bmpActive;*/
        
        public new event EventHandler OnClick;
        public override event Action<Bitmap, Point> OnDirty;

        public List<Bitmap> Images { get; set; } = new List<Bitmap>();
        //private bool isDirty = false;
        public Label ContentLabel => contentLabel;
        public bool CanPress => Visible;
        public override int Display { get { return ((IBitmapable)Parent).Display; } }
        public bool Disabled { get; set; }
        private readonly SolidBrush textBrush = new SolidBrush(Color.White);
        private readonly StringBuilder textBuilder = new StringBuilder();
        private Bitmap bmpPressed;
        private bool isPressed;
        private int tempState;
        private int state = 0;
        public ButtonImage()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;
        }

        /*public Task<(Bitmap, Point)> GetBitmap(Bitmap back)
        {
            Console.WriteLine($"GetBitmap {state}");
            using (Graphics g = Graphics.FromImage(back))
            {
                //Images[state].MakeTransparent(Images[state].GetPixel(0,0));
                using (var attribs = new ImageAttributes())
                {
                    //attribs.SetColorKey(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(100, 255, 255, 255));
                    //g.DrawImage(Images[state], new Rectangle(Point.Empty, back.Size), 0,0, back.Width, back.Height, GraphicsUnit.Pixel, attribs);
                    g.DrawImage(Images[state], new Rectangle(Point.Empty, back.Size), 0,0, back.Width, back.Height, GraphicsUnit.Pixel);

                }
                //g.DrawImage(Resources.trans, 0,0, Width,Height);
                StringFormat format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                g.DrawString(textBuilder.ToString(), ContentLabel.Font, textBrush, ContentLabel.DisplayRectangle, format);

            }
            back.Save("back.png");
            isDirty = false;
            return Task.FromResult((back, Location));
        }*/

        public void Press()
        {
            if (Disabled || isHidden) return;
            //tempState = state;
            //state++;
            //Repaint();
            IsDirty = true;
            OnDirty?.Invoke(bmpPressed, location);
            isPressed = true;
            //Console.WriteLine("Press");
        }

        public void Release()
        {
            if (Disabled || isHidden) return;
            //state = tempState;
            //Repaint();
            IsDirty = true;
            OnDirty?.Invoke(bmpActive, location);
            OnClick?.Invoke(this, EventArgs.Empty);
            isPressed = false;
            //Console.WriteLine("Release");
        }

        public bool IsHit(Point location) => (new Rectangle(LocationAbsolute, Size).Contains(location));

        private void SetText(string text)
        {
            if (textBuilder.ToString() != text)
            {
                textBuilder.Clear();
                textBuilder.Append(text);
                IsDirty = true;
            }
        }
        private void SetState(int value)
        {
            if (state != value && value >= 0 && value < Images.Count)
            {
                state = value;
                IsDirty = true;
                /*if (isPressed) tempState = value;
                else
                {
                    state = value;
                    IsDirty = true;

                }*/
            }
        }
        public void SetText(string text, Color color)
        {
            if (textBrush.Color!= color)
            {
                textBrush.Color = color;
                IsDirty = true;
            }
            SetText(text);
        }

        object lockobj = new object();
        private void Repaint()
        {
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(bmpBack, Point.Empty);
                g.DrawImage(Images[state], 0, 0, bmpActive.Width, bmpActive.Height);
                StringFormat format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                g.DrawString(textBuilder.ToString(), ContentLabel.Font, textBrush, ContentLabel.DisplayRectangle, format);
            }
            using (Graphics g = Graphics.FromImage(bmpPressed))
            {
                g.DrawImageUnscaled(bmpBack, Point.Empty);
                g.DrawImage(Images[state + 1], 0, 0, bmpActive.Width, bmpActive.Height);
                StringFormat format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                g.DrawString(textBuilder.ToString(), ContentLabel.Font, textBrush, ContentLabel.DisplayRectangle, format);
            }
        }
        private async Task RepaintAsync()
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
            syncContext.Post(obj =>
            {
                Repaint();
                source.TrySetResult(true);
            }, null);
            await source.Task;
        }
        public void UpdateState(int state, string text, Color color)
        {
            SetState(state);
            SetText(text, color);
            if (IsDirty) Repaint();
            
        }
        public async Task<(Bitmap, Point, bool)> MakeDirty(int state, string text, Color color)
        {
            SetState(state);
            SetText(text, color);
            if (IsDirty)
            {
                await RepaintAsync();
                IsDirty = false;
                return await Task.FromResult(((Bitmap)ActiveBitmap, location, true));
            }

            return await Task.FromResult(((Bitmap)ActiveBitmap, location, false));
        }
        public override void ApplyBackground(Bitmap bitmap, Point point)
        {
            base.ApplyBackground(bitmap, point);
            bmpPressed = new Bitmap(bitmap.Width, bitmap.Height);
            Repaint();
        }
        

        public override Image ActiveBitmap
        {
            get
            {
                IsDirty = false;
                if (isHidden) return bmpBack;
                if (isPressed)
                    return bmpPressed;
                else
                    return bmpActive;
            }
        }
        /*public void TransferBackground(Bitmap bitmap, Point pont)
        {
            location = pont;
            bmpBack = bitmap;
            bmpActive = new Bitmap(bmpBack.Width, bmpBack.Height);
        }*/
    }
}
