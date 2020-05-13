using bui.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationNormalize : Operation
    {
        public event Func<string, Task<bool>> OnGetOk;

        private Device_BV bv1;
        private Device_BV bv2;
        private Device_BKN bkn;
        private SettingsInfo settings;
        public OperationNormalize(Device_BV bv1, Device_BV bv2, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv1 = bv1;
            this.bv2 = bv2;
            this.bkn = bkn;
            this.settings = settings;
        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            if (bkn.IsKmOn)
            {
                if(!await new OperationKMOff(bv1, bv2, bkn, settings).StartAsync())
                {
                    return await Task.FromResult(false);
                }
            }
            settings.With_OS_OnCurrent = true;
            settings.Normalize();
           
            settings.ApplyLoadedUset();
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync(), bkn.SettingsAsync())).Any(r => !r))
            {
                await (OnGetOk?.Invoke($"Не удалось привести к исходному состоянию") ?? Task.FromResult(true));
                return await Task.FromResult(false); 
            }

            if (bv1.IsOn || bv2.IsOn)
            {
                await Task.Delay(1500);
            }

            settings.ApplyLoadedMinMax();
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync(), bkn.SettingsAsync())).Any(r => !r))
            {
                await (OnGetOk?.Invoke($"Не удалось привести к исходному состоянию") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            Stopwatch sw = Stopwatch.StartNew();
            if (!bv1.IsOn && !await bv1.OnAsync())
            {
                await (OnGetOk?.Invoke($"Не удалось привести к исходному состоянию (включить БВ1)") ?? Task.FromResult(true));
                return await Task.FromResult(false); 
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
            if (!bv2.IsOn && !await bv2.OnAsync())
            {
                await (OnGetOk?.Invoke($"Не удалось привести к исходному состоянию (включить БВ2)") ?? Task.FromResult(true));
                return await Task.FromResult(false); 
            }
            return await Task.FromResult(true);
        }
    }
}
