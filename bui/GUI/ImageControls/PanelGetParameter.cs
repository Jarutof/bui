using bui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bui.GUI.ImageControls
{
    public partial class PanelGetParameter : ImageControl, IBitmapable
    {
        private CultureInfo cultureInfo;
        private string message;
        private Func<float> getValue;
        private Action<float> confirmAction;
        public event Action OnCancel;
        private float min;
        private float max;
        private StringBuilder stringBuilder = new StringBuilder();
        public PanelGetParameter()
        {
            InitializeComponent();

            cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            buttonConfirm.Images = new List<Bitmap> { Resources.btn_empty_big_green, Resources.btn_empty_big_green_pressed, Resources.btn_empty_big_disabled_green, Resources.btn_empty_big_disabled_green };
            buttonCancel.Images = new List<Bitmap> { Resources.btn_empty_big, Resources.btn_empty_big_pressed };
            buttonConfirm.SetText("Принять", Color.FromArgb(100, 255, 100));
            buttonCancel.SetText("Отмена", Color.LightGray);
            labelImageMessage.LabelContent.Font = new Font("Tahoma", 16);
            labelTop.LabelContent.Font = new Font("Tahoma", 16);
            labelBottom.LabelContent.Font = new Font("Tahoma", 16);
            labelInput.LabelContent.Font = new Font("Tahoma", 28);
            labelMeasure.LabelContent.Font = new Font("Tahoma", 28);

            panelKeyboard.OnDigit += digit =>
            {
                if (stringBuilder.Length < 6)
                {
                    stringBuilder.Append(digit);
                    Validate();
                }
            };
            panelKeyboard.OnBackspace += () =>
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    Validate();
                }
            };

            panelKeyboard.OnDot += () =>
            {
                if (stringBuilder.Length < 6)
                {
                    stringBuilder.Append(".");
                    Validate();
                }
            };
            buttonConfirm.OnClick += (s, e) =>
            {
                if (float.TryParse(stringBuilder.ToString(), NumberStyles.Any, cultureInfo, out float result))
                {
                    confirmAction?.Invoke(result);
                    stringBuilder.Clear();
                }
                else
                {
                    stringBuilder.Clear();
                }
            };
            buttonCancel.OnClick += (s, e) => OnCancel?.Invoke();
        }

        bool isValid = false;
        private new void Validate()
        {
            if (float.TryParse(stringBuilder.ToString(), NumberStyles.Any, cultureInfo, out float result))
            {
                isValid = result.IsInRange(min, max);
            }
            else
                isValid = false;
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
                g.DrawImageUnscaled(Resources.get_parameter, Point.Empty);
            }
        }

        public void Init(string message, float min, float max, Func<float> getValue, Action<float> confirmAction)
        {
            this.message = message;
            this.getValue = getValue;
            this.confirmAction = confirmAction;
            this.min = min;
            this.max = max;
        }

        public void UpdateData(DataManager data)
        {
            labelImageMessage.UpdateText(message, Color.White);
            labelTop.UpdateText("Введенное", Color.White);
            labelBottom.UpdateText("Замеренное", Color.White);

            labelInput.UpdateText(stringBuilder.ToString(), Color.Black);
            labelMeasure.UpdateText(getValue?.Invoke().ToString("0.00", cultureInfo) ?? "", Color.Black);
            buttonConfirm.Disabled = !isValid;
            buttonConfirm.UpdateState(buttonConfirm.Disabled ? 2 : 0, "Принять", Color.FromArgb(100, 255, 100));

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
    }
}
