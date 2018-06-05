using System;

namespace Hytera.EEMS.Dispatcher
{
    /// <summary>
    /// 需要接收消息的类继承此抽象类
    /// </summary>
    public abstract class MessageDispatch : IDisposable
    {
        #region 属性、构造

        public MessageDispatch()
        {
            EventManager.Instance.MessageNotice += MessageNotice;
            EventManager.Instance.UnknownMessageNotice += UnknownMessageNotice;
            EventManager.Instance.SelfMessageNotic += AppSelfMessageNotic;
            EventManager.Instance.ConnectNetWorkNotic += OnConnectNetWorkNotic;
            Init(Responsible);
        }

        public DataResponsible Responsible
        {
            get
            {
                return EventManager.Instance.DataResponsible;
            }
        }
        #endregion

        #region 重写方式通知
        /// <summary>
        /// 处理一只类型消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageNotice(object sender, MessageEventArgs e)
        {
            OnMessageNotice(sender, e);
        }

        private void UnknownMessageNotice(object sender, UnknownMessageEventArgs e)
        {
            OnUnknownMessageNotice(sender, e);
        }

        private void AppSelfMessageNotic(object sender, SelfMessageEventArgs e)
        {
            OnAppSelfMessageNotic(sender, e);
        }


        protected abstract void Init(DataResponsible responsible);

        protected abstract void OnUnknownMessageNotice(object sender, UnknownMessageEventArgs e);

        protected abstract void OnMessageNotice(object sender, MessageEventArgs e);

        protected virtual void OnAppSelfMessageNotic(object sender, SelfMessageEventArgs e)
        { }

        protected virtual void OnConnectNetWorkNotic(DataResponsible responsible)
        { }


        public void Dispose()
        {
            EventManager.Instance.MessageNotice -= OnMessageNotice;
            EventManager.Instance.UnknownMessageNotice -= OnUnknownMessageNotice;
            EventManager.Instance.SelfMessageNotic -= AppSelfMessageNotic;
            EventManager.Instance.ConnectNetWorkNotic -= OnConnectNetWorkNotic;
        }
        #endregion
    }
}
