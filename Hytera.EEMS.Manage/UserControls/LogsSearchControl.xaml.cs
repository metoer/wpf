using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// LogsSearchControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogsSearchControl : UserControl
    {
        public event Action<bool> CloseEvent = null;
        public LogsSearchControl()
        {
            InitializeComponent();
        }

        public void InitData()
        {
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateAll").ToString(), ItemCode = "", ItemID = "All" });
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateNoUpload").ToString(), ItemCode = "0", ItemID = "0" });
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateUploading").ToString(), ItemCode = "1", ItemID = "1" });
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateUploaded").ToString(), ItemCode = "2", ItemID = "2" });
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateUploadFail").ToString(), ItemCode = "3", ItemID = "3" });
            cmbUpLoadState.Text = TryFindResource("ManageUploadStateAll").ToString();
            cmbUpLoadState.SelectValue = "";
            if (this.DataContext is CollectLogsSerach && SearchManager.GetInstance().CollectLogsSerach.UploadState != null)
            {
                switch (SearchManager.GetInstance().CollectLogsSerach.UploadState)
                {
                    case "0":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateNoUpload").ToString();
                        cmbUpLoadState.SelectValue = "0";
                        break;
                    case "1":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateUploading").ToString();
                        cmbUpLoadState.SelectValue = "1";
                        break;
                    case "2":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateUploaded").ToString();
                        cmbUpLoadState.SelectValue = "2";
                        break;
                    case "3":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateUploadFail").ToString();
                        cmbUpLoadState.SelectValue = "3";
                        break;
                }
            }

            if (this.DataContext is CameraLogsSerach && SearchManager.GetInstance().CameraLogsSerach.UploadState != null)
            {
                switch (SearchManager.GetInstance().CameraLogsSerach.UploadState)
                {
                    case "0":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateNoUpload").ToString();
                        cmbUpLoadState.SelectValue = "0";
                        break;
                    case "1":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateUploading").ToString();
                        cmbUpLoadState.SelectValue = "1";
                        break;
                    case "2":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateUploaded").ToString();
                        cmbUpLoadState.SelectValue = "2";
                        break;
                    case "3":
                        cmbUpLoadState.Text = TryFindResource("ManageUploadStateUploadFail").ToString();
                        cmbUpLoadState.SelectValue = "3";
                        break;
                }
            }

            if (AppConfigInfos.LimitsUserInfos.Users != null && AppConfigInfos.LimitsUserInfos.Users.UserList.Count > 0 && AppConfigInfos.LimitsUserInfos.UserType == "1")
            {
                cmbUserList.TextEnabled = true;
                cmbUserList.CodeVisibility = true;
                foreach (UserInfos ui in AppConfigInfos.LimitsUserInfos.Users.UserList)
                {
                    if (!(ui.UserID.ToLower().Equals("admin") && this.DataContext is CameraLogsSerach))
                    {
                        cmbUserList.Items.Add(new Enums.ComBoxItem() { ItemName = ui.UserName, ItemCode = ui.UserGuid, ItemID = ui.UserCode });
                    }

                    if (this.DataContext is CollectLogsSerach && SearchManager.GetInstance().CollectLogsSerach.UserGuid == ui.UserGuid)
                    {
                        cmbUserList.Text = ui.UserName;
                        cmbUserList.SelectValue = ui.UserGuid;
                    }

                    if (this.DataContext is CameraLogsSerach && SearchManager.GetInstance().CameraLogsSerach.UserGuid == ui.UserGuid)
                        cmbUserList.Text = ui.UserName;
                }
            }
            else
            {
                if (!(AppConfigInfos.LimitsUserInfos.UserID.ToLower().Equals("admin") && this.DataContext is CameraLogsSerach))
                {
                    cmbUserList.Items.Add(new Enums.ComBoxItem() { ItemName = AppConfigInfos.LimitsUserInfos.UserName, ItemCode = AppConfigInfos.LimitsUserInfos.UserGuid, ItemID = AppConfigInfos.LimitsUserInfos.UserID });
                    cmbUserList.Text = AppConfigInfos.LimitsUserInfos.UserName;
                    cmbUserList.SelectValue = AppConfigInfos.LimitsUserInfos.UserGuid;
                }
            }

            if (this.DataContext is CollectLogsSerach && !string.IsNullOrEmpty(SearchManager.GetInstance().CollectLogsSerach.OrgName))
            {
                cmbOrgList.Text = SearchManager.GetInstance().CollectLogsSerach.OrgName;
                cmbOrgList.SelectValue = SearchManager.GetInstance().CollectLogsSerach.OrgID;
            }

            if (this.DataContext is CameraLogsSerach && !string.IsNullOrEmpty(SearchManager.GetInstance().CameraLogsSerach.OrgName))
            {
                cmbOrgList.Text = SearchManager.GetInstance().CameraLogsSerach.OrgName;
                cmbOrgList.SelectValue = SearchManager.GetInstance().CameraLogsSerach.OrgID;
            }

            if (this.DataContext is CollectLogsSerach)
                tbDeviceID.Text = SearchManager.GetInstance().CollectLogsSerach.DeviceID;

            if (this.DataContext is CameraLogsSerach)
                tbDeviceID.Text = SearchManager.GetInstance().CameraLogsSerach.DeviceID;
        }
        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbOrgList.Text))
            {
                OrgInfos sub = AppConfigInfos.LimitsUserInfos.OrgList.orgList.Find(oi => oi.OrgIDCode.ToLower().Contains(cmbOrgList.Text.ToLower()) || oi.OrgName.ToLower().Contains(cmbOrgList.Text.ToLower()));
                if (sub == null)
                {
                    tbMsg.Text = TryFindResource("MediaSearchControlError").ToString();
                    return;
                }
            }

            if (this.DataContext is CollectLogsSerach)
            {
                ModelResponsible.Instance.ClearCollectLogs();
                SearchManager.GetInstance().CollectLogsSerach.UserGuid = cmbUserList.SelectValue;
                SearchManager.GetInstance().CollectLogsSerach.OrgID = cmbOrgList.SelectValue;
                SearchManager.GetInstance().CollectLogsSerach.UploadState = cmbUpLoadState.SelectValue;
                SearchManager.GetInstance().CollectLogsSerach.OrgName = cmbOrgList.Text;
                SearchManager.GetInstance().CollectLogsSerach.PageIndex = 1;
                SearchManager.GetInstance().CollectLogsSerach.LogType = "1";
                SearchManager.GetInstance().CollectLogsSerach.IsAdvanced = true;
                SearchManager.GetInstance().SearchCollectLogCount(SearchManager.GetInstance().CollectLogsSerach);
                SearchManager.GetInstance().SearchCollectLogDetail(SearchManager.GetInstance().CollectLogsSerach);
                LogHelper.Instance.WirteLog(string.Format("LogsSearchControl: UserCode:{0} btnSure CollectLogsSerach", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

                SearchManager.GetInstance().SendOperationLog("CollectLogHighSearch");
            }
            else if (this.DataContext is CameraLogsSerach)
            {
                ModelResponsible.Instance.ClearCameraLogs();
                SearchManager.GetInstance().CameraLogsSerach.PageIndex = 1;
                SearchManager.GetInstance().CameraLogsSerach.UserGuid = cmbUserList.SelectValue;
                SearchManager.GetInstance().CameraLogsSerach.OrgID = cmbOrgList.SelectValue;
                SearchManager.GetInstance().CameraLogsSerach.UploadState = cmbUpLoadState.SelectValue;
                SearchManager.GetInstance().CameraLogsSerach.OrgName = cmbOrgList.Text;

                SearchManager.GetInstance().CameraLogsSerach.LogType = "2";
                SearchManager.GetInstance().CameraLogsSerach.IsAdvanced = true;

                SearchManager.GetInstance().SearchCameraLogCount(SearchManager.GetInstance().CameraLogsSerach);
                SearchManager.GetInstance().SearchCameraLogDetail(SearchManager.GetInstance().CameraLogsSerach);
                LogHelper.Instance.WirteLog(string.Format("LogsSearchControl: UserCode:{0} btnSure CameraLogsSerach", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

                SearchManager.GetInstance().SendOperationLog("CollectCameraLogHighSearch");
            }

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
