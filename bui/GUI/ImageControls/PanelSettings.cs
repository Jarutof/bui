using bui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bui.GUI.ImageControls
{
    public partial class PanelSettings : ImageControl, IBitmapable
    {
        private int selectedSetting;
        bool isModify = false;
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private StringBuilder stringBuilderSecret = new StringBuilder();
        public event Action<int, float> OnSettingChanged = (n, v) => { };

        private CultureInfo cultureInfo;
        
        public PanelSettings()
        {
            InitializeComponent();
            buttonConfirm.Images = new List<Bitmap> { Resources.btn_empty_big_green, Resources.btn_empty_big_green_pressed };
            buttonConfirm.SetText("Принять", Color.FromArgb(100, 255, 100));
            labelUset.LabelContent.Font = new Font("Tahoma", 28);
            labelUmin.LabelContent.Font = new Font("Tahoma", 28);
            labelUmax.LabelContent.Font = new Font("Tahoma", 28);
            labelImax.LabelContent.Font = new Font("Tahoma", 28);

            buttonUset.Images = new List<Bitmap> { Resources.Uset, Resources.Uset, Resources.Uset_HL, Resources.Uset_HL };
            buttonUmin.Images = new List<Bitmap> { Resources.Umin, Resources.Umin, Resources.Umin_HL, Resources.Umin_HL };
            buttonUmax.Images = new List<Bitmap> { Resources.Umax, Resources.Umax, Resources.Umax_HL, Resources.Umax_HL };
            buttonImax.Images = new List<Bitmap> { Resources.Imax, Resources.Imax, Resources.Imax_HL, Resources.Imax_HL };
            /*buttonUset.SetText("", Color.White);
            buttonUmin.SetText("", Color.White);
            buttonUmax.SetText("", Color.White);
            buttonImax.SetText("", Color.White);*/

            cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            buttonUset.OnClick += (s, e) => { stringBuilder.Clear(); selectedSetting = 0; isModify = false; };
            buttonUmin.OnClick += (s, e) => { stringBuilder.Clear(); selectedSetting = 1; isModify = false; };
            buttonUmax.OnClick += (s, e) => { stringBuilder.Clear(); selectedSetting = 2; isModify = false; };
            buttonImax.OnClick += (s, e) => { stringBuilder.Clear(); selectedSetting = 3; isModify = false; };

            buttonConfirm.OnClick += (s, e) =>
            {
                if (stringBuilderSecret.ToString() == "120461")
                {
                    this.Play();
                    Application.Exit();
                    return;
                }
                if (stringBuilderSecret.Length > 6)
                    stringBuilderSecret.Clear();
                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";
                if (float.TryParse(stringBuilder.ToString(), NumberStyles.Any, ci, out float result))
                {

                    OnSettingChanged(selectedSetting, result);
                    stringBuilder.Clear();
                    isModify = false;
                }
                /*else
                {
                    OnSettingChanged(-1, 0);
                }*/
            };


            panelKeyboard.OnDigit += digit =>
            {
                stringBuilderSecret.Append(digit);


                if (stringBuilder.Length < 6)
                {
                    stringBuilder.Append(digit);
                }
                isModify = true;
            };
            panelKeyboard.OnBackspace += () =>
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
                isModify = true;
            };

            panelKeyboard.OnDot += () =>
            {
                if (stringBuilder.Length < 6)
                {
                    stringBuilder.Append(".");
                }
                isModify = true;
            };
        }
        public override Image ActiveBitmap
        {
            get
            {
                IsDirty = false;
                return bmpActive;
            }
        }
        public override void ApplyBackground(Bitmap bitmap, Point point)
        {
            base.ApplyBackground(bitmap, point);

            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(Resources.settings_back, Point.Empty);
            }
        }
        public void UpdateData(DataManager data)
        {

            labelUset.UpdateText($"{ (selectedSetting == 0 && isModify ? stringBuilder.ToString() : data.Setting.U_Set.ToString("0.00", cultureInfo))}", Color.Black);
            labelUmin.UpdateText($"{ (selectedSetting == 1 && isModify ? stringBuilder.ToString() : data.Setting.U_Min.ToString("0.00", cultureInfo))}", Color.Black);
            labelUmax.UpdateText($"{ (selectedSetting == 2 && isModify ? stringBuilder.ToString() : data.Setting.U_Max.ToString("0.00", cultureInfo))}", Color.Black);
            labelImax.UpdateText($"{ (selectedSetting == 3 && isModify ? stringBuilder.ToString() : data.Setting.I_Max.ToString("0.00", cultureInfo))}", Color.Black);
            buttonUset.UpdateState(selectedSetting == 0 ? 2 : 0, "", Color.White);
            buttonUmin.UpdateState(selectedSetting == 1 ? 2 : 0, "", Color.White);
            buttonUmax.UpdateState(selectedSetting == 2 ? 2 : 0, "", Color.White);
            buttonImax.UpdateState(selectedSetting == 3 ? 2 : 0, "", Color.White);

            var toRepaint = paintedControls.Where(pc => pc.IsDirty && pc.Visible).Select(c => (c.ActiveBitmap, c.Location, (c as Control).Name)).ToList();
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

        /*internal async Task<(Bitmap, Point, bool)> MakeDirty(DataManager data)
        {
            Task<(Bitmap, Point, bool)>[] tasks =
            {
                labelUset.MakeDirty($"{data.Setting.U_Set} В", Color.Black),
                labelUmin.MakeDirty($"{data.Setting.U_Min} В", Color.Black),
                labelUmax.MakeDirty($"{data.Setting.U_Max} А", Color.Black),
                labelImax.MakeDirty($"{data.Setting.I_Max} В", Color.Black),
                buttonUset.MakeDirty(selectedSetting == 0 ? 2 : 0, "", Color.White),
                buttonUmin.MakeDirty(selectedSetting == 1 ? 2 : 0, "", Color.White),
                buttonUmax.MakeDirty(selectedSetting == 2 ? 2 : 0, "", Color.White),
                buttonImax.MakeDirty(selectedSetting == 3 ? 2 : 0, "", Color.White),
            };

            IBitmapable[] staticControls = { panelKeyboard , buttonConfirm };


            (Bitmap bmp, Point location, bool isDirty)[] result = await Task.WhenAll(tasks);
            if (result.Any(r => r.isDirty) || staticControls.Any(s => s.IsDirty))
            {

                using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    foreach (var r in result)
                    {
                        if (r.isDirty)
                        {
                            g.DrawImageUnscaled(r.bmp, r.location);

                        }
                    }
                    foreach (var p in staticControls)
                    {
                        if (p.Visible && p.IsDirty)
                        {
                            g.DrawImageUnscaled(p.ActiveBitmap, p.Location);
                        }
                    }
                }
                IsDirty = true;
            }

            if (IsDirty)
            {
                IsDirty = false;
                return await Task.FromResult((bmpActive, location, true));
            }
            return await Task.FromResult((bmpActive, location, false));
            /*Task<(Bitmap, Point, bool)>[] tasks =
            {
                label_BV1_I.MakeDirty($"{data.BV1.Status.I_Out} А", Color.Black),
                label_BV1_U.MakeDirty($"{data.BV1.Status.U_Out} В", Color.Black),
                label_BV2_I.MakeDirty($"{data.BV2.Status.I_Out} А", Color.Black),
                label_BV2_U.MakeDirty($"{data.BV2.Status.U_Out} В", Color.Black),
                MakeBtnBVDirty(data.BV1, buttonBV1),
                MakeBtnBVDirty(data.BV2, buttonBV2)
            };
            (Bitmap bmp, Point location, bool isDirty)[] result = await Task.WhenAll(tasks);
            if (result.Any(r => r.isDirty))
            {

                using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    g.DrawImageUnscaled(bmpSCS, Point.Empty);
                    foreach (var r in result)
                    {
                        if (r.isDirty)
                        {
                            g.DrawImageUnscaled(r.bmp, r.location);
                            isDirty = true;
                        }
                    }
                }
            }

            if (isDirty)
            {
                isDirty = false;
                return await Task.FromResult((bmpActive, location, true));
            }
        }*/
    }
}
