using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationBVStateChange : Operation
    {
        public event Func<string, Task<bool>> OnGetConfirm;
        public event Func<string, Task<bool>> OnGetOk;

        private Device_BV bv;
        private Device_BKN bkn;
        private SettingsInfo settings;
        private string bvName;
        private bool newState;


        public OperationBVStateChange(string bvName, Device_BV bv, Device_BKN bkn, SettingsInfo settings, bool newState)
        {
            this.bv = bv;
            this.bkn = bkn;
            this.settings = settings;
            this.bvName = bvName;
            this.newState = newState;

            this.Name = newState ? $"Вкючение { bvName }" : $"Откючение { bvName }";
        }

        public override async Task<bool> ProcessAsync(CancellationToken token)
        {


            if (!newState)
            {
                if (!await (OnGetConfirm?.Invoke($"Подтвердите «Откючение» {bvName}") ?? Task.FromResult(true))) return await Task.FromResult(false);
                OnProgress(-1, Name);
                
                return await bv.OffAsync();
            }
            else
            {
                
                if (!await (OnGetConfirm?.Invoke($"Подтвердите «Вкючение» {bvName}") ?? Task.FromResult(true))) return await Task.FromResult(false);
                OnProgress(-1, Name);
                
                if (!await bv.OnAsync()) return await Task.FromResult(false);
                if (bkn.IsKmOn)
                {
                    if (!await bv.OsOnAsync()) return await Task.FromResult(false);
                }
                return await Task.FromResult(true);
            }
        }
    }
}
