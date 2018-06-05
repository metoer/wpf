using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// AlarmSearchControl.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmSearchControl : UserControl
    {
        public event Action<bool> CloseEvent = null;
        public AlarmSearchControl()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            alarmLevel.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("AlarmSearchControlAlarmLevelOrdinary").ToString(), ItemCode = "1", ItemID = "1" });
            alarmLevel.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("AlarmSearchControlAlarmLevelSecondary").ToString(), ItemCode = "2", ItemID = "2" });
            alarmLevel.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("AlarmSearchControlAlarmLevelSerious").ToString(), ItemCode = "3", ItemID = "3" });
            alarmLevel.TextEnabled = true;
            if (SearchManager.GetInstance().AlarmLogsSearch.AlarmLevel != null)
            {
                switch (SearchManager.GetInstance().AlarmLogsSearch.AlarmLevel)
                {
                    case "1":
                        alarmLevel.Text = TryFindResource("AlarmSearchControlAlarmLevelOrdinary").ToString();
                        alarmLevel.SelectValue = "1";
                        break;
                    case "2":
                        alarmLevel.Text = TryFindResource("AlarmSearchControlAlarmLevelSecondary").ToString();
                        alarmLevel.SelectValue = "2";
                        break;
                    case "3":
                        alarmLevel.Text = TryFindResource("AlarmSearchControlAlarmLevelSerious").ToString();
                        alarmLevel.SelectValue = "3";
                        break;
                }
            }

            alarmStatus.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("AlarmSearchControlAlarmStatusNoSolve").ToString(), ItemCode = "1", ItemID = "1" });
            alarmStatus.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("AlarmSearchControlAlarmStatusSolve").ToString(), ItemCode = "2", ItemID = "2" });
            alarmStatus.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("AlarmSearchControlAlarmStatusLose").ToString(), ItemCode = "3", ItemID = "3" });
            alarmStatus.TextEnabled = true;
            if (SearchManager.GetInstance().AlarmLogsSearch.AlarmStatus != null)
            {
                switch (SearchManager.GetInstance().AlarmLogsSearch.AlarmStatus)
                {
                    case "1":
                        alarmStatus.Text = TryFindResource("AlarmSearchControlAlarmStatusNoSolve").ToString();
                        alarmStatus.SelectValue = "1";
                        break;
                    case "2":
                        alarmStatus.Text = TryFindResource("AlarmSearchControlAlarmStatusSolve").ToString();
                        alarmStatus.SelectValue = "2";
                        break;
                    case "3":
                        alarmStatus.Text = TryFindResource("AlarmSearchControlAlarmStatusLose").ToString();
                        alarmStatus.SelectValue = "3";
                        break;
                }
            }

            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_00").ToString(), ItemCode = "16,17,18,21,23,25,28", ItemID = "16,17,18,21,23,25,28" });
            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_16").ToString(), ItemCode = "16", ItemID = "16" });
            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_17").ToString(), ItemCode = "17", ItemID = "17" });
            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_18").ToString(), ItemCode = "18", ItemID = "18" });
            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_21").ToString(), ItemCode = "21", ItemID = "21" });
            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_23").ToString(), ItemCode = "23", ItemID = "23" });
            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_25").ToString(), ItemCode = "25", ItemID = "25" });
            alarmModule.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("Alarm_28").ToString(), ItemCode = "28", ItemID = "28" });
            alarmModule.TextEnabled = true;
            if (SearchManager.GetInstance().AlarmLogsSearch.AlarmModule != null)
            {
                switch (SearchManager.GetInstance().AlarmLogsSearch.AlarmModule)
                {
                    case "16":
                        alarmModule.Text = TryFindResource("Alarm_16").ToString();
                        alarmModule.SelectValue = "16";
                        break;
                    case "17":
                        alarmModule.Text = TryFindResource("Alarm_17").ToString();
                        alarmModule.SelectValue = "17";
                        break;
                    case "18":
                        alarmModule.Text = TryFindResource("Alarm_18").ToString();
                        alarmModule.SelectValue = "18";
                        break;
                    case "21":
                        alarmModule.Text = TryFindResource("Alarm_21").ToString();
                        alarmModule.SelectValue = "21";
                        break;
                    case "23":
                        alarmModule.Text = TryFindResource("Alarm_23").ToString();
                        alarmModule.SelectValue = "23";
                        break;
                    case "25":
                        alarmModule.Text = TryFindResource("Alarm_25").ToString();
                        alarmModule.SelectValue = "25";
                        break;
                    case "28":
                        alarmModule.Text = TryFindResource("Alarm_28").ToString();
                        alarmModule.SelectValue = "28";
                        break;
                    case "16,17,18,21,23,25,28":
                        alarmModule.Text = TryFindResource("Alarm_00").ToString();
                        alarmModule.SelectValue = "16,17,18,21,23,25,28";
                        break;
                }
            }

            tbAlarmCode.Text = SearchManager.GetInstance().AlarmLogsSearch.AlarmCode;
        }

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            ModelResponsible.Instance.ClearAlarmLogs();
            SearchManager.GetInstance().AlarmLogsSearch.PageIndex = 1;
            SearchManager.GetInstance().AlarmLogsSearch.IsAdvanced = true;
            SearchManager.GetInstance().AlarmLogsSearch.AlarmLevel = alarmLevel.SelectValue;
            SearchManager.GetInstance().AlarmLogsSearch.AlarmModule = alarmModule.SelectValue;
            SearchManager.GetInstance().AlarmLogsSearch.AlarmStatus = alarmStatus.SelectValue;
            SearchManager.GetInstance().SearchAlarmLogCount(SearchManager.GetInstance().AlarmLogsSearch);
            SearchManager.GetInstance().SearchAlarmLogDetail(SearchManager.GetInstance().AlarmLogsSearch);
            SearchManager.GetInstance().SendOperationLog("CollectHistoryWarnHighSearch");
            LogHelper.Instance.WirteLog(string.Format("AlarmSearchControl: UserCode:{0} btnSure", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            if (CloseEvent != null)
                CloseEvent(true);
        }
        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            if (CloseEvent != null)
                CloseEvent(true);
        }
    }
}
