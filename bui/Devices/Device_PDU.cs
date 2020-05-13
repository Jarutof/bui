using DataExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui.Devices
{
    public class Command_PDU<T> : DeviceCommand<T> where T : struct, Enum
    {
        public Command_PDU(byte[] data, int address) : base(data, address) { WaitForAnswer = 200; }
        public enum CommandEnum { SetState }

        public static Command_PDU<T> StateCommand(byte data, int address)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)CommandEnum.SetState);
            buffer.Add(data);
            return new Command_PDU<T>(buffer.ToArray(), address);
        }
        public override byte[] ToArray() => data;
    }
    public class Device_PDU : Device<ComPort.StatusEnum, Command_PDU<ComPort.StatusEnum>>
    {
        public class StatusInfo
        {
            public enum SignalsEnum
            {
                Input_1,
                Input_2,
                SetDistance,
                SetBV_1,
                SetBV_2,
                SetOUT,
                SetProtect,
                IsDistance,
                IsBV_1,
                IsBV_2,
                IsOUT,
                IsProtect,
            }

            public enum PDUCommandEnum
            {
                ON_BV_1,
                ON_BV_2,
                ON_OUT
            }
            public byte Command { get; private set; }
            public UInt16 Signals { get; set; }
            public UInt16 VersionPO { get; set; }
            public StatusInfo() { }
            public StatusInfo(byte[] array) : this()
            {
                DataBuffer db = new DataBuffer(array);
                Signals = db.GetUInt16();
                Command = db.GetByte();
                VersionPO = db.GetUInt16();
                Version.PDU = $"{VersionPO >> 8}.{(byte)VersionPO}";
            }

            public static StatusInfo FromArray(byte[] array) => new StatusInfo(array);
        }

        public enum ModelEnum
        {
            SetDistance,
            SetBV_1,
            SetBV_2,
            SetOUT,
            SetProtect,
        }
        public byte Model { get; set; } = 0x01;
        public StatusInfo Status { get; set; } = new StatusInfo();
        public bool IsInput1 => IsConnected && Status.Signals.IsBit((int)StatusInfo.SignalsEnum.Input_1);
        public bool IsInput2 => IsConnected && Status.Signals.IsBit((int)StatusInfo.SignalsEnum.Input_2);
        public bool IsCMD_BV1_On => Status.Command.IsBit((int)StatusInfo.PDUCommandEnum.ON_BV_1);
        public bool IsCMD_BV2_On => Status.Command.IsBit((int)StatusInfo.PDUCommandEnum.ON_BV_2);
        public bool IsCMD_KM_On => Status.Command.IsBit((int)StatusInfo.PDUCommandEnum.ON_OUT);

        public bool IsDistance => Status.Signals.IsBit((int)StatusInfo.SignalsEnum.IsDistance);

        public Device_PDU(IExchangeable<ComPort.StatusEnum> exchanger, int address) : base(exchanger, address)
        {
            exchanger.AddBaseCommand(() => Command_PDU<ComPort.StatusEnum>.StateCommand(Model, Address));
        }

        public override Task<bool> CheckAsync()
        {
            throw new NotImplementedException();
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
                    break;
            }
        }

        public override void ErrorHandler(ICommand<ComPort.StatusEnum> value)
        {
            
        }

        public override Task<bool> InitAsync()
        {
            throw new NotImplementedException();
        }
    }
}
