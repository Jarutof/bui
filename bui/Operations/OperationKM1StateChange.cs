using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationKM1StateChange : Operation
    {
        public event Func<string, Task<bool>> OnGetConfirm;
        public event Func<string, Task<bool>> OnGetOk;

        private DataManager data;

        private bool newState;

        public OperationKM1StateChange(DataManager data, bool newState)
        {
            this.data = data;
 
            this.newState = newState;
        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            if (data.IsNormalMode)
            {
                if (data.BKN.IsRsk || !data.BKN.IsPlus || !data.BKN.IsMinus)
                {
                    await (OnGetOk?.Invoke($"Для управления КМ1 в\nрежиме работы «Штатный»\nтребуется наличие контроля\nстыковки «+», «-»\nи отсутствие «ЭПН»") ?? Task.FromResult(true));
                    return await Task.FromResult(false);
                }
            }
            else
            {
                if (!data.BKN.IsRsk)
                {
                    await (OnGetOk?.Invoke($"Для вкючения КМ1 в режиме работы «Регламент» требуется наличие контроля стыковки «ЭПН»") ?? Task.FromResult(true));
                    return await Task.FromResult(false);
                }
            }


            if (!newState)
            {
                if (!await (OnGetConfirm?.Invoke($"Подтвердите «Отключение» КМ1") ?? Task.FromResult(true)))
                    return await Task.FromResult(false);
                OperationKMOff operation = new OperationKMOff(data.BV1, data.BV2, data.BKN, data.Setting);
                operation.Progress += e => OnProgress(e);
                return await operation.StartAsync();
            }
            else
            {
                if (!data.BV1.IsOn && !data.BV2.IsOn)
                {
                    await (OnGetOk?.Invoke($"Для вкючения КМ1 должен быть включен хотя бы один БВ") ?? Task.FromResult(true));
                    return await Task.FromResult(false);
                }

                if (!await (OnGetConfirm?.Invoke($"Подтвердите «Вкючение» КМ1") ?? Task.FromResult(true)))
                    return await Task.FromResult(false);
                OperationKMOn operation = new OperationKMOn(data.BV1, data.BV2, data.BKN, data.Setting);
                operation.OnGetOk += message => OnGetOk?.Invoke(message);
                operation.Progress += e => OnProgress(e);
                return await operation.StartAsync();

            }
        }
    }
}
