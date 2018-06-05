using Hytera.EEMS.Common;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Enums;
using Hytera.EEMS.Resources.Controls;
using System;
using System.Timers;
using System.Windows;

namespace Hytera.EEMS.Manage
{
    /// <summary>
    /// SearchConditionWindows.xaml 的交互逻辑
    /// </summary>
    public partial class SearchConditionWindows : BaseWindow
    {
        Timer timer = new Timer();
        public QueryType QueryType
        {
            set
            {
                mediaSC.Visibility = Visibility.Collapsed;
                cameralogsSC.Visibility = Visibility.Collapsed;
                alarmSC.Visibility = Visibility.Collapsed;
                collectlogsSC.Visibility = Visibility.Collapsed;
                switch (value)
                {
                    case QueryType.CameraOperateLog:
                        cameralogsSC.Visibility = Visibility.Visible;
                        this.Height = 234;
                        break;
                    case QueryType.CollectOperateLog:
                        collectlogsSC.Visibility = Visibility.Visible;
                        this.Height = 234;
                        break;
                    case QueryType.MediaLog:
                        mediaSC.Visibility = Visibility.Visible;
                        this.Height = 295;
                        break;
                    case QueryType.HisAlarm:
                        alarmSC.Visibility = Visibility.Visible;
                        this.Height = 234;
                        break;
                }
            }
        }
        public SearchConditionWindows()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cameralogsSC.DataContext = SearchManager.GetInstance().CameraLogsSerach;
            alarmSC.DataContext = SearchManager.GetInstance().AlarmLogsSearch;
            mediaSC.DataContext = SearchManager.GetInstance().MediaLogsSerach;
            collectlogsSC.DataContext = SearchManager.GetInstance().CollectLogsSerach;

            cameralogsSC.InitData();
            collectlogsSC.InitData();

            mediaSC.CloseEvent += new Action<bool>(ColseWindow);
            cameralogsSC.CloseEvent += new Action<bool>(ColseWindow);
            alarmSC.CloseEvent += new Action<bool>(ColseWindow);
            collectlogsSC.CloseEvent += new Action<bool>(ColseWindow);

            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (SystemInfo.GetLastInputTime() > 300000)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        timer.Stop();
                        this.Close();
                    }));
                }
            }));
        }

        private void ColseWindow(bool close)
        {
            this.Close();
        }
    }
}
