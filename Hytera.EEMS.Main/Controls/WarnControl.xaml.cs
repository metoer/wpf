using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Xml;

namespace Hytera.EEMS.Main.Controls
{
    /// <summary>
    /// WarnControl.xaml 的交互逻辑
    /// </summary>
    public partial class WarnControl : UserControl
    {
        Storyboard storyboard;

        ThicknessAnimation animation;

        /// <summary>
        /// 新top
        /// </summary>

        double newTop = 0;

        List<AlertInfo> msgList = new List<AlertInfo>();

        bool isShow = false;

        readonly object warnMsgListLock = new object();

        SoundHelper soundHelper = null;

        //定时提示未取消的告警
        System.Timers.Timer timer = null;

        /// <summary>
        /// 当前消息索引
        /// </summary>
        int currentIndex = 0;

        public WarnControl()
        {
            InitializeComponent();

            Init();
        }

        private void Init() 
        {
            soundHelper = new SoundHelper(AppDomain.CurrentDomain.BaseDirectory + "Data\\Alarm.wav", true);

            timer = new System.Timers.Timer(AppConfigInfos.AppStateInfos.WarnCanelTime * 1000);
            timer.Elapsed += timer_Elapsed;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(WarnControl));

        /// <summary>
        /// 消息索引
        /// </summary>
        public string MsgIndex
        {
            get
            {
                return (string)GetValue(MsgIndexProperty);
            }
            set
            {
                SetValue(MsgIndexProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty MsgIndexProperty = DependencyProperty.Register("MsgIndex", typeof(string), typeof(WarnControl));

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="msg"></param>
        public void ReceiveMsg(string msg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(msg);

                string type = xmlDoc.SelectSingleNode("hytera/cmd_name").InnerXml;

                AlarmType alarmType = AlarmType.alert;

                bool enumConver = Enum.TryParse<AlarmType>(type, out alarmType);

                if (alarmType == AlarmType.alert)
                {
                    AlarmContent(xmlDoc);
                }
                else
                {
                    AlramCancel(xmlDoc);
                }

            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg("Warn Xml Error:" + e.Message);
            }
        }

        /// <summary>
        /// 去除模块的告警
        /// </summary>
        /// <param name="xmlDoc"></param>
        private void AlramCancel(XmlDocument xmlDoc)
        {
            string alertCode = xmlDoc.SelectSingleNode("hytera/content/alert/alert_code").InnerXml;

            lock (warnMsgListLock)
            {
                for (int i = msgList.Count - 1; i >= 0; i--)
                {
                    if (msgList[i].AlertCode.Equals(alertCode))
                    {
                        msgList.RemoveAt(i);
                    }
                }

                SetDisplayMsg(currentIndex);
            }

            // 隐藏
            if (msgList.Count == 0)
            {
                Button_Click(null, null);
            }
        }


        /// <summary>
        /// 告警内容
        /// </summary>
        /// <param name="xmlDoc"></param>
        private void AlarmContent(XmlDocument xmlDoc)
        {
            // 消息内容
            string content = xmlDoc.SelectSingleNode("hytera/content/alert/alert_message").InnerXml;
            string msgLevel = xmlDoc.SelectSingleNode("hytera/content/alert/level").InnerXml;
            string alertCode = xmlDoc.SelectSingleNode("hytera/content/alert/alert_code").InnerXml;

            lock (warnMsgListLock)
            {
                // 过滤重复告警信息
                if (msgList.Find(p => p.AlertCode.Equals(alertCode)) == null)
                {
                    msgList.Add(new AlertInfo() { AlertCode = alertCode,  AlertMessage = content, Level = msgLevel });

                    SetDisplayMsg(msgList.Count - 1);
                }
            }
        }

        /// <summary>
        /// 暂时不处理，隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            storyboard = new Storyboard();

            newTop = this.Margin.Top - this.Height - 10;

            animation = CreateDoubleAnimation(this.Margin.Top, newTop, this, 200);

            storyboard.Children.Add(animation);

            storyboard.Completed += storyboard_Completed;

            storyboard.Begin(this);

            // 停止播放
            //SoundHelperBak.StopSound();
            soundHelper.Stop();

            if (msgList.Count > 0 && currentIndex < msgList.Count)
            {
                msgList[currentIndex].IsMute = true;
                // 如果未取消的告警全部被静音，启动定时器，定时提示告警
                if (!msgList.Exists(m => !m.IsMute))
                {
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// 动画结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void storyboard_Completed(object sender, EventArgs e)
        {
            if (storyboard != null)
                storyboard.Stop(this);

            storyboard = null;

            animation = null;

            this.Margin = new Thickness(0, newTop, 0, 0);

            isShow = newTop == 0;
        }

        /// <summary>
        /// 创建动画
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="element"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private ThicknessAnimation CreateDoubleAnimation(double from, double to, FrameworkElement element, int seconds)
        {
            ThicknessAnimation animation = new ThicknessAnimation();

            animation.From = new Thickness(0, from, 0, 0);

            animation.To = new Thickness(0, to, 0, 0);

            animation.Duration = new Duration(TimeSpan.FromMilliseconds(seconds));

            Storyboard.SetTargetName(animation, element.Name);

            Storyboard.SetTargetProperty(animation, new PropertyPath(FrameworkElement.MarginProperty));

            return animation;
        }

        /// <summary>
        /// 显示
        /// </summary>
        private void Show()
        {
            if (isShow)
            {
                return;
            }

            isShow = true;

            storyboard = new Storyboard();

            newTop = 0;

            animation = CreateDoubleAnimation(this.Margin.Top, newTop, this, 200);

            storyboard.Children.Add(animation);

            storyboard.Completed += storyboard_Completed;

            storyboard.Begin(this);

            // 播放音乐
            //SoundHelperBak.PlaySound(AppDomain.CurrentDomain.BaseDirectory + "Data\\Alarm.wav");
            soundHelper.Play();
        }

        /// <summary>
        /// 设置显示第几条消息
        /// </summary>
        /// <param name="index"></param>
        private void SetDisplayMsg(int index)
        {
            timer.Stop();

            if (msgList.Count == 0)
                return;

            if (index < 0)
            {
                index = msgList.Count - 1;
            }
            else if (index >= msgList.Count)
            {
                index = 0;
            }

            currentIndex = index;

            //告警信息多语言翻译，在告警码前加上"Alarm_"前缀
            object data = TryFindResource(string.Format("Alarm_{0}", msgList[currentIndex].AlertCode));

            Message = object.Equals(data, null) ? msgList[currentIndex].AlertMessage.ToString() : data.ToString();

            MsgIndex = string.Format("{0}/{1}", currentIndex + 1, msgList.Count);

            Show();
        }

        /// <summary>
        /// 显示下一条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            SetDisplayMsg(currentIndex + 1);
        }

        /// <summary>
        /// 显示上一条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            SetDisplayMsg(currentIndex - 1);
        }
        /// <summary>
        /// 暂不处理稍后提示 - 定时提示还未取消的告警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                SetDisplayMsg(currentIndex);
            }));
        }
    }
}
