using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using bui.Devices;
using bui.GUI;
using bui.GUI.Windows;
using bui.Properties;

namespace bui
{
    public partial class MainForm : Form
    {
        public event Action<int, float> OnSettingChanged;
        public event EventHandler OnStartCalibrationBV1;
        public event EventHandler OnStartCalibrationBV2;
        public event EventHandler OnStartCalibrationBKN;

        public event EventHandler OnStartSoftProtection;
        public event EventHandler OnStartHardProtection;
        public event EventHandler OnProtectionClear;
        
        public event Action OnBV1StateChange;
        public event Action OnBV2StateChange;
        public event Action OnSupervismodeChange;
        public event Action OnWorkmodeChange;
        public event Action OnKM1StateChange;

        public event Action OnOSStateChange;
        private TaskCompletionSource<bool> tcsConfirm;
        private TaskCompletionSource<(bool, float)> tcsParam;
        private List<Func<Task>> DrawList = new List<Func<Task>>();
        private bool isProtectionPanelBlocked = false;

        public async Task<bool> GetConfirmAsync(string msg)
        {
            tcsConfirm = new TaskCompletionSource<bool>();
            ShowConfirmWindow(msg, () => tcsConfirm?.TrySetResult(true), () => tcsConfirm?.TrySetResult(false));
            return await tcsConfirm.Task;
        }

        public async Task<(bool, float)> GetParameter(string msg, float min, float max, Func<float> getValue)
        {
            tcsParam = new TaskCompletionSource<(bool, float)>();
            ShowGetParameter(msg, min, max, getValue, val => tcsParam?.TrySetResult((true, val)));
            return await tcsParam.Task;
        }

        public async Task<bool> GetOk(string msg)
        {
            tcsConfirm = new TaskCompletionSource<bool>();
            ShowConfirmWindow(msg, () => tcsConfirm?.TrySetResult(true), () => tcsConfirm?.TrySetResult(false), true);
            return await tcsConfirm.Task;
        }

        /*public async Task<(bool, float)> GetParameter(string msg) => await mainView.GetParameter(msg);
        public async Task<bool> GetConfirm(string msg) => await mainView.GetConfirm(msg);
        public async Task<bool> GetOk(string msg) => await mainView.GetOk(msg);*/
        List<Label> labels = new List<Label>();

        private TaskCompletionSource<bool> completionSource;
        private readonly SynchronizationContext syncContext;
        bool isDirty = false;

