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
    /// PlayVoiceControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayVoiceControl : UserControl
    {
        public event Action<bool> PlayEvent = null;

        public MediaInfo SelectMediaInfo;

        public MediaInfo CruMediaInfo = new MediaInfo();

        public static readonly DependencyProperty CirculateValueProperty = DependencyProperty.Register("CirculateValue", typeof(int), typeof(PlayVoiceControl), new PropertyMetadata(0));
        public int CirculateValue
        {
            get
            {
                return (int)GetValue(CirculateValueProperty);
            }
            set
            {
                SetValue(CirculateValueProperty, value);
            }
        }

        public static readonly DependencyProperty IsPlayProperty = DependencyProperty.Register("IsPlay", typeof(bool), typeof(PlayVoiceControl), new PropertyMetadata(true));
        public bool IsPlay
        {
            get
            {
                return (bool)GetValue(IsPlayProperty);
            }
            set
            {
                EEMS.Dispatcher.AppConfigInfos.IsPlay = value;
                SetValue(IsPlayProperty, value);
            }
        }

        private bool isVLCTag = false;

        private bool isEnd = false;

        private bool IsPlayBtn = false;

        public static readonly DependencyProperty IsMuteProperty = DependencyProperty.Register("IsMute", typeof(bool), typeof(PlayVoiceControl), new PropertyMetadata(false));
        public bool IsMute
        {
            get
            {
                return (bool)GetValue(IsMuteProperty);
            }
            set
            {
                vlcPlay.MediaPlayer.Audio.IsMute = value;
                gVolumeBar.Opacity = value ? 0.3 : 1;
                SetValue(IsMuteProperty, value);
            }
        }

        public static readonly DependencyProperty VolumeValueProperty = DependencyProperty.Register("VolumeValue", typeof(double), typeof(PlayVoiceControl));
        public double VolumeValue
        {
            get
            {
                return (double)GetValue(VolumeValueProperty);
            }
            set
            {
                SetValue(VolumeValueProperty, value);
            }
        }
        public PlayVoiceControl()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            lbVoice.ItemsSource = ManageViewModel.VoicePlayMediaList;

            if (ManageViewModel.VoicePlayMediaList.Count < 1)
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
                Log.LogHelper.Instance.WirteLog(string.Format("PlayVoiceControl: UserCode:{0} MediaPlayer_EndReached  FileName:{1}", EEMS.Dispatcher.AppConfigInfos.CurrentUserInfos.UserCode, SelectMediaInfo.FileName), LogLevel.LogDebug);

                isEnd = true;
                IsPlay = false;
                CruMediaInfo.PlayPosition = 1;
                pbMemory.Value = 0;
                switch (CirculateValue)
                {
                    case 0:
                        spTime.Visibility = Visibility.Collapsed;
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
                isEnd = false;
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
                if (!isVLCTag)
                {
                    vlcPlay.MediaPlayer.Audio.Volume -= 2;
                    IsMute = false;
                    if (vlcPlay.MediaPlayer.Audio.Volume == 0)
                        IsMute = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (!isVLCTag)
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

        private void VlcPlay_PositionChanged(object sender, Media.VlcMediaPlayerPositionChangedEventArgs e)
        {
            VlcControl vp = sender as VlcControl;

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (vp.State == Media.Signatures.MediaStates.Stopped)
                    return;
                isVLCTag = true;
                pbMemory.Value = vp.Position;
                if (!IsMute)
                    VolumeValue = vp.Audio.Volume;
                isVLCTag = false;

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

        public void Play()
        {
            if (SelectMediaInfo != null && SelectMediaInfo.FilePath != null)
            {
                string outfilepath = SearchManager.GetInstance().DecipherFile(SelectMediaInfo);
                if (string.IsNullOrEmpty(outfilepath))
                    return;

                IsPlayBtn = false;
                vlcPlay.MediaPlayer.Play(new Uri(outfilepath));
                
                CruMediaInfo = SelectMediaInfo;
                lbVoice.SelectedItem = SelectMediaInfo;

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
            if (lbVoice.Items.Count < 2)
                return;

            if (lbVoice.SelectedIndex < 1)
                lbVoice.SelectedIndex = lbVoice.Items.Count - 1;
            else
                lbVoice.SelectedIndex--;

            SelectMediaInfo = lbVoice.SelectedItem as MediaInfo;

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
            if (SelectMediaInfo != null)
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

                        if (PlayEvent != null)
                            PlayEvent(true);
                    }
                }
                else
                    vlcPlay.MediaPlayer.Pause();
                IsPlay = !IsPlay;

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
            if (lbVoice.Items.Count < 2)
                return;

            if (lbVoice.SelectedIndex == lbVoice.Items.Count - 1)
                lbVoice.SelectedIndex = 0;
            else
                lbVoice.SelectedIndex++;

            SelectMediaInfo = lbVoice.SelectedItem as MediaInfo;

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
        }
        
        private void btnVolume_Click(object sender, RoutedEventArgs e)
        {
            IsMute = !IsMute;
        }
        
        /// <summary>
        /// 全屏显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            MediaInfo mi = btn.DataContext as MediaInfo;

            if (mi == SelectMediaInfo)
            {
                Stop();
                if (lbVoice.Items.Count > 1)
                {
                    if (lbVoice.SelectedIndex == lbVoice.Items.Count - 1)
                        lbVoice.SelectedIndex = 0;
                    else
                        lbVoice.SelectedIndex++;

                    SelectMediaInfo = lbVoice.SelectedItem as MediaInfo;
                }
            }

            if (mi != null)
            {
                ModelResponsible.Instance.RemoveHisPlayListByItem(mi);
                ManageViewModel.VoicePlayMediaList.Remove(mi);
            }

            if (ManageViewModel.VoicePlayMediaList.Count < 1)
            {
                SelectMediaInfo = null;
                spNodata.Visibility = Visibility.Visible;
                Stop();
            }
        }

        private void pbVolumeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isVLCTag)
            {
                vlcPlay.MediaPlayer.Audio.Volume = Convert.ToInt32(e.NewValue);
                IsMute = false;
            }
        }

        private void pbVolumeBar2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (!isVLCTag && !isEnd)
                {
                    vlcPlay.MediaPlayer.Position = (float)e.NewValue;
                    tbdf.Text = TimeSpan.FromMilliseconds(vlcPlay.MediaPlayer.Time).ToString().Substring(0, 8);
                    IsPlayBtn = true;
                }
            }));
        }

        private void lbVoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectMediaInfo = lbVoice.SelectedItem as MediaInfo;
        }

        private void lbVoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbVoice.Items.Count < 1)
                return;
            SelectMediaInfo = lbVoice.SelectedItem as MediaInfo;
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
            }
        }

        private void Slider_MouseWheel(object sender, MouseWheelEventArgs e)
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
            if (!isVLCTag)
            {
                vlcPlay.MediaPlayer.Audio.Volume += value;
                IsMute = false;
            }
        }
    }
}
