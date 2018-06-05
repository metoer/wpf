
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace Hytera.EEMS.Dispatcher
{
    /// <summary>
    /// 模块入口
    /// </summary>
    public abstract class ModuleBaseEntry : IComparable<ModuleBaseEntry>
    {

        public event ShowViewNoticeHandler ShowViewNotice;

        internal DelegateAppSelfMessageNotic SelfMessageNotic
        {
            get;
            set;
        }

        /// <summary>
        /// 模块导航
        /// </summary>
        public ControlNavigable ModuleNavigable
        {
            get;
            protected set;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string ModuleCode
        {
            get;
            private set;
        }

        public ModuleBaseEntry()
        {
            ModuleCode = Guid.NewGuid().ToString("N");
        }


        public abstract void Init(DataResponsible responsible);

        /// <summary>
        /// 显示加载模块视图
        /// </summary>
        public virtual void ShowModelView(Frame parentFrame, Window parentWindow)
        {

        }

        /// <summary>
        /// 网络连接断开
        /// </summary>
        public virtual void DisConnectNetWork()
        {           
        }

        /// <summary>
        /// 连接上网络
        /// </summary>
        public virtual void ConnectNetWork(DataResponsible responsible)
        { 
        
        
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public abstract void OnMessageNotice(MsgType msgType, string message);

        /// <summary>
        /// 未知消息通知
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public abstract void OnUnknownMessageNotice(string msgType, string message);

        /// <summary>
        /// 内部通知
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public virtual void AppSelfMessageNotic(object sender, SelfMessageEventArgs e)
        { }

        /// <summary>
        /// 发送内部消息
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public void SendMsgToSelf(AppSelfMsgType msgType, string message)
        {
            if (SelfMessageNotic != null)
            {
                SelfMessageNotic(this, new SelfMessageEventArgs(msgType, message));
            }
        }

        /// <summary>
        /// 通知显示自己模块
        /// </summary>
        public void ShowSelfViewNotice()
        {
            if (ShowViewNotice != null)
            {
                ShowViewNotice(this);
            }
        }

        public int CompareTo(ModuleBaseEntry other)
        {
            if (object.Equals(other, null))
            {
                return 1;
            }

            if (this.ModuleNavigable.Index < other.ModuleNavigable.Index)
            {
                return -1;
            }
            else
            {
                return 1;
            }

        }
    }
}
