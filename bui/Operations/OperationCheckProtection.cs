using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationCheckProtection : Operation
    {
        public event Func<string, Task<bool>> OnGetConfirm;
        public event Func<string, Task<bool>> OnGetOk;

        private const string operationName = "«Проверка срабатывания защит»";
        private const float U_start = 30;
        private const float U_Min = 24;
        private const float U_Max = 38;
        private const float U_Step = 0.2f;
        private const float I_Max = 50f;
        private const float I_Step = 0.2f;


        private Device_BV bv1;
        private Device_BV bv2;
        private Device_BKN bkn;
        private SettingsInfo settings;

        public OperationCheckProtection(Device_BV bv1, Device_BV bv2, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv1 = bv1;
            this.bv2 = bv2;
            this.bkn = bkn;
            this.settings = settings;
            this.Name = $"Проверка срабатывания защит";
        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            OnProgress(-1, Name);
            
            if (!bkn.IsConnected)
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана. Нет соединения с БКН");
                return await Task.FromResult(false);
            }
            if (!bv1.IsConnected)
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана. Нет соединения с БВ1");
                return await Task.FromResult(false);
            }
            if (!bv2.IsConnected)
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана. Нет соединения с БВ2");
                return await Task.FromResult(false);
            }

            settings.Extend();
            settings.With_OS_OnCurrent = true;

            settings.U_Min = 0;
            settings.U_Max = 55;
            settings.I_Max = 55;

            (Device_BV device, string name)[] bVs = { (bv1, "БВ1"), (bv2, "БВ2") };
            
            for (int i = 0; i < bVs.Length; i++)
            {
                settings.U_Set = U_start;
                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана.\nНе удалось установить напряжение {U_start}В");
                    return await Task.FromResult(false);
                }
                if (!await bVs[i].device.OnAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }
                if (!await bVs[(i + 1) % 2].device.OffAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }

                OnProgress(-1, $"Проверка БКН по Uмин с {bVs[i].name}"); 
                if (!await CheckUminAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки БКН по Uмин с {bVs[i].name}");
                    return await Task.FromResult(false);
                }
                if (bVs[i].device.AnyProtection) Console.WriteLine($"prot {bVs[i].device.Status.Signals}");
                settings.U_Set = U_start;
                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана.\nНе удалось установить напряжение {U_start}В");
                    return await Task.FromResult(false);
                }
                if (!await bVs[i].device.OnAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }
                if (!await bVs[(i + 1) % 2].device.OffAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }

                OnProgress(-1, $"Проверка Uмакс {bVs[i].name}");
                if (!await CheckUmaxBVAsync(() => bVs[i].device.Status.Signals.IsBit((int)Device_BV.StatusInfo.SignalsEnum.OV)))
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки Uмакс {bVs[i].name}");
                    return await Task.FromResult(false);
                }
                if (bVs[i].device.AnyProtection) Console.WriteLine($"prot {bVs[i].device.Status.Signals}");
                settings.U_Set = U_start;
                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    return await Task.FromResult(false);
                }
                if (!await bVs[i].device.OnAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }
                if (!await bVs[(i + 1) % 2].device.OffAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }
                OnProgress(-1, $"Проверка БКН по Uмакс с {bVs[i].name}");
                if (!await CheckUmaxAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки БКН по Uмакс с {bVs[i].name}");
                    return await Task.FromResult(false);
                }
            }
            
            if (bv2.AnyProtection) Console.WriteLine($"prot {bv2.Status.Signals}");
            settings.U_Set = U_start;
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }

            if (!await bv1.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ1 не включается");
                return await Task.FromResult(false);
            }
            if (!await bv2.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ2 не включается");
                return await Task.FromResult(false);
            }
            
            OnProgress(-1, $"Проверка БКН по Uмин с БВ1 и БВ2");
            if (!await CheckUminAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки БКН по Uмин с БВ1 и БВ2");
                return await Task.FromResult(false);
            }
            settings.U_Set = U_start;
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }
            if (!await bv1.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ1 не включается");
                return await Task.FromResult(false);
            }
            if (!await bv2.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ2 не включается");
                return await Task.FromResult(false);
            }
            OnProgress(-1, $"Проверка Uмакс БВ1 и БВ2");
            if (!await CheckUmaxBVAsync(() => bv1.Status.Signals.IsBit((int)Device_BV.StatusInfo.SignalsEnum.OV) && bv2.Status.Signals.IsBit((int)Device_BV.StatusInfo.SignalsEnum.OV)))
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки Uмакс БВ1 и БВ2");
                return await Task.FromResult(false);
            }
            settings.U_Set = U_start;
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }
            if (!await bv1.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ1 не включается");
                return await Task.FromResult(false);
            }
            if (!await bv2.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ2 не включается");
                return await Task.FromResult(false);
            }
            OnProgress(-1, $"Проверка БКН по Uмакс с БВ1 и БВ2");
            if (!await CheckUmaxAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки БКН по Uмакс с БВ1 и БВ2");
                return await Task.FromResult(false);
            }
            
            //Current

            if (!await OnGetConfirm?.Invoke($"Подтвердите отсутствие нагрузки в ЭПН"))
            {
                await (OnGetOk?.Invoke($"Операция {operationName} прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            for (int i = 0; i < bVs.Length; i++)
            {
                OnProgress(-1, $"Проверка {bVs[i].name} по Iмакс");
                settings.U_Set = U_start;
                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    return await Task.FromResult(false);
                }
                if (!await bVs[i].device.OnAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }
                if (!await bVs[(i + 1) % 2].device.OffAsync())
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана");
                    return await Task.FromResult(false);
                }
                
                if (!await CheckImaxBVAsync(() => bVs[i].device.Status.Signals.IsBit((int)Device_BV.StatusInfo.SignalsEnum.OC)))
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки Iмакс {bVs[i].name}");
                    return await Task.FromResult(false);
                }
            }
            OnProgress(-1, $"Проверка БКН по Iмакс");
            settings.U_Set = U_start;
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }
            if (!await bv1.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ1 не включается");
                return await Task.FromResult(false);
            }
            if (!await bv2.OnAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана\nБВ2 не включается");
                return await Task.FromResult(false);
            }
            
            if (!await CheckImaxAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана.\nОшибка проверки Iмакс БКН");
                return await Task.FromResult(false);
            }


            OperationNormalize operationNormalize = new OperationNormalize(bv1, bv2, bkn, settings);
            operationNormalize.OnGetOk += async msg => await OnGetOk?.Invoke(msg);
            await operationNormalize.StartAsync();

            this.PlayComplete();
            await OnGetOk?.Invoke($"Операция {operationName} завершена\nОтключите ЭПН");
            return await Task.FromResult(true);
        }

        private async Task<bool> CheckImaxAsync()
        {
            settings.Extend();
            settings.U_Max = 50;
            settings.I_Max = 50;
            bv1.Model.I_MAX = 60;
            bv2.Model.I_MAX = 60;

            if (!await bkn.SettingsAsync())
            {
                return await Task.FromResult(false);
            }
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }
            if (!await new OperationKMOn(bv1, bv2, bkn, settings).StartAsync())
            {
                Console.WriteLine($"CriticalValue { bkn.Status.CriticalValue}");

                return await Task.FromResult(false);
            }
            if (bkn.AnyProtection)
                Console.WriteLine($"CriticalValue { bkn.Status.CriticalValue}");

            settings.Extend();

            if (!bkn.Status.I_BUS.IsInArea(40, 2))
            {
                if (!await OnGetConfirm?.Invoke($"Установите нагрузку\n40 А при 30 В"))
                {
                    await (OnGetOk?.Invoke($"Операция {operationName} прервана оператором.") ?? Task.FromResult(true));
                    return await Task.FromResult(false);
                }
            }

            while (bkn.Status.U_OS < (settings.U_Max - 2.5f))
            {
                settings.U_Set += U_Step;

                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    return await Task.FromResult(false);
                }

                if (bkn.AnyProtection) break;
            }

            if (!bkn.Status.Signals.IsBit((int)Device_BKN.StatusInfo.SignalsEnum.IMAX))
            {
                return await Task.FromResult(false);
            }

            if (!await new OperationClearProtection(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        private async Task<bool> CheckImaxBVAsync(Func<bool> condition)
        {
            settings.Extend();
            settings.U_Max = 50;
            settings.I_Max = 60;
            bv1.Model.I_MAX = 50;
            bv2.Model.I_MAX = 50;

            if (!await bkn.SettingsAsync())
            {
                return await Task.FromResult(false);
            }
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }
            
            if (!await new OperationKMOn(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }
            settings.Extend();

            await Task.Delay(1000);
            while (!bkn.Status.I_BUS.IsInArea(40, 3))
            {
                if (!await OnGetConfirm?.Invoke($"Установите нагрузку\n40 А при 30 В"))
                {
                    await (OnGetOk?.Invoke($"Операция {operationName} прервана оператором.") ?? Task.FromResult(true));
                    return await Task.FromResult(false);
                }
            }

            while (bkn.Status.U_OS < (settings.U_Max - 2.5f))
            {
                settings.U_Set += U_Step;

                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    return await Task.FromResult(false);
                }

                if (bkn.AnyProtection) break;
            }
            if (!await bv1.NewStatusAsync()) return await Task.FromResult(false);
            if (!await bv2.NewStatusAsync()) return await Task.FromResult(false);
            if (!condition())
            {
                return await Task.FromResult(false);
            }

            if (!await new OperationClearProtection(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        private async Task<bool> CheckUmaxAsync()
        {
            settings.Extend();
            settings.U_Max = U_Max;
            if (!await bkn.SettingsAsync())
            {
                return await Task.FromResult(false);
            }
            bv1.Model.U_MAX = 55;
            bv2.Model.U_MAX = 55;
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }
            
            if (!await new OperationKMOn(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }

            settings.Extend();
            while (bkn.Status.U_OS < (settings.U_Max + 0.5f))
            {
                settings.U_Set += U_Step;
                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    return await Task.FromResult(false);
                }
                if (bkn.AnyProtection) break;
            }
            Console.WriteLine(settings.U_Set);

            if (!bkn.Status.Signals.IsBit((int)Device_BKN.StatusInfo.SignalsEnum.UMAX))
            {
                return await Task.FromResult(false);
            }

            if (!await new OperationClearProtection(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        private async Task<bool> CheckUmaxBVAsync(Func<bool> condition)
        {
            settings.Extend();
            settings.U_Max = 55;
            if (!await bkn.SettingsAsync())
            {
                return await Task.FromResult(false);
            }
            bv1.Model.U_MAX = U_Max;
            bv2.Model.U_MAX = U_Max;
            if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
            {
                return await Task.FromResult(false);
            }

            if (!await new OperationKMOn(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }
            settings.Extend();

            while (bkn.Status.U_OS < (U_Max + 0.5f))
            {
                settings.U_Set += U_Step;
                if ((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r => !r))
                {
                    return await Task.FromResult(false);
                }

                if (bkn.AnyProtection) break;
            }
            await Task.Delay(1000);
            if (!await bv1.NewStatusAsync()) return await Task.FromResult(false);
            if (!await bv2.NewStatusAsync()) return await Task.FromResult(false);

            if (!condition())
            {
                return await Task.FromResult(false);
            }

            if (!await new OperationClearProtection(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        private async Task<bool> CheckUminAsync()
        {
            settings.U_Min = U_Min;
            if (!await bkn.SettingsAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана.\nНе удалось установить уставку Uмин");
                return await Task.FromResult(false);
            }
            if (!await new OperationKMOn(bv1, bv2, bkn, settings).StartAsync())
            {
                await OnGetOk?.Invoke($"Операция {operationName} прервана.\nНе удалось включить КМ1");

                return await Task.FromResult(false);
            }
            settings.Extend();

            while (bkn.Status.U_OS > (U_Min - 0.5f))
            {
                settings.U_Set -= U_Step;
                if((await Task.WhenAll(bv1.SettingsAsync(), bv2.SettingsAsync())).Any(r=>!r))
                {
                    await OnGetOk?.Invoke($"Операция {operationName} прервана.");
                    return await Task.FromResult(false);
                }

                if (bkn.AnyProtection) break;
            }

            if (!bkn.Status.Signals.IsBit((int)Device_BKN.StatusInfo.SignalsEnum.UMIN))
            {
                return await Task.FromResult(false);
            }
            if (!await new OperationClearProtection(bv1, bv2, bkn, settings).StartAsync())
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
    }
}