        bool isParams = true;
        int currentDisplay = 0;
        List<Bitmap> displays = new List<Bitmap>();
        List<IBitmapable> paintedControls = new List<IBitmapable>();
        List<IPressable> pressableControls = new List<IPressable>();
        Panel panelDisplay;
        public MainForm()
        {

            InitializeComponent();
            this.Cursor = new Cursor(new MemoryStream(Resources._1pic));

            syncContext = SynchronizationContext.Current;   
            //this.Click += (s, e) => Console.WriteLine("Click");
            Action releaseAction = () => { };
            panelDisplay = new Panel();
            //panelDisplay.Click += (s, e) => Console.WriteLine("panelDisplay Click");

            Controls.Add(panelDisplay);
            panelDisplay.Dock = DockStyle.Fill;
            //panelDisplay.BackColor = Color.Transparent;
            panelDisplay.BringToFront();
            panelDisplay.MouseUp += (s, e) => releaseAction?.Invoke();
            panelDisplay.MouseDown += (s, e) =>
            {
                //Console.WriteLine("panelDisplay mouse down");
                foreach (var pc in pressableControls)
                {
                    if ((((IBitmapable)pc).Display == currentDisplay) && pc.CanPress && pc.IsHit(e.Location))
                    {
                        pc.Press();
                        releaseAction = () => { pc.Release(); releaseAction = () => { }; };
                        return;
                    }
                }
            };

            displays.Add(new Bitmap(640, 480));
            displays.Add(new Bitmap(640, 480));

            panelSCS.Display = 0;


            confirmWindow.Display = 1;
            confirmWindow.OnCancel += (s, e) =>
            {
                currentDisplay = 0;
                isDirty = true;
                //Repaint();
            };

            foreach (Control c in Controls)
            {
                if (c is IBitmapable)
                {
                    var bitmapable = c as IBitmapable;
                    bitmapable.OnDirty += (b, l) =>
                    {
                        /*if (bitmapable.Display == currentDisplay)
                        {
                            syncContext.Post(obj =>
                            {
                                using (Graphics g = Graphics.FromHwnd(panelDisplay.Handle))
                                {
                                    g.DrawImageUnscaled(b, l);
                                }
                            }, null);
                        }*/
                    };
                    paintedControls.Add(bitmapable);
                }
            }
            pressableControls.AddRange(this.GetIPressable());


            //typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty).SetValue(buttonMenu, true, null);
            /*mainView.OnSettingChanged += (n, v) => OnSettingChanged?.Invoke(n, v);

            mainView.OnStartCalibrationBV1 += (s,e)=> OnStartCalibrationBV1?.Invoke(this, EventArgs.Empty);
            mainView.OnStartCalibrationBV2 += (s,e)=> OnStartCalibrationBV2?.Invoke(this, EventArgs.Empty);
            mainView.OnStartCalibrationBKN += (s,e)=> OnStartCalibrationBKN?.Invoke(this, EventArgs.Empty);
            mainView.OnStartSoftProtection += (s,e)=> OnStartSoftProtection?.Invoke(this, EventArgs.Empty);
            mainView.OnStartHardProtection += (s,e)=> OnStartHardProtection?.Invoke(this, EventArgs.Empty);


            mainView.ButtonOS.OnClick += (s, e) => OnOSStateChange?.Invoke();
            mainView.ButtonKM.OnClick += (s, e) => OnKM1StateChange?.Invoke();
            mainView.ButtonBV1.OnClick += (s, e) => OnBV1StateChange?.Invoke();
            mainView.ButtonBV2.OnClick += (s, e) => OnBV2StateChange?.Invoke();*/
            panelSettings.OnSettingChanged += (n, v) => OnSettingChanged?.Invoke(n, v);
            panelProtection.OnClear += () =>
            {
                OnProtectionClear?.Invoke(this, EventArgs.Empty);
            };
            panelProtection.OnClose += () =>
            {
                DrawList.Add(() => Task.Run(() =>
                {
                    panelProtection.Visible = false;
                    panelSCS.Visible = true;
                    using (Graphics g = Graphics.FromImage(displays[0]))
                    {
                        g.DrawImageUnscaled(panelSCS.ActiveBitmap, panelSCS.Location);
                    }
                }));
            };

            panelSCS.ButtonProtections.OnClick += (s, e) => 
            {
                ShowProtection();
            };

            panelMenu.OnStartCalibrationBV1 += (s, e) => ShowConfirmWindow("Подтвердите старт операции «Калибровка БВ1»", () =>
            {
                HideMenu();
                if (panelSettings.Visible) HideSettings();
                OnStartCalibrationBV1?.Invoke(this, EventArgs.Empty);
            }, ()=> { });
            panelMenu.OnStartCalibrationBV2 += (s, e) => ShowConfirmWindow("Подтвердите старт операции «Калибровка БВ2»", () =>
            {
                HideMenu();
                if (panelSettings.Visible) HideSettings();
                OnStartCalibrationBV2?.Invoke(this, EventArgs.Empty);
            }, () => { });
            panelMenu.OnStartCalibrationBKN += (s, e) => ShowConfirmWindow("Подтвердите старт операции «Калибровка БКН»", () =>
            {
                HideMenu();
                if (panelSettings.Visible) HideSettings();
                OnStartCalibrationBKN?.Invoke(this, EventArgs.Empty);
            }, () => { });
            panelMenu.OnStartSoftProtection += (s, e) => ShowConfirmWindow("Подтвердите старт операции\n«Проверка срабатывания защит»", () =>
            {
                HideMenu();
                if (panelSettings.Visible) HideSettings();
                OnStartSoftProtection?.Invoke(this, EventArgs.Empty);
            }, () => { });

            panelMenu.OnStartHardProtection += (s, e) => ShowConfirmWindow("Подтвердите старт операции\n«Проверка срабатывания\nаппаратных защит»", () =>
            {
                HideMenu();
                if (panelSettings.Visible) HideSettings();
                OnStartHardProtection?.Invoke(this, EventArgs.Empty);
            }, ()=> { });

            panelSCS.ButtonBV1.OnClick += (s, e) => OnBV1StateChange?.Invoke();
            panelSCS.ButtonBV2.OnClick += (s, e) => OnBV2StateChange?.Invoke();
            panelSCS.ButtonSmode.OnClick += (s, e) => { HideMenu(); OnSupervismodeChange?.Invoke(); };
            panelSCS.ButtonWmode.OnClick += (s, e) => { HideMenu(); OnWorkmodeChange?.Invoke(); };



            panelParams.OnMenuPressed += (s, e) => ShowMenu();
            panelParams.OnKMPressed += (s, e) => OnKM1StateChange?.Invoke();
            panelParams.OnOSPressed += (s, e) => OnOSStateChange?.Invoke();
            panelParams.OnSettingsPressed += (s, e) =>
            {
                if (panelSettings.Visible) HideSettings();
                else ShowSettings();
            };

            panelGetParameter.OnCancel += () =>
            {
                DrawList.Add(() => Task.Run(() =>
                {
                    panelGetParameter.Visible = false;
                    panelSCS.Visible = true;
                    using (Graphics g = Graphics.FromImage(displays[0]))
                    {
                        g.DrawImageUnscaled(panelSCS.ActiveBitmap, panelSCS.Location);
                    }
                }));
                tcsParam?.TrySetResult((false, 0));
            };

            panelMenu.OnClose += (s, e) => HideMenu();
            panelMenu.Visible = false;
            /*panelMenu.Visible = true;
            panelParams.Visible = false;*/
            panelSettings.Visible = false;
            Shown += (s, e) =>
            {
                Console.WriteLine("Shown");
                TransferBackground();
                //InitDisplay(0);
            };
            FormBorderStyle = FormBorderStyle.None;
           /*WindowState = FormWindowState.Maximized;
           Size = Screen.PrimaryScreen.WorkingArea.Size;
           Console.WriteLine(Screen.PrimaryScreen.Bounds);
            Console.WriteLine(Screen.PrimaryScreen.WorkingArea);
            Console.WriteLine(Size);
            Console.WriteLine(Location);*/
        }

