using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui
{
    public static class Remounter
    {

        private static bool IsWritePermission = false;
        private static DateTime writePermissionTimer;
        public static async Task SetWritePermissionAsync()
        {
            writePermissionTimer = DateTime.Now;
            if (!IsWritePermission)
            {
                IsWritePermission = true;
                await SetRemountRW(true);

                _ = Task.Run(async () =>
                {
                    while ((DateTime.Now - writePermissionTimer).TotalSeconds < 5)
                    {
                        await Task.Delay(100);
                    }
                    await SetRemountRW(false);
                    IsWritePermission = false;
                });
            }
        }

        private static async Task SetRemountRW(bool state)
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.FileName = state ? "remountrw" : "remountro";
                    if (myProcess.Start())
                    {
                        Console.WriteLine($"myProcess.WaitForExitAsync {state}");
                        await myProcess.WaitForExitAsync();
                        Console.WriteLine("WaitForExitAsync");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
