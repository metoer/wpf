using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
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
    /// LogsControl1.xaml 的交互逻辑
    /// </summary>
    public partial class CameraLogsControl : UserControl
    {
        public static readonly DependencyProperty CheckCameraProperty = DependencyProperty.Register("CameraCheck", typeof(bool), typeof(CameraLogsControl), new PropertyMetadata(false));
        public bool CameraCheck
        {
            get
            {
                return (bool)GetValue(CheckCameraProperty);
            }
            set
            {
                SetValue(CheckCameraProperty, value);
            }
        }
        
        public static readonly DependencyProperty CameraShowMoreTextProperty = DependencyProperty.Register("CameraShowMoreText", typeof(string), typeof(CameraLogsControl));
        public string CameraShowMoreText
        {
            get
            {
                return (string)GetValue(CameraShowMoreTextProperty);
            }
            set
            {
                SetValue(CameraShowMoreTextProperty, value);
            }
        }
        
        private int CameraCount;
        
        public CameraLogsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            dataGrid.ItemsSource = ManageViewModel.CameraLogs;
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OpTime="2017-01-15  23:12:56", OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2" });
            //ManageViewModel.CameraLogs.Add(new CameraLogInfo() { OperatedName = "dsa", SequenceNum = 2, UpLoadState = "2", Vis = Visibility.Visible });
        }
        
        public void UpdateDetailCount()
        {
            CameraShowMoreText = string.Format("{0}/{1},", ManageViewModel.CameraLogs.Count, CameraCount);
            UpdateBtnVisible();
            UpdateCameraCheckAll();
            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} UpdateDetailCount", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        internal void UpdateCount( string count)
        {
            Int32.TryParse(count, out CameraCount);
            UpdateDetailCount();
            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} UpdateCount", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }

        private void UpdateBtnVisible()
        {
            ManageViewModel.CameraLogs.ToList().ForEach(p => p.Vis = Visibility.Hidden);

            if (ManageViewModel.CameraLogs.Count < CameraCount && CameraCount > AppConfigInfos.AppStateInfos.SearchPageCount && ManageViewModel.CameraLogs.Count > 0)
                ManageViewModel.CameraLogs[ManageViewModel.CameraLogs.Count - 1].Vis = Visibility.Visible;

            if (CameraCount < 1)
                spNodata.Visibility = Visibility.Visible;
            else
                spNodata.Visibility = Visibility.Collapsed;

            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} UpdateBtnVisible", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        
        private void btnCameraView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchManager.GetInstance().CameraLogsSerach.PageIndex++;
            SearchManager.GetInstance().SearchCameraLogDetail(SearchManager.GetInstance().CameraLogsSerach);
            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} btnCameraView ShowMore", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        
        private void btnCameraUp_Click(object sender, RoutedEventArgs e)
        {
            string logIds = string.Empty;
            foreach (CameraLogInfo mi in ManageViewModel.CameraLogs)
            {
                if (mi.IsChecked && mi.UpLoadState != "2")
                    logIds += mi.LogID + ",";
            }
            if (string.IsNullOrEmpty(logIds))
            {
                NewMessageBox.Show(TryFindResource("LogsControlBtnCameraUploadEmpty").ToString());
                return;
            }
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(ModelResponsible.Instance.ParentWindow, PermissionConfig.DataSearchModuleUpload, "0");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                SearchManager.GetInstance().UploadCameraLog(logIds);
                SearchManager.GetInstance().SendOperationLog("CollectCameraLogBulkUpload");
                LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} btnCameraUp Upload logIds:{1}", AppConfigInfos.CurrentUserInfos.UserCode, logIds), LogLevel.LogDebug);
            }
        }
        
        private void cameraChk_Click(object sender, RoutedEventArgs e)
        {
            CameraCheck = !CameraCheck;
            foreach (CameraLogInfo mi in ManageViewModel.CameraLogs)
            {
                    mi.IsChecked = CameraCheck;
            }
        }
        
        private void chkCamera_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            CameraLogInfo mi = btn.DataContext as CameraLogInfo;
            mi.IsChecked = !mi.IsChecked;
            UpdateCameraCheckAll();
        }

        private void lvzfyData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCameraCheckAll();
        }

        private void UpdateCameraCheckAll()
        {
            bool check = true;
            foreach (CameraLogInfo mi in ManageViewModel.CameraLogs)
            {
                if (!mi.IsChecked)
                    check = false;
            }
            if (ManageViewModel.CameraLogs.Count < 1)
                check = false;
            CameraCheck = check;
        }
        
    }
}
