using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Main.Converter;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Resources.Controls;
using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hytera.EEMS.Main.Controls
{
    /// <summary>
    /// AppStateControl.xaml 的交互逻辑
    /// </summary>
    public partial class AppStateControl : UserControl, IDisposable
    {
        /// <summary>
        /// 计数
        /// </summary>
        int count = 0;

        /// <summary>
        /// 定时器
        /// </summary>
        Timer timer = new Timer();

        /// <summary>
        /// 构造
        /// </summary>
        public AppStateControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.Interval = 1000;
                timer.Elapsed += timer_Elapsed;
                timer.Start();

                btnData.SetBinding(SelectButton.IsSelectProperty, new Binding("DataBaseState") { Source = AppConfigInfos.AppStateInfos, Converter = new Int32Converter() });
                btnServer.SetBinding(SelectButton.IsSelectProperty, new Binding("ServerState") { Source = AppConfigInfos.AppStateInfos, Converter = new Int32Converter() });
                tbServerIp.SetBinding(TextBlock.TextProperty, new Binding("ServerIp") { Source = AppConfigInfos.AppStateInfos, Converter = new EmptyTextConverter() });

                pbMemory.Maximum = SystemInfo.PhysicalMemory;

                AppConfigInfos.AppStateInfos.PropertyChanged += AppStateInfos_PropertyChanged;

                // 获取本机网络信息 
                MainMessage.Instance.SendCommand(Model.MsgType.PcStateRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.WirteErrorMsg(ex.Message);
            }
        }


        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    count++;
                    tbTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    tbCpu.Text = string.Format("CPU: {0:f}%", SystemInfo.GetCpuLoad.ToString("0.00"));

                    if (count > 10)
                    {
                        count = 0;
                    }

                    if (count == 1)
                    {
                        SetDriveInfo(AppConfigInfos.AppStateInfos.MemoryPath);
                        pbMemory.Value = SystemInfo.PhysicalMemory - SystemInfo.MemoryAvailable;
                        tbp.Text = string.Format("{0}/{1}", SystemInfo.GetMemoryUnit((long)pbMemory.Value), SystemInfo.GetMemoryUnit(SystemInfo.PhysicalMemory));
                    }
                }));
            }
            catch (Exception ex)
            {
                SystemInfo.StartProcess("cmd.exe", "lodctr /r");
                LogHelper.Instance.WirteErrorMsg(ex.Message);
            }
        }

        public void Dispose()
        {
            timer.Stop();
        }

        /// <summary>
        /// 设置剩余内存
        /// </summary>
        /// <param name="path"></param>
        private void SetDriveInfo(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            path = path.Replace("/", "\\");

            DriveInfo[] allDirves = DriveInfo.GetDrives();
            //检索计算机上的所有逻辑驱动器名称
            foreach (DriveInfo item in allDirves)
            {
                //Fixed 硬盘
                if (item.IsReady && path.StartsWith(item.Name))
                { 
                    //单位B
                    long totalSize = item.TotalSize;/// (1024 * 1024 * 1024);
                    long useSize = totalSize - item.TotalFreeSpace;// -(item.TotalFreeSpace / (1024 * 1024 * 1024));
                    pbMemorySize.Maximum = totalSize;
                    pbMemorySize.Value = useSize;
                    tbMemorySize.Text = string.Format("{0}/{1}", SystemInfo.GetMemoryUnit(useSize), SystemInfo.GetMemoryUnit(totalSize));
                }
            }
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            popupDataStatus.IsOpen = SetDataPopup();
        }

        private bool SetDataPopup()
        {
            if (AppConfigInfos.AppStateInfos.DataBaseState.Equals("0"))
            {
                popupDataStatus.IsOpen = false;
                return false;
            }

            string msg = (TryFindResource("DataBaseConnect_" + AppConfigInfos.AppStateInfos.DataBaseMsgCode) ?? string.Empty).ToString();
            msg = string.IsNullOrEmpty(msg) ? "Undefine Error" : msg;

            txtDataMsg.Text = msg;

            return true;
        }

        private void btnServer_Click(object sender, RoutedEventArgs e)
        {
            popupServerStatus.IsOpen = SetServerPopup();
        }

        private bool SetServerPopup()
        {
            if (AppConfigInfos.AppStateInfos.ServerState.Equals("0"))
            {
                popupServerStatus.IsOpen = false;
                return false;
            }

            string msg = (TryFindResource("ServerConnect_" + AppConfigInfos.AppStateInfos.ServerMsgCode) ?? string.Empty).ToString();
            msg = string.IsNullOrEmpty(msg) ? "Undefine Error" : msg;

            txtServerMsg.Text = msg;
            return true;
        }

        /// <summary>
        /// 监听连接状态,状态提示框打开情况下，如状态正常，则关闭状态，否则修改状态内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppStateInfos_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DataBaseState"))
            {
                // 获取数据库记录
                if (AppConfigInfos.AppStateInfos.DataBaseState.Equals("0"))
                {
                    // 获取设备类型
                    MainMessage.Instance.SendCommand(Model.MsgType.DeviceTypesRequest);
                }

                if (popupDataStatus.IsOpen)
                {
                    SetDataPopup();
                }
            }
            else if (e.PropertyName.Equals("ServerState") && popupServerStatus.IsOpen)
            {
                SetServerPopup();
            }

        }
    }
}
