using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Common
{
    public class SoundHelperBak
    {
        public static uint SND_ASYNC = 0x0001;
        public static uint SND_FILENAME = 0x00020000;
        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand, string lpstrReturnString, uint uReturnLength, IntPtr hWndCallback);

        /// <summary>
        /// 播放指定文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void PlaySound(string filePath)
        {
            if (File.Exists(filePath))
            {
                mciSendString(@"close temp_alias", null, 0, IntPtr.Zero);
                mciSendString(string.Format(@"open ""{0}"" type waveaudio alias temp_alias", filePath), null, 0, IntPtr.Zero);
                mciSendString("play temp_alias repeat", null, 0, IntPtr.Zero);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void StopSound()
        {
            mciSendString(@"close temp_alias", null, 0, IntPtr.Zero);
        }
    }
}
