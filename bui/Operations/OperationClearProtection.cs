using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationClearProtection : Operation
    {
        private Device_BV bv1;
        private Device_BV bv2;
        private Device_BKN bkn;
        private SettingsInfo settings;

        public OperationClearProtection(Device_BV bv1, Device_BV bv2, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv1 = bv1;
            this.bv2 = bv2;
            this.bkn = bkn;
            this.settings = settings;
        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            OnProgress(-1, $"Сброс защит");

            var res = await Task.WhenAll(bkn.ClearProtectionAsync(), bv1.ClearProtectionAsync(), bv2.ClearProtectionAsync());
            return await Task.FromResult(res.All(r => r));
        }
    }
}
