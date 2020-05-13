using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataExchange
{
    public abstract class Device
    {
        public abstract Task<bool> InitAsync();
        public event Action OnDataReceive;
        protected void DataReceiveEvent() => OnDataReceive?.Invoke();
        public abstract Task<bool> CheckAsync();
        public abstract void SetInitState();
    }
    public abstract class Device<T, TCommand> : Device where TCommand : DeviceCommand<T> where T : struct, Enum 
    {
        IExchangeable<T> exchanger;
        public bool IsConnected { get; set; } = true;
        public int Address { get; set; }
        public DeviceData<T> Data { get; set; }

        protected int attemptsMax = 10;
        protected int attempts;
        private bool isFirstExhange = true;

        public abstract void DataChangeHandler(ICommand<T> value);
        public abstract void ErrorHandler(ICommand<T> value);
        public override void SetInitState()
        {
            IsConnected = true;
        }
        public Device(IExchangeable<T> exchanger,  int address)
        {

            Address = address;
            this.exchanger = exchanger;
            exchanger.OnDataReceive += (s, e) =>
            {
                if (e.Address == Address)
                {
                    IsConnected = true;
                    attempts = 0;
                    DataChangeHandler(e);
                    exchanger.RemoveCommand(e);
                }
            };

            exchanger.OnError += (s, e) =>
            {
                if (e.Address != Address) return;
                if (attempts == attemptsMax)
                {
                    if (IsConnected || isFirstExhange)
                    {
                        IsConnected = false;
                        isFirstExhange = false;
                        ErrorHandler(e);
                    }
                    exchanger.RemoveCommand(e);
                    attempts = 0;
                }
                else attempts++;

            };
        }
        public async Task<DeviceData<T>> SendCommandAsync(TCommand cmd)
        {
            cmd.CompletionSource = new TaskCompletionSource<DeviceData<T>>();
            exchanger.AddCommand(cmd);
            return await cmd.CompletionSource.Task;
        }
    }
}
