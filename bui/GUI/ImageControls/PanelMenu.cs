using bui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace bui.GUI
{
    public partial class PanelMenu : ImageControl, IBitmapable
    {
        public event EventHandler OnClose;
        public event EventHandler OnStartCalibrationBV1;
        public event EventHandler OnStartCalibrationBV2;
        public event EventHandler OnStartCalibrationBKN;
        public event EventHandler OnStartSoftProtection;
        public event EventHandler OnStartHardProtection;
        public PanelMenu()
        {
            InitializeComponent();
            buttonClose.Images = new List<Bitmap>
            {
                Resources.btn_empty_big_cyan,
                Resources.btn_empty_big_cyan_pressed,
            };
            buttonClose.SetText("закрыть", Color.Cyan);
            buttonClose.OnClick += (s, e) => OnClose?.Invoke(this, EventArgs.Empty);

            buttonCalibrBV1.Images = new List<Bitmap>
            {
                Resources.btn_empty_big,
                Resources.btn_empty_big_pressed,
            };
            buttonCalibrBV1.SetText("Калибровка БВ1", Color.White);
            buttonCalibrBV1.OnClick += (s, e) => OnStartCalibrationBV1?.Invoke(this, EventArgs.Empty);
            buttonCalibrBV2.Images = new List<Bitmap>
            {
                Resources.btn_empty_big,
                Resources.btn_empty_big_pressed,
            };
            buttonCalibrBV2.SetText("Калибровка БВ2", Color.White);
            buttonCalibrBV2.OnClick += (s, e) => OnStartCalibrationBV2?.Invoke(this, EventArgs.Empty);
            buttonCalibrBKN.Images = new List<Bitmap>
            {
                Resources.btn_empty_big,
                Resources.btn_empty_big_pressed,
            };
            buttonCalibrBKN.SetText("Калибровка БКН", Color.White);
            buttonCalibrBKN.OnClick += (s, e) => OnStartCalibrationBKN?.Invoke(this, EventArgs.Empty);

            buttonSoftProt.Images = new List<Bitmap>
            {
                Resources.btn_empty_big_yellow,
                Resources.btn_empty_big_yellow_pressed,
            };
            buttonSoftProt.SetText("Проверка срабатывания защит", Color.Yellow);
            buttonSoftProt.ContentLabel.Font = new Font("Tahoma", 12);
            buttonSoftProt.OnClick += (s, e) => OnStartSoftProtection?.Invoke(this, EventArgs.Empty);

            /*buttonHardProt.Images = new List<Bitmap>
            {
                Resources.btn_empty_big_yellow,
                Resources.btn_empty_big_yellow_pressed,
            };
            buttonHardProt.SetText("Проверка срабатывания аппаратных защит", Color.Yellow);
            buttonHardProt.ContentLabel.Font = new Font("Tahoma", 12);
            buttonHardProt.OnClick += (s, e) => OnStartHardProtection?.Invoke(this, EventArgs.Empty);*/
        }

        /*public override void ApplyBackground(Bitmap bitmap, Point point)
        {
            base.ApplyBackground(bitmap, point);

            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                using (Font font = new Font("Tahoma", 6))
                {
                    using (Brush brush = new SolidBrush(Color.Gray))
                    {
                        StringFormat format = new StringFormat()
                        {
                            Alignment = StringAlignment.Near,
                        };
                        Console.WriteLine($"{ Version.BKN }; {Version.Current} ");
                        g.DrawString($"{ Version.BKN }; {Version.Current} ", font, brush, new Rectangle(0, 0, bmpActive.Width, bmpActive.Height), format);
                    }
                }
            }

        }*/
        public override Image ActiveBitmap
        {
            get
            {
                using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    StringFormat format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    g.DrawImageUnscaled(bmpActive, Point.Empty);

                    g.DrawString("меню", Font, new SolidBrush(Color.Cyan), new RectangleF(0, 0, Width, 30), format);
                    g.DrawImageUnscaled(buttonClose.ActiveBitmap, buttonClose.Location);
                    g.DrawImageUnscaled(buttonCalibrBV1.ActiveBitmap, buttonCalibrBV1.Location);
                    g.DrawImageUnscaled(buttonCalibrBV2.ActiveBitmap, buttonCalibrBV2.Location);
                    g.DrawImageUnscaled(buttonCalibrBKN.ActiveBitmap, buttonCalibrBKN.Location);
                    g.DrawImageUnscaled(buttonSoftProt.ActiveBitmap, buttonSoftProt.Location);

                    using (Font font = new Font("Tahoma", 6))
                    {
                        using (Brush brush = new SolidBrush(Color.Gray))
                        {
                            StringFormat formatR = new StringFormat()
                            {
                                Alignment = StringAlignment.Center,
                            };
                            g.DrawString($"{ Version.BKN }; {Version.Current} ", font, brush, new Rectangle(0, 0, bmpActive.Width, bmpActive.Height), formatR);
                        }
                    }

                    //g.DrawImageUnscaled(buttonHardProt.ActiveBitmap, buttonHardProt.Location);
                    /*g.DrawImageUnscaled(buttonKM.ActiveBitmap, buttonKM.Location);
                    g.DrawImageUnscaled(buttonOS.ActiveBitmap, buttonOS.Location);
                    g.DrawImageUnscaled(buttonSettings.ActiveBitmap, buttonSettings.Location);*/

                }
                IsDirty = false;
                return base.ActiveBitmap;
            }
        }
    }
}
