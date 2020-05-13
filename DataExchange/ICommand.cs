using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExchange
{
    public interface ICommand<T> where T: struct, Enum
    {
        DeviceData<T> Data { get; set; }
        TaskCompletionSource<DeviceData<T>> CompletionSource { get; set; }
        byte[] ToArray();
        int AnswerLength { get; set; }
        int Address { get; set; }
        bool MustBeEqualToRequest { get; set; }
        int WaitForAnswer { get; set; }
        bool IsBroadcast { get; set; }

        void Complete();
    }
}
