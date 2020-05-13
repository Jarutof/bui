using bui.Devices;
using bui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bui.GUI
{
    public partial class PanelSCS : ImageControl, IBitmapable
    {
        private Bitmap bmpSCS;

        object lockObj = new object();
        public ButtonImage ButtonBV1 => buttonBV1;
        public ButtonImage ButtonBV2 => buttonBV2;
        public ButtonImage ButtonSmode => buttonSmode;
        public ButtonImage ButtonWmode => buttonWmode;
        public ButtonImage ButtonProtections => buttonProtections;

        UInt16 segment = 0xFFFF;
        private CultureInfo cultureInfo;

        public PanelSCS()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;

            buttonProtections.Images = new List<Bitmap> { Resources.protection_Icon, Resources.protection_Icon_pressed };
            buttonProtections.ContentLabel.Text = "";

            buttonSmode.Images = new List<Bitmap>
            { 
                Resources.btn_long_empty_green,
                Resources.btn_long_empty_green_pressed, 
                Resources.btn_long_empty, 
                Resources.btn_long_empty_pressed,
                Resources.btn_long_empty_green_disabled,
                Resources.btn_long_empty_green_disabled,
                Resources.btn_long_empty_disabled,
                Resources.btn_long_empty_disabled,
            };
            buttonWmode.Images = new List<Bitmap>
            {
                Resources.btn_long_empty_green,
                Resources.btn_long_empty_green_pressed,
                Resources.btn_long_empty_yellow,
                Resources.btn_long_empty_yellow_pressed,
                Resources.btn_long_empty_yellow_disabled,
                Resources.btn_long_empty_yellow_disabled,
                Resources.btn_long_empty_green_disabled,
                Resources.btn_long_empty_green_disabled,
            };
            buttonBV1.Images = new List<Bitmap>
            { 
                Resources.btn_empty_green,
                Resources.btn_empty_green_pressed, 
                Resources.btn_empty,
                Resources.btn_empty_pressed,
                Resources.btn_empty_green_disabled,
                Resources.btn_empty_green_disabled,
                Resources.btn_empty_disabled,
                Resources.btn_empty_disabled,

            };
            buttonBV2.Images = new List<Bitmap>
            {
                Resources.btn_empty_green,
                Resources.btn_empty_green_pressed, 
                Resources.btn_empty,
                Resources.btn_empty_pressed,
                Resources.btn_empty_green_disabled,
                Resources.btn_empty_green_disabled,
                Resources.btn_empty_disabled,
                Resources.btn_empty_disabled,
            };

            labelImageBV1.BackGround = Resources.bv_panel;
            labelImageBV1.LabelContent.Font = new Font("Tahoma", 16);
            labelImageBV2.BackGround = Resources.bv_panel;
            labelImageBV2.LabelContent.Font = new Font("Tahoma", 16);
            labelImageBKN.BackGround = Resources.bkn_panel;
            labelImageBKN.LabelContent.Font = new Font("Tahoma", 16);
            //stateImageBV2.Images = new List<Bitmap> { Resources.bv_panel };

            stateImageKMbus.Images = new List<Bitmap> { Resources.km_off_gray, Resources.km_on_gray, Resources.km_off_green, Resources.km_on_green };
            stateImageKM1.Images = new List<Bitmap> { Resources.km_off_gray, Resources.km_on_gray, Resources.km_off_green, Resources.km_on_green };

            stateImageOS_BV1.Images = new List<Bitmap> { Resources.sign_OS_off, Resources.sign_OS_on };
            stateImageOS_BV2.Images = new List<Bitmap> { Resources.sign_OS_off, Resources.sign_OS_on };

            stateImageK1.Images = new List<Bitmap> { Resources.km_off_blue, Resources.km_on_blue };
            stateImageK2.Images = new List<Bitmap> { Resources.km_off_blue, Resources.km_on_blue };

            stateImageKSplus.Images = new List<Bitmap> { Resources.KS_plus_off, Resources.KS_plus_green, Resources.KS_plus_yellow };
            stateImageKSminus.Images = new List<Bitmap> { Resources.KS_minus_off, Resources.KS_minus_green, Resources.KS_minus_yellow };
            stateImageKSrsk.Images = new List<Bitmap> { Resources.KS_epn_off, Resources.KS_epn_green, Resources.KS_epn_yellow };

            label_K1.LabelContent.Font = new Font("Tahoma", 12);
            label_K2.LabelContent.Font = new Font("Tahoma", 12);
            buttonSmode.ContentLabel.Font = new Font("Tahoma", 14);
            buttonWmode.ContentLabel.Font = new Font("Tahoma", 14);

            label_KM1.LabelContent.Font = new Font("Tahoma", 12);
            label_KMbus.LabelContent.Font = new Font("Tahoma", 10);
            label_BV1_temp.LabelContent.Font = new Font("Tahoma", 14);
            label_BV2_temp.LabelContent.Font = new Font("Tahoma", 14);

            cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
        }

        protected override void OnPaint(PaintEventArgs pe) { }
        protected override void OnPaintBackground(PaintEventArgs pevent) { }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Font = new Font("Tahoma", 16);
            bmpSCS = new Bitmap(Size.Width, Size.Height);
        }

        bool isSCSDirty = false;
        public void ConnectionChange()
        {
            
        }
        private void UpdateSCS(DataManager data)
        {
            if (segment != data.SCS_Segment)
            {
                segment = data.SCS_Segment;

                using (Graphics g = Graphics.FromImage(bmpSCS))
                {
                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.input1))
                    {
                        g.DrawImageUnscaled(Resources.scs_bs_green, new Point(98, 72));
                        g.DrawImageUnscaled(Resources.Input1_on, new Point(61, 52));
                    }
                    else
                    {
                        g.DrawImageUnscaled(Resources.scs_bs, new Point(98, 72));
                        g.DrawImageUnscaled(Resources.Input1_off, new Point(61, 52));
                    }
                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.input2))
                    {
                        g.DrawImageUnscaled(Resources.scs_bs_green, new Point(284, 72));
                        g.DrawImageUnscaled(Resources.Input2_on, new Point(247, 52));
                    }
                    else
                    {
                        g.DrawImageUnscaled(Resources.scs_bs, new Point(284, 72));
                        g.DrawImageUnscaled(Resources.Input2_off, new Point(247, 52));
                    }


                    if (!segment.IsBit((int)DataManager.SCS_SegmentEnum.bv1))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot12, new Point(98, 255));
                        stateImageKMbus.IsDirty = true;
                    }
                    if (!segment.IsBit((int)DataManager.SCS_SegmentEnum.bv2))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot22, new Point(190, 255));
                        stateImageKMbus.IsDirty = true;
                    }
                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.bv1))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot12_green, new Point(98, 255));
                        stateImageKMbus.IsDirty = true;
                    }
                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.bv2))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot22_green, new Point(190, 255));
                        stateImageKMbus.IsDirty = true;
                    }
                    /*g.DrawImageUnscaled(segment.IsBit((int)DataManager.SCS_SegmentEnum.bv1) ? Resources.scs_bv_green : Resources.scs_bv, new Point(98, 259));
                    g.DrawImageUnscaled(segment.IsBit((int)DataManager.SCS_SegmentEnum.bv2) ? Resources.scs_bv_green : Resources.scs_bv, new Point(284, 259));

                    if (!segment.IsBit((int)DataManager.SCS_SegmentEnum.protect1))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot1, new Point(98, 294));
                        stateImageKMbus.IsDirty = true;
                    }
                    if (!segment.IsBit((int)DataManager.SCS_SegmentEnum.protect2))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot2, new Point(190, 294));
                        stateImageKMbus.IsDirty = true;
                    }
                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.protect1))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot1_green, new Point(98, 294));
                        stateImageKMbus.IsDirty = true;
                    }
                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.protect2))
                    {
                        g.DrawImageUnscaled(Resources.scs_prot2_green, new Point(190, 294));
                        stateImageKMbus.IsDirty = true;
                    }*/

                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.bus))
                    {
                        g.DrawImageUnscaled(Resources.scs_bus_green, new Point(190, 348));
                        g.DrawImageUnscaled(Resources.bus_on, new Point(111, 355));
                    }
                    else
                    {
                        g.DrawImageUnscaled(Resources.scs_bus, new Point(190, 348));
                        g.DrawImageUnscaled(Resources.bus_off, new Point(111, 355));
                    }

                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.feeder))
                    {
                        g.DrawImageUnscaled(Resources.scs_feeder_green, new Point(191, 419));
                        g.DrawImageUnscaled(Resources.feeder_on, new Point(24, 438));
                        stateImageKM1.IsDirty = true;
                    }
                    else
                    {
                        g.DrawImageUnscaled(Resources.scs_feeder, new Point(191, 419));
                        g.DrawImageUnscaled(Resources.feeder_off, new Point(24, 438));
                        stateImageKM1.IsDirty = true;
                    }

                    /*if (segment.IsBit((int)DataManager.SCS_SegmentEnum.protect1))
                        g.DrawImageUnscaled(Resources.protect1_on, new Point(61, 273));
                    else
                    {
                        if (data.BKN.IsProtect1)
                            g.DrawImageUnscaled(Resources.protect1_error, new Point(61, 273));
                        else
                            g.DrawImageUnscaled(Resources.protect1_off, new Point(61, 273));
                    }

                    if (segment.IsBit((int)DataManager.SCS_SegmentEnum.protect2))
                        g.DrawImageUnscaled(Resources.protect2_on, new Point(247, 273));
                    else
                    {
                        if (data.BKN.IsProtect2)
                            g.DrawImageUnscaled(Resources.protect2_error, new Point(247, 273));
                        else
                            g.DrawImageUnscaled(Resources.protect2_off, new Point(247, 273));
                    }*/
                    /*if (data.BKN.IsProtect1) g.DrawImageUnscaled(Resources.protect1_error, new Point(61, 273));
                    else
                    {
                        if (segment.IsBit((int)DataManager.SCS_SegmentEnum.protect1))
                            g.DrawImageUnscaled(Resources.protect1_on, new Point(61, 273));
                        else
                            g.DrawImageUnscaled(Resources.protect1_off, new Point(61, 273));
                    }*/


                    /*if (data.BKN.IsProtect2) g.DrawImageUnscaled(Resources.protect2_error, new Point(247, 273));
                    else
                    {
                        if (segment.IsBit((int)DataManager.SCS_SegmentEnum.protect2))
                            g.DrawImageUnscaled(Resources.protect2_on, new Point(247, 273));
                        else
                            g.DrawImageUnscaled(Resources.protect2_off, new Point(247, 273));

                    }*/
                }

                isSCSDirty = true;
            }
            if (data.SCS_Segment.IsBit((int)DataManager.SCS_SegmentEnum.protects))
            {
                if (data.SCS_Segment.IsBit((int)DataManager.SCS_SegmentEnum.bus))
                {
                    stateImageKMbus.SetState(3);
                    label_KMbus.UpdateText("КМ\nшпп", Color.FromArgb(100, 255, 100));
                }
                else
                {
                    stateImageKMbus.SetState(0);
                    label_KMbus.UpdateText("КМ\nшпп", Color.LightGray);
                }
            }
            else
            {
                label_KMbus.UpdateText("КМ\nшпп", Color.LightGray);
                stateImageKMbus.SetState(0);
            }

            if (data.BKN.IsKmOn)
            {
                if (segment.IsBit((int)DataManager.SCS_SegmentEnum.bus))
                {
                    stateImageKM1.SetState(3);
                    label_KM1.UpdateText("КМ1", Color.FromArgb(100, 255, 100));
                }
                else
                {
                    stateImageKM1.SetState(1);
                    label_KM1.UpdateText("КМ1", Color.LightGray);
                }
            }
            else
            {
                stateImageKM1.SetState(0);
                label_KM1.UpdateText("КМ1", Color.LightGray);
            }
            if (data.BKN.IsOsOn)
            {
                stateImageK1.SetState(0);
                stateImageK2.SetState(1);
                
            }
            else
            {
                stateImageK1.SetState(1);
                stateImageK2.SetState(0);
            }
            label_K1.UpdateText("K1", Color.LightBlue);
            label_K2.UpdateText("K2", Color.LightBlue);
        }
        public void UpdateData(DataManager data)
        {
            UpdateSCS(data);
            buttonProtections.UpdateState(0, "", Color.White);
            buttonProtections.IsHidden = !(data.BKN.AnyProtection || data.BV1.AnyProtection || data.BV2.AnyProtection);

            stateImageOS_BV1.Visible = data.BV1.IsConnected;
            label_BV1_temp.Visible = data.BV1.IsConnected;
            label_BV1_I.Visible = data.BV1.IsConnected;
            label_BV1_U.Visible = data.BV1.IsConnected;

            stateImageOS_BV1.SetState(data.BV1.IsOS_On ? 1 : 0);
            label_BV1_temp.UpdateText($"t {data.BV1.Status.Tamperature} С°", Color.LightGray);
            label_BV1_I.UpdateText($"{data.BV1.Status.I_Out.ToString("0.00", cultureInfo)} А", Color.Black);
            label_BV1_U.UpdateText($"{data.BV1.Status.U_Out.ToString("0.00", cultureInfo)} В", Color.Black);
            SetBtnBVState(data.BV1, buttonBV1, data);

            labelImageBV1.UpdateText("БВ1\nОтсутствует соединение", Color.LightGray);
            labelImageBV1.IsHidden = data.BV1.IsConnected;

            stateImageOS_BV2.Visible = data.BV2.IsConnected;
            label_BV2_temp.Visible = data.BV2.IsConnected;
            label_BV2_I.Visible = data.BV2.IsConnected;
            label_BV2_U.Visible = data.BV2.IsConnected;

            stateImageOS_BV2.SetState(data.BV2.IsOS_On ? 1 : 0);
            label_BV2_temp.UpdateText($"t {data.BV2.Status.Tamperature} С°", Color.LightGray);
            label_BV2_I.UpdateText($"{data.BV2.Status.I_Out.ToString("0.00", cultureInfo)} А", Color.Black);
            label_BV2_U.UpdateText($"{data.BV2.Status.U_Out.ToString("0.00", cultureInfo)} В", Color.Black);
            SetBtnBVState(data.BV2, buttonBV2, data);

            labelImageBV2.UpdateText("БВ2\nОтсутствует соединение", Color.LightGray);
            labelImageBV2.IsHidden = data.BV2.IsConnected;

            stateImageKMbus.Visible = data.BKN.IsConnected;
            stateImageKM1.Visible = data.BKN.IsConnected;
            stateImageK1.Visible = data.BKN.IsConnected;
            stateImageK2.Visible = data.BKN.IsConnected;
            label_KMbus.Visible = data.BKN.IsConnected;
            label_KM1.Visible = data.BKN.IsConnected;
            label_K1.Visible = data.BKN.IsConnected;
            label_K2.Visible = data.BKN.IsConnected;

            labelImageBKN.UpdateText("БКН\nОтсутствует соединение", Color.LightGray);
            labelImageBKN.IsHidden = data.BKN.IsConnected;

            buttonSmode.Disabled = data.BKN.IsKmOn || data.IsButtonsBlocked;
            //buttonSmode.Disabled = !data.IsNormalMode || data.IsButtonsBlocked;
            if (data.IsLocalMode)
            {
                buttonWmode.Disabled = data.BKN.IsKmOn || data.IsButtonsBlocked;
                buttonSmode.UpdateState(buttonSmode.Disabled ? 4 : 0, "Местный", Color.FromArgb(100, 255, 100));
            }
            else
            {
                buttonSmode.UpdateState(buttonSmode.Disabled ? 6 : 2, "Дистанционный", Color.LightGray);
                buttonWmode.Disabled = true;
            }

            if (data.IsNormalMode)
            {
                buttonWmode.UpdateState(buttonWmode.Disabled ? 6 : 0, "Штатный", Color.FromArgb(100, 255, 100));
            }
            else
            {
                buttonWmode.UpdateState(buttonWmode.Disabled ? 4 : 2, "Регламент", Color.Yellow);
            }

            SetKS(data);

            var toRepaint = paintedControls.Where(pc => pc.IsDirty && pc.Visible).Select(c => (c.ActiveBitmap, c.Location, ((Control)c).Name)).ToList();
            if (isSCSDirty)
            {
                if (data.BKN.IsConnected)
                {
                    if (toRepaint.Count > 0)
                    {
                        if (toRepaint[0].Name == "labelImageBKN")
                        {
                            toRepaint.Insert(1, (bmpSCS, Point.Empty, "scs"));
                        }
                        else
                            toRepaint.Insert(0, (bmpSCS, Point.Empty, "scs"));
                    }
                    else
                        toRepaint.Add((bmpSCS, Point.Empty, "scs"));
                }
                else
                {
                    toRepaint.Insert(0, (bmpSCS, Point.Empty, "scs"));
                    toRepaint.Insert(1, (labelImageBKN.ActiveBitmap, labelImageBKN.Location, "bkn"));
                    
                }
                isSCSDirty = false;
            }
            if (toRepaint.Count > 0)
            {
                //Console.WriteLine("---------------------------------");

                using (Graphics g = Graphics.FromImage(bmpActive))
                {
                    for(int i = 0; i < toRepaint.Count; i++)
                    {
                        //Console.WriteLine(toRepaint[i].Name);
                        g.DrawImageUnscaled(toRepaint[i].ActiveBitmap, toRepaint[i].Location);
                    }
                    /*foreach (var c in toRepaint)
                    {
                        Console.WriteLine(c.Name);
                        g.DrawImageUnscaled(c.ActiveBitmap, c.Location);
                    }*/
                }
                IsDirty = true;
            }
        }

        private void SetKS(DataManager data)
        {
            stateImageKSrsk.Visible = data.BKN.IsConnected;
            stateImageKSplus.Visible = data.BKN.IsConnected;
            stateImageKSminus.Visible = data.BKN.IsConnected;

            if (data.BKN.IsRsk)
            {
                stateImageKSrsk.SetState(2);
            }
            else
            {
                stateImageKSrsk.SetState(0);
            }
            if (data.BKN.IsPlus)
            {
                stateImageKSplus.SetState(1);
            }
            else
            {
                stateImageKSplus.SetState(0);
            }
            if (data.BKN.IsMinus)
            {
                stateImageKSminus.SetState(1);
            }
            else
            {
                stateImageKSminus.SetState(0);
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

        private void SetBtnBVState(Device_BV bv, ButtonImage button, DataManager data)
        {
            button.Visible = bv.IsConnected;
            if (data.IsLocalMode)
            {
                button.Disabled = /*(data.BKN.IsKmOn && !bv.IsOn) ||*/ data.IsButtonsBlocked;
            }
            else
            {
                button.Disabled = true;
            }

            if (bv.IsOn) button.UpdateState(button.Disabled ? 4 : 0, "ВКЛ", Color.FromArgb(100, 255, 100));
            else button.UpdateState(button.Disabled ? 6 : 2, "ОТКЛ", button.Disabled? Color.Gray : Color.LightGray);
        }
        public override void ApplyBackground(Bitmap bitmap, Point point)
        {
            base.ApplyBackground(bitmap, point);

            DrawBackground();
        }

        private void DrawBackground()
        {
            using (Graphics g = Graphics.FromImage(bmpActive))
            {
                g.DrawImageUnscaled(Resources.blocks_shadow2, Point.Empty);
                using (Font font = new Font("Tahoma", 10))
                {
                    using (Brush brush = new SolidBrush(Color.LightGray))
                    {
                        g.DrawString("контроль стыковки", font, brush, new Point(240, 395));
                    }
                }
            }
        }
    }
}
