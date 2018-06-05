using System;
using System.Windows;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Manage.BLL;
using System.Timers;
using Hytera.EEMS.Common;
using Hytera.EEMS.Log;
using Hytera.EEMS.Dispatcher;
using System.Threading;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage
{
    /// <summary>
    /// PlayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlayWindow : BaseWindow
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        bool isload = false;
        public PlayWindow()
        {
            InitializeComponent();

            timer.Interval = 400;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (SystemInfo.GetLastInputTime() > 300000)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (!videoControl.IsPlay && !voiceControl.IsPlay)
                        {
                            timer.Stop();
                            this.Close();
                        }
                    }));
                }

                if (!isload)
                {
                    this.Width = 1219;
                    this.Height = 815;
                    tabControl.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
                    isload = true;
                }

                if (videoControl.lbVideo.Visibility == Visibility.Collapsed)
                {
                    if (SystemInfo.GetLastInputTime() > 3000)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            videoControl.wfh.Visibility = Visibility.Collapsed;
                        }));
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            videoControl.wfh.Visibility = Visibility.Visible;
                        }));
                    }
                }
                else
                    videoControl.wfh.Visibility = Visibility.Visible;

                if (picControl.lbPic.Visibility == Visibility.Collapsed)
                {
                    if (SystemInfo.GetLastInputTime() > 3000)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            picControl.gdBar.Visibility = Visibility.Collapsed;
                        }));
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            picControl.gdBar.Visibility = Visibility.Visible;
                        }));
                    }
                }
                else
                    picControl.gdBar.Visibility = Visibility.Visible;
            }));
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            voiceControl.PlayEvent += VoiceControl_PlayEvent;
            videoControl.PlayEvent += VideoControl_PlayEvent;
            picControl.FullScreenEvent += PicControl_FullScreenEvent;
            videoControl.FullScreenEvent += VideoControl_FullScreenEvent;
        }

        private void VideoControl_FullScreenEvent(bool obj)
        {
            if (obj)
            {
                videoControl.lbVideo.Visibility = Visibility.Collapsed;
                tabPic.Visibility = sp.Visibility = tabVoice.Visibility = Visibility.Collapsed;
                videoControl.gd.ColumnDefinitions[0].Width = new GridLength(0);
                videoControl.gdPlay.RowDefinitions[1].Height = new GridLength(0);
                videoControl.Margin = new Thickness(-3,-8,-3,-3);
                tabVideo.Height = 0;
                Grid.SetRow(videoControl.wfh, 0);
                this.WindowState = WindowState.Maximized;
                videoControl.btnFullScreen.SetResourceReference(ToolTipProperty, "FullScreenWindowToolTip");
            }
            else
            {
                Grid.SetRow(videoControl.wfh, 1);
                videoControl.Margin = new Thickness(0);
                tabVideo.Height = 49;
                videoControl.gd.ColumnDefinitions[0].Width = new GridLength(226);
                videoControl.gdPlay.RowDefinitions[1].Height = new GridLength(74);
                videoControl.lbVideo.Visibility = Visibility.Visible;
                tabPic.Visibility = sp.Visibility = tabVoice.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
                videoControl.btnFullScreen.SetResourceReference(ToolTipProperty, "PlayControlBtnFull");
            }
        }
        private void PicControl_FullScreenEvent(bool obj)
        {
            if (obj)
            {
                picControl.lbPic.Visibility = Visibility.Collapsed;
                tabVideo.Visibility = sp.Visibility = tabVoice.Visibility = Visibility.Collapsed;
                picControl.gd.ColumnDefinitions[0].Width = new GridLength(0);
                picControl.gdPlay.RowDefinitions[1].Height = new GridLength(0);
                picControl.Margin = new Thickness(-3, -8, -3, -3);
                tabPic.Height = 0;
                Grid.SetRow(picControl.gdBar, 0);
                this.WindowState = WindowState.Maximized;
                picControl.btnFullScreen.SetResourceReference(ToolTipProperty, "FullScreenWindowToolTip");
            }
            else
            {
                Grid.SetRow(picControl.gdBar, 1);
                picControl.Margin = new Thickness(0);
                tabPic.Height = 49;
                picControl.gd.ColumnDefinitions[0].Width = new GridLength(226);
                picControl.gdPlay.RowDefinitions[1].Height = new GridLength(70);
                picControl.lbPic.Visibility = Visibility.Visible;
                tabVideo.Visibility = sp.Visibility = tabVoice.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
                picControl.btnFullScreen.SetResourceReference(ToolTipProperty, "PlayControlBtnFull");
            }
        }

        private void VideoControl_PlayEvent(bool obj)
        {
            voiceControl.Stop();
        }

        private void VoiceControl_PlayEvent(bool obj)
        {
            videoControl.Stop();
        }

        public void UpdateSelectTab(MediaInfo dataContext)
        {
            switch (dataContext.MediaType)
            {
                case "1":
                    ModelResponsible.Instance.AnalyzeVideoPlayMediaList(dataContext);
                    tabVideo.IsSelected = true;
                    videoControl.SelectMediaInfo = dataContext;
                    videoControl.Play();
                    break;
                case "2":
                    ModelResponsible.Instance.AnalyzeVoicePlayMediaList(dataContext);
                    tabVoice.IsSelected = true;
                    voiceControl.SelectMediaInfo = dataContext;
                    voiceControl.Play();
                    break;
                case "3":
                    ModelResponsible.Instance.AnalyzePicturePlayMediaList(dataContext);
                    tabPic.IsSelected = true;
                    picControl.SelectMediaInfo = dataContext;
                    break;
            }

            LogHelper.Instance.WirteLog(string.Format("PlayWindow: UserCode:{0} UpdateSelectTab  ", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            UpdateImpState();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            voiceControl.Stop();
            videoControl.Stop();
            picControl.Stop();
            timer.Stop();
            this.Close();
        }

        private void impSelect_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                impSelect.IsEnabled = false;

                Thread thread = new Thread(new ThreadStart(delegate ()
                {
                    Thread.Sleep(1000);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        impSelect.IsEnabled = true;
                    }));
                }));
                thread.Start();

                impSelect.IsSelect = !impSelect.IsSelect;

                if (tabVideo != null && tabVideo.IsSelected && videoControl.CruMediaInfo.RecordID != null)
                {
                    videoControl.CruMediaInfo.UpdateMark = videoControl.CruMediaInfo.UserTag;
                    videoControl.CruMediaInfo.UpdateUserImp = impSelect.IsSelect ? "1" : "0";
                    SearchManager.GetInstance().SaveMediaInfo(videoControl.CruMediaInfo);
                    SearchManager.GetInstance().SendOperationLog("CollectEvidenceKeynote", videoControl.CruMediaInfo);
                }

                if (tabVoice != null && tabVoice.IsSelected && voiceControl.CruMediaInfo.RecordID != null)
                {
                    voiceControl.CruMediaInfo.UpdateMark = voiceControl.CruMediaInfo.UserTag;
                    voiceControl.CruMediaInfo.UpdateUserImp = impSelect.IsSelect ? "1" : "0";
                    SearchManager.GetInstance().SaveMediaInfo(voiceControl.CruMediaInfo);
                    SearchManager.GetInstance().SendOperationLog("CollectEvidenceKeynote", voiceControl.CruMediaInfo);
                }

                if (tabPic != null && tabPic.IsSelected && picControl.SelectMediaInfo.RecordID != null)
                {
                    picControl.SelectMediaInfo.UpdateMark = picControl.SelectMediaInfo.UserTag;
                    picControl.SelectMediaInfo.UpdateUserImp = impSelect.IsSelect ? "1" : "0";
                    SearchManager.GetInstance().SaveMediaInfo(picControl.SelectMediaInfo);
                    SearchManager.GetInstance().SendOperationLog("CollectEvidenceKeynote", picControl.SelectMediaInfo);
                }
                LogHelper.Instance.WirteLog(string.Format("PlayWindow: UserCode:{0} impSelect  ", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
            }));
        }

        private void UpdateImpState()
        {
            if (tabVideo != null && tabVideo.IsSelected)
            {
                impSelect.IsSelect = videoControl.SelectMediaInfo.UserImp == "1" ? true : false;
            }

            if (tabVoice != null && tabVoice.IsSelected)
            {
                impSelect.IsSelect = voiceControl.SelectMediaInfo.UserImp == "1" ? true : false;
            }

            if (tabPic != null && tabPic.IsSelected)
            {
                impSelect.IsSelect = picControl.SelectMediaInfo.UserImp == "1" ? true : false;
            }

            LogHelper.Instance.WirteLog(string.Format("PlayWindow: UserCode:{0} UpdateImpState  ", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
        }
        
    }
}
