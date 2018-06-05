using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Lib;
using Hytera.EEMS.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// PlayPicControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayPicControl : UserControl
    {
        public event Action<bool> FullScreenEvent = null;

        public MediaInfo SelectMediaInfo;
        public PlayPicControl()
        {
            InitializeComponent();
            bic.IsManipulationDelete = false;
            bic.IsManipulationDoubleClick = false;
            bic.IsAutoSelect = true;
            lbPic.ItemsSource = ManageViewModel.PicturePlayMediaList;

            if (ManageViewModel.PicturePlayMediaList.Count < 1)
                spNodata.Visibility = Visibility.Visible;
        }
        public void Play()
        {
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                Play(outfilepath);
                lbPic.SelectedItem = SelectMediaInfo;

                spNodata.Visibility = Visibility.Collapsed;
                bic.ShowOriginal();
            }
        }

        public void Stop()
        {
            bic.RemoveAll();
        }
        #region 图片播放
        
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (lbPic.Items.Count < 2)
                return;

            if (lbPic.SelectedIndex < 1)
                lbPic.SelectedIndex = lbPic.Items.Count - 1;
            else
                lbPic.SelectedIndex--;

            SelectMediaInfo = lbPic.SelectedItem as MediaInfo;

            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                Play(outfilepath);
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                Play(outfilepath);
            }
        }
        /// <summary>
        /// 下一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (lbPic.Items.Count < 2)
                return;

            if (lbPic.SelectedIndex == lbPic.Items.Count - 1)
                lbPic.SelectedIndex = 0;
            else
                lbPic.SelectedIndex++;

            SelectMediaInfo = lbPic.SelectedItem as MediaInfo;

            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;

                Play(outfilepath);
            }
        }

        private void Play(string filePath)
        {
            bic.RemoveAll();
            bic.Add(filePath);
        }
        private void btnBig_Click(object sender, RoutedEventArgs e)
        {
            bic.ShowMoreBig();
        }

        private void btnSmall_Click(object sender, RoutedEventArgs e)
        {
            bic.ShowMoreSmall();
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            bic.ShowCW();
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            bic.ShowCWW();
        }

        private void btnFit_Click(object sender, RoutedEventArgs e)
        {
            bic.ShowOriginal();
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (lbPic.Visibility == Visibility.Visible)
            {
                if (FullScreenEvent != null)
                    FullScreenEvent(true);

                bic.ShowFitFull(true);
            }
            else
            {
                if (FullScreenEvent != null)
                    FullScreenEvent(false);

                bic.ShowFitFull(false);
            }
            
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            MediaInfo mi = btn.DataContext as MediaInfo;

            if (mi == SelectMediaInfo)
            {
                bic.RemoveAll();
                if(lbPic.Items.Count>1)
                {
                    if (lbPic.SelectedIndex == lbPic.Items.Count - 1)
                        lbPic.SelectedIndex = 0;
                    else
                        lbPic.SelectedIndex++;

                    SelectMediaInfo = lbPic.SelectedItem as MediaInfo;
                }
            }

            if (mi != null)
            {
                ModelResponsible.Instance.RemoveHisPlayListByItem(mi);
                ManageViewModel.PicturePlayMediaList.Remove(mi);
            }

            if (ManageViewModel.PicturePlayMediaList.Count < 1)
            {
                SelectMediaInfo = null;
                spNodata.Visibility = Visibility.Visible;
            }
        }
        #endregion

        private void btnDel_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectMediaInfo != null)
            {
                ModelResponsible.Instance.RemoveHisPlayListByItem(SelectMediaInfo);
                ManageViewModel.PicturePlayMediaList.Remove(SelectMediaInfo);
            }
            if (ManageViewModel.PicturePlayMediaList.Count < 1)
            {
                SelectMediaInfo = null;
                bic.RemoveAll();
                spNodata.Visibility = Visibility.Visible;
            }
        }

        private void lbPic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectMediaInfo = lbPic.SelectedItem as MediaInfo;
        }

        private void lbPic_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbPic.Items.Count < 1)
                return;

            SelectMediaInfo = lbPic.SelectedItem as MediaInfo;
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                Play(outfilepath);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Play("E:\\12.bmp");
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                Play(outfilepath);
                lbPic.SelectedItem = SelectMediaInfo;
                spNodata.Visibility = Visibility.Collapsed;
            }
        }
    }
}
