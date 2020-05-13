using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationRemoteProtection : Operation
    {
        Device_BKN bkn;
        public OperationRemoteProtection(Device_BKN bkn)
        {
            this.bkn = bkn;
        }
        public override async Task<bool> ProcessAsync(CancellationToken token) => await bkn.RemoteProtect();
    }
}
