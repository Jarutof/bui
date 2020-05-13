using DataExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui.Devices
{
    public class Command_BV<T> : DeviceCommand<T> where T : struct, Enum
    {
        public Command_BV(byte[] data, int address) : base(data, address) { WaitForAnswer = 200; }
        public enum CommandEnum
        {
            BV_CMD_ON = 0xD1,
            BV_CMD_OFF = 0xD0,
            BV_CMD_SETTING = 0xB1,
            BV_CMD_CALIBRATE = 0xC1,
            BV_CMD_CL_DEF = 0xE0,
            BV_CMD_OS = 0x0C,
        }
        public static Command_BV<T> OSOn(int address) => new Command_BV<T>(new byte[] { (byte)CommandEnum.BV_CMD_OS, 1 }, address);
        public static Command_BV<T> OSOff(int address) => new Command_BV<T>(new byte[] { (byte)CommandEnum.BV_CMD_OS, 0 }, address);
        
        public static Command_BV<T> On(int address) => new Command_BV<T>(new byte[] { (byte)CommandEnum.BV_CMD_ON }, address);
        public static Command_BV<T> Off(int address) => new Command_BV<T>(new byte[] { (byte)CommandEnum.BV_CMD_OFF }, address);

        public static Command_BV<T> SetSettingsCommand(IEnumerable<byte> data, int address)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.BV_CMD_SETTING);
            buffer.AddRange(data);
            return new Command_BV<T>(buffer.ToArray(), address);
        }
        public static Command_BV<T> Calibrate(int address, int param, int point, float val)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.BV_CMD_CALIBRATE);
            buffer.Add((byte)param);
            buffer.Add((byte)point);
            buffer.AddRange(val.ToUInt16().ToArray());
            return new Command_BV<T>(buffer.ToArray(), address);
        }

        public override byte[] ToArray() => data;

        internal static Command_BV<T> ClearProtection(int address)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.BV_CMD_CL_DEF);
            return new Command_BV<T>(buffer.ToArray(), address);
        }
    }

    public class Device_BV : Device<ComPort.StatusEnum, Command_BV<ComPort.StatusEnum>>
    {
        private TaskCompletionSource<bool> statusCompletationSource;
        public class ModelInfo
        {
            public float U_Set { get; set; } = 30f;
            public float I_Set { get; set; } = 60;
            public float U_MAX { get; set; } = 32f;
            public float I_MAX { get; set; } = 60f;
            public byte Contact { get; set; }

            public byte[] ToArray()
            {
                List<byte> buffer = new List<byte>();
                buffer.AddRange(I_Set.ToArray());
                buffer.AddRange(U_Set.ToArray());
                buffer.AddRange(U_MAX.ToArray());
                buffer.AddRange(I_MAX.ToArray());

                return buffer.ToArray();
            }
        }

        public class StatusInfo
        {
            public enum SignalsEnum
            {
                N1,
                N2,
                ERR,      //nКонтактор отключен/nс БКН';
                OV,       //nПревышено напряжение (47В)';
                OC,       //nПревышен ток (70А)';
                UV_OS,    //nНеисправность внутреннего источника';
                OV_OS,    //nПерегрузка внутреннего источника';
                DELTA,    //nПревышение допустимого/nпадения в линии';
            }
            public enum ContactEnum
            {
                Contactor,
                InternalSource,
                OS
            }

            public float Tamperature { get; set; }
            public float U_Set { get; set; }
            public float U_Out { get; set; }
            public float U_os { get; set; }
            public float I_Set { get; set; }
            public float I_Out { get; set; }
            public float U_MAX { get; set; }
            public float I_MAX { get; set; }
            public byte Contact { get; set; }
            public byte Signals { get; set; }
            public byte StabilMode { get; set; }

            public static StatusInfo FromArray(byte[] array)
            {
                DataBuffer db = new DataBuffer(array);
                StatusInfo statusInfo = new StatusInfo
                {
                    Tamperature = db.GetUInt16().ToFloat(),
                    U_Set = (float)Math.Round(db.GetUInt16().ToFloat(), 1, MidpointRounding.AwayFromZero),
                    U_Out = db.GetUInt16().ToFloat(),
                    U_os = db.GetUInt16().ToFloat(),
                    I_Set = (float)Math.Round(db.GetUInt16().ToFloat(), 1, MidpointRounding.AwayFromZero),
                    I_Out = db.GetUInt16().ToFloat(),
                    Contact = db.GetByte(),
                    Signals = db.GetByte(),
                    StabilMode = db.GetByte(),
                    U_MAX = (float)Math.Round(db.GetUInt16().ToFloat(), 1, MidpointRounding.AwayFromZero),
                    I_MAX = (float)Math.Round(db.GetUInt16().ToFloat(), 1, MidpointRounding.AwayFromZero),
                };
                return statusInfo;
            }
        }

        public ModelInfo Model { get; set; } = new ModelInfo();
        public StatusInfo Status { get; set; } = new StatusInfo();
        public bool IsOn => IsConnected && Status.Contact.IsBit((int)StatusInfo.ContactEnum.Contactor);

        public bool AnyProtection =>
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.OV) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.OC) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.UV_OS) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.OV_OS) ||
            Status.Signals.IsBit((int)StatusInfo.SignalsEnum.DELTA);

        public bool IsOS_On => Status.Contact.IsBit((int)StatusInfo.ContactEnum.OS);

        public Device_BV(IExchangeable<ComPort.StatusEnum> exchanger, int address) : base(exchanger, address)
        {
            exchanger.AddBaseCommand(() => Command_BV<ComPort.StatusEnum>.SetSettingsCommand(Model.ToArray(), address));
        }
        public async Task<bool> NewStatusAsync()
        {
            if (!IsConnected) return await Task.FromResult(false);
            statusCompletationSource = new TaskCompletionSource<bool>();
            return await statusCompletationSource.Task;
        }
        public async Task<bool> SettingsAsync(int timeout = 1000)
        {
            if (!await NewStatusAsync()) return await Task.FromResult(false);

            DateTime dateTime = DateTime.Now;
            while (!Model.U_Set.IsInArea(Status.U_Set, 0.015f) ||
                !Model.I_Set.IsInArea(Status.I_Set, 0.015f) ||
                !Model.U_MAX.IsInArea(Status.U_MAX, 0.015f) ||
                !Model.I_MAX.IsInArea(Status.I_MAX, 0.015f))
            {
               
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }


        public async Task<bool> OsOffAsync(int timeout = 1000)
        {
            if(!Status.Contact.IsBit((int)StatusInfo.ContactEnum.OS)) return await Task.FromResult(true);
            DateTime dateTime = DateTime.Now;
            if ((await SendCommandAsync(Command_BV<ComPort.StatusEnum>.OSOff(Address))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);
            while (Status.Contact.IsBit((int)StatusInfo.ContactEnum.OS))
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        public async Task<bool> OsOnAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;
            if ((await SendCommandAsync(Command_BV<ComPort.StatusEnum>.OSOn(Address))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);
            while (!Status.Contact.IsBit((int)StatusInfo.ContactEnum.OS))
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        public async Task<bool> Calibrate(int param, int point, float val)
        {
            if ((await SendCommandAsync(Command_BV<ComPort.StatusEnum>.Calibrate(Address, param, point, val))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);
            return await Task.FromResult(true);
        }
        public async Task<bool> OnAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;
            Model.Contact = 1;
            if ((await SendCommandAsync(Command_BV<ComPort.StatusEnum>.On(Address))).Status != ComPort.StatusEnum.Ok)
            {
                return await Task.FromResult(false);
            }
            while (!IsOn)
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }
            await Task.Delay(1000);
            return await Task.FromResult(true);
        }

        public async Task<bool> OffAsync(int timeout = 1000)
        {
            DateTime dateTime = DateTime.Now;
            Model.Contact = 0;
            if ((await SendCommandAsync(Command_BV<ComPort.StatusEnum>.Off(Address))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);
            while (IsOn)
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dateTime).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        public override Task<bool> CheckAsync()
        {
            throw new NotImplementedException();
        }

        public override void DataChangeHandler(ICommand<ComPort.StatusEnum> cmd)
        {
            byte command = cmd.Data.Buffer[0];
            cmd.Data.Buffer.RemoveAt(0);
            switch ((Command_BV<ComPort.StatusEnum>.CommandEnum)command)
            {
                case Command_BV<ComPort.StatusEnum>.CommandEnum.BV_CMD_SETTING:
                    StatusInfo status = StatusInfo.FromArray(cmd.Data.Buffer.ToArray());
                    Status = status;
                    DataReceiveEvent();

                    statusCompletationSource?.TrySetResult(true);
                    break;
            }
        }

        public override void ErrorHandler(ICommand<ComPort.StatusEnum> value)
        {
            statusCompletationSource?.TrySetResult(false);
            Status.Signals = 0;
            Status.Contact = 0;
            Console.WriteLine($"BV Error {Address} {value.Data.Status}");
        }

        public override Task<bool> InitAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearProtectionAsync(int timeout = 1000)
        {
            if ((await SendCommandAsync(Command_BV<ComPort.StatusEnum>.ClearProtection(Address))).Status != ComPort.StatusEnum.Ok) return await Task.FromResult(false);
            DateTime dt = DateTime.Now;
            while (AnyProtection)
            {
                if (!await NewStatusAsync()) return await Task.FromResult(false);
                if ((DateTime.Now - dt).TotalMilliseconds > timeout) return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
    }
}
