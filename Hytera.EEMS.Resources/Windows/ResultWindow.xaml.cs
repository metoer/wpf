using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Hytera.EEMS.Resources.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ResultWindow : BaseWindow
    {
        #region 属性、变量、构造
        /// <summary>
        /// 计算超时
        /// </summary>
        private DispatcherTimer testTimer = new DispatcherTimer();

        /// <summary>
        /// 发送的消息类型
        /// </summary>
        private MsgType sendMsgType;

        /// <summary>
        /// 发送消息内容
        /// </summary>
        private Conditions con;

        /// <summary>
        /// 是否需要通过此窗口发送消息
        /// </summary>
        bool isSendMsg = false;

        public ResultWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造
        /// </summary>

        public ResultWindow(MsgType recieveMsgType, string contentMsg)
            : this()
        {
            tbMsg.Text = contentMsg;
            ReceiveMsgType = recieveMsgType;
            MessageBoxResult = MessageBoxResult.No;
        }

        public ResultWindow(MsgType sendMsgType, MsgType recieveMsgType, Conditions con, string contentMsg)
            : this(recieveMsgType, contentMsg)
        {
            this.sendMsgType = sendMsgType;
            this.con = con;
            isSendMsg = true;
        }

        public ResultWindow(MsgType sendMsgType, MsgType recieveMsgType, Conditions con, string contentMsg, int searchTimeOut)
            : this(recieveMsgType, contentMsg)
        {
            this.sendMsgType = sendMsgType;
            this.con = con;
            isSendMsg = true;
            TimeMilliseconds = searchTimeOut * 1000;
        }

        public ResultWindow(MsgType sendMsgType, MsgType recieveMsgType, string contentMsg)
            : this(recieveMsgType, contentMsg)
        {
            this.sendMsgType = sendMsgType;
            isSendMsg = true;
        }


        private long timemilliseconds = 15000;

        /// <summary>
        /// 默认超时时间
        /// </summary>
        public long TimeMilliseconds
        {
            get
            {
                return timemilliseconds;
            }
            set
            {
                timemilliseconds = value;
            }
        }

        /// <summary>
        /// 数据发送入口
        /// </summary>
        public static DataResponsible Responsible
        {
            get;
            set;
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public MsgType ReceiveMsgType
        {
            get;
            private set;
        }

        /// <summary>
        /// 结果
        /// </summary>
        public object ResultValue
        {
            get;
            set;
        }

        /// <summary>
        /// 操作结果,成功YES，失败NO,超时Canel
        /// </summary>
        public MessageBoxResult MessageBoxResult
        {
            get;
            private set;
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            testTimer.Interval = TimeSpan.FromMilliseconds(TimeMilliseconds);
            testTimer.Tick += new EventHandler(Timer_Tick);
            loading.ChangeState();
            testTimer.Start();
            if (isSendMsg)
            {
                if (con == null || con.Count == 0)
                {
                    Responsible.SendCommand(sendMsgType);
                }
                else
                {
                    Responsible.SendMsg(sendMsgType, con);
                }
            }
        }

        /// <summary>
        /// 失败时关闭窗口
        /// </summary>
        public void FailedCloseWindow(object result)
        {
            ResultValue = result;
            MessageBoxResult = MessageBoxResult.No;
            this.Close();
        }

        /// <summary>
        /// 成功时关闭窗口
        /// </summary>
        public void SuccessCloseWindow()
        {
            MessageBoxResult = MessageBoxResult.Yes;
            this.Close();
        }

        /// <summary>
        /// 超时时关闭窗口
        /// </summary>
        public void OverTimeCloseWindow()
        {
            MessageBoxResult = MessageBoxResult.Cancel;
            this.Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OverTimeCloseWindow();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            testTimer.Stop();
        }
    }
}
