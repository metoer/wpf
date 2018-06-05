using Hytera.EEMS.Common;
using Hytera.EEMS.Ice;
using Hytera.EEMS.Log;
using Hytera.EEMS.Model;
using IRPC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Hytera.EEMS.Dispatcher
{
    /// <summary>
    /// 数据发送到ICE接口
    /// </summary>
    public class DataResponsible : IDisposable
    {
        bool isSend = true;

        /// <summary>
        /// 待发送消息
        /// </summary>
        Queue<MessagePackage> msgList = new Queue<MessagePackage>();

        public DataResponsible()
        {
            SendMsgThread();
            HeartbeatThread();
        }

        /// <summary>
        /// 发送线程
        /// </summary>
        private void SendMsgThread()
        {
            new Thread(() =>
            {
                while (isSend)
                {
                    try
                    {
                        MessagePackage package = null;
                        if (msgList.Count > 0)
                        {

                            lock (((ICollection)msgList).SyncRoot)
                            {
                                package = msgList.Dequeue();
                            }
                        }

                        if (IceHelper.Instance.Client != null && package != null)
                        {
                            IceHelper.Instance.Client.TransDataFromClient(IceHelper.Instance.ClientID, GetMsgPackage(package));
                        }
                    }
                    catch { }
                    finally
                    {
                        Thread.Sleep(5);
                    }
                }
            }) { IsBackground = true }.Start();
        }

        /// <summary>
        /// 10秒钟发一条心跳包
        /// </summary>
        private void HeartbeatThread()
        {
            new Thread(() =>
            {
                while (isSend)
                {
                    if (AppConfigInfos.IceConnect)
                    {
                        SendHeartbeat();
                    }

                    Thread.Sleep(10000);
                }

            }) { IsBackground = true }.Start();
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        public void SendHeartbeat()
        {
            Conditions con = new Conditions();
            con.AddItem("ClientID", IceHelper.Instance.ClientID);
            AddMsg(MsgType.Heartbeat, con);
        }


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void SendMsg(MsgType msgType, object data)
        {
            AddMsg(msgType, data);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public void SendCommand(MsgType msgType)
        {
            AddMsg(msgType, null);
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void SendMsg(string msgType, object data)
        {
            AddMsg(msgType, data);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="data"></param>
        private void AddMsg(object msgType, object data)
        {
            lock (((ICollection)msgList).SyncRoot)
            {
                if (msgList.Count > 100)
                {
                    msgList.Dequeue();
                }

                msgList.Enqueue(new MessagePackage(msgType, data));
            }
        }

        /// <summary>
        /// 组装数据包
        /// </summary>
        /// <param name="MsgType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetMsgPackage(MessagePackage package)
        {
            string value = @"<hytera>
                                     <product_name>{0}</product_name>
                                     <version>{1}</version>
                                     <objectid>{2}</objectid>
                                     <objecttype>{3}</objecttype>
                                     <cmd_guid>{4}</cmd_guid>
                                     <cmd_name>{5}</cmd_name>
                                     <cmd_time>{6}</cmd_time>
                                     <cmd_content>{7}</cmd_content>
                             </hytera> ";

            value = string.Format(
                                    value,
                                    "EEMS",
                                    "1.0",
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    package.MsgType,
                                    DateTime.Now.ToString("yyyyMMdd HHmmss"),
                                    package.Data == null || package.Data.ToString() == "" ? string.Empty : XmlUnityConvert.XmlSerialize(package.Data)
                                    );

            LogHelper.Instance.WirteLog("SendMessage:" + value, LogLevel.LogDebug);
            return value;
        }

        /// <summary>
        /// 特殊字符转义
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string SpecialCharTransferred(string value)
        {
            value = value.Replace("&", "&amp;");
            value = value.Replace("<", "&lt;");
            value = value.Replace(">", "&gt;");
            value = value.Replace("'", "&apos;");
            value = value.Replace("\"", "&quot;");

            return value;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            isSend = false;
            lock (((ICollection)msgList).SyncRoot)
            {
                msgList.Clear();
            }
        }

        /// <summary>
        /// 正常退出
        /// </summary>
        public void Exit()
        {
            while (msgList.Count > 0 && AppConfigInfos.IceConnect)
            {
                Thread.Sleep(100);
            }

            isSend = false;
        }
    }
}
