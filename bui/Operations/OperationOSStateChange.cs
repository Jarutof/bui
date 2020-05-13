using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationOSStateChange : Operation
    {
        public event Func<string, Task<bool>> OnGetConfirm;
        private Device_BV bv1;
        private Device_BV bv2;
        private Device_BKN bkn;
        private SettingsInfo settings;

        public OperationOSStateChange(Device_BV bv1, Device_BV bv2, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv1 = bv1;
            this.bv2 = bv2;
            this.bkn = bkn;
            this.settings = settings;
        }

        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            if (settings.With_OS_OnCurrent)
            {
                if (!await (OnGetConfirm?.Invoke($"ОС будет подключена к «ШПП") ?? Task.FromResult(true))) 
                    return await Task.FromResult(false);
                settings.With_OS_OnCurrent = false;
            }
            else
            {
                if (!await (OnGetConfirm?.Invoke($"ОС будет подключена к «Нагрузке»") ?? Task.FromResult(true))) 
                    return await Task.FromResult(false);
                settings.With_OS_OnCurrent = true;
            }
            return await Task.FromResult(true);
        }
    }
}
