using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace bui.GUI.ImageControls
{
    public partial class StateImage : ImageControl, IBitmapable
    {
        public List<Bitmap> Images { get; set; } = new List<Bitmap>();
        private int state = -1;
        public StateImage()
        {
            InitializeComponent();
        }
        private void Repaint()
        {
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(bmpBack, Point.Empty);
                g.DrawImage(Images[state], 0, 0, bmpActive.Width, bmpActive.Height);
            }
        }
        public void SetState(int value, bool repaintAnyway = false)
        {
            if (repaintAnyway || (state != value && value >= 0 && value < Images.Count))
            {
                state = value;
                Repaint();
                IsDirty = true;
            }
        }
        public override Image ActiveBitmap
        {
            get
            {
                IsDirty = false;
                return bmpActive;
            }
        }
    }
}
