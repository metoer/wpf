using Hytera.EEMS.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace IRPC
{
    /// <summary>
    /// 接收ICE接口消息
    /// </summary>
    public class CallbackClientI : CallbackClientDisp_
    {
        public event Action<string> TransDataToClientHandler;

        /// <summary>
        /// 消息队列
        /// </summary>
        private Queue<string> msgs = new Queue<string>();

        public CallbackClientI()
        {
            new Thread(() =>
            {
                DealMsg();
            }) { IsBackground = true }.Start();
        }

        /// <summary>
        /// 接收ICE消息
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override int TransDataToClient(string strXml, Ice.Current current__)
        {
            lock (((ICollection)msgs).SyncRoot)
            {
                msgs.Enqueue(strXml);
            }

            return 1;
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        private void DealMsg()
        {
            while (true)
            {
                try
                {
                    if (TransDataToClientHandler != null)
                    {
                        lock (((ICollection)msgs).SyncRoot)
                        {
                            if (msgs.Count > 0)
                            {
                                string msg = msgs.Dequeue();

                                TransDataToClientHandler.BeginInvoke(msg, null, null);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Instance.WirteErrorMsg(e.Message);
                }
                finally
                {
                    Thread.Sleep(5);
                }
            }
        }

        public override void TransHeartbeat(long lTime, Ice.Current current__)
        {

        }
    }
}
