using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Lib;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// AlarmControl.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmControl : UserControl
    {
        public static readonly DependencyProperty ShowMoreTextProperty = DependencyProperty.Register("ShowMoreText", typeof(string), typeof(AlarmControl));
        public string ShowMoreText
        {
            get
            {
                return (string)GetValue(ShowMoreTextProperty);
            }
            set
            {
                SetValue(ShowMoreTextProperty, value);
            }
        }

        private int Count;

        public AlarmControl()
        {
            InitializeComponent();
        }
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            dataGrid.ItemsSource = ManageViewModel.AlarmLogs;
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa" });
            //ManageViewModel.AlarmLogs.Add(new AlarmInfo() { AlarmCode = "9925", AlarmLevel = "1", DeviceID = "safsa", Vis = Visibility.Visible });
        }

        public void UpdateDetailCount()
        {
            ShowMoreText = string.Format("{0}/{1},", ManageViewModel.AlarmLogs.Count, Count);
            UpdateBtnVisible();
            LogHelper.Instance.WirteLog(string.Format("AlarmControl: UserCode:{0} UpdateDetailCount Count:{1}", AppConfigInfos.CurrentUserInfos.UserCode,Count.ToString()), LogLevel.LogDebug);
        }

        public void UpdateCount(string datacount)
        {
            Int32.TryParse(datacount, out Count);
            UpdateDetailCount();
            LogHelper.Instance.WirteLog(string.Format("AlarmControl: UserCode:{0} UpdateCount Count:{1}", AppConfigInfos.CurrentUserInfos.UserCode, Count.ToString()), LogLevel.LogDebug);
        }

        private void UpdateBtnVisible()
        {
            ManageViewModel.AlarmLogs.ToList().ForEach(p => p.Vis = Visibility.Hidden);

            if (ManageViewModel.AlarmLogs.Count < Count && Count > AppConfigInfos.AppStateInfos.SearchPageCount && ManageViewModel.AlarmLogs.Count > 0)
                ManageViewModel.AlarmLogs[ManageViewModel.AlarmLogs.Count - 1].Vis = Visibility.Visible;

            if (Count < 1)
                spNodata.Visibility = Visibility.Visible;
            else
                spNodata.Visibility = Visibility.Collapsed;
        }

        private void btnPicView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateDetailCount();
            SearchManager.GetInstance().AlarmLogsSearch.PageIndex++;
            SearchManager.GetInstance().SearchAlarmLogDetail(SearchManager.GetInstance().AlarmLogsSearch);
            LogHelper.Instance.WirteLog(string.Format("AlarmControl: UserCode:{0} ShoeMoreBtn", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
    }
}
