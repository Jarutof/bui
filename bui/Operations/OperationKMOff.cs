using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationKMOff : Operation
    {
        private Device_BV bv1;
        private Device_BV bv2;
        private Device_BKN bkn;
        private SettingsInfo settings;

        public OperationKMOff(Device_BV bv1, Device_BV bv2, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv1 = bv1;
            this.bv2 = bv2;
            this.bkn = bkn;
            this.settings = settings;
            this.Name = "Отключение КМ1";

        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            OnProgress(-1, Name);

            var res = await Task.WhenAll(bv1.OsOffAsync(), bv2.OsOffAsync());
            if (!res.All(r => r))
            {
                return await Task.FromResult(false);
            }
            if(!await bkn.OsOffAsync())
            {
                return await Task.FromResult(false);
            }
            if (!await bkn.KMOffAsync())
            {
                return await Task.FromResult(false);
            }

            bv1.Model.U_Set = settings.U_Set;
            bv2.Model.U_Set = settings.U_Set;

            return await Task.FromResult(true);

        }
    }
}
