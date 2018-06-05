using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hytera.EEMS.Resources
{
    public class AppHelper
    {
        /// <summary>
        /// 检查App状态
        /// </summary>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public static bool CheckAppState(Window Owner)
        {
            if (!AppConfigInfos.IceConnect)
            {
                NewMessageBox.Show(Owner.TryFindResource("appMainDataConnect").ToString(), Owner);
                return false;
            }

            //修改比较顺序，AppConfigInfos.AppStateInfos.DataBaseState可能为空
            if ("1".Equals(AppConfigInfos.AppStateInfos.DataBaseState))
            {
                NewMessageBox.Show(Owner.TryFindResource("appMainDataBaseFailed").ToString(), Owner);
                return false;
            }

            //if (AppConfigInfos.AppStateInfos.ServerState.Equals("1"))
            //{
            //    NewMessageBox.Show("采集站与后台管理中心连接失败，请检查参数配置！", Owner);
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 启动键盘
        /// </summary>
        public static void StartKey(Window Owner)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Write.exe";

            if (File.Exists(filePath))
            {
                Process p = new Process();
                p.StartInfo.FileName = filePath;
                p.StartInfo.UseShellExecute = false;
                p.Start();
                p.Close();
            }
            else
            {
                NewMessageBox.Show(Owner.TryFindResource("appMainKeyFileNoFound").ToString());
            }
        }

        /// <summary>
        /// 是否已经显示键盘
        /// </summary>
        /// <returns></returns>
        public static bool IsWriteShow()
        {
            Process[] processes = Process.GetProcesses();
            foreach (var p in processes)
            {
                if (p.ProcessName.Equals("Write"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 关闭键盘
        /// </summary>
        public static void CloseKey()
        { 
             Process[] processes = Process.GetProcesses();
             foreach (var p in processes)
             {
                 if (p.ProcessName.Equals("Write"))
                 {
                     p.Kill();
                     p.Close();
                     break;
                 }
             }
        }
    }
}
