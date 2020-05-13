using bui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bui.GUI.ImageControls
{
    public partial class PanelProtection : ImageControl, IBitmapable
    {
        public event Action OnClose;
        public event Action OnClear;
        public PanelProtection()
        {
            InitializeComponent();
            buttonClear.Images = new List<Bitmap> { Resources.btn_empty_big_green, Resources.btn_empty_big_green_pressed };
            buttonClose.Images = new List<Bitmap> { Resources.btn_empty_big, Resources.btn_empty_big_pressed };

            stateImage1.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage2.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage3.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage4.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage5.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage6.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage7.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage8.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage9.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage10.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage11.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage12.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage13.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage14.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage15.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage16.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage17.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage18.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            stateImage19.Images = new List<Bitmap> { Resources.radio_off, Resources.radio_on };
            
            buttonClose.OnClick += (s, e) => OnClose?.Invoke();
            buttonClear.OnClick += (s, e) => OnClear?.Invoke();
        }

        public override Image ActiveBitmap
        {
            get
            {
                IsDirty = false;
                return base.ActiveBitmap;
            }
        }

        public void UpdateData(DataManager data)
        {
            buttonClear.UpdateState(0, "Сброс", Color.FromArgb(100,255,100));
            buttonClose.UpdateState(0, "Закрыть", Color.LightGray);
            stateImage1.SetState(data.BV1.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.ERR) ? 1 : 0);
            stateImage2.SetState(data.BV1.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.OV) ? 1 : 0);
            stateImage3.SetState(data.BV1.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.OC) ? 1 : 0);
            stateImage4.SetState(data.BV1.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.UV_OS) ? 1 : 0);
            stateImage5.SetState(data.BV1.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.OV_OS) ? 1 : 0);
            stateImage6.SetState(data.BV1.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.DELTA) ? 1 : 0);

            stateImage7.SetState(data.BV2.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.ERR) ? 1 : 0);
            stateImage8.SetState(data.BV2.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.OV) ? 1 : 0);
            stateImage9.SetState(data.BV2.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.OC) ? 1 : 0);
            stateImage10.SetState(data.BV2.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.UV_OS) ? 1 : 0);
            stateImage11.SetState(data.BV2.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.OV_OS) ? 1 : 0);
            stateImage12.SetState(data.BV2.Status.Signals.IsBit((int)Devices.Device_BV.StatusInfo.SignalsEnum.DELTA) ? 1 : 0);

            stateImage13.SetState(data.BKN.Status.Signals.IsBit((int)Devices.Device_BKN.StatusInfo.SignalsEnum.Protect1) ? 1 : 0);
            stateImage14.SetState(data.BKN.Status.Signals.IsBit((int)Devices.Device_BKN.StatusInfo.SignalsEnum.Protect2) ? 1 : 0);
            stateImage15.SetState(data.BKN.Status.Signals.IsBit((int)Devices.Device_BKN.StatusInfo.SignalsEnum.UMIN) ? 1 : 0);
            stateImage16.SetState(data.BKN.Status.Signals.IsBit((int)Devices.Device_BKN.StatusInfo.SignalsEnum.UMAX) ? 1 : 0);
            stateImage17.SetState(data.BKN.Status.Signals.IsBit((int)Devices.Device_BKN.StatusInfo.SignalsEnum.IMAX) ? 1 : 0);
            stateImage18.SetState(data.BKN.Status.Signals.IsBit((int)Devices.Device_BKN.StatusInfo.SignalsEnum.CONT_ERR) ? 1 : 0);
            stateImage19.SetState(data.BKN.Status.Signals.IsBit((int)Devices.Device_BKN.StatusInfo.SignalsEnum.REM) ? 1 : 0);

            var toRepaint = paintedControls.Where(pc => pc.IsDirty && pc.Visible).Select(c => (c.ActiveBitmap, c.Location)).ToList();
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
        }

        public override void ApplyBackground(Bitmap bitmap, Point point)
        {
            base.ApplyBackground(bitmap, point);

            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(Resources.protection_panel, Point.Empty);
                using (Font font = new Font("Tahoma", 14))
                {
                    using (Brush brush = new SolidBrush(Color.FromArgb(255, 209, 112)))
                    {
                        g.DrawString("БВ1", font, brush, new PointF(150, 65));
                        g.DrawString("БВ2", font, brush, new PointF(340, 65));
                        g.DrawString("БКН", font, brush, new PointF(338, 245));
                    }
                }

                using (Font font = new Font("Tahoma", 8))
                {
                    using (Brush brush = new SolidBrush(Color.FromArgb(255, 209, 112)))
                    {
                        //nПревышен ток (70А)';
                        //nНеисправность внутреннего источника';
                        //nПерегрузка внутреннего источника';
                        //nПревышение допустимого/nпадения в линии';
                        g.DrawString("Отключен\nс БКН", font, brush, new PointF(stateImage1.Location.X + 20, stateImage1.Location.Y));
                        g.DrawString("Превышено напряжение", font, brush, new PointF(stateImage2.Location.X + 20, stateImage2.Location.Y + 4));
                        g.DrawString("Перегрузка", font, brush, new PointF(stateImage3.Location.X + 20, stateImage3.Location.Y + 4));
                        g.DrawString("Неисправность\nвнутреннего источника", font, brush, new PointF(stateImage4.Location.X + 20, stateImage4.Location.Y));
                        g.DrawString("Перегрузка\nвнутреннего источника", font, brush, new PointF(stateImage5.Location.X + 20, stateImage5.Location.Y));
                        g.DrawString("Превышение допустимого\nпадения в линии", font, brush, new PointF(stateImage6.Location.X + 20, stateImage6.Location.Y));

                        g.DrawString("Отключен\nс БКН", font, brush, new PointF(stateImage7.Location.X + 20, stateImage7.Location.Y));
                        g.DrawString("Превышено напряжение", font, brush, new PointF(stateImage8.Location.X + 20, stateImage8.Location.Y + 4));
                        g.DrawString("Перегрузка", font, brush, new PointF(stateImage9.Location.X + 20, stateImage9.Location.Y + 4));
                        g.DrawString("Неисправность\nвнутреннего источника", font, brush, new PointF(stateImage10.Location.X + 20, stateImage10.Location.Y));
                        g.DrawString("Перегрузка\nвнутреннего источника", font, brush, new PointF(stateImage11.Location.X + 20, stateImage11.Location.Y));
                        g.DrawString("Превышение допустимого\nпадения в линии", font, brush, new PointF(stateImage12.Location.X + 20, stateImage12.Location.Y));

                        g.DrawString("Блокировка БВ1", font, brush, new PointF(stateImage13.Location.X + 20, stateImage13.Location.Y + 4));
                        g.DrawString("Блокировка БВ2", font, brush, new PointF(stateImage14.Location.X + 20, stateImage14.Location.Y + 4));
                        g.DrawString("Срабатывание защиты UMin", font, brush, new PointF(stateImage15.Location.X + 20, stateImage15.Location.Y + 4));
                        g.DrawString("Срабатывание защиты UMax", font, brush, new PointF(stateImage16.Location.X + 20, stateImage16.Location.Y + 4));
                        g.DrawString("Срабатывание защиты IMax", font, brush, new PointF(stateImage17.Location.X + 20, stateImage17.Location.Y + 4));
                        g.DrawString("Неисправность КМ1", font, brush, new PointF(stateImage18.Location.X + 20, stateImage18.Location.Y + 4));
                        g.DrawString("Блокировка по команде от БУИ", font, brush, new PointF(stateImage19.Location.X + 20, stateImage19.Location.Y + 4));


                    }
                }
            }
        }
    }
}
