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
    public partial class CollectLogsControl : UserControl
    {
        public static readonly DependencyProperty CheckCollectProperty = DependencyProperty.Register("CollectCheck", typeof(bool), typeof(CollectLogsControl), new PropertyMetadata(false));
        public bool CollectCheck
        {
            get
            {
                return (bool)GetValue(CheckCollectProperty);
            }
            set
            {
                SetValue(CheckCollectProperty, value);
            }
        }
        

        public static readonly DependencyProperty CollectShowMoreTextProperty = DependencyProperty.Register("CollectShowMoreText", typeof(string), typeof(CollectLogsControl));
        public string CollectShowMoreText
        {
            get
            {
                return (string)GetValue(CollectShowMoreTextProperty);
            }
            set
            {
                SetValue(CollectShowMoreTextProperty, value);
            }
        }

        private int CollectCount;
        
        public CollectLogsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            dataGrid.ItemsSource = ManageViewModel.CollectLogs;
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { OpTime = "2017-01-15  23:12:56", SequenceNum =2,UpLoadState ="2",OperatedName="dsa"});
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa" });
            //ManageViewModel.CollectLogs.Add(new CollectLogInfo() { SequenceNum = 2, UpLoadState = "2", OperatedName = "dsa",Vis=Visibility.Visible });
        }

        public void UpdateDetailCount( )
        {
            CollectShowMoreText = string.Format("{0}/{1},", ManageViewModel.CollectLogs.Count, CollectCount);
            UpdateBtnVisible();
            UpdateCollectCheckAll();
            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} UpdateDetailCount", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        internal void UpdateCount(string count)
        {
            Int32.TryParse(count, out CollectCount);
            UpdateDetailCount();
            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} UpdateCount", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }

        private void UpdateBtnVisible()
        {

            ManageViewModel.CollectLogs.ToList().ForEach(p => p.Vis = Visibility.Hidden);

            if (ManageViewModel.CollectLogs.Count < CollectCount && CollectCount > AppConfigInfos.AppStateInfos.SearchPageCount && ManageViewModel.CollectLogs.Count > 0)
                ManageViewModel.CollectLogs[ManageViewModel.CollectLogs.Count - 1].Vis = Visibility.Visible;

            if (CollectCount < 1)
                spCollectNodata.Visibility = Visibility.Visible;
            else
                spCollectNodata.Visibility = Visibility.Collapsed;
            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} UpdateBtnVisible", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        
        private void btnCollectView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchManager.GetInstance().CollectLogsSerach.PageIndex++;
            SearchManager.GetInstance().SearchCollectLogDetail(SearchManager.GetInstance().CollectLogsSerach);
            LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} btnCollectView ShowMore", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        
        private void btnCollectUp_Click(object sender, RoutedEventArgs e)
        {
            string logIds = string.Empty;
            foreach (CollectLogInfo mi in ManageViewModel.CollectLogs)
            {
                if (mi.IsChecked && mi.UpLoadState!="2")
                    logIds += mi.LogID + ",";
            }
            if (string.IsNullOrEmpty(logIds))
            {
                NewMessageBox.Show(TryFindResource("LogsControlBtnCollectUploadEmpty").ToString());
                return;
            }

            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(ModelResponsible.Instance.ParentWindow, PermissionConfig.DataSearchModuleUpload, "0");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                SearchManager.GetInstance().UploadCollectLog(logIds);
                SearchManager.GetInstance().SendOperationLog("CollectLogBulkUpload");
                LogHelper.Instance.WirteLog(string.Format("LogsControl: UserCode:{0} btnCollectUp Upload logIds:{1}", AppConfigInfos.CurrentUserInfos.UserCode, logIds), LogLevel.LogDebug);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CollectCheck = !CollectCheck;
            foreach (CollectLogInfo mi in ManageViewModel.CollectLogs)
            {
                    mi.IsChecked = CollectCheck;
            }
        }
        
        private void chkCollect_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            CollectLogInfo mi = btn.DataContext as CollectLogInfo;
                mi.IsChecked = !mi.IsChecked;
            UpdateCollectCheckAll();
        }
        
        private void UpdateCollectCheckAll()
        {
            bool check = true;
            foreach (CollectLogInfo mi in ManageViewModel.CollectLogs)
            {
                if (!mi.IsChecked)
                    check = false;
            }
            if (ManageViewModel.CollectLogs.Count < 1)
                check = false;
            CollectCheck = check;
        }
        
    }
}
