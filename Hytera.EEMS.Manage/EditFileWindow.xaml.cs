using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using System;
using System.Timers;
using System.Windows;

namespace Hytera.EEMS.Manage
{
    /// <summary>
    /// EditFileWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditFileWindow : BaseWindow
    {
        Timer timer = new Timer();
        public EditFileWindow()
        {
            InitializeComponent();

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

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            ((MediaInfo)this.DataContext).UpdateMark = tbMark.Text;
            ((MediaInfo)this.DataContext).UpdateUserImp = impSelect.IsSelect ? "1" : "0";
            SearchManager.GetInstance().SaveMediaInfo((MediaInfo)this.DataContext);
            LogHelper.Instance.WirteLog(string.Format("EditFileWindow: UserCode:{0} btnSure  ", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            SearchManager.GetInstance().SendOperationLog("CollectEvidenceEditor", (MediaInfo)this.DataContext);
            this.Close();
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            impSelect.IsSelect = !impSelect.IsSelect;
        }
        public void UpdateMsg(MediaInfo mi)
        {
            this.DataContext = mi;
            impSelect.IsSelect = mi.UserImp == "1" ? true : false;
        }
    }
}
