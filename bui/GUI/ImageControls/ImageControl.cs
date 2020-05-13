using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace bui.GUI
{
    public partial class ImageControl : UserControl, IBitmapable
    {
        protected Point location;
        protected Bitmap bmpBack;
        protected Bitmap bmpActive;
        protected List<IBitmapable> paintedControls = new List<IBitmapable>();
        public virtual Image ActiveBitmap => bmpActive;

        public Point LocationAbsolute
        {
            get 
            {
                if (Parent != null && Parent is IBitmapable)
                {
                    Point res = (Parent as IBitmapable).LocationAbsolute;
                    res.X += location.X;
                    res.Y += location.Y;
                    return res;
                }
                else return location;
            }
        }
        [Browsable(false)]
        public virtual int Display { get; set; } = 0;
        protected bool isHidden = false;
        public bool IsHidden
        {
            get => isHidden;
            set
            {
                if (isHidden != value)
                {
                    IsDirty = true;
                    isHidden = value;
                }
            }
        }
        private bool visible = true;
        public new virtual bool Visible
        {
            get
            {
                if (Parent != null && Parent is IBitmapable)
                    return (Parent as IBitmapable).Visible && visible;
                return visible;
            }
            set
            {
                if (visible != value)
                {
                    IsDirty = true;
                    visible = value;
                }
            }
        }

        public bool IsDirty { get; set; } = true;

        protected readonly SynchronizationContext syncContext;
        public virtual event Action<Bitmap, Point> OnDirty;
        public ImageControl()
        {
            InitializeComponent();
            syncContext = SynchronizationContext.Current;
            HandleCreated += (s, e) => GetBitmapable();
        }

        private void GetBitmapable()
        {
            foreach (Control c in Controls)
            {
                if (c is IBitmapable)
                {
                    var bitmapable = c as IBitmapable;
                    bitmapable.OnDirty += (b, l) =>
                    {
                        /*using (Graphics g = Graphics.FromImage(bmpActive))
                        {
                            g.DrawImageUnscaled(b, l);
                        }*/
                        IsDirty = true;
                        OnDirty?.Invoke(b, new Point(l.X + location.X, l.Y + location.Y));
                        //bitmapable.IsDirty = true;
                    };
                    paintedControls.Add(bitmapable);
                }
            }
        }

        public virtual void ApplyBackground(Bitmap bitmap, Point point)
        {
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(bmpBack, Point.Empty);
            }
        }

        public virtual void TransferBackground()
        {
            foreach (var p in paintedControls)
            {
                var control = p as Control;
                p.TransferBackground(this.GetBakground(bmpActive, new Rectangle(control.Location, control.Size)), control.Location);
            }
        }

        public virtual void TransferBackground(Bitmap bitmap, Point point)
        {
            location = point;
            bmpBack = bitmap;
            bmpActive = new Bitmap(bmpBack.Width, bmpBack.Height);
            ApplyBackground(bmpBack, location);
            TransferBackground();
        }
    }
}
