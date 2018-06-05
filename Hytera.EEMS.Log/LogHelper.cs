using IRPC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Hytera.EEMS.Log
{
    /// <summary>
    /// 写日志类
    /// </summary>
    public class LogHelper : IDisposable
    {
        private static LogHelper instance;

        /// <summary>
        /// ICE对象
        /// </summary>
        private Ice.Communicator ic;

        /// <summary>
        /// 数据发送
        /// </summary>
        private LogServer_RPCPrx client;

        private string severID = "StationClient";

        private string pluginName = "Client";

        private Queue<LogContent> logMsg = new Queue<LogContent>();

        private bool isDis = false;

        private LogHelper()
        { }

        /// <summary>
        /// 单实例
        /// </summary>
        public static LogHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    Interlocked.CompareExchange(ref instance, new LogHelper(), null);
                }

                return instance;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                ic = Ice.Util.initialize();

                // 连接
                Ice.ObjectPrx objectPrx = ic.stringToProxy(strLogPluginRPCId.value + 
                                                            string.Format(":tcp -h {0} -p {1} -t 5000",
                                                            EEMSConfigHelper.GetValueByCommomConfig("config/CommonConfig/log_target_ip", "127.0.0.1"),
                                                             EEMSConfigHelper.GetValueByCommomConfig("config/CommonConfig/log_target_port", "40040")));

                client = LogServer_RPCPrxHelper.checkedCast(objectPrx);

                WirterContent();
                return true;
            }
            catch
            {
                return false;

            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logLevel"></param>
        public void WirteLog(string msg, LogLevel logLevel)
        {
            AddLogMsg(msg, logLevel);
        }

        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="msg"></param>
        public void WirteInfoMsg(string msg)
        {
            AddLogMsg(msg, LogLevel.LogInfo);
        }

        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="msg"></param>
        public void WirteWarnMsg(string msg)
        {
            AddLogMsg(msg, LogLevel.LogWran);
        }

        /// <summary>
        /// 写Error日志
        /// </summary>
        public void WirteErrorMsg(string msg)
        {
            AddLogMsg(msg, LogLevel.LogError);
        }

        private void AddLogMsg(string msg, LogLevel logLevel)
        {
            lock (((ICollection)logMsg).SyncRoot)
            {
                if (logMsg.Count > 50)
                {
                    logMsg.Dequeue();
                }

                logMsg.Enqueue(new LogContent(msg, logLevel));
            }
        }

        /// <summary>
        /// 启动日志
        /// </summary>

        private void WirterContent()
        {         
            new Thread(() =>
            {
                while (!isDis)
                {
                    try
                    {
                        if (client == null || logMsg.Count == 0)
                        {                         
                            continue;
                        }

                        LogContent logContent = null;
                        lock (((ICollection)logMsg).SyncRoot)
                        {
                            logContent = logMsg.Dequeue();
                        }

                        client.WriteLog(severID, pluginName, logContent.Msg, (int)logContent.LogLevel);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        Thread.Sleep(10);
                    }
                }
            }) { IsBackground = true }.Start();

        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            isDis = true;
            client = null;
            ic.shutdown();
            ic.Dispose();
            ic = null;
        }
    }

    public class LogContent
    {
        public LogContent(string msg, LogLevel logLevel)
        {
            Msg = msg;
            LogLevel = logLevel;
        }

        public string Msg
        {
            get;
            set;
        }

        public LogLevel LogLevel
        {
            get;
            set;
        }
    }
}
