using bui.Devices;
using bui.Operations;
using DataExchange;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui
{
    public class DataManager
    {
        public enum DevicesEnum { bkn, pdu, bv1, bv2 }
        public enum SCS_SegmentEnum { input1, input2, bv1, bv2, protect1, protect2, protects, bus, feeder }
        public static DataManager Singleton { get ; private set; }
        public event Action OnSettingsChanged;
        public Dictionary<DevicesEnum, Device> Devices { get; private set; } = new Dictionary<DevicesEnum, Device>();
        public Device_BKN BKN => Devices[DevicesEnum.bkn] as Device_BKN;
        public Device_BV BV1 => Devices[DevicesEnum.bv1] as Device_BV;
        public Device_BV BV2 => Devices[DevicesEnum.bv2] as Device_BV;
        public Device_PDU PDU => Devices[DevicesEnum.pdu] as Device_PDU;

        public UInt16 SCS_Segment { get;  set; }
        public bool IsNormalMode { get; set; } = true;
        public bool IsLocalMode { get; set; }
        public bool IsButtonsBlocked { get; internal set; }

        private readonly object lockObj = new object();
       
        public event Func<string, float, float, Func<float>, Task<(bool, float)>> OnGetParameter;
        public event Func<string, Task<bool>> OnGetConfirm;
        public event Func<string, Task<bool>> OnGetOk;

        private readonly OperationsManager operationsManager = new OperationsManager();
        public OperationProgressEventArgs OperationProgressInfo { get; private set; }
        public SettingsInfo Setting;
        public DataManager()
        {
            Setting = new SettingsInfo();
            Setting.OnUSetChanged += u =>
            {
                BV1.Model.U_Set = u;
                BV2.Model.U_Set = u;
                OnSettingsChanged?.Invoke();
            };
            Setting.OnUMinChanged += u =>
            {
                BKN.Model.U_Min = u;
                OnSettingsChanged?.Invoke();
            };
            Setting.OnUMaxChanged += u =>
            {
                BKN.Model.U_Max = u;
                BV1.Model.U_MAX = u;
                BV2.Model.U_MAX = u;
                OnSettingsChanged?.Invoke();
            };
            Setting.OnIMaxChanged += u =>
            {
                BKN.Model.I_Max = u;
                BV1.Model.I_MAX = u;
                BV2.Model.I_MAX = u;
                OnSettingsChanged?.Invoke();
            };

            operationsManager.OnCountChanged += count =>
            {
                IsButtonsBlocked = count != 0;
            };
            operationsManager.OnOperationProgressChange += e => OperationProgressInfo = e;
            operationsManager.OnOperationFinished += res => OperationProgressInfo = null;
        }
        public void AddDevice(DevicesEnum t, Device dev)
        { 
            Devices.Add(t, dev);
            dev.OnDataReceive += () =>
            {
                UpdateSCSSegment();

                PDU.Model = IsLocalMode ? PDU.Model.RemBit((int)Device_PDU.ModelEnum.SetDistance) : PDU.Model.SetBit((int)Device_PDU.ModelEnum.SetDistance);
                PDU.Model = BV1.IsOn ? PDU.Model.SetBit((int)Device_PDU.ModelEnum.SetBV_1) : PDU.Model.RemBit((int)Device_PDU.ModelEnum.SetBV_1);
                PDU.Model = BV2.IsOn ? PDU.Model.SetBit((int)Device_PDU.ModelEnum.SetBV_2) : PDU.Model.RemBit((int)Device_PDU.ModelEnum.SetBV_2);
                PDU.Model = BKN.IsKmOn ? PDU.Model.SetBit((int)Device_PDU.ModelEnum.SetOUT) : PDU.Model.RemBit((int)Device_PDU.ModelEnum.SetOUT);
                PDU.Model = BKN.AnyProtection || BV1.AnyProtection || BV2.AnyProtection ?
                PDU.Model.SetBit((int)Device_PDU.ModelEnum.SetProtect) : PDU.Model.RemBit((int)Device_PDU.ModelEnum.SetProtect);
            };
        }

        public void UpdateSCSSegment()
        {
            UInt16 segment = 0;

            if (PDU.IsInput1) segment = segment.SetBit((int)SCS_SegmentEnum.input1);
            if (PDU.IsInput2) segment = segment.SetBit((int)SCS_SegmentEnum.input2);
            if (BV1.IsOn) segment = segment.SetBit((int)SCS_SegmentEnum.bv1);
            if (BV2.IsOn) segment = segment.SetBit((int)SCS_SegmentEnum.bv2);
            if (segment.IsBit((int)SCS_SegmentEnum.bv1) || segment.IsBit((int)SCS_SegmentEnum.bv2)) segment = segment.SetBit((int)SCS_SegmentEnum.protects);

            if (BKN.IsBUS && segment.IsBit((int)SCS_SegmentEnum.protects)) segment = segment.SetBit((int)SCS_SegmentEnum.bus);
            if (BKN.IsKmOn && segment.IsBit((int)SCS_SegmentEnum.bus)) segment = segment.SetBit((int)SCS_SegmentEnum.feeder);
            lock (lockObj)
            {
                SCS_Segment = segment;
            }
        }
        Stopwatch sw = Stopwatch.StartNew();
        internal void VoltageTuning()
        {
            if (BKN.AnyProtection)
            {
                Console.WriteLine(BKN.Status.CriticalValue);
            }
            if (sw.ElapsedMilliseconds < 200) return;
            if (BKN.IsKmOn && !IsButtonsBlocked)
            {
                if (!BKN.Status.U_OS.IsInArea(Setting.U_Set, 0.25f))
                {
                    float delta = (Setting.U_Set - BKN.Status.U_OS) * 0.95f;
                    float newVolt = BV1.Model.U_Set + Math.Max(Math.Min(delta, 1f), -1f);
                    BV1.Model.U_Set = newVolt;
                    BV2.Model.U_Set = newVolt;
                    Console.WriteLine($"VoltageTuning {BKN.Status.U_OS} {newVolt} {delta}");
                    Console.WriteLine(sw.ElapsedMilliseconds);
                    sw = Stopwatch.StartNew();
                    BKN.SendBroadcastVoltage(newVolt);
                }
               
            }
        }

        public static void Init()
        {
            Singleton = new DataManager();
        }
        public void ClearProtection()
        {
            operationsManager.Add(() => new OperationClearProtection(BV1, BV2, BKN, Setting));
            if (!IsNormalMode) operationsManager.Add(() => new OperationNormalize(BV1, BV2, BKN, Setting));
        }

        bool isBV1commandBlocked = false;
        bool isBV2commandBlocked = false;
        bool isKMcommandBlocked = false;

        public void RemoteProtection()
        {
            _ = BKN.RemoteProtect();
        }
        internal void SetPDUCommand()
        {
            if (BKN.AnyProtection || BV1.AnyProtection || BV2.AnyProtection) return;
            if (!IsLocalMode && PDU.IsDistance)
            {
                if ((PDU.IsCMD_BV1_On != BV1.IsOn) && !isBV1commandBlocked && BV1.IsConnected)
                {
                    isBV1commandBlocked = true;
                    operationsManager.Add(() =>
                    {
                        OperationBVStateChange operation = new OperationBVStateChange("БВ1", BV1, BKN, Setting, PDU.IsCMD_BV1_On);
                        operation.OnFinished += res => isBV1commandBlocked = false;
                        return operation;
                    });
                }

                if ((PDU.IsCMD_BV2_On != BV2.IsOn) && !isBV2commandBlocked && BV2.IsConnected)
                {
                    isBV2commandBlocked = true;
                    operationsManager.Add(() =>
                    {
                        OperationBVStateChange operation = new OperationBVStateChange("БВ2", BV2, BKN, Setting, PDU.IsCMD_BV2_On);
                        operation.OnFinished += res => isBV2commandBlocked = false;
                        return operation;
                    });
                }

                if ((PDU.IsCMD_KM_On != BKN.IsKmOn) && !isKMcommandBlocked)
                {
                    isKMcommandBlocked = true;
                    if (!Setting.With_OS_OnCurrent)
                        operationsManager.Add(() =>
                        {
                            OperationOSStateChange operation = new OperationOSStateChange(BV1, BV2, BKN, Setting);
                            return operation;
                        });

                    operationsManager.Add(() =>
                    {
                        OperationKM1StateChange operation = new OperationKM1StateChange(this, PDU.IsCMD_KM_On);
                        operation.OnFinished += res => isKMcommandBlocked = false;
                        return operation;
                    });

                }

                
            }
        }


        public void StartCheckProtection()
        {
            operationsManager.Add(() =>
            {
                OperationCheckProtection operation = new OperationCheckProtection(BV1, BV2, BKN, Setting);

                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                operation.OnGetOk += msg => OnGetOk?.Invoke(msg);
                return operation;
            });
        }
        public void BV1StateChange()
        {
            bool newState = !BV1.IsOn;
            operationsManager.Add(() => 
            {
                OperationBVStateChange operation = new OperationBVStateChange("БВ1", BV1, BKN, Setting, newState);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                operation.OnGetOk += msg => OnGetOk?.Invoke(msg);
                return operation;
            });
        }
        public void BV2StateChange()
        {
            bool newState = !BV2.IsOn;
            operationsManager.Add(() =>
            {
                OperationBVStateChange operation = new OperationBVStateChange("БВ2", BV2, BKN, Setting, newState);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                operation.OnGetOk += msg => OnGetOk?.Invoke(msg);
                return operation;
            });
        }
        internal void WorkModeChange()
        {
            operationsManager.Add(() =>
            {
                OperationWorkModeChange operation = new OperationWorkModeChange(this);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                return operation;
            });
        }

        internal void SupervisModeChange()
        {
            operationsManager.Add(() =>
            {
                OperationSupervisModeChange operation = new OperationSupervisModeChange(this);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                return operation;
            });
        }
        public void KM1StateChange()
        {
            bool newState = !BKN.IsKmOn;
            operationsManager.Add(() =>
            {
                OperationKM1StateChange operation = new OperationKM1StateChange(this, newState);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                operation.OnGetOk += msg => OnGetOk?.Invoke(msg);
                return operation;
            });
        }
        public void OSStateChange()
        {
            operationsManager.Add(() =>
            {
                OperationOSStateChange operation = new OperationOSStateChange(BV1, BV2, BKN, Setting);
                operation.OnGetConfirm += async msg => await OnGetConfirm?.Invoke(msg);
                return operation;
            });
        }
        

        internal void StartCalibrationBV1()
        {
            operationsManager.Add(() =>
            {
                OperationCalibrationBV operation = new OperationCalibrationBV("БВ1", "БВ2", BV1, BV2, BKN, Setting);
                operation.OnGetParameter += (msg, min, max, getValue) => OnGetParameter?.Invoke(msg, min, max, getValue);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                operation.OnGetOk += msg => OnGetOk?.Invoke(msg);
                return operation;
            });
        }
        internal void StartCalibrationBV2()
        {
            operationsManager.Add(() =>
            {
                OperationCalibrationBV operation = new OperationCalibrationBV("БВ2", "БВ1",  BV2, BV1, BKN, Setting);
                operation.OnGetParameter += (msg, min, max, getValue) => OnGetParameter?.Invoke(msg, min, max, getValue);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                operation.OnGetOk += msg => OnGetOk?.Invoke(msg);
                return operation;
            });
        }

        internal void StartCalibrationBKN()
        {
            operationsManager.Add(() =>
            {
                OperationCalibrationBKN operation = new OperationCalibrationBKN(BV1, BV2, BKN, Setting);
                operation.OnGetParameter += (msg, min, max, getValue) => OnGetParameter?.Invoke(msg, min, max, getValue);
                operation.OnGetConfirm += msg => OnGetConfirm?.Invoke(msg);
                operation.OnGetOk += msg => OnGetOk?.Invoke(msg);
                return operation;
            });
        }

        public void ChangeSetting(int number, float value)
        {
            switch (number)
            {
                case 0: Setting.U_Set = value; Setting.Save();  break;
                case 1: Setting.U_Min = value; Setting.Save(); break;
                case 2: Setting.U_Max = value; Setting.Save(); break;
                case 3: Setting.I_Max = value; Setting.Save(); break;
                default: OnSettingsChanged?.Invoke(); break;
            }
        }

        
    }
}