       /* internal void CheckProtectionStateChange(bool state)
        {
            isProtectionPanelBlocked = state;
            Console.WriteLine($"isProtectionPanelBlocked {state}");
        }*/

        private void ShowGetParameter(string message, float min, float max, Func<float> getValue, Action<float> confirmAction)
        {
            panelGetParameter.Init(message, min, max, getValue, val =>
            {
                DrawList.Add(() => Task.Run(() =>
                {
                    panelGetParameter.Visible = false;
                    panelSCS.Visible = true;
                    using (Graphics g = Graphics.FromImage(displays[0]))
                    {
                        g.DrawImageUnscaled(panelSCS.ActiveBitmap, panelSCS.Location);
                    }
                }));
                confirmAction(val);
            });

            DrawList.Add(() => Task.Run(() =>
            {
                panelGetParameter.Visible = true;
                panelSCS.Visible = false;
                using (Graphics g = Graphics.FromImage(displays[0]))
                {
                    g.DrawImageUnscaled(panelGetParameter.ActiveBitmap, panelGetParameter.Location);
                }
            }));
        }

        private void HideSettings()
        {
            DrawList.Add(() => Task.Run(() =>
            {
                panelSettings.Visible = false;
                panelSCS.Visible = true;
                using (Graphics g = Graphics.FromImage(displays[0]))
                {
                    g.DrawImageUnscaled(panelSCS.ActiveBitmap, panelSCS.Location);
                }
            }));
        }

        private void ShowSettings()
        {
            DrawList.Add(() => Task.Run(() =>
            {
                panelSettings.Visible = true;
                panelSCS.Visible = false;
                panelProtection.Visible = false;
                using (Graphics g = Graphics.FromImage(displays[0]))
                {
                    g.DrawImageUnscaled(panelSettings.ActiveBitmap, panelSettings.Location);
                }
            }));
        }

        private void HideMenu()
        {
            DrawList.Add(() => Task.Run(() =>
            {
                panelMenu.Visible = false;
                panelParams.Visible = true;
                using (Graphics g = Graphics.FromImage(displays[0]))
                {
                    g.DrawImageUnscaled(panelParams.ActiveBitmap, panelParams.Location);
                }
            }));
        }

        private void ShowMenu()
        {
            
            DrawList.Add(() => Task.Run(() =>
            {
                panelMenu.Visible = true;
                panelParams.Visible = false;
                using (Graphics g = Graphics.FromImage(displays[0]))
                {
                    g.DrawImageUnscaled(panelMenu.ActiveBitmap, panelMenu.Location);
                }
            }));
        }

