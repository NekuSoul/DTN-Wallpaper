using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Win32;
using Timer = System.Timers.Timer;

namespace DTN_Wallpaper
{
    internal static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private static float _fileCount;

        [STAThread]
        private static void Main()
        {
            _fileCount = Directory.GetFiles(@"Rotation", @"*.png").Length;

            Timer_Elapsed(null, null);
            var timer = new Timer { Interval = 24 * 60 / _fileCount * 60 * 1000 * 0.9 };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Application.Run();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int currentTime = (int)((DateTime.Now.Hour * 60 + DateTime.Now.Minute) / (24 * 60 / _fileCount)) + 1;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (key != null)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            SystemParametersInfo(20, 0, Path.Combine(Directory.GetCurrentDirectory(), @"Rotation", currentTime + ".png"), 0x01 | 0x02);
        }
    }
}
