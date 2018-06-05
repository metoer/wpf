using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Lib;
using Hytera.EEMS.Media.Controls;
using Hytera.EEMS.Model;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// PlayVideoControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayVideoControl : UserControl
    {
        public event Action<bool> FullScreenEvent = null;

        public event Action<bool> PlayEvent = null;

        public MediaInfo SelectMediaInfo = new MediaInfo();

        public MediaInfo CruMediaInfo = new MediaInfo();

        public static readonly DependencyProperty CirculateValueProperty = DependencyProperty.Register("CirculateValue", typeof(int), typeof(PlayVideoControl), new PropertyMetadata(0));
        public int CirculateValue
        {
            get
            {
                return (int)GetValue(CirculateValueProperty);
            }
            set
            {
                cmbMode.Tag = value.ToString();
                SetValue(CirculateValueProperty, value);
            }
        }

        public static readonly DependencyProperty IsPlayProperty = DependencyProperty.Register("IsPlay", typeof(bool), typeof(PlayVideoControl), new PropertyMetadata(true));
        public bool IsPlay
        {
            get
            {
                return (bool)GetValue(IsPlayProperty);
            }
            set
            {
                try
                {
                    AppConfigInfos.IsPlay = value;
                    pbMemory.IsEnabled = value;
                    cmbPlay.Tag = value.ToString();
                    btnTakeSnapshot.IsEnabled = value;
                    SetValue(IsPlayProperty, value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        public static readonly DependencyProperty IsMuteProperty = DependencyProperty.Register("IsMute", typeof(bool), typeof(PlayVideoControl), new PropertyMetadata(false));
        public bool IsMute
        {
            get
            {
                return (bool)GetValue(IsMuteProperty);
            }
            set
            {
                vlcPlay.MediaPlayer.Audio.IsMute = value;
                cmbVolume.Tag = value.ToString();
                gVolumeBar.Opacity = value ? 0.3 : 1;
                SetValue(IsMuteProperty, value);
            }
        }

        public static readonly DependencyProperty VolumeValueProperty = DependencyProperty.Register("VolumeValue", typeof(double), typeof(PlayVideoControl));
        public double VolumeValue
        {
            get
            {
                return (double)GetValue(VolumeValueProperty);
            }
            set
            {
                pbVolumeBar.Value = value;
                SetValue(VolumeValueProperty, value);
            }
        }
        private bool IsVlcTag = false;

        private bool IsEnd = false;

        private bool IsPlayBtn = false;

        public PlayVideoControl()
        {
            InitializeComponent();

            Init();
            
        }

        private void Init()
        {
            lbVideo.ItemsSource = ManageViewModel.VideoPlayMediaList;

            if (ManageViewModel.VideoPlayMediaList.Count < 1)
                spNodata.Visibility = Visibility.Visible;

            vlcPlay.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            vlcPlay.MediaPlayer.PositionChanged += VlcPlay_PositionChanged;
            vlcPlay.MediaPlayer.EndReached += MediaPlayer_EndReached;
            vlcPlay.MediaPlayer.EndInit();

            pcRate.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("PlayControlCmbNegative2").ToString(), ItemCode = "0.25", ItemID = "0.25" });
            pcRate.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("PlayControlCmbNegative1").ToString(), ItemCode = "0.5", ItemID = "0.5" });
            pcRate.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("PlayControlCmbNormal").ToString(), ItemCode = "1.0", ItemID = "1.0" });
            pcRate.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("PlayControlCmbMultiple1").ToString(), ItemCode = "2.0", ItemID = "2.0" });
            pcRate.Items.Add(new Enums.ComBoxItem() { ItemName = TryFindResource("PlayControlCmbMultiple2").ToString(), ItemCode = "4.0", ItemID = "4.0" });
            pcRate.Text = TryFindResource("PlayControlCmbNormal").ToString();
            pcRate.TextChanged += PcRate_TextChanged;
            this.KeyUp += PlayVideoControl_KeyUp;
            IsPlay = false;
        }

        private void MediaPlayer_EndReached(object sender, Media.VlcMediaPlayerEndReachedEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                LogHelper.Instance.WirteLog(string.Format("PlayVideoControl: UserCode:{0} MediaPlayer_EndReached  FileName:{1}", AppConfigInfos.CurrentUserInfos.UserCode, SelectMediaInfo.FileName), LogLevel.LogDebug);

                IsEnd = true;
                IsPlay = false;
                CruMediaInfo.PlayPosition = 1;
                pbMemory.Value = 0;
                switch (CirculateValue)
                {
                    case 0:
                        spTime.Visibility = Visibility.Collapsed;
                        Stop();
                        break;
                    case 1:
                        if (CruMediaInfo != null && CruMediaInfo.FilePath != null)
                        {
                            string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                            if (string.IsNullOrEmpty(outfilepath))
                                return;

                            IsPlayBtn = false;
                            vlcPlay.MediaPlayer.Play(new Uri(outfilepath));

                            if (PlayEvent != null)
                                PlayEvent(true);

                            IsPlay = true;
                        }
                        break;
                    case 2:
                        btnNext_Click(null, null);
                        break;
                }
                IsEnd = false;
            }));
        }

        private void OnVlcControlNeedsLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            string curpath = System.Windows.Forms.Application.StartupPath;
            if (curpath.EndsWith("\\"))
            {
                curpath = curpath.Substring(0, curpath.Length - 1);
            }
            if (AssemblyName.GetAssemblyName(Assembly.GetEntryAssembly().Location).ProcessorArchitecture == ProcessorArchitecture.X86)
            {
                curpath += @"\VlcLib\x86";
            }
            else
            {
                curpath += @"\VlcLib\x64";
            }

            e.VlcLibDirectory = new DirectoryInfo(curpath);
        }

        private void PlayVideoControl_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (!IsVlcTag)
                {
                    vlcPlay.MediaPlayer.Audio.Volume -= 2;
                    IsMute = false;

                    if (vlcPlay.MediaPlayer.Audio.Volume == 0)
                        IsMute = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (!IsVlcTag)
                {
                    vlcPlay.MediaPlayer.Audio.Volume += 2;
                    IsMute = false;
                }
            }
        }

        private void PcRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pcRate.SelectValue != null && pcRate.SelectValue.Length > 0)
                vlcPlay.MediaPlayer.Rate = (float)Convert.ToDecimal(pcRate.SelectValue);
        }

        /// <summary>
        /// 本地视频播放
        /// </summary>
        public void Play()
        {
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                {
                    return;
                }

                IsPlayBtn = false;
                vlcPlay.MediaPlayer.Play(new Uri(outfilepath));

                CruMediaInfo = SelectMediaInfo;
                lbVideo.SelectedItem = SelectMediaInfo;

                spNodata.Visibility = Visibility.Collapsed;

                if (PlayEvent != null)
                    PlayEvent(true);
                IsPlay = true;
            }
        }

        public void Stop()
        {
            new Thread(() =>
            {
                Thread.Sleep(300);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    vlcPlay.MediaPlayer.Stop();
                }));
            })
            { IsBackground = true }.Start();
            IsPlay = false;
            IsMute = false;
            pbMemory.Value = vlcPlay.MediaPlayer.Position = 0;
            spTime.Visibility = Visibility.Collapsed;
        }
        private void VlcPlay_PositionChanged(object sender, Media.VlcMediaPlayerPositionChangedEventArgs e)
        {
            VlcControl vp = sender as VlcControl;
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (vp.State == Media.Signatures.MediaStates.Stopped)
                {
                    return;
                }
                IsVlcTag = true;
                pbMemory.Value = vp.Position;
                if (!IsMute)
                    VolumeValue = vp.Audio.Volume;
                IsVlcTag = false;


                spTime.Visibility = Visibility.Visible;
                tbdf.Text = TimeSpan.FromMilliseconds(vp.Time).ToString().Substring(0, 8);
                tbTotal.Text = CruMediaInfo.ShowDuration;

                if (CruMediaInfo != null && CruMediaInfo.PlayPosition < vp.Position)
                    CruMediaInfo.PlayPosition = vp.Position;
                //从上次加载的位置开始播放视频
                if (!IsPlayBtn && CruMediaInfo.PlayPosition < 1 && CruMediaInfo.PlayPosition > vp.Position)
                    vlcPlay.MediaPlayer.Position = CruMediaInfo.PlayPosition;
            }));

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Stop();
            }));
        }
        /// <summary>
        /// 上一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (lbVideo.Items.Count < 2)
                return;

            if (lbVideo.SelectedIndex < 1)
                lbVideo.SelectedIndex = lbVideo.Items.Count - 1;
            else
                lbVideo.SelectedIndex--;

            SelectMediaInfo = lbVideo.SelectedItem as MediaInfo;
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                IsPlayBtn = false;
                vlcPlay.MediaPlayer.Play(new Uri(outfilepath));
                CruMediaInfo = SelectMediaInfo;

                if (PlayEvent != null)
                    PlayEvent(true);

                IsPlay = true;
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                if (!IsPlay)
                {
                    if (vlcPlay.MediaPlayer.State == Media.Signatures.MediaStates.Paused)
                    {
                        IsPlayBtn = true;
                        vlcPlay.MediaPlayer.Play();
                    }
                    else
                        if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
                    {
                        string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                        if (string.IsNullOrEmpty(outfilepath))
                            return;
                        IsPlayBtn = false;
                        vlcPlay.MediaPlayer.Play(new Uri(outfilepath));
                        CruMediaInfo = SelectMediaInfo;
                    }
                    if (PlayEvent != null)
                        PlayEvent(true);
                }
                else
                    vlcPlay.MediaPlayer.Pause();
                IsPlay = !IsPlay;


                btnTakeSnapshot.IsEnabled = true;
                pbMemory.IsEnabled = true;
            }
        }
        /// <summary>
        /// 下一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (lbVideo.Items.Count < 2)
                return;

            if (lbVideo.SelectedIndex == lbVideo.Items.Count - 1)
                lbVideo.SelectedIndex = 0;
            else
                lbVideo.SelectedIndex++;

            SelectMediaInfo = lbVideo.SelectedItem as MediaInfo;

            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                IsPlayBtn = false;
                vlcPlay.MediaPlayer.Play(new Uri(outfilepath));
                CruMediaInfo = SelectMediaInfo;

                if (PlayEvent != null)
                    PlayEvent(true);
                IsPlay = true;
            }
        }

        /// <summary>
        /// 循环播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMode_Click(object sender, RoutedEventArgs e)
        {
            if (CirculateValue > 1)
                CirculateValue = 0;
            else
                CirculateValue++;
        }

        private void btnNextFrame_Click(object sender, RoutedEventArgs e)
        {
            vlcPlay.MediaPlayer.NextFrame();
            IsPlay = false;
            pbMemory.IsEnabled = true;
            btnTakeSnapshot.IsEnabled = true;
        }


        private void btnVolume_Click(object sender, RoutedEventArgs e)
        {
            IsMute = !IsMute;
        }
        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTakeSnapshot_Click(object sender, RoutedEventArgs e)
        {
            btnTakeSnapshot.IsEnabled = false;

            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Thread.Sleep(700);
                    btnTakeSnapshot.IsEnabled = true;
                }));
            }));
            thread.Start();

            if (AppConfigInfos.AppStateInfos.TakeSnapshotPath != string.Empty && Directory.Exists(AppConfigInfos.AppStateInfos.TakeSnapshotPath))
            {
                vlcPlay.MediaPlayer.TakeSnapshot(AppConfigInfos.AppStateInfos.TakeSnapshotPath);
                LogHelper.Instance.WirteLog(string.Format("PlayVideoControl: UserCode:{0} btnTakeSnapshot  RecordID:{1} Path:{2}", AppConfigInfos.CurrentUserInfos.UserCode, SelectMediaInfo.RecordID, AppConfigInfos.AppStateInfos.TakeSnapshotPath), LogLevel.LogDebug);
            }
            else
            {
                EEMS.Resources.Windows.NewMessageBox.Show(Application.Current.FindResource("PlayControlBtnTakeSnapshotSetPath").ToString());
            }
        }

        private void btnSavePath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
            ofd.Description = TryFindResource("PlayControlBtnTakeSnapshotDescription").ToString() + AppConfigInfos.AppStateInfos.TakeSnapshotPath;
            ofd.SelectedPath = AppConfigInfos.AppStateInfos.TakeSnapshotPath;
            ofd.ShowDialog();

            if (ofd.SelectedPath != string.Empty)
            {
                AppConfigInfos.AppStateInfos.TakeSnapshotPath = ofd.SelectedPath;
            }
        }

        /// <summary>
        /// 全屏显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (lbVideo.Visibility == Visibility.Visible)
            {
                if (FullScreenEvent != null)
                    FullScreenEvent(true);
            }
            else
            {
                if (FullScreenEvent != null)
                    FullScreenEvent(false);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            MediaInfo mi = btn.DataContext as MediaInfo;
            if (mi == SelectMediaInfo)
            {
                Stop();

                if (lbVideo.Items.Count > 1)
                {
                    if (lbVideo.SelectedIndex == lbVideo.Items.Count - 1)
                        lbVideo.SelectedIndex = 0;
                    else
                        lbVideo.SelectedIndex++;

                    SelectMediaInfo = lbVideo.SelectedItem as MediaInfo;
                }
            }

            if (mi != null)
            {
                ModelResponsible.Instance.RemoveHisPlayListByItem(mi);
                ManageViewModel.VideoPlayMediaList.Remove(mi);
            }

            if (ManageViewModel.VideoPlayMediaList.Count < 1)
            {
                SelectMediaInfo = null;
                spNodata.Visibility = Visibility.Visible;
                Stop();
            }
        }

        private void pbVolumeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsVlcTag)
            {
                vlcPlay.MediaPlayer.Audio.Volume = Convert.ToInt32(e.NewValue);
                IsMute = false;

                if (vlcPlay.MediaPlayer.Audio.Volume == 0)
                    IsMute = true;
            }
        }

        private void pbMemory_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (!IsVlcTag && !IsEnd)
                {
                    vlcPlay.MediaPlayer.Position = (float)e.NewValue;
                    tbdf.Text = TimeSpan.FromMilliseconds(vlcPlay.MediaPlayer.Time).ToString().Substring(0, 8);
                    IsPlayBtn = true;
                }
            }));
        }

        private void lbVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectMediaInfo = lbVideo.SelectedItem as MediaInfo;
        }

        private void lbVideo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lbVideo.Items.Count < 1)
                return;

            SelectMediaInfo = lbVideo.SelectedItem as MediaInfo;
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;
                IsPlayBtn = false;

                new Thread(() =>
                {
                    Thread.Sleep(300);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        vlcPlay.MediaPlayer.Play(new Uri(outfilepath));
                    }));
                })
                { IsBackground = true }.Start();

                CruMediaInfo = SelectMediaInfo;
                IsPlay = true;
            }
        }

        private void Slider_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            int value = e.Delta;
            if (value > 0)
            {
                value = 2;
            }
            else
            {
                value = -2;
            }
            if (!IsVlcTag)
            {
                vlcPlay.MediaPlayer.Audio.Volume += value;
                IsMute = false;

                if (vlcPlay.MediaPlayer.Audio.Volume == 0)
                    IsMute = true;
            }
        }
        
    }
}
