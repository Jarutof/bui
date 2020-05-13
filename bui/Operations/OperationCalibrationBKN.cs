using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public enum NotesEnum { a, b, c, d, e, f, g, a1, b1, c1, d1, e1 }

    public class OperationCalibrationBKN : Operation
    {
        const float limit = 5;
        float[] feqs = { 220, 246.94f, 261.63f, 293.66f, 329.63f, 349.23f, 415.3f, 440f, 493.88f, 523.26f, 587.32f, 659.26f };


        public event Func<string, float, float, Func<float>, Task<(bool, float)>> OnGetParameter;
        public event Func<string, Task<bool>> OnGetConfirm;
        public event Func<string, Task<bool>> OnGetOk;
        private Device_BV bv1;
        private Device_BV bv2;
        private Device_BKN bkn;
        private SettingsInfo settings;
        public OperationCalibrationBKN(Device_BV bv1, Device_BV bv2, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv1 = bv1;
            this.bv2 = bv2;
            this.bkn = bkn;
            this.settings = settings;
            this.Name = "Калибровка БКН";
        }

        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            OnProgress(-1, Name);


            if (!await bkn.ClearCalibr()) 
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана. Нет соединения с БКН") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            (bool IsNorm, float Value) result;
            settings.Extend();
            settings.U_Min = 0;
            settings.U_Max = 55;
            settings.I_Max = 55;
            if (!await bv1.OnAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nНе удалось включить БВ1") ?? Task.FromResult(true));
                return await Task.FromResult(false); 
            }
            if (!await bv2.OnAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nНе удалось включить БВ2") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }

            settings.With_OS_OnCurrent = false;
            OnProgress(-1, "Включение КМ1");
            if (!await new OperationKMOn(bv1, bv2, bkn, settings).StartAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nНе удалось включить KM1") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            settings.Extend();

            float[] u_set = { 10, 40 };
            List<string> names = new List<string>() { "UБВ1", "UБВ2", "UШПП", "IНАГР", "UНАГР", "UОС" };
            for (Device_BKN.StatusInfo.CalibrationParamsEnum i = Device_BKN.StatusInfo.CalibrationParamsEnum.U_BV1; i <= Device_BKN.StatusInfo.CalibrationParamsEnum.U_OS; i++)
            {
                if (i == Device_BKN.StatusInfo.CalibrationParamsEnum.I_BUS) continue;

                this.Play($"-f {feqs[(int)NotesEnum.f]} -l 50 -n -f {feqs[(int)NotesEnum.a1]} -l 50 -n -f {feqs[(int)NotesEnum.c1]} -l 50 -n -f {feqs[(int)NotesEnum.e1]} -l 200");

                for (int p = 0; p < u_set.Length; p++)
                {
                    OnProgress(-1, $"Калибровка «{names[(int)i]}» {u_set[p]} В");

                    settings.U_Set = u_set[p];
                    if (!await bv1.SettingsAsync())
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nНе удалось установить напряжение {u_set[p]}В в БВ1") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }
                    if (!await bv2.SettingsAsync())
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nНе удалось установить напряжение {u_set[p]}В в БВ2") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }

                    result = await OnGetParameter?.Invoke($"Введите значение, замеренное на клеммах\n«{names[(int)i]}»", u_set[p] - limit, u_set[p] + limit, () =>
                    {
                        switch (i)
                        {
                            case Device_BKN.StatusInfo.CalibrationParamsEnum.U_BV1: return bkn.Status.U_BV1;
                            case Device_BKN.StatusInfo.CalibrationParamsEnum.U_BV2: return bkn.Status.U_BV2;
                            case Device_BKN.StatusInfo.CalibrationParamsEnum.U_BUS: return bkn.Status.U_BUS;
                            case Device_BKN.StatusInfo.CalibrationParamsEnum.U_CUR: return bkn.Status.U_CUR;
                            case Device_BKN.StatusInfo.CalibrationParamsEnum.U_OS: return bkn.Status.U_OS;
                            default: return 0;
                        }

                    });

                    if (!result.IsNorm)
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана оператором.") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }
                    this.Play($"-f {feqs[(int)NotesEnum.c1]} -l 50 -n -f {feqs[(int)NotesEnum.a1]} -l 100");

                    if (!await bkn.Calibrate(i, p, result.Value))
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nБКН не принимает калибровочную точку") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }
                    if (!await bkn.NewStatusAsync())
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана. Нет соединения с БКН") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }
                }
            }


            settings.U_Set = u_set[1];
            if (!await bv1.SettingsAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nНе удалось установить напряжение {u_set[1]}В в БВ1") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            if (!await bv2.SettingsAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nНе удалось установить напряжение {u_set[1]}В в БВ2") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }

            if (!await OnGetConfirm?.Invoke($"Подтвердите отсутствие нагрузки в ЭПН"))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }


            if (!await bkn.Calibrate(Device_BKN.StatusInfo.CalibrationParamsEnum.I_BUS, 0, 0))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nБКН не принимает калибровочную точку") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }

            await bkn.NewStatusAsync();
            OnProgress(-1, $"Калибровка «Iнагр» 40 А");
            if (!await OnGetConfirm?.Invoke($"Установите на ЭПН ток 40А"))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            result = await OnGetParameter?.Invoke($"Введите значение, замеренное на клеммах\n«Iнагр» (мВ)", 40 - limit, 40 + limit, () => bkn.Status.I_BUS);
            if (!result.IsNorm)
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            if (!await bkn.Calibrate(Device_BKN.StatusInfo.CalibrationParamsEnum.I_BUS, 1, result.Value))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана.\nБКН не принимает калибровочную точку") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            if (!await bkn.NewStatusAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка БКН» прервана. Нет соединения с БКН") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }


            OperationNormalize operationNormalize = new OperationNormalize(bv1, bv2, bkn, settings);
            operationNormalize.OnGetOk += async msg => await OnGetOk?.Invoke(msg);
            if(!await operationNormalize.StartAsync())
            {
                return await Task.FromResult(false);
            }

            await (OnGetOk?.Invoke($"Операция «Клибровка БКН» завершена\nОтключите ЭПН") ?? Task.FromResult(true)); 
            return await Task.FromResult(true);
        }
    }
}
