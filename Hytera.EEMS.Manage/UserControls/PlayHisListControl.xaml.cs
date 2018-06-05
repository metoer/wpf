using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// PlayHisListControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayHisListControl : UserControl
    {
        
        public PlayHisListControl()
        {
            InitializeComponent();
            lvPlayList.ItemsSource = ManageViewModel.HisPlayMediaList;
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            MediaInfo mi = btn.DataContext as MediaInfo;
            if (mi != null)
            {
                string filepath = mi.FilePath + mi.FileName;
                if (!System.IO.File.Exists(filepath))
                {
                    NewMessageBox.Show(TryFindResource("SearchManagerPlayFileNoData").ToString());
                    return;
                }

                WindowsHelper.GetOrNewWindow<PlayWindow>(true).UpdateSelectTab(mi);
                WindowsHelper.ShowDialogWindow<PlayWindow>(ModelResponsible.Instance.ParentWindow);
                LogHelper.Instance.WirteLog(string.Format("PlayHisListControl: UserCode:{0} btnPlay  RecordID:{1}", AppConfigInfos.CurrentUserInfos.UserCode, mi.RecordID), LogLevel.LogDebug);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            MediaInfo mi = btn.DataContext as MediaInfo;
            if (mi != null)
            {
                ModelResponsible.Instance.RemoveHisPlayListByItem(mi);
                ManageViewModel.PicturePlayMediaList.Remove(mi);
                ManageViewModel.VideoPlayMediaList.Remove(mi);
                ManageViewModel.VoicePlayMediaList.Remove(mi);
                LogHelper.Instance.WirteLog(string.Format("PlayHisListControl: UserCode:{0} btnDel  RecordID:{1}", AppConfigInfos.CurrentUserInfos.UserCode, mi.RecordID), LogLevel.LogDebug);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ManageViewModel.PicturePlayMediaList.Clear();
            ManageViewModel.VideoPlayMediaList.Clear();
            ManageViewModel.VoicePlayMediaList.Clear();
            ModelResponsible.Instance.ClearHisPlayMediaList();
            LogHelper.Instance.WirteLog(string.Format("PlayHisListControl: UserCode:{0} btnClear", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
    }
}
