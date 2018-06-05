using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Enums;
using Hytera.EEMS.Manage.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// MediaControl.xaml 的交互逻辑
    /// </summary>
    public partial class MediaControl : UserControl
    {
        public static readonly DependencyProperty BtnPicProperty = DependencyProperty.Register("PicVisible", typeof(Visibility), typeof(MediaControl), new PropertyMetadata(Visibility.Collapsed));
        public Visibility PicVisible
        {
            get
            {
                return (Visibility)GetValue(BtnPicProperty);
            }
            set
            {
                SetValue(BtnPicProperty, value);
            }
        }
        
        public static readonly DependencyProperty CheckProperty = DependencyProperty.Register("CheckAll", typeof(bool), typeof(MediaControl), new PropertyMetadata(false));
        public bool CheckAll
        {
            get
            {
                return (bool)GetValue(CheckProperty);
            }
            set
            {
                SetValue(CheckProperty, value);
            }
        }

        public static readonly DependencyProperty ShowMoreTextProperty = DependencyProperty.Register("ShowMoreText", typeof(string), typeof(MediaControl));
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

        public FileType SelectFileType = FileType.All;

        private int AllCount;
        private int PicCount;
        private int VideoCount;
        private int VoiceCount;
        public MediaControl()
        {
            InitializeComponent();
        }
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            dataGrid.ItemsSource = ManageViewModel.MediaList;
            lvPicData.ItemsSource = ManageViewModel.MediaList;
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 1, UpLoadState = "1", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 3, UpLoadState = "3", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 4, UpLoadState = "0", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 5, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 6, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 7, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf" });
            //ManageViewModel.MediaList.Add(new MediaInfo() { SequenceNum = 2, UpLoadState = "2", OrgName = "dsaf", UserCode = "fsdaf", Vis = Visibility.Visible });

        }

        private void btnGrid_Click(object sender, RoutedEventArgs e)
        {
            btnPic.IsSelect = false;
            btnGrid.IsSelect = true;
            dataGrid.Visibility =  Visibility.Visible;
            chkAll.Visibility = lvPicData.Visibility = Visibility.Collapsed;
            Grid.SetColumn(btnUp, 0);
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} GridorCardChange ShowGrid", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }

        private void btnPic_Click(object sender, RoutedEventArgs e)
        {
            btnPic.IsSelect = true;
            btnGrid.IsSelect = false;
            dataGrid.Visibility  = Visibility.Collapsed;
            chkAll.Visibility = lvPicData.Visibility = Visibility.Visible;
            Grid.SetColumn(btnUp, 1);
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} GridorCardChange ShowCard", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }

        public void UpdateCount(FileType ft, string datacount)
        {
            switch(ft)
            {
                case FileType.All:
                    Int32.TryParse(datacount, out AllCount);
                    btnAll.ShowText = string.Format("({0})", AllCount);
                    btnAll.IsCharacterEllipsis = AllCount < 10000 ? false : true;
                    LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} UpdateAllTotal Total：{1}", AppConfigInfos.CurrentUserInfos.UserCode, datacount), LogLevel.LogDebug);
                    break;
                case FileType.Picture:
                    Int32.TryParse(datacount, out PicCount);
                    btnPicC.ShowText = string.Format("({0})", PicCount);
                    btnPicC.IsCharacterEllipsis = PicCount < 10000 ? false : true;
                    LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} UpdatePicTotal Total：{1}", AppConfigInfos.CurrentUserInfos.UserCode, datacount), LogLevel.LogDebug);
                    break;
                case FileType.Video:
                    Int32.TryParse(datacount, out VideoCount);
                    btnVideoC.ShowText = string.Format("({0})", VideoCount);
                    btnVideoC.IsCharacterEllipsis = VideoCount < 10000 ? false : true;
                    LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} UpdateVideoTotal Total：{1}", AppConfigInfos.CurrentUserInfos.UserCode, datacount), LogLevel.LogDebug);
                    break;
                case FileType.Voice:
                    Int32.TryParse(datacount, out VoiceCount);
                    btnVoiceC.ShowText = string.Format("({0})", VoiceCount);
                    btnVoiceC.IsCharacterEllipsis = VoiceCount < 10000 ? false : true;
                    LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} UpdateVoiceTotal Total：{1}", AppConfigInfos.CurrentUserInfos.UserCode, datacount), LogLevel.LogDebug);
                    break;
            }

            UpdateDetailCount();
        }

        public void UpdateDetailCount()
        {
            int Count = 0;

            switch (SelectFileType)
            {
                case FileType.All:
                    Count = AllCount;
                    break;
                case FileType.Picture:
                    Count = PicCount;
                    break;
                case FileType.Video:
                    Count = VideoCount;
                    break;
                case FileType.Voice:
                    Count = VoiceCount;
                    break;
            }

            ShowMoreText = string.Format("{0}/{1},", ManageViewModel.MediaList.Count, Count);
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} UpdateDetailCount DetailCount：{1}", AppConfigInfos.CurrentUserInfos.UserCode, Count.ToString()), LogLevel.LogDebug);

            UpdateBtnVisible();
            UpdateCheckAll();
        }

        private void UpdateBtnVisible()
        {
            int Count=0;

            switch (SelectFileType)
            {
                case FileType.All:
                    Count = AllCount;
                    break;
                case FileType.Picture:
                    Count = PicCount;
                    break;
                case FileType.Video:
                    Count = VideoCount;
                    break;
                case FileType.Voice:
                    Count = VoiceCount;
                    break;
            }
            ManageViewModel.MediaList.ToList().ForEach(p => p.Vis = Visibility.Hidden);

            if (ManageViewModel.MediaList.Count < Count && Count > AppConfigInfos.AppStateInfos.SearchPageCount && ManageViewModel.MediaList.Count > 0)
                PicVisible = ManageViewModel.MediaList[ManageViewModel.MediaList.Count - 1].Vis = Visibility.Visible;
            else
                PicVisible = Visibility.Collapsed;

            if (Count < 1)
                spNodata.Visibility = Visibility.Visible;
            else
                spNodata.Visibility = Visibility.Collapsed;

            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0} UpdateBtnVisible Visible：{1}", AppConfigInfos.CurrentUserInfos.UserCode, PicVisible.ToString()), LogLevel.LogDebug);

        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            string RecordIds = string.Empty;
            foreach (MediaInfo mi in ManageViewModel.MediaList)
            {
                if (mi.IsChecked && mi.UpLoadState != "2")
                    RecordIds += mi.RecordID + ",";
            }
            if (string.IsNullOrEmpty(RecordIds))
            {
                NewMessageBox.Show(TryFindResource("MediaControlUploadEmpty").ToString());
                return;
            }
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(ModelResponsible.Instance.ParentWindow, PermissionConfig.DataSearchModuleUpload, "0");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                SearchManager.GetInstance().UploadMediaInfo(RecordIds);
                LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  UploadMediaInfo RecordIds:{1}", AppConfigInfos.CurrentUserInfos.UserCode, RecordIds), LogLevel.LogDebug);
                SearchManager.GetInstance().SendOperationLog("CollectEvidenceBulkUpload");
            }
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            ModelResponsible.Instance.ClearMediaList();
            SelectFileType = FileType.All;
            UpdateBtnSelect();
            SearchManager.GetInstance().MediaLogsSerach.FileType = Enums.FileType.All.GetHashCode().ToString();
            SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
            SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  SearchAllBtn", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            SearchManager.GetInstance().SendOperationLog("CollectEvidenceSearch");
        }

        private void btnVideoC_Click(object sender, RoutedEventArgs e)
        {
            ModelResponsible.Instance.ClearMediaList();
            SelectFileType = FileType.Video;
            UpdateBtnSelect();
            SearchManager.GetInstance().MediaLogsSerach.FileType = Enums.FileType.Video.GetHashCode().ToString();
            SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
            SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  SearchVideoBtn", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            SearchManager.GetInstance().SendOperationLog("CollectEvidenceSearch");
        }

        private void btnVoiceC_Click(object sender, RoutedEventArgs e)
        {
            ModelResponsible.Instance.ClearMediaList();
            SelectFileType = FileType.Voice;
            UpdateBtnSelect();
            SearchManager.GetInstance().MediaLogsSerach.FileType = Enums.FileType.Voice.GetHashCode().ToString();
            SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
            SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  SearchVoiceBtn", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            SearchManager.GetInstance().SendOperationLog("CollectEvidenceSearch");
        }

        private void btnPicC_Click(object sender, RoutedEventArgs e)
        {
            ModelResponsible.Instance.ClearMediaList();
            SelectFileType = FileType.Picture;
            UpdateBtnSelect();
            SearchManager.GetInstance().MediaLogsSerach.FileType = Enums.FileType.Picture.GetHashCode().ToString();
            SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
            SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);

            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  SearchPicBtn", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            SearchManager.GetInstance().SendOperationLog("CollectEvidenceSearch");
        }

        public void UpdateBtnSelect()
        {
            btnAll.IsSelect = false;
            btnVideoC.IsSelect = false;
            btnVoiceC.IsSelect = false;
            btnPicC.IsSelect = false;
            switch (SelectFileType)
            {
                case FileType.All:
                    btnAll.IsSelect = true;
                    break;
                case FileType.Video:
                    btnVideoC.IsSelect = true;
                    break;
                case FileType.Voice:
                    btnVoiceC.IsSelect = true;
                    break;
                case FileType.Picture:
                    btnPicC.IsSelect = true;
                    break;
            }
            UpdateDetailCount();
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  UpdateBtnSelect SelectFileType:{1}", AppConfigInfos.CurrentUserInfos.UserCode,SelectFileType.ToString()), LogLevel.LogDebug);
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckAll =!CheckAll;
            foreach (MediaInfo mi in ManageViewModel.MediaList)
            {
                mi.IsChecked = CheckAll;
            }

            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  CheckAllBtn", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }

        private void btnPicView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchManager.GetInstance().MediaLogsSerach.PageIndex++;
            SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  ShowMoreBtn", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            MediaInfo mi = btn.DataContext as MediaInfo;
            mi.IsChecked = !mi.IsChecked;
            UpdateCheckAll();
            LogHelper.Instance.WirteLog(string.Format("MediaControl: UserCode:{0}  CheckMediaInfo RecordID:{1}", AppConfigInfos.CurrentUserInfos.UserCode,mi.RecordID), LogLevel.LogDebug);
        }

        private void lvPicData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCheckAll();
        }

        private void UpdateCheckAll()
        {
            bool check = true;
            foreach (MediaInfo mi in ManageViewModel.MediaList)
            {
                if (!mi.IsChecked)
                    check = false;
            }
            if(ManageViewModel.MediaList.Count < 1)
                check = false;
            CheckAll = check;
        }
        
    }
}
