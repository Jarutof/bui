using bui.Devices;
using bui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bui.GUI
{
    public partial class PanelParams : ImageControl, IBitmapable
    {
        public event EventHandler OnMenuPressed;
        public event EventHandler OnKMPressed;
        public event EventHandler OnOSPressed;
        public event EventHandler OnSettingsPressed;
        private CultureInfo cultureInfo;

        public PanelParams()
        {
            InitializeComponent();
            buttonSettings.Images = new List<Bitmap>
            {
                Resources.btn_long_empty,
                Resources.btn_long_empty_pressed,
                Resources.btn_long_empty_disabled,
                Resources.btn_long_empty_disabled,

            };
            buttonSettings.SetText("уставки", Color.White);
            buttonSettings.OnClick += (s, e) => OnSettingsPressed?.Invoke(this, EventArgs.Empty);
            buttonMenu.Images = new List<Bitmap>
            { 
                Resources.btn_empty_big_cyan,
                Resources.btn_empty_big_cyan_pressed, 
                Resources.btn_empty_big_disabled_cyan,
                Resources.btn_empty_big_disabled_cyan
            };
            buttonMenu.SetText("меню", Color.Cyan);
            buttonMenu.OnClick += (s, e) => OnMenuPressed?.Invoke(this, EventArgs.Empty);

            buttonKM.Images = new List<Bitmap>
            { 
                Resources.btn_empty_big,
                Resources.btn_empty_big_pressed, 
                Resources.btn_empty_big_green,
                Resources.btn_empty_big_green_pressed,
                Resources.btn_empty_big_disabled,
                Resources.btn_empty_big_disabled,
                Resources.btn_empty_big_disabled_green,
                Resources.btn_empty_big_disabled_green,
            };
            buttonKM.SetText("КМ1 ОТКЛ", Color.LightGray);
            buttonKM.OnClick += (s, e) => OnKMPressed?.Invoke(this, EventArgs.Empty);
            buttonOS.Images = new List<Bitmap>
            {
                Resources.btn_empty_big_yellow,
                Resources.btn_empty_big_yellow_pressed,
                Resources.btn_empty_big_green,
                Resources.btn_empty_big_green_pressed,
                Resources.btn_empty_big,
                Resources.btn_empty_big_pressed,
                Resources.btn_empty_big_disabled_green,
                Resources.btn_empty_big_disabled_green,
                Resources.btn_empty_big_disabled,
                Resources.btn_empty_big_disabled,
                Resources.btn_empty_big_yellow_disabled,
                Resources.btn_empty_big_yellow_disabled
            };
            buttonOS.SetText("ОС к нагрузке", Color.Yellow);
            buttonOS.OnClick += (s, e) => OnOSPressed?.Invoke(this, EventArgs.Empty);

            labelImageProgress.LabelContent.Font = new Font("Tahoma", 10);

            labelUbus.LabelContent.Font = new Font("Tahoma", 28);
            labelUos.LabelContent.Font = new Font("Tahoma", 28);
            labelIcur.LabelContent.Font = new Font("Tahoma", 28);
            labelUcur.LabelContent.Font = new Font("Tahoma", 28);

            cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
        }

        public override void ApplyBackground(Bitmap bitmap, Point point)
        {
            base.ApplyBackground(bitmap, point);
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(Resources.paramerers, Point.Empty);
            }

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
            
            labelImageProgress.UpdateText(data.OperationProgressInfo?.Message??"", Color.Cyan);
            labelUbus.UpdateText($"{data.BKN.Status.U_BUS.ToString("0.00", cultureInfo)} В", Color.Black);
            labelUos.UpdateText($"{data.BKN.Status.U_OS.ToString("0.00", cultureInfo)} В", Color.Black);
            labelIcur.UpdateText($"{data.BKN.Status.I_BUS.ToString("0.00", cultureInfo)} А", Color.Black);
            labelUcur.UpdateText($"{data.BKN.Status.U_CUR.ToString("0.00", cultureInfo)} В", Color.Black);

            buttonSettings.Disabled = !data.IsLocalMode || data.IsButtonsBlocked;
            buttonSettings.UpdateState(buttonSettings.Disabled ? 2 : 0, "уставки", buttonSettings.Disabled ? Color.Gray : Color.LightGray);

            buttonMenu.Disabled = !data.IsLocalMode || data.IsNormalMode || data.IsButtonsBlocked || data.BKN.IsKmOn;
            buttonMenu.UpdateState(buttonMenu.Disabled ? 2 : 0, "меню", buttonMenu.Disabled ? Color.DarkCyan : Color.Cyan);

            SetBtnKMState(data);
            SetBtnOSState(data);

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

        private void SetBtnOSState(DataManager data)
        {
            buttonOS.Disabled = data.BKN.IsKmOn || !data.IsLocalMode || data.IsButtonsBlocked;

            if (data.BKN.IsKmOn)
            {
                if (data.BKN.IsOsOn)
                {
                    buttonOS.UpdateState(6, "ОС к нагрузке", Color.FromArgb(100, 255, 100));
                }
                else
                {
                    buttonOS.UpdateState(8, "ОС к ШПП", Color.Gray);
                }
            }
            else
            {
                if (data.Setting.With_OS_OnCurrent)
                {
                    buttonOS.UpdateState(buttonOS.Disabled ? 10 : 0, "ОС к нагрузке", buttonOS.Disabled ? Color.FromArgb(180, 180, 0) : Color.Yellow);
                }
                else
                {
                    buttonOS.UpdateState(buttonOS.Disabled ? 8 : 4, "ОС к ШПП", buttonOS.Disabled ? Color.Gray : Color.LightGray);
                }
            }
        }
        private void SetBtnKMState(DataManager data)
        {
            buttonKM.Disabled = 
                !data.IsLocalMode || 
                data.IsButtonsBlocked;

            if (data.BKN.IsKmOn) buttonKM.UpdateState(buttonKM.Disabled ? 6:2, "КМ1 включен", Color.FromArgb(100, 255, 100));
            else buttonKM.UpdateState(buttonKM.Disabled ? 4: 0, "КМ1 отключен", buttonKM.Disabled ? Color.Gray : Color.LightGray);
        }
    }
}
