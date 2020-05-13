using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExchange
{
    public abstract class DeviceCommand<T> : ICommand<T> where T : struct, Enum
    {
        protected byte[] data;

        public DeviceData<T> Data { get; set; }
        public TaskCompletionSource<DeviceData<T>> CompletionSource { get; set; }
        public int AnswerLength { get; set; }
        public bool MustBeEqualToRequest { get ; set; }
        public int Address { get; set; }
        public int WaitForAnswer { get; set; }
        public bool IsBroadcast { get; set; }

        public void Complete()
        {
            CompletionSource?.SetResult(Data);
        }

        public abstract byte[] ToArray();
        public DeviceCommand()
        {
            Data = new DeviceData<T>();
        }

        public DeviceCommand(byte[] data, int address) : this()
        {
            this.data = data;
            Address = address;
        }



        public DeviceCommand(byte[] data, int address, int answLength) : this(data, address)
        {
            AnswerLength = answLength;
        }
    }
}
