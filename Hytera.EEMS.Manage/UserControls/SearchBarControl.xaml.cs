using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Enums;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// SearchControl.xaml 的交互逻辑
    /// </summary>
    public partial class SearchBarControl : UserControl
    {
        public QueryType QueryType;
        
        public SearchBarControl()
        {
            InitializeComponent();
            InitDate();
        }
        private void InitDate()
        {
            startTime.CheckTimeEvent += new Action<string>(CheckStartTimeEvent);
            endTime.CheckTimeEvent += new Action<string>(CheckEndTimeEvent);
        }
        private void CheckEndTimeEvent(string endTimestr)
        {
            if (DateTime.Parse(endTimestr) < DateTime.Parse(startTime.txtDate.Text))
            {
                NewMessageBox.Show(TryFindResource("SearchBarControlCheckEndTime").ToString());
                return;
            }

            SearchManager.GetInstance().MediaLogsSerach.CollectEndTime = endTimestr;

            SearchManager.GetInstance().CollectLogsSerach.CollectEndTime = endTimestr;

            SearchManager.GetInstance().CameraLogsSerach.CollectEndTime = endTimestr;

            SearchManager.GetInstance().AlarmLogsSearch.AlarmtEndTime = endTimestr;

            endTime.txtDate.Text = endTimestr;
        }

        private void CheckStartTimeEvent(string startTimestr)
        {
            if (DateTime.Parse(endTime.txtDate.Text) < DateTime.Parse(startTimestr))
            {
                NewMessageBox.Show(TryFindResource("SearchBarControlCheckStartTime").ToString());
                return;
            }

            SearchManager.GetInstance().MediaLogsSerach.CollectStartTime = startTimestr;

            SearchManager.GetInstance().CollectLogsSerach.CollectStartTime = startTimestr;

            SearchManager.GetInstance().CameraLogsSerach.CollectStartTime = startTimestr;

            SearchManager.GetInstance().AlarmLogsSearch.AlarmStartTime = startTimestr;

            startTime.txtDate.Text = startTimestr;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //NewMessageBox.ShowTip(TryFindResource("FileButtonControlAddSucceed").ToString(),null,1000);
            //ModelResponsible.Instance.UpdateCount(QueryType.MediaLog, FileType.Video, "10000000");
            //WindowsHelper.ShowDialogWindow<PlayWindow>(ModelResponsible.Instance.ParentWindow);
            //WindowsHelper.ShowDialogWindow<EditFileWindow>(ModelResponsible.Instance.ParentWindow);
            //return;
            switch (QueryType)
            {
                case QueryType.MediaLog:
                    ModelResponsible.Instance.ClearMediaList();
                    SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
                    SearchManager.GetInstance().MediaLogsSerach.IsAdvanced = false;
                    SearchManager.GetInstance().MediaLogsSerach.SearchTime = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    SearchManager.GetInstance().SearchMediaLogCount(SearchManager.GetInstance().MediaLogsSerach);
                    SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);
                    SearchManager.GetInstance().SendOperationLog("CollectEvidenceSearch");
                    break;
                case QueryType.CollectOperateLog:
                    ModelResponsible.Instance.ClearCollectLogs();
                    SearchManager.GetInstance().CollectLogsSerach.PageIndex = 1;
                    SearchManager.GetInstance().CollectLogsSerach.LogType = "1";
                    SearchManager.GetInstance().CollectLogsSerach.IsAdvanced = false;
                    SearchManager.GetInstance().SearchCollectLogCount(SearchManager.GetInstance().CollectLogsSerach);
                    SearchManager.GetInstance().SearchCollectLogDetail(SearchManager.GetInstance().CollectLogsSerach);
                    SearchManager.GetInstance().SendOperationLog("CollectLogSearch");
                    break;
                case QueryType.CameraOperateLog:
                    ModelResponsible.Instance.ClearCameraLogs();
                    SearchManager.GetInstance().CameraLogsSerach.PageIndex = 1;
                    SearchManager.GetInstance().CameraLogsSerach.LogType = "2";
                    SearchManager.GetInstance().CameraLogsSerach.IsAdvanced = false;
                    SearchManager.GetInstance().SearchCameraLogCount(SearchManager.GetInstance().CameraLogsSerach);
                    SearchManager.GetInstance().SearchCameraLogDetail(SearchManager.GetInstance().CameraLogsSerach);
                    SearchManager.GetInstance().SendOperationLog("CollectCameraLogSearch");
                    break;
                case QueryType.HisAlarm:
                    ModelResponsible.Instance.ClearAlarmLogs();
                    SearchManager.GetInstance().AlarmLogsSearch.PageIndex = 1;
                    SearchManager.GetInstance().AlarmLogsSearch.IsAdvanced = false;
                    SearchManager.GetInstance().SearchAlarmLogCount(SearchManager.GetInstance().AlarmLogsSearch);
                    SearchManager.GetInstance().SearchAlarmLogDetail(SearchManager.GetInstance().AlarmLogsSearch);
                    SearchManager.GetInstance().SendOperationLog("CollectHistoryWarnSearch");
                    break;
            }
            LogHelper.Instance.WirteLog(string.Format("SearchBarControl: UserCode:{0} btnSearch  QueryType:{1}", AppConfigInfos.CurrentUserInfos.UserCode, QueryType), LogLevel.LogDebug);
            
        }

        private void btnAdvanced_Click(object sender, RoutedEventArgs e)
        {
            WindowsHelper.GetOrNewWindow<SearchConditionWindows>(true).QueryType = this.QueryType;
            WindowsHelper.ShowDialogWindow<SearchConditionWindows>(ModelResponsible.Instance.ParentWindow);
            LogHelper.Instance.WirteLog(string.Format("SearchBarControl: UserCode:{0} btnAdvanced  QueryType:{1}", AppConfigInfos.CurrentUserInfos.UserCode, QueryType), LogLevel.LogDebug);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppConfigInfos.LimitsUserInfos.UserCode != null && !AppConfigInfos.LimitsUserInfos.UserCode.Equals(SearchManager.GetInstance().CurrentUserInfo.UserCode))
            {
                SearchManager.GetInstance().MediaLogsSerach = new MediaLogsSerach();
                SearchManager.GetInstance().MediaLogsSerach.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;
                SearchManager.GetInstance().CollectLogsSerach = new CollectLogsSerach();
                SearchManager.GetInstance().CollectLogsSerach.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;
                SearchManager.GetInstance().CameraLogsSerach = new CameraLogsSerach();
                SearchManager.GetInstance().CameraLogsSerach.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;
                SearchManager.GetInstance().AlarmLogsSearch = new AlarmLogsSearch();
                SearchManager.GetInstance().AlarmLogsSearch.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;


                startTime.txtDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
                endTime.txtDate.Text = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                SearchManager.GetInstance().MediaLogsSerach.CollectEndTime = endTime.txtDate.Text;
                SearchManager.GetInstance().CollectLogsSerach.CollectEndTime = endTime.txtDate.Text;
                SearchManager.GetInstance().CameraLogsSerach.CollectEndTime = endTime.txtDate.Text;
                SearchManager.GetInstance().AlarmLogsSearch.AlarmtEndTime = endTime.txtDate.Text;
                SearchManager.GetInstance().MediaLogsSerach.CollectStartTime = startTime.txtDate.Text;
                SearchManager.GetInstance().CollectLogsSerach.CollectStartTime = startTime.txtDate.Text;
                SearchManager.GetInstance().CameraLogsSerach.CollectStartTime = startTime.txtDate.Text;
                SearchManager.GetInstance().AlarmLogsSearch.AlarmStartTime = startTime.txtDate.Text;
                SearchManager.GetInstance().UpdateSearchData();
            }
        }
    }
}
