using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// FileButtonControl.xaml 的交互逻辑
    /// </summary>
    public partial class FileButtonControl : UserControl
    {
        
        public FileButtonControl()
        {
            InitializeComponent();
            
        }
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            
        }
        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            MediaInfo mi = this.DataContext as MediaInfo;
            mi.IsChecked = !mi.IsChecked;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(ModelResponsible.Instance.ParentWindow, PermissionConfig.DataSearchModuleEdit, "0");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                WindowsHelper.GetOrNewWindow<EditFileWindow>(true).UpdateMsg((MediaInfo)this.DataContext);
                WindowsHelper.ShowDialogWindow<EditFileWindow>(ModelResponsible.Instance.ParentWindow);
                LogHelper.Instance.WirteLog(string.Format("FileButtonControl: UserCode:{0} btnEdit", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MediaInfo dataContext = (MediaInfo)this.DataContext;

            bool isHas = ModelResponsible.Instance.AnalyzeHisPlayMediaList(dataContext);

            switch (dataContext.MediaType)
            {
                case "1":
                    ModelResponsible.Instance.AnalyzeVideoPlayMediaList(dataContext);
                    break;
                case "2":
                    ModelResponsible.Instance.AnalyzeVoicePlayMediaList(dataContext);
                    break;
                case "3":
                    ModelResponsible.Instance.AnalyzePicturePlayMediaList(dataContext);
                    break;
            }
            LogHelper.Instance.WirteLog(string.Format("FileButtonControl: UserCode:{0} btnAdd", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

            if (isHas)
                NewMessageBox.ShowTip(TryFindResource("FileButtonControlAddSucceed").ToString());
            else
                NewMessageBox.ShowTip(TryFindResource("FileButtonControlAddRepeat").ToString());
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (!(((MediaInfo)this.DataContext).UpLoadState == "1" || ((MediaInfo)this.DataContext).UpLoadState == "2"))
            {
                LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(ModelResponsible.Instance.ParentWindow, PermissionConfig.DataSearchModuleUpload, "0");
                if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
                {
                    SearchManager.GetInstance().UploadMediaInfo(((MediaInfo)this.DataContext).RecordID);
                    SearchManager.GetInstance().SendOperationLog("CollectEvidenceUpload", (MediaInfo)this.DataContext);

                    LogHelper.Instance.WirteLog(string.Format("FileButtonControl: UserCode:{0} btnUp", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
                }
            }
        }
    }
}
