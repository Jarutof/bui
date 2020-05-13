using bui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationCalibrationBV : Operation
    {
        float[] feqs = { 220, 246.94f, 261.63f, 293.66f, 329.63f, 349.23f, 415.3f, 440f, 493.88f, 523.26f, 587.32f, 659.26f };

        const float limit = 5;
        const float cur_limit = 25;
        

        public event Func<string, float, float, Func<float>, Task<(bool, float)>> OnGetParameter;
        public event Func<string, Task<bool>> OnGetConfirm;
        public event Func<string, Task<bool>> OnGetOk;
        private Device_BV bv_target;
        private Device_BV bv_other;
        private Device_BKN bkn;
        private SettingsInfo settings;
        private string bvName;
        private string bvNameOther;

        public OperationCalibrationBV(string bvName, string bvNameOther, Device_BV bv_target, Device_BV bv_other, Device_BKN bkn, SettingsInfo settings)
        {
            this.bv_target = bv_target;
            this.bv_other = bv_other;
            this.bkn = bkn;
            this.settings = settings;
            this.bvName = bvName;
            this.bvNameOther = bvNameOther;
            this.Name = $"Калибровка {bvName}";

        }
        public override async Task<bool> ProcessAsync(CancellationToken token)
        {
            OnProgress(-1, Name);

            (bool IsNorm, float Value) result;
            settings.Extend();
            settings.U_Min = 0;
            settings.U_Max = 55;
            settings.I_Max = 55;
            if (!await bv_target.SettingsAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\nНет соединения с {bvName}") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }

            if (!await bv_target.OnAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\nНет соединения с {bvName}") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }

            if (bv_other.IsConnected)
            {
                if(!await bv_other.OffAsync())
                {
                    await (OnGetOk?.Invoke($"Операция «Калибровка {bvNameOther}» прервана.\nНе удается отключить {bvNameOther}") ?? Task.FromResult(true));
                    return await Task.FromResult(false);
                }
            }
            settings.With_OS_OnCurrent = false;

            OnProgress(-1, "Включение КМ1");
            if (!await new OperationKMOn(bv_target, bv_other, bkn, settings).StartAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\nНе удалось включить KM1") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            settings.Extend();

            float[] u_set = { 10, 40 };
            List<string> names = new List<string>() { "Uвых", "Uос" };


            for (int i = 0; i < 2; i++)
            {
                
                this.Play($"-f {feqs[(int)NotesEnum.f]} -l 50 -n -f {feqs[(int)NotesEnum.a1]} -l 50 -n -f {feqs[(int)NotesEnum.c1]} -l 50 -n -f {feqs[(int)NotesEnum.e1]} -l 200");

                for (int p = 0; p < u_set.Length; p++)
                {
                    OnProgress(-1, $"Калибровка «{names[i]}» {u_set[p]} В");

                    settings.U_Set = u_set[p];
                    if (!await bv_target.SettingsAsync())
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\nНе удалось установить напряжение {u_set[p]}В в {bvName}") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }

                    result = await OnGetParameter?.Invoke($"Введите значение, замеренное на клеммах\n«{names[i]}» {bvName} (В)", u_set[p] - limit, u_set[p] + limit, () => bv_target.Status.U_Out);
                    if (!result.IsNorm)
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана оператором.") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }
                    this.Play($"-f {feqs[(int)NotesEnum.c1]} -l 50 -n -f {feqs[(int)NotesEnum.a1]} -l 100");

                    if (!await bv_target.Calibrate(i, p, result.Value))
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\n{bvName} не принимает калибровочную точку") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }
                    if (!await bv_target.NewStatusAsync())
                    {
                        await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана. Нет соединения с {bvName}") ?? Task.FromResult(true));
                        return await Task.FromResult(false);
                    }
                }
            }


            settings.U_Set = 30;
            if (!await bv_target.SettingsAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\nНе удалось установить напряжение 30 В в {bvName}") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            OnProgress(-1, $"Калибровка «Iвых» 10А");

            if (!await OnGetConfirm?.Invoke($"Установите на ЭПН ток 10А"))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            this.Play($"-f {feqs[(int)NotesEnum.f]} -l 50 -n -f {feqs[(int)NotesEnum.a1]} -l 50 -n -f {feqs[(int)NotesEnum.c1]} -l 50 -n -f {feqs[(int)NotesEnum.e1]} -l 200");
            
            result = await OnGetParameter?.Invoke($"Введите значение, замеренное на клеммах\n«Iвых» {bvName} (мВ)", 100 - cur_limit, 100 + cur_limit, () => bv_target.Status.I_Out * 10);
            if (!result.IsNorm)
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            this.Play($"-f {feqs[(int)NotesEnum.c1]} -l 50 -n -f {feqs[(int)NotesEnum.a1]} -l 100");

            if (!await bv_target.Calibrate(2, 0, result.Value * 0.1f))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\n{bvName} не принимает калибровочную точку") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            if (!await bv_target.NewStatusAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана. Нет соединения с {bvName}") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            OnProgress(-1, $"Калибровка «Iвых» 40А");
            if (!await OnGetConfirm?.Invoke($"Установите на ЭПН ток 40А"))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }

            result = await OnGetParameter?.Invoke($"Введите значение, замеренное на клеммах\n«Iвых» {bvName} (мВ)", 400 - limit, 400 + limit, () => bv_target.Status.I_Out * 10);
            if (!result.IsNorm)
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана оператором.") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            this.Play($"-f {feqs[(int)NotesEnum.c1]} -l 50 -n -f {feqs[(int)NotesEnum.a1]} -l 100");

            if (!await bv_target.Calibrate(2, 1, result.Value * 0.1f))
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана.\n{bvName} не принимает калибровочную точку") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }
            if (!await bv_target.NewStatusAsync())
            {
                await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}» прервана. Нет соединения с {bvName}") ?? Task.FromResult(true));
                return await Task.FromResult(false);
            }

            OperationNormalize operationNormalize = new OperationNormalize(bv_target.Address == 1 ? bv_target : bv_other, bv_target.Address == 2 ? bv_target : bv_other, bkn, settings);
            operationNormalize.OnGetOk += async msg => await OnGetOk?.Invoke(msg);
            if(!await operationNormalize.StartAsync())
            {
                return await Task.FromResult(false);
            }
            await (OnGetOk?.Invoke($"Операция «Калибровка {bvName}»\nзавершена.\nОтключите ЭПН.") ?? Task.FromResult(true));
            return await Task.FromResult(true);

        }


    }
}
