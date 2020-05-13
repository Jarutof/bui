using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationWorkModeChange : Operation
    {
        private DataManager data;
        public event Func<string, Task<bool>> OnGetConfirm;
        public OperationWorkModeChange(DataManager data)
        {
            this.data = data;
        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            if (data.IsNormalMode)
            {
                if (!await (OnGetConfirm?.Invoke($"Подтвердите переход в режим работы\n«Регламент»") ?? Task.FromResult(true)))
                    return await Task.FromResult(false);

                data.IsNormalMode = false;
            }
            else
            {
                if (!await (OnGetConfirm?.Invoke($"Подтвердите переход в режим работы\n«Штатный»") ?? Task.FromResult(true))) 
                    return await Task.FromResult(false);
                ;
                data.IsNormalMode = true;
            }
            return await Task.FromResult(true);

        }
    }
}
