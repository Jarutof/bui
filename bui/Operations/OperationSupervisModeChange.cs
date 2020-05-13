using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationSupervisModeChange : Operation
    {
        private DataManager data;
        public event Func<string, Task<bool>> OnGetConfirm;
        public OperationSupervisModeChange(DataManager data)
        {
            this.data = data;
        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            if (data.IsLocalMode)
            {
                if (!await OnGetConfirm?.Invoke($"Подтвердите переход в режим управления\n«Дистанционный»"))
                    return await Task.FromResult(false);
                data.IsLocalMode = false;

            }
            else
            {
                if (!await OnGetConfirm?.Invoke($"Подтвердите переход в режим управления\n«Местный»")) 
                    return await Task.FromResult(false);
                data.IsLocalMode = true;
            }
            return await Task.FromResult(true);
        }
    }
}