        private void ShowConfirmWindow(string message, Action action, Action cancel, bool isMessageOnly = false)
        {
            DrawList.Add(() => Task.Run(() =>
            {
                confirmWindow.UpdateData(message, displays[0], action, cancel, isMessageOnly);

                using (Graphics g = Graphics.FromImage(displays[1]))
                {
                    g.DrawImageUnscaled(confirmWindow.ActiveBitmap, confirmWindow.Location);
                }
                currentDisplay = 1;
                isDirty = true;
            }));
        }
        /*private async Task ShowConfirmWindowAsync(string message, Action action)
        {
            Console.WriteLine("ShowConfirmWindowAsync");
            
            confirmWindow.UpdateData(message, displays[0], action);
            //var (bmp, location, isDirty) = await confirmWindow.MakeDirty(message, displays[0], action);

            using (Graphics g = Graphics.FromImage(displays[1]))
            {
                g.DrawImageUnscaled(confirmWindow.ActiveBitmap, confirmWindow.Location);
            }
            currentDisplay = 1;
            isDirty = true;
        }*/

        private void TransferBackground()
        {
            foreach (var p in paintedControls)
            {
                var control = p as Control;
                
                p.TransferBackground(this.GetBakground(BackgroundImage, new Rectangle(control.Location, control.Size)), control.Location);
            }
        }
        protected override void OnPaint(PaintEventArgs pe) { }
        protected override void OnPaintBackground(PaintEventArgs pevent) { }

        private void Repaint()
        {
            //Console.WriteLine("Repaint");
            syncContext.Post(obj =>
            {
                using (Graphics g = Graphics.FromHwnd(panelDisplay.Handle))
                {
                    g.DrawImageUnscaled(displays[currentDisplay], Point.Empty);
                }
            }, null);
        }
        public void InitDisplay(int n, DataManager data)
        {
            panelSCS.UpdateData(data);
            panelParams.UpdateData(data);
            panelSettings.UpdateData(data);
            panelGetParameter.UpdateData(data);

            var toRepaint = paintedControls.Where(pc => pc.Visible && pc.Display == n).Select(c => (c.ActiveBitmap, c.Location, (c as Control).Name)).ToList();
            if (toRepaint.Count > 0)
            {
                using (Graphics g = Graphics.FromImage(displays[n]))
                {
                    g.DrawImageUnscaled(Resources.background, Point.Empty);
                    foreach (var c in toRepaint)
                    {
                        g.DrawImageUnscaled(c.ActiveBitmap, c.Location);
                    }
                }
                if (currentDisplay == 0)
                    isDirty = true;
            }
            
            Repaint();
        }

        async Task test()
        {
            Console.WriteLine("test start");
            await Task.Delay(5000);
            Console.WriteLine("test end");
        }


        private void ShowProtection()
        {
            panelProtection.Visible = true;
            panelSCS.Visible = false;
            panelSettings.Visible = false;
            panelGetParameter.Visible = false;
            DrawList.Add(() => Task.Run(() =>
            {
                using (Graphics g = Graphics.FromImage(displays[0]))
                {
                    g.DrawImageUnscaled(panelProtection.ActiveBitmap, panelProtection.Location);
                }
            }));
        }
        private void CheckProtection(DataManager data)
        {
            if (data.IsButtonsBlocked) return;
            if (!data.IsLocalMode) return;
            if (data.BKN.AnyProtection || data.BV1.AnyProtection || data.BV2.AnyProtection)
            {
                if (isProtectionPanelBlocked) return;

                isProtectionPanelBlocked = true;
                ShowProtection();
            }
            else 
            {
                isProtectionPanelBlocked = false;
            }
        }
        public async Task UpdateStatusAsync(DataManager data)
        {
            CheckProtection(data);

            while (DrawList.Count > 0)
            {
                //Console.WriteLine("DrawList task");
                Stopwatch stopwatch = Stopwatch.StartNew();
                await DrawList[0]();
                DrawList.RemoveAt(0);
                //Console.WriteLine($"DrawList {stopwatch.ElapsedMilliseconds}");
                isDirty = true;
            }
            panelSCS.UpdateData(data);
            panelParams.UpdateData(data);
            panelSettings.UpdateData(data);
            panelGetParameter.UpdateData(data);
            panelProtection.UpdateData(data);
            var toRepaint = paintedControls.Where(pc => pc.IsDirty && pc.Visible && pc.Display == 0).Select(c => (c.ActiveBitmap, c.Location, (c as Control).Name)).ToList();
            if (toRepaint.Count > 0)
            {
                using (Graphics g = Graphics.FromImage(displays[0]))
                {
                    foreach (var c in toRepaint)
                    {
                        g.DrawImageUnscaled(c.ActiveBitmap, c.Location);
                    }
                }
                if (currentDisplay == 0)
                    isDirty = true;
            }


            /*List<Task<(Bitmap, Point, bool)>> tasks = new List<Task<(Bitmap, Point, bool)>>();
            if (panelSCS.Visible) tasks.Add(panelSCS.MakeDirty(data));
            if (panelParams.Visible) tasks.Add(panelParams.MakeDirty(data));
            if (panelSettings.Visible) tasks.Add(panelSettings.MakeDirty(data));

            IBitmapable[] staticControls = { panelMenu };
            (Bitmap bmp, Point location, bool isDirty)[] result = await Task.WhenAll(tasks);

            if (result.Any(r => r.isDirty) || staticControls.Any(sc => sc.Visible && sc.IsDirty))
            {
                using (Graphics g = Graphics.FromImage(displays[0]))
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
                        if (p.Visible && p.IsDirty) g.DrawImageUnscaled(p.ActiveBitmap, p.Location);
                    }

                }
                if (currentDisplay == 0)
                    isDirty = true;


            }*/

            if (currentDisplay == 1)
            {
                if (confirmWindow.IsDirty)
                {
                    using (Graphics g = Graphics.FromImage(displays[1]))
                    {
                        g.DrawImageUnscaled(confirmWindow.ActiveBitmap, confirmWindow.Location);
                    }
                    isDirty = true;
                }
            }

            if (isDirty)
            {
                Repaint();
                isDirty = false;
            }

        }

