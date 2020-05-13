using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationKMOn : Operation
    {
        public event Func<string, Task<bool>> OnGetOk;
        private Device_BV bv1;
        private Device_BV bv2;
        private Device_BKN bkn;
        private SettingsInfo settings;

        public OperationKMOn(Device_BV bv1, Device_BV bv2, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv1 = bv1;
            this.bv2 = bv2;
            this.bkn = bkn;
            this.settings = settings;
        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            OnProgress(-1, $"Включение КМ1");


            float savedUset = settings.U_Set;
            float savedUmin = settings.U_Min;
            settings.Extend();
            settings.U_Min = 0;
            if (!await bkn.SettingsAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Нет соединения с БКН") ?? Task.FromResult(true));
                settings.U_Min = savedUmin;
                settings.U_Set = savedUset;
                settings.Normalize();
                return await Task.FromResult(false);
            }
            if (!await bkn.NewStatusAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Нет соединения с БКН") ?? Task.FromResult(true));
                settings.Normalize();
                return await Task.FromResult(false);
            }
            OnProgress(-1, $"Установка Uшпп равное Uнагр");
            DateTime dateTime = DateTime.Now;
            while (!bkn.Status.U_BUS.IsInArea(bkn.Status.U_CUR + 5, 1f))
            {
                settings.U_Set = bkn.Status.U_CUR + 5;
                if (!await bkn.NewStatusAsync())
                {
                    await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Нет соединения с БКН") ?? Task.FromResult(true));
                    settings.U_Min = savedUmin;
                    settings.U_Set = savedUset;
                    settings.Normalize();
                    return await Task.FromResult(false);
                }
                if ((DateTime.Now - dateTime).TotalSeconds > 5)
                {
                    await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Превышено время ожидания") ?? Task.FromResult(true));
                    settings.U_Min = savedUmin;
                    settings.U_Set = savedUset;
                    settings.Normalize();
                    return await Task.FromResult(false);
                }
            }

            OnProgress(-1, $"Включение КМ1");
            if (!await bkn.KMOnAsync())
            {
                Console.WriteLine($"Включение КМ1");

                await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Ошибка включения КМ1") ?? Task.FromResult(true));
                settings.U_Min = savedUmin;
                settings.U_Set = savedUset;
                settings.Normalize();
                return await Task.FromResult(false);
            }

            if (settings.With_OS_OnCurrent)
            {
                OnProgress(-1, $"ОС к нагрузке");

                if (!await bkn.OsOnAsync())
                {
                    Console.WriteLine($"ОС к нагрузке");

                    await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Ошибка подключения ОС в БКН") ?? Task.FromResult(true));
                    settings.U_Min = savedUmin;
                    settings.U_Set = savedUset;
                    settings.Normalize();
                    return await Task.FromResult(false);
                }
            }

            OnProgress(-1, $"ОС в БВ");

            if (bv1.IsOn)
                if (!await bv1.OsOnAsync())
                {
                    Console.WriteLine($"ОС в БВ");

                    await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Ошибка подключения ОС в БВ1") ?? Task.FromResult(true));
                    settings.U_Min = savedUmin;
                    settings.U_Set = savedUset;
                    settings.Normalize();
                    return await Task.FromResult(false);
                }
            if (bv2.IsOn)
                if (!await bv2.OsOnAsync())
                {
                    Console.WriteLine($"ОС в БВ");
                    await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Ошибка подключения ОС в БВ2") ?? Task.FromResult(true));
                    settings.U_Min = savedUmin;
                    settings.U_Set = savedUset;
                    settings.Normalize();
                    return await Task.FromResult(false);
                }

            
            settings.U_Set = savedUset;
            dateTime = DateTime.Now;
            OnProgress(-1, $"Установка заданного напряжения");
            while (!bkn.Status.U_CUR.IsInArea(settings.U_Set, 3f))
            {
                if (!await bkn.NewStatusAsync())
                {
                    await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Нет соединения с БКН") ?? Task.FromResult(true));
                    settings.U_Min = savedUmin;
                    settings.U_Set = savedUset;
                    settings.Normalize();
                    return await Task.FromResult(false);
                }
                if ((DateTime.Now - dateTime).TotalSeconds > 5)
                {
                    await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Превышено время ожидания") ?? Task.FromResult(true));
                    settings.U_Min = savedUmin;
                    settings.U_Set = savedUset;
                    settings.Normalize();
                    return await Task.FromResult(false);
                }
            }

            settings.U_Min = savedUmin;
            settings.Normalize();

            if (!await bkn.SettingsAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Включение КМ1» прервана. Нет соединения с БКН") ?? Task.FromResult(true));

                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
    }
}
