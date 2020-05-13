using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui
{

    public class SettingsInfo
    {
        public struct Limits
        {
            public float U_Set_min;
            public float U_Set_max;
            public float U_Min_min;
            public float U_Min_max;
            public float U_Max_min;
            public float U_Max_max;
            public float I_Max_min;
            public float I_Max_max;

            public static Limits Normal => new Limits()
            {
                U_Set_min = 24,
                U_Set_max = 38,
                U_Min_min = 22,
                U_Min_max = 38,
                U_Max_min = 26,
                U_Max_max = 40,
                I_Max_min = 0,
                I_Max_max = 55,
            };
            public static Limits Extended => new Limits()
            {
                U_Set_min = 0,
                U_Set_max = 50,
                U_Min_min = 0,
                U_Min_max = 50,
                U_Max_min = 0,
                U_Max_max = 50,
                I_Max_min = 0,
                I_Max_max = 60,
            };
        }
        string fileName = "settings.txt";
        public Limits limits = Limits.Normal;

        public event Action<float> OnUSetChanged;
        public event Action<float> OnUMinChanged;
        public event Action<float> OnUMaxChanged;
        public event Action<float> OnIMaxChanged;
        private float u_set = 30;
        private float u_min = 22;
        private float u_max = 32f;
        private float i_max = 60;
        private float[] loaded;
        public float U_Set
        {
            get => u_set;
            set
            {
                if (u_set != value)
                {
                    u_set = value.Clamp(limits.U_Set_min, limits.U_Set_max);
                    OnUSetChanged?.Invoke(u_set);
                }
            }
        }
        public float U_Min
        {
            get => u_min;
            set
            {
                if (u_min != value)
                {
                    u_min = value.Clamp(limits.U_Min_min, limits.U_Min_max);
                    OnUMinChanged?.Invoke(u_min);
                }
            }
        }
        public float U_Max
        {
            get => u_max;
            set
            {
                if (u_max != value)
                {
                    u_max = value.Clamp(limits.U_Max_min, limits.U_Max_max);
                    OnUMaxChanged?.Invoke(u_max);
                }
            }
        }
        public float I_Max
        {
            get => i_max;
            set
            {
                if (i_max != value)
                {
                    i_max = value.Clamp(limits.I_Max_min, limits.I_Max_max);
                    OnIMaxChanged?.Invoke(i_max);
                }
            }
        }
        public void Init()
        {
            loaded = new float[] { u_set, u_min, u_max, i_max };
            Load();
            ApplyLoadedUset();
            ApplyLoadedMinMax();
        }

        private void Load()
        {
            if (File.Exists(fileName)) 
            {
                var lines = File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; i++)
                {
                    if(float.TryParse(lines[i].Replace(" ", "").Split(':')[1], out float res))
                    {
                        loaded[i] = res;
                    }
                }
            }
        }

        public void Save()
        {
            loaded[0] = U_Set;
            loaded[1] = U_Min;
            loaded[2] = U_Max;
            loaded[3] = I_Max;
            string[] lines =
            { 
               $"U_Set: {U_Set}", 
               $"U_Min: {U_Min}", 
               $"U_Max: {U_Max}", 
               $"I_Max: {I_Max}"
            };
            Task.Run(async () =>
            {
                await Remounter.SetWritePermissionAsync();
                File.WriteAllLines(fileName, lines);
            });
        }

        public bool With_OS_OnCurrent { get; set; } = true;

        public void Normalize() => limits = Limits.Normal;
        public void Extend() => limits = Limits.Extended;

        public void ApplyLoadedUset()
        {
            u_set = loaded[0];
            OnUSetChanged?.Invoke(u_set);
        }
        internal void ApplyLoadedMinMax()
        {
            u_min = loaded[1];
            u_max = loaded[2];
            i_max = loaded[3];
            OnUMinChanged?.Invoke(u_min);
            OnUMaxChanged?.Invoke(u_max);
            OnIMaxChanged?.Invoke(i_max);

        }
    }
}
