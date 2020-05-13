using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace bui.GUI
{
    public partial class LabelImage : ImageControl, IBitmapable
    {
        private readonly SolidBrush textBrush = new SolidBrush(Color.White);
        private readonly StringBuilder textBuilder = new StringBuilder();
        public Bitmap BackGround;
        public Label LabelContent => labelContent;
        public LabelImage()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;
            
        }

        private bool isDirty = false;

        public override int Display => ((IBitmapable)Parent).Display;

        private void SetText(string text)
        {
            if (textBuilder.ToString() != text)
            {
                textBuilder.Clear();
                textBuilder.Append(text);
                isDirty = true;
            }
        }
        private void SetText(string text, Color color)
        {
            if (textBrush.Color != color)
            {
                textBrush.Color = color;
                isDirty = true;
            }
            SetText(text);
        }

        public void UpdateText(string text, Color color)
        {
            if (textBrush.Color != color)
            {
                textBrush.Color = color;
                IsDirty = true;
            }
            if (textBuilder.ToString() != text)
            {
                textBuilder.Clear();
                textBuilder.Append(text);
                IsDirty = true;
            }
            if (IsDirty) Repaint();
        }

        private void Repaint()
        {
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(bmpBack, Point.Empty);
                if(BackGround!= null) g.DrawImageUnscaled(BackGround, Point.Empty);
                StringFormat format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                
                g.DrawString(textBuilder.ToString(), labelContent.Font, textBrush, labelContent.DisplayRectangle, format);
            }
        }
        public override Image ActiveBitmap
        {
            get
            {
                IsDirty = false;

                if (isHidden) return bmpBack;
                return bmpActive;
            }
        }
        /*public async Task<(Bitmap, Point, bool)> MakeDirty(string text, Color color)
        {
            SetText(text, color);
            if (isDirty)
            {
                using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    g.DrawImageUnscaled(bmpBack, Point.Empty);
                    StringFormat format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    g.DrawString(textBuilder.ToString(), labelContent.Font, textBrush, labelContent.DisplayRectangle, format);

                }
                isDirty = false;
                return await Task.FromResult((bmpActive, location, true));
            }
            return await Task.FromResult((bmpActive, location, false));

        }*/
    }
}