        internal void ChangeSetting(DataManager data) {}// mainView.ChangeSetting(data.Setting);
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                //cp.ExStyle |= 0x02000000;
                //cp.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return cp;
            }
        }
        /*internal void UpdatePDUStatus(Device_PDU.StatusInfo status)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => UpdatePDUStatus(status)));
                return;
            }
            Console.WriteLine($"UpdatePDUStatus {status.Signals.ToString("X")} {status.Command.ToString("X")}");
            spInput1.State = status.Signals.IsBit((int)Device_PDU.StatusInfo.SignalsEnum.Input_1) ? 0 : 1;
            spInput2.State = status.Signals.IsBit((int)Device_PDU.StatusInfo.SignalsEnum.Input_2) ? 0 : 1;
        }

        internal void UpdateBKNStatus(Device_BKN.StatusInfo status)
        {

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => UpdateBKNStatus(status)));
                return;
            }

            label1.Text = $"{status.U_BUS.ToString("0.00")} В";
            label2.Text = $"{status.U_OS.ToString("0.00")} В";
            label3.Text = $"{status.I_BUS.ToString("0.00")} А";
            label4.Text = $"{status.U_CUR.ToString("0.00")} В";

            buttonKM.State = (int)(status.Signals.IsBit((int)Device_BKN.StatusInfo.SignalsEnum.KM_CONTROL) ? GUI.ButtonPicture.StateEnum.On : GUI.ButtonPicture.StateEnum.Off);
        }

        internal void UpdateBV1Status(Device_BV.StatusInfo status)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => UpdateBV1Status(status)));
                return;
            }
            //Console.WriteLine("UpdateBV1Status");
            labelBV1_I.Text = $"{status.I_Out.ToString("0.00")} А";
            labelBV1_U.Text = $"{status.U_os.ToString("0.00")} В";
            labelBV1_T.Text = $"{status.Tamperature.ToString("0.00")} С°";
            buttonBV1.State = (int)(status.Contact.IsBit((int)Device_BV.StatusInfo.ContactEnum.Contactor) ? GUI.ButtonPicture.StateEnum.On : GUI.ButtonPicture.StateEnum.Off);
        }

        internal void UpdateBV2Status(Device_BV.StatusInfo status)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => UpdateBV2Status(status)));
                return;
            }
            //Console.WriteLine("UpdateBV2Status");
            labelBV2_I.Text = $"{status.I_Out.ToString("0.00")} А";
            labelBV2_U.Text = $"{status.U_os.ToString("0.00")} В";
            labelBV2_T.Text = $"t {status.Tamperature.ToString("0.00")} С°";
            buttonBV2.State = (int)(status.Contact.IsBit((int)Device_BV.StatusInfo.ContactEnum.Contactor) ? GUI.ButtonPicture.StateEnum.On : GUI.ButtonPicture.StateEnum.Off);
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {

            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }*/
    }
}
