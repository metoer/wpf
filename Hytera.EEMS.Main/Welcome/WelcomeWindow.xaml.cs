using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Hytera.EEMS.Main.Welcome
{
    /// <summary>
    /// WelcomeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WelcomeWindow : BaseWindow
    {
        bool isClosed = false;

        public WelcomeWindow()
        {
            AppConfigHelper.InitAppConfig();

            // 加载默认语言资源
            LanguageInfos languageInfo = AppConfigInfos.LanguageList.Find(p => p.IsChecked);
            if (languageInfo != null)
            {
                ThemesHelper.SetLanguage(languageInfo.ID);
            }

            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(() => { LoadData(); }) { IsBackground = true }.Start();
        }

        /// <summary>
        /// 线程加载数据
        /// </summary>
        private void LoadData()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
          
            PrintMsg("appMainLoadConfig", "20%");

            LogHelper.Instance.Init();

            PrintMsg("appMainInitModule", "40%");

            // 模块初始化
            ModuleDispather.Instance.Init();

            WarnInfo.WarnHelper.Instance.Init();

            if (!AppConfigInfos.IceConnect)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    NewMessageBox.Show(TryFindResource("appMainDataConnect").ToString(), this);
                    App.Current.Shutdown();
                }));
                return;
            }

            // 采集端口信息
            MainMessage.Instance.SendCommand(Model.MsgType.PortInfosRequest);

            // license信息
            MainMessage.Instance.SendCommand(Model.MsgType.LicenseRequest);

            PrintMsg("appMainLoadPcInfo", "60%");

            // 设置优先端口
            if (!string.IsNullOrEmpty(AppConfigInfos.PortDeviceList.FirstPortCode))
            {
                Conditions con = new Conditions();
                con.AddItem("PortCode", AppConfigInfos.PortDeviceList.FirstPortCode.Equals("----") ? string.Empty : AppConfigInfos.PortDeviceList.FirstPortCode);
                con.AddItem("Respond", "1");
                MainMessage.Instance.SendMessage(Model.MsgType.SetFirstPortRequest, con);
            }

            PrintMsg("appMainLoadDeviceInfo", "80%");

            // 获取执法记录仪信息列表
            MainMessage.Instance.SendCommand(Model.MsgType.DeviceInfosRequest);

            PrintMsg("appMainLoadPersonInfo", "100%");

            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds < 5000)
            {
                Thread.Sleep((int)(5000 - stopwatch.ElapsedMilliseconds));
            }

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                isClosed = true;
                this.Close();
                WindowsHelper.ShowWindow<MainWindow>();
            }));
        }

        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="percent"></param>
        private void PrintMsg(string message, string percent)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
               {
                   labpg.Content = percent;
                   tbMsg.Text = TryFindResource(message).ToString();
               }));
        }

        /// <summary>
        /// 非正常关闭时退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (!isClosed)
            {
                App.Current.Shutdown();
            }
        }
    }
}
