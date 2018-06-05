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
    /// MediaSearchControl.xaml 的交互逻辑
    /// </summary>
    public partial class MediaSearchControl : UserControl
    {
        public event Action<bool> CloseEvent = null;
        public MediaSearchControl()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName= TryFindResource("ManageUploadStateAll").ToString(), ItemCode="",ItemID="All"});
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateNoUpload").ToString(), ItemCode = "0", ItemID = "0" });
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateUploading").ToString(), ItemCode = "1", ItemID = "1" });
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateUploaded").ToString(), ItemCode = "2", ItemID = "2" });
            cmbUpLoadState.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("ManageUploadStateUploadFail").ToString(), ItemCode = "3", ItemID = "3" });
            cmbUpLoadState.Text = TryFindResource("ManageUploadStateAll").ToString();
            cmbUpLoadState.SelectValue = "";
            if (SearchManager.GetInstance().MediaLogsSerach.UploadState != null)
            {
                switch (SearchManager.GetInstance().MediaLogsSerach.UploadState)
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

            cmbFileImp.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("MediaSearchControlFileAll").ToString(), ItemCode = "0",ItemID="0" });
            cmbFileImp.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("MediaSearchControlFileImp").ToString(), ItemCode = "1", ItemID = "1" });
            cmbFileImp.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("MediaSearchControlFileOrdinary").ToString(), ItemCode = "2", ItemID = "2" });
            cmbFileImp.Text = TryFindResource("MediaSearchControlFileAll").ToString();
            cmbFileImp.SelectValue = "0";
            if (SearchManager.GetInstance().MediaLogsSerach.UserImp != null)
            {
                switch (SearchManager.GetInstance().MediaLogsSerach.UserImp)
                {
                    case "0":
                        cmbFileImp.Text = TryFindResource("MediaSearchControlFileAll").ToString();
                        cmbFileImp.SelectValue = "0";
                        break;
                    case "1":
                        cmbFileImp.Text = TryFindResource("MediaSearchControlFileImp").ToString();
                        cmbFileImp.SelectValue = "1";
                        break;
                    case "2":
                        cmbFileImp.Text = TryFindResource("MediaSearchControlFileOrdinary").ToString();
                        cmbFileImp.SelectValue = "2";
                        break;
                }
            }
            if(AppConfigInfos.LimitsUserInfos.Users !=null && AppConfigInfos.LimitsUserInfos.Users.UserList.Count>0 && AppConfigInfos.LimitsUserInfos.UserType == "1")
            {
                cmbUserList.TextEnabled = true;
                cmbUserList.CodeVisibility = true;
                foreach (UserInfos ui in AppConfigInfos.LimitsUserInfos.Users.UserList)
                {
                    if (!ui.UserID.ToLower().Equals("admin"))
                    {
                        cmbUserList.Items.Add(new Enums.ComBoxItem() { ItemName = ui.UserName, ItemCode = ui.UserGuid, ItemID = ui.UserCode });
                        if (SearchManager.GetInstance().MediaLogsSerach.UserGuid == ui.UserGuid)
                        {
                            cmbUserList.Text = ui.UserName;
                            cmbUserList.SelectValue = ui.UserGuid;
                        }
                    }
                }
            }
            else
            {
                if (!AppConfigInfos.LimitsUserInfos.UserID.Equals("admin"))
                {
                    cmbUserList.Items.Add(new Enums.ComBoxItem() { ItemName = AppConfigInfos.LimitsUserInfos.UserName, ItemCode = AppConfigInfos.LimitsUserInfos.UserGuid, ItemID = AppConfigInfos.LimitsUserInfos.UserID });
                    cmbUserList.Text = AppConfigInfos.LimitsUserInfos.UserName;
                    cmbUserList.SelectValue = AppConfigInfos.LimitsUserInfos.UserGuid;
                }
            }

            if (!string.IsNullOrEmpty(SearchManager.GetInstance().MediaLogsSerach.OrgName))
            {
                cmbOrgList.Text = SearchManager.GetInstance().MediaLogsSerach.OrgName;
                cmbOrgList.SelectValue = SearchManager.GetInstance().MediaLogsSerach.OrgID;
            }

            tbDeviceID.Text = SearchManager.GetInstance().MediaLogsSerach.DeviceID;
            tbUserTag.Text = SearchManager.GetInstance().MediaLogsSerach.UserTag;
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
            ModelResponsible.Instance.ClearMediaList();

            SearchManager.GetInstance().MediaLogsSerach.UserImp = cmbFileImp.SelectValue;

            SearchManager.GetInstance().MediaLogsSerach.UserGuid = cmbUserList.SelectValue;
            SearchManager.GetInstance().MediaLogsSerach.OrgID = cmbOrgList.SelectValue;
            SearchManager.GetInstance().MediaLogsSerach.UploadState = cmbUpLoadState.SelectValue;
            SearchManager.GetInstance().MediaLogsSerach.OrgName = cmbOrgList.Text;
            SearchManager.GetInstance().MediaLogsSerach.DeviceID= tbDeviceID.Text;
            SearchManager.GetInstance().MediaLogsSerach.UserTag= tbUserTag.Text;

            SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
            SearchManager.GetInstance().MediaLogsSerach.IsAdvanced = true;
            SearchManager.GetInstance().MediaLogsSerach.SearchTime = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            SearchManager.GetInstance().SearchMediaLogCount(SearchManager.GetInstance().MediaLogsSerach);
            SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);
            SearchManager.GetInstance().SendOperationLog("CollectHighSearch");
            if (CloseEvent != null)
                CloseEvent(true);
            LogHelper.Instance.WirteLog(string.Format("MediaSearchControl: UserCode:{0} btnSure MediaLogsSerach", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            if (CloseEvent != null)
                CloseEvent(true);
        }
    }
}
