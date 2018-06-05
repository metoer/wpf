using Hytera.EEMS.Model;
using System;
using System.Threading;

namespace Hytera.EEMS.Dispatcher
{
    internal class EventManager : ModuleBaseEntry
    {
        #region 变量、属性
        private static EventManager instance;

        /// <summary>
        /// 单实例
        /// </summary>
        internal static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Interlocked.CompareExchange(ref instance, new EventManager(), null);
                }

                return instance;
            }
        }

        /// <summary>
        /// 发送数据对象
        /// </summary>
        internal DataResponsible DataResponsible
        {
            get;
            private set;
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        private event MessageNoticeEventHandler messageNotice;

        internal event MessageNoticeEventHandler MessageNotice
        {
            add
            {
                messageNotice += value;
            }
            remove
            {
                if (messageNotice != null)
                {
                    messageNotice -= value;
                }
            }
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        private event UnknownMessageNoticeEventHandler unknownMessageNotice;

        internal event UnknownMessageNoticeEventHandler UnknownMessageNotice
        {
            add
            {
                unknownMessageNotice += value;
            }
            remove
            {
                if (unknownMessageNotice != null)
                {
                    unknownMessageNotice -= value;
                }
            }
        }

        private event DelegateAppSelfMessageNotic selfMessageNotic;

        internal event DelegateAppSelfMessageNotic SelfMessageNotic
        {
            add
            {
                selfMessageNotic += value;
            }
            remove
            {
                if (selfMessageNotic != null)
                {
                    selfMessageNotic -= value;
                }
            }
        }

        private event InitEventHandler connectNetWorkNotic;

        internal event InitEventHandler ConnectNetWorkNotic
        {
            add
            {
                connectNetWorkNotic += value;
            }
            remove
            {
                if (connectNetWorkNotic != null)
                {
                    connectNetWorkNotic -= value;
                }
            }
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="responsible">数据发送接口</param>
        public override void Init(DataResponsible responsible)
        {
            this.DataResponsible = responsible;
        }

        public override void DisConnectNetWork()
        {

        }

        public override void ConnectNetWork(DataResponsible responsible)
        {
            if (connectNetWorkNotic != null)
            {
                connectNetWorkNotic.BeginInvoke(responsible, null, null);
            }
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="message">消息内容</param>
        public override void OnMessageNotice(MsgType msgType, string message)
        {
            if (messageNotice != null)
            {
                messageNotice.BeginInvoke(DataResponsible, new MessageEventArgs(msgType, message), null, null);
            }
        }

        /// <summary>
        /// 未知消息通知
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public override void OnUnknownMessageNotice(string msgType, string message)
        {
            if (unknownMessageNotice != null)
            {
                unknownMessageNotice.BeginInvoke(DataResponsible, new UnknownMessageEventArgs(msgType, message), null, null);
            }
        }

        public override void AppSelfMessageNotic(object sender, SelfMessageEventArgs e)
        {
            if (selfMessageNotic != null)
            {
                selfMessageNotic.BeginInvoke(sender, e, null, null);
            }
        }
    }
}
