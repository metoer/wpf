using Hytera.EEMS.Media.Signatures;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Media.Controls
{
    /// <summary>
    /// VlcPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class VlcPlayer : UserControl
    {
        #region 属性、事件、构造
        public VlcPlayer()
        {
            InitializeComponent();

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
            playControl.MediaPlayer.VlcLibDirectory = new DirectoryInfo(curpath);
            
            playControl.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            playControl.MediaPlayer.EndInit();
        }

        /// <summary>
        /// 播放文件进度修改
        /// </summary>
        [Category("Media Player")]
        public event EventHandler<VlcMediaPlayerPositionChangedEventArgs> PositionChanged;

        /// <summary>
        /// 状态
        /// </summary>
        [Browsable(false)]
        public MediaStates State
        {
            get
            {
                if (playControl.MediaPlayer != null)
                {
                    return playControl.MediaPlayer.State;
                }
                else
                {
                    return MediaStates.NothingSpecial;
                }
            }
        }

        /// <summary>
        /// 设置播放速率
        /// </summary>
        public float Rate
        {
            get
            {
                return playControl.MediaPlayer.Rate;
            }
            set
            {
                playControl.MediaPlayer.Rate = value;
            }
        }

        /// <summary>
        /// 声音
        /// </summary>
        public int Volume
        {
            get { return playControl.MediaPlayer.Audio.Volume; }
            set { playControl.MediaPlayer.Audio.Volume = value; }
        }

        /// <summary>
        /// 设置文件播放位置
        /// </summary>
        public float Position
        {
            get
            {
                return playControl.MediaPlayer.Position;
            }
            set
            {
                playControl.MediaPlayer.Position = value;
            }
        }

        /// <summary>
        /// 当前播放时间或者设置播放的时间点(单位毫秒)
        /// </summary>
        public long CurrentTime
        {
            get
            {
                return playControl.MediaPlayer.Time;
            }
            set
            {
                playControl.MediaPlayer.Time = value;
            }
        }

        /// <summary>
        /// 总时间
        /// </summary>
        public long TotalTime
        {
            get
            {
                return playControl.MediaPlayer.Length;
            }
        }
        #endregion

        #region  接口方法
        /// <summary>
        /// 在指定播放文件或者暂停后继续播放
        /// </summary>
        public void Play()
        {
            playControl.MediaPlayer.Play();
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="uri">地址</param>
        /// <param name="options">参数</param>
        public void Play(Uri uri, params string[] options)
        {
            playControl.MediaPlayer.Play(uri, options);
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="uri">地址</param>
        /// <param name="options">参数</param>
        public void Play(string uri, params string[] options)
        {
            playControl.MediaPlayer.Play(uri, options);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (playControl.MediaPlayer.State == MediaStates.Playing)
            {
                playControl.MediaPlayer.Pause();
            }
        }

        /// <summary>
        /// 停止 
        /// </summary>
        public void Stop()
        {
            playControl.MediaPlayer.Stop();
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="fileName">文件存放地址</param>
        public void TakeSnapshot(string fileName)
        {
            playControl.MediaPlayer.TakeSnapshot(fileName);
        }

        /// <summary>
        /// 下一帧
        /// </summary>
        public void NextFrame()
        {
            playControl.MediaPlayer.NextFrame();
        }
        #endregion

        /// <summary>
        /// 播放文件点改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (PositionChanged != null)
                {
                    PositionChanged(this, e);
                }
            }));

        }
    }
}
