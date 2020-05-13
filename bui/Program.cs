using bui.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bui
{
    public class Config
    {
        public enum KeysEnum
        {
            port_pdu,
            port_devices,

            boudrate_pdu,
            boudrate_devices,
        }
        private static string configFileName = "config.txt";
        private readonly Dictionary<string, string> dictionaryConfig = new Dictionary<string, string>();

        public string PortPDU => dictionaryConfig[KeysEnum.port_pdu.ToString()];
        public string PortDevices => dictionaryConfig[KeysEnum.port_devices.ToString()];

        public int BoudratePDU => int.Parse(dictionaryConfig[KeysEnum.boudrate_pdu.ToString()]);
        public int BoudrateDevices => int.Parse(dictionaryConfig[KeysEnum.boudrate_devices.ToString()]);

        public void AddKeyValuePair(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                if (!dictionaryConfig.ContainsKey(key))
                    dictionaryConfig.Add(key, value);
        }

        public static Config FromFile 
        {
           get
           {
                Config config = new Config();
                string[] lines = File.ReadAllLines(configFileName);
                foreach (string line in lines)
                {
                    string[] parts = line.Replace(" ", string.Empty).Split(':');
                    config.AddKeyValuePair(parts[0], parts[1]);
                }

                try
                {
                    int bd = config.BoudrateDevices;
                    int bp = config.BoudratePDU;
                }
                catch
                {
                    config = Default;
                }

                return config;
            }
        }

        public static Config Default
        {
            get
            {
                Config config = new Config();
                config.AddKeyValuePair(KeysEnum.port_pdu.ToString(), "/dev/ttyS0");
                config.AddKeyValuePair(KeysEnum.port_devices.ToString(), "/dev/ttyS1");
                config.AddKeyValuePair(KeysEnum.boudrate_pdu.ToString(), "57600");
                config.AddKeyValuePair(KeysEnum.boudrate_devices.ToString(), "57600");

                return config;
            }
        }

        private void Save()
        {
            var kvps = dictionaryConfig.Select(kvp => $"{kvp.Key}: {kvp.Value}");
            File.WriteAllLines(configFileName, kvps);
        }

        public static Config Create()
        {
            if (!File.Exists(configFileName))
            {
                Config config = Default;
                config.Save();
                return config;
            }
            return FromFile;
        }
    }
        

    class Program
    {
        Config config;
        MainForm mainForm;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _ = new Program();
        }

        public Program()
        {
            config = Config.Create();
            mainForm = new MainForm();

            DataManager.Init();

            Exchanger_ComPort exchanger_devices = new Exchanger_ComPort(config.PortDevices, config.BoudrateDevices);
            Exchanger_ComPort exchanger_pdu = new Exchanger_ComPort(config.PortPDU, config.BoudratePDU);

            DataManager.Singleton.AddDevice(DataManager.DevicesEnum.bkn, new Device_BKN(exchanger_devices, address: 3));
            DataManager.Singleton.AddDevice(DataManager.DevicesEnum.bv1, new Device_BV(exchanger_devices, address: 1));
            exchanger_devices.AddBaseCommand(() => Command_BKN<ComPort.StatusEnum>.GetStateCommand(DataManager.Singleton.BKN.Model.ToArray(), 3));
            DataManager.Singleton.AddDevice(DataManager.DevicesEnum.bv2, new Device_BV(exchanger_devices, address: 2));
            DataManager.Singleton.AddDevice(DataManager.DevicesEnum.pdu, new Device_PDU(exchanger_pdu, address: 1));
            DataManager.Singleton.Setting.Init();
            DataManager.Singleton.PDU.OnDataReceive += () => DataManager.Singleton.SetPDUCommand();
            DataManager.Singleton.BV1.OnDataReceive += () =>
            {
                if (DataManager.Singleton.BV1.AnyProtection && DataManager.Singleton.BV2.AnyProtection && !DataManager.Singleton.BKN.AnyProtection) DataManager.Singleton.RemoteProtection();
            };
            DataManager.Singleton.BV2.OnDataReceive += () =>
            {
                if (DataManager.Singleton.BV1.AnyProtection && DataManager.Singleton.BV2.AnyProtection && !DataManager.Singleton.BKN.AnyProtection) DataManager.Singleton.RemoteProtection();
            };
            DataManager.Singleton.BKN.OnDataReceive += () =>
            {
                DataManager.Singleton.VoltageTuning();
            };

            mainForm.OnBV1StateChange += () => DataManager.Singleton.BV1StateChange();
            mainForm.OnBV2StateChange += () => DataManager.Singleton.BV2StateChange();
            mainForm.OnKM1StateChange += () => DataManager.Singleton.KM1StateChange();
            mainForm.OnOSStateChange += () => DataManager.Singleton.OSStateChange();
            mainForm.OnWorkmodeChange += () => DataManager.Singleton.WorkModeChange();
            mainForm.OnSupervismodeChange += () => DataManager.Singleton.SupervisModeChange();
            mainForm.OnProtectionClear += (s, e) => DataManager.Singleton.ClearProtection();

            mainForm.OnStartCalibrationBV1 += (s, e) => DataManager.Singleton.StartCalibrationBV1();
            mainForm.OnStartCalibrationBV2 += (s, e) => DataManager.Singleton.StartCalibrationBV2();
            mainForm.OnStartCalibrationBKN += (s, e) => DataManager.Singleton.StartCalibrationBKN();
            mainForm.OnStartSoftProtection += (s, e) => DataManager.Singleton.StartCheckProtection();
            mainForm.OnSettingChanged += (number, value) =>
            {
                DataManager.Singleton.ChangeSetting(number, value);
            };
            DataManager.Singleton.OnSettingsChanged += () => mainForm.ChangeSetting(DataManager.Singleton);
            DataManager.Singleton.OnGetConfirm += async msg => await mainForm.GetConfirmAsync(msg);
            DataManager.Singleton.OnGetParameter += async (msg, min, max, getValue) => await mainForm.GetParameter(msg, min, max, getValue);
            DataManager.Singleton.OnGetOk += async msg => await mainForm.GetOk(msg);
            System.Timers.Timer timer = new System.Timers.Timer(20);
            Random random = new Random();

            Stopwatch stopwatch = new Stopwatch();

           

            mainForm.Shown += (s, e) =>
            {
                Task.Run(async ()=> 
                {
                    Task.Delay(1000).Wait();
                    mainForm.InitDisplay(0, DataManager.Singleton);
                    exchanger_devices.StartRequest();
                    exchanger_pdu.StartRequest();
                    while (true)
                    {
                        await Task.Delay(100);

                        stopwatch.Restart();
                        try
                        {
                            await mainForm.UpdateStatusAsync(DataManager.Singleton);

                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                    }
                });
            };
            Application.Run(mainForm);
        }
    }
}
