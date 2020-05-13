using DataExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui.Devices
{
    public class Command_BKN<T> : DeviceCommand<T> where T : struct, Enum
    {
        public Command_BKN(byte[] data, int address) : base(data, address) { WaitForAnswer = 200; }
        public enum CommandEnum { SetState, Calibrate, ClearProtection, Block, ClearCalibr }

        public static Command_BKN<T> GetStateCommand(IEnumerable<byte> data, int address)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.SetState);
            buffer.AddRange(data);
            return new Command_BKN<T>(buffer.ToArray(), address);
        }
        public override byte[] ToArray() => data;

        public static Command_BKN<T> Calibrate(int address, Device_BKN.StatusInfo.CalibrationParamsEnum param, int point, float value)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.Calibrate);
            buffer.Add((byte)param);
            buffer.Add((byte)point);
            buffer.AddRange(value.ToUInt16().ToArray());

            return new Command_BKN<T>(buffer.ToArray(), address) { WaitForAnswer = 1000 };
        }
        public static Command_BKN<T> RemoteProtect(int address)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.Block);
            return new Command_BKN<T>(buffer.ToArray(), address);
        }
        public static Command_BKN<T> ClearProtection(int address)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.ClearProtection);
            return new Command_BKN<T>(buffer.ToArray(), address);
        }

        internal static Command_BKN<T> ClearCalibr(int address)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.ClearCalibr);
            return new Command_BKN<T>(buffer.ToArray(), address);
        }

        internal static Command_BKN<T> BroadcastVoltage(float value)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add(0);
            buffer.AddRange(value.ToUInt16().ToArray());
            return new Command_BKN<T>(buffer.ToArray(), 0) { IsBroadcast = true };
        }
    }
    public class Device_BKN : Device<ComPort.StatusEnum, Command_BKN<ComPort.StatusEnum>>
    {
        private TaskCompletionSource<bool> statusCompletationSource;

        
        public class ModelInfo
        {
            public enum SignalsEnum
            {
                KM = 0,
                OS = 1,
                KM_Supervision = 4,
                OS_Supervision = 5,
            }
            public byte Signals { get; set; }
            public float U_ZIR_MIN { get; set; }
            public float U_Max { get; set; }
            public float U_Min { get; set; }
            public float I_Max { get; set; }
            public UInt16 MeasWeight { get; set; } = 10;
            public UInt16 MeasDelta { get; set; } = 1;
            public byte[] ToArray()
            {
                List<byte> buffer = new List<byte>();
                buffer.Add(Signals);
                buffer.AddRange(U_Max.ToArray());
                buffer.AddRange(I_Max.ToArray());
                buffer.AddRange(U_Min.ToArray());
                buffer.AddRange(MeasWeight.ToArray());
                buffer.AddRange(MeasDelta.ToArray());

                return buffer.ToArray();
            }
        }
        public async Task<bool> BroadcastVoltageAsync(float value)
        {
            if ((await SendCommandAsync(Command_BKN<ComPort.StatusEnum>.BroadcastVoltage(value))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);

            return await Task.FromResult(true);
        }
        public async Task<bool> Calibrate(StatusInfo.CalibrationParamsEnum param, int point, float value)
        {

            if ((await SendCommandAsync(Command_BKN<ComPort.StatusEnum>.Calibrate(Address, param, point, value))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);

            return await Task.FromResult(true);
        }

        public async Task<bool> ClearCalibr()
        {
            if ((await SendCommandAsync(Command_BKN<ComPort.StatusEnum>.ClearCalibr(Address))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);

            return await Task.FromResult(true);
        }

        public class StatusInfo
        {
            public enum SignalsEnum
            {
                Protect1,
                Protect2,
                OUT_CONTROL,
                OC_CONTROL,
                BS_POWER,
                BS_OP,
                KM_CONTROL,
                BS_IN,
                KS_PLUS,
                KS_MINUS,
                KS_RSK,
                UMAX,
                IMAX,
                UMIN,
                REM,
                CONT_ERR,
            }
            public enum CalibrationParamsEnum
            {

                U_BV1,
                U_BV2,
                U_BUS,
                I_BUS,
                U_CUR,
                U_OS,
            }

            public enum ValuesEnum
            {
               
                U_BV1,
                U_BV2,
                U_BUS,
                U_CUR,
                U_OS,
                I_BUS,
                U_Max,
                U_Min,
                I_Max,
            }

            private float[] values = new float[Enum.GetNames(typeof(ValuesEnum)).Length];
            public float U_BV1=> values[(int)ValuesEnum.U_BV1];
            public float U_BV2=> values[(int)ValuesEnum.U_BV2];
            public float U_BUS=> values[(int)ValuesEnum.U_BUS];
            public float I_BUS=> values[(int)ValuesEnum.I_BUS];
            public float U_CUR=> values[(int)ValuesEnum.U_CUR];
            public float U_OS => values[(int)ValuesEnum.U_OS];
            public float U_Max => values[(int)ValuesEnum.U_Max];
            public float U_Min => values[(int)ValuesEnum.U_Min];
            public float I_Max => values[(int)ValuesEnum.I_Max];
            public UInt16 Signals { get; set; }
            public UInt16 VersionPO { get; set; }
            public float CriticalValue { get; set; }

            public byte[] ToArray()
            {
                List<byte> buffer = new List<byte>();

                return buffer.ToArray();
            }
            public StatusInfo()
            { }
            public StatusInfo(float ubus, float uos) : this()
            {
                values[(int)ValuesEnum.U_BUS] = ubus;
                values[(int)ValuesEnum.U_OS] = uos;
            }
            public void SetValue(ValuesEnum vtype, float value)
            {
                values[(int)vtype] = value;
            }
            public StatusInfo(byte[] array) : this()
            {
                DataBuffer db = new DataBuffer(array);
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = db.GetFloat();
                }
                Signals = db.GetUInt16();
                CriticalValue = db.GetFloat();
                VersionPO = db.GetUInt16();
                Version.BKN = $"БКН {VersionPO>>8}.{(byte)VersionPO}";
            }
            public static StatusInfo FromArray(byte[] array) => new StatusInfo(array);
        }

        internal void SendBroadcastVoltage(float value)
        {
            Task.Run(async ()=> await BroadcastVoltageAsync(value));
        }

        public ModelInfo Model { get; set; } = new ModelInfo();
        public StatusInfo Status { get; set; } = new StatusInfo();
        public bool IsKmOn => Status.Signals.IsBit((int)StatusInfo.SignalsEnum.KM_CONTROL);

        public bool IsOsOn => Status.Signals.IsBit((int)StatusInfo.SignalsEnum.OC_CONTROL);

        public bool IsProtect1 => IsConnected && Status.Signals.IsBit((int)StatusInfo.SignalsEnum.Protect1);
        public bool IsProtect2 => IsConnected && Status.Signals.IsBit((int)StatusInfo.SignalsEnum.Protect2);
        public bool IsRsk => IsConnected && Status.Signals.IsBit((int)StatusInfo.SignalsEnum.KS_RSK);
        public bool IsPlus => IsConnected && Status.Signals.IsBit((int)StatusInfo.SignalsEnum.KS_PLUS);
        public bool IsMinus => IsConnected && Status.Signals.IsBit((int)StatusInfo.SignalsEnum.KS_MINUS);

        public bool IsBUS => IsConnected && (Status.U_BUS > 1f) && !(IsProtect1 && IsProtect2);

        public bool AnyProtection =>
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.Protect1) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.Protect2) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.UMIN) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.UMAX) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.IMAX) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.REM) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.CONT_ERR);

        public Device_BKN(IExchangeable<ComPort.StatusEnum> ex, int address) : base(ex, address) 
        {
            ex.AddBaseCommand(() => Command_BKN<ComPort.StatusEnum>.GetStateCommand(Model.ToArray(), Address));
        }

        public async Task<bool> NewStatusAsync()
        {
            if (!IsConnected) return await Task.FromResult(false);
            statusCompletationSource = new TaskCompletionSource<bool>();
            return await statusCompletationSource.Task;
        }
        
        public async Task<bool> OsOnAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;
            Model.Signals = Model.Signals.SetBit((int)ModelInfo.SignalsEnum.OS);
            Model.Signals = Model.Signals.SetBit((int)ModelInfo.SignalsEnum.OS_Supervision);
            Console.WriteLine($"Status.Signals {Status.Signals}");
            while (!Status.Signals.IsBit((int)StatusInfo.SignalsEnum.OC_CONTROL))
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }
            Model.Signals = Model.Signals.RemBit((int)ModelInfo.SignalsEnum.OS_Supervision);
            return await Task.FromResult(true);
        }

        public async Task<bool> OsOffAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;
            Model.Signals = Model.Signals.RemBit((int)ModelInfo.SignalsEnum.OS);
            Model.Signals = Model.Signals.SetBit((int)ModelInfo.SignalsEnum.OS_Supervision);
            while (Status.Signals.IsBit((int)StatusInfo.SignalsEnum.OC_CONTROL))
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }

            Model.Signals = Model.Signals.RemBit((int)ModelInfo.SignalsEnum.OS_Supervision);
            return await Task.FromResult(true);
        }

        public async Task<bool> KMOnAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;
            Model.Signals = Model.Signals.SetBit((int)ModelInfo.SignalsEnum.KM);
            Model.Signals = Model.Signals.SetBit((int)ModelInfo.SignalsEnum.KM_Supervision);
            while (!Status.Signals.IsBit((int)StatusInfo.SignalsEnum.KM_CONTROL))
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }
            Model.Signals = Model.Signals.RemBit((int)ModelInfo.SignalsEnum.KM_Supervision);

            return await Task.FromResult(true);
        }

        public async Task<bool> KMOffAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;

            Model.Signals = Model.Signals.RemBit((int)ModelInfo.SignalsEnum.KM);
            Model.Signals = Model.Signals.SetBit((int)ModelInfo.SignalsEnum.KM_Supervision);

            while (Status.Signals.IsBit((int)StatusInfo.SignalsEnum.KM_CONTROL))
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }
            Model.Signals = Model.Signals.RemBit((int)ModelInfo.SignalsEnum.KM_Supervision);
            return await Task.FromResult(true);
        }

        public override Task<bool> CheckAsync()
        {
            return Task.FromResult(false);
        }

        public override void DataChangeHandler(ICommand<ComPort.StatusEnum> cmd)
        {
            byte command = cmd.Data.Buffer[0];
            cmd.Data.Buffer.RemoveAt(0);
            switch ((Command_BKN<ComPort.StatusEnum>.CommandEnum)command)
            {
                case Command_BKN<ComPort.StatusEnum>.CommandEnum.SetState:
                    StatusInfo status = StatusInfo.FromArray(cmd.Data.Buffer.ToArray());

                    Status = status;
                    DataReceiveEvent();
                    statusCompletationSource?.TrySetResult(true);
                    break;
            }
        }

        public override void ErrorHandler(ICommand<ComPort.StatusEnum> cmd)
        {
            statusCompletationSource?.TrySetResult(false);
            Console.WriteLine($"BKN Error");

        }

        internal async Task<bool> SettingsAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;
            while (!Model.U_Max.IsInArea(Status.U_Max, 0.01f) ||
                !Model.I_Max.IsInArea(Status.I_Max, 0.01f) ||
                !Model.U_Min.IsInArea(Status.U_Min, 0.01f))
            {

                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        public override Task<bool> InitAsync()
        {
            return Task.FromResult(false);
        }

        public override void SetInitState()
        {
            
        }

        public async Task<bool> ClearProtectionAsync(int timeout = 1000)
        {
            if ((await SendCommandAsync(Command_BKN<ComPort.StatusEnum>.ClearProtection(Address))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);
            DateTime dt = DateTime.Now;
            while (AnyProtection)
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dt).TotalMilliseconds > timeout) return await Task.FromResult(false);

            }
            return await Task.FromResult(true);
        }

        internal async Task<bool> RemoteProtect()
        {
            if ((await SendCommandAsync(Command_BKN<ComPort.StatusEnum>.RemoteProtect(Address))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);
            return await Task.FromResult(true);
            
        }
    }
}
