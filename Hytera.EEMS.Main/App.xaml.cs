using Hytera.EEMS.Common;
using Hytera.EEMS.Log;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Main.Welcome;
using System;
using System.Windows;

namespace Hytera.EEMS.Main
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                InitException();
                //  this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
                Startup += Application_Startup;
                Exit += Application_Exit;

                // ThemesHelper.InitResource();

                ThemesHelper.InitSkinData();

                ThemesHelper.SetSkin("Default");
            }
            catch (Exception ex)
            {
                LogHelper.Instance.WirteErrorMsg(string.Format("App Error,Msg:{0}", ex.Message));
                LogHelper.Instance.WirteErrorMsg(string.Format("App Error,Msg:{0}", ex.StackTrace));
                MiniDump.TryDump(String.Format("{0}MiniDmp.dmp", DateTime.Now.ToString()));
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WindowsHelper.ShowWindow<WelcomeWindow>();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }

        /// <summary>
        /// 初始化异常处理
        /// </summary>
        void InitException()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        /// <summary>
        /// 主线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogHelper.Instance.WirteErrorMsg(e.Exception.Message);
            LogHelper.Instance.WirteErrorMsg(e.Exception.StackTrace);
            e.Handled = true;
        }

        /// <summary>
        /// 其他线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Instance.WirteErrorMsg(e.ExceptionObject.ToString());
        }
    }
}
