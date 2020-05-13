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
using System.Threading;

namespace bui.GUI
{
    public partial class ConfirmWindow : ImageControl, IBitmapable
    {
        //public override event Action<Bitmap, Point> OnDirty;
        public event EventHandler OnCancel;
        private Action confirmAction; 
        private Action cancelAction; 
        //string message;
        //private Action cancelAction;
        //private readonly SynchronizationContext syncContext;
        public int GetDisplay { get; set; }
        //List<IBitmapable> paintedControls = new List<IBitmapable>();
        public ConfirmWindow()
        {
            InitializeComponent();
            //syncContext = SynchronizationContext.Current;
            Width = 640;
            Height = 480;
            AutoScaleMode = AutoScaleMode.None;
            buttonConfirm.Images = new List<Bitmap> { Resources.btn_empty_big_green, Resources.btn_empty_big_green_pressed };
            buttonCancel.Images = new List<Bitmap> { Resources.btn_empty_big, Resources.btn_empty_big_pressed };
            buttonOk.Images = new List<Bitmap> { Resources.btn_empty_big_green, Resources.btn_empty_big_green_pressed };
            /*foreach (Control c in Controls)
            {
                if (c is IBitmapable)
                {
                    var bitmapable = c as IBitmapable;
                    bitmapable.OnDirty += (b, l) =>
                    {
                        using (Graphics g = Graphics.FromImage(bmpActive))
                        {
                            g.DrawImageUnscaled(b, l);
                        }
                        OnDirty?.Invoke(b, new Point(l.X + location.X, l.Y + location.Y));
                    };

                    paintedControls.Add(bitmapable);
                }
            }*/
            buttonOk.OnClick += (s, e) =>
            {
                OnCancel?.Invoke(this, EventArgs.Empty);
                confirmAction?.Invoke();
            };
            buttonConfirm.OnClick += (s, e) =>
            {
                OnCancel?.Invoke(this, EventArgs.Empty);
                confirmAction?.Invoke();
            };
            buttonCancel.OnClick += (s, e) =>
            {
                OnCancel?.Invoke(this, EventArgs.Empty);
                cancelAction?.Invoke();
            };
            /*buttonPicture1.OnClick += (s, e) => 
            {
                Hide();
                Task.Run(() =>
                {
                    Task.Delay(200).Wait();
                    confirmAction?.Invoke();
                });
            };
            buttonPicture2.OnClick += (s, e) =>
            {
                OnCancel?.Invoke(this, EventArgs.Empty);
                Hide();
            };

            buttonPicture1.Images = new System.Collections.ObjectModel.Collection<PictuireKeeper>() { new PictuireKeeper { Picture = Resources.btn_empty_big }, new PictuireKeeper { Picture = Resources.btn_empty_big_pressed } };
            buttonPicture2.Images = new System.Collections.ObjectModel.Collection<PictuireKeeper>() { new PictuireKeeper { Picture = Resources.btn_empty_big }, new PictuireKeeper { Picture = Resources.btn_empty_big_pressed } };
            */
        }

        internal void Init(string message, Action confirmAction)
        {
            /*this.message = message;
            this.confirmAction = confirmAction;*/
            //syncContext.Post(obj => label1.Text = message, null);
        }

        public override void ApplyBackground(Bitmap bitmap, Point point)
        {
            base.ApplyBackground(bitmap, point);
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(Resources.window_confirm, Point.Empty);
            }
        }
        /*public void TransferBackground(Bitmap bitmap, Point point)
        {
            location = point;
            bmpBack = bitmap;
            bmpActive = new Bitmap(bmpBack.Width, bmpBack.Height);
            foreach (var p in paintedControls)
            {
                var control = p as Control;
                p.TransferBackground(this.GetBakground(Resources.window_confirm, new Rectangle(control.Location, control.Size)), control.Location);
            }
        }*/
        public void UpdateData(string message, Bitmap bitmap, Action confirmAction, Action cancelAction, bool isMessageOnly = false)
        {
            this.confirmAction = confirmAction;
            this.cancelAction = cancelAction;

            if (isMessageOnly)
            {
                buttonOk.Visible = true;
                buttonCancel.Visible = false;
                buttonConfirm.Visible = false;
                buttonOk.UpdateState(0, "Принять", Color.FromArgb(100, 255, 100));

            }
            else
            {
                buttonOk.Visible = false;
                buttonCancel.Visible = true;
                buttonConfirm.Visible = true;

                buttonConfirm.UpdateState(0, "Принять", Color.FromArgb(100, 255, 100));
                buttonCancel.UpdateState(0, "Отмена", Color.LightGray);
            }

            var toRepaint = paintedControls.Where(pc => pc.Visible).Select(c => (c.ActiveBitmap, c.Location)).ToList();

            if (toRepaint.Count > 0)
            {
                using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    g.DrawImageUnscaled(bitmap, Point.Empty);
                    g.DrawImageUnscaled(Resources.window_confirm, Point.Empty);
                    StringFormat format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    using (Brush brush = new SolidBrush(ForeColor))
                    {
                        g.DrawString(message, Font, brush, new RectangleF(110, 110, 420, 200), format);
                    }
                    foreach (var c in toRepaint)
                    {
                        g.DrawImageUnscaled(c.ActiveBitmap, c.Location);
                    }
                }
                IsDirty = true;
            }
        }



        internal async Task<(Bitmap, Point, bool)> MakeDirty(string message, Bitmap bitmap, Action confirmAction, bool isMessageOnly = false)
        {
            this.confirmAction = confirmAction;

            if (isMessageOnly)
            {

            }

            Task<(Bitmap, Point, bool)>[] tasks =
            {
                buttonConfirm.MakeDirty(0, "Принять", Color.FromArgb(100, 255, 100)),
                buttonCancel.MakeDirty(0, "Отмена", Color.LightGray)
            };
            (Bitmap bmp, Point location, bool isDirty)[] result = await Task.WhenAll(tasks);
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(bitmap, Point.Empty);
                g.DrawImageUnscaled(Resources.window_confirm, Point.Empty);
                StringFormat format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                g.DrawString(message, Font, new SolidBrush(ForeColor), new RectangleF(110, 110, 420, 200), format);

                foreach (var r in result)
                {
                    g.DrawImageUnscaled(r.bmp, r.location);
                }
            }
            IsDirty = false;
            return await Task.FromResult((bmpActive, location, true));
        }

        public override Image ActiveBitmap
        {
            get 
            {
                var toRepaint = paintedControls.Where(pc => pc.Visible).Select(c => (c.ActiveBitmap, c.Location)).ToList();

                if (toRepaint.Count > 0)
                {
                    using (Graphics g = Graphics.FromImage(bmpActive))
                    {
                        foreach (var c in toRepaint)
                        {
                            g.DrawImageUnscaled(c.ActiveBitmap, c.Location);
                        }
                    }
                    IsDirty = true;
                }
                /*using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    
                    if (buttonConfirm.IsDirty)
                        g.DrawImageUnscaled(buttonConfirm.ActiveBitmap, buttonConfirm.Location);
                    if (buttonCancel.IsDirty)
                        g.DrawImageUnscaled(buttonCancel.ActiveBitmap, buttonCancel.Location);
                }*/
                IsDirty = false;
                return bmpActive;
            }
        }

    }
}
