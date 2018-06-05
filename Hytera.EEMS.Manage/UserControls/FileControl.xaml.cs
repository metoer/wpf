using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Model;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// FileControl.xaml 的交互逻辑
    /// </summary>
    public partial class FileControl : UserControl
    {
        public FileControl()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            MediaInfo mi = this.DataContext as MediaInfo;
            mi.IsChecked = !mi.IsChecked;
        }

        private void videopic_Click(object sender, RoutedEventArgs e)
        {
            LogHelper.Instance.WirteLog(string.Format("FileControl: UserCode:{0} btnPlay", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            
            if ((MediaInfo)this.DataContext != null)
            {
                string filepath = ((MediaInfo)this.DataContext).FilePath + ((MediaInfo)this.DataContext).FileName;
                if (!System.IO.File.Exists(filepath))
                {
                    EEMS.Resources.Windows.NewMessageBox.Show(TryFindResource("SearchManagerPlayFileNoData").ToString());
                    return;
                }

                WindowsHelper.GetOrNewWindow<PlayWindow>(true).UpdateSelectTab((MediaInfo)this.DataContext);

                LogHelper.Instance.WirteLog(string.Format("FileControl: UserCode:{0} btnPlay  AddHisPlayMediaList", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

                ModelResponsible.Instance.AnalyzeHisPlayMediaList((MediaInfo)this.DataContext);

                LogHelper.Instance.WirteLog(string.Format("FileControl: UserCode:{0} btnPlay  Playing", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

                SearchManager.GetInstance().SendOperationLog("CollectEvidencePlay", (MediaInfo)this.DataContext);
                WindowsHelper.ShowDialogWindow<PlayWindow>(ModelResponsible.Instance.ParentWindow);
            }
            else
            {
                LogHelper.Instance.WirteLog(string.Format("FileControl: UserCode:{0} btnPlay  NoMediaInfo", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            }
        }
    }
}
