using Hytera.EEMS.Log;
using Hytera.EEMS.Model;
using IRPC;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Hytera.EEMS.Dispatcher
{
    /// <summary>
    /// 模块适配对象
    /// </summary>
    public class ModuleDispather
    {
        #region 变量、属性
        private long lastTick;

        bool isEnd = false;

        private static ModuleDispather instance;

        /// <summary>
        /// 数据发送接口对象
        /// </summary>
        private DataResponsible dataResponsible;

        /// <summary>
        /// 模块
        /// </summary>
        public List<ModuleBaseEntry> Modules
        {
            get;
            private set;
        }

        /// <summary>
        /// 单实例
        /// </summary>
        public static ModuleDispather Instance
        {
            get
            {
                if (instance == null)
                {
                    Interlocked.CompareExchange(ref instance, new ModuleDispather(), null);
                }

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 默认构造方法
        /// </summary>
        private ModuleDispather()
        {
            dataResponsible = new DataResponsible();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // 加载模块
            DelegateAppSelfMessageNotic delegateAction = ModulesSelfMessageNotic;

            Modules = LoadModule.LoadPlugModule(AppDomain.CurrentDomain.BaseDirectory + "\\Plugs", delegateAction);

            AppConfigInfos.IceConnect = IceHelper.Instance.Init();

            IceHelper.Instance.ReceiveDataHandler += Instance_ReceiveDataHandler;

            Modules.ForEach(p => p.Init(dataResponsible));

            Modules.ForEach(p => p.ConnectNetWork(dataResponsible));

            EventManager.Instance.Init(dataResponsible);

           // EventManager.Instance.ConnectNetWork(dataResponsible);

            lastTick = DateTime.Now.Ticks;

            CheckHeartbeat();
        }

        /// <summary>
        /// 模块间信息转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ModulesSelfMessageNotic(object sender, SelfMessageEventArgs e)
        {
            ModuleBaseEntry moduleBase = sender as ModuleBaseEntry;
            List<ModuleBaseEntry> moduleList = Modules.FindAll(p => !p.ModuleCode.Equals(moduleBase.ModuleCode));
            if (moduleList.Count > 0)
            {
                moduleList.ForEach(p => p.AppSelfMessageNotic(sender, e));
            }

            if (!EventManager.Instance.ModuleCode.Equals(moduleBase.ModuleCode))
            {
                EventManager.Instance.AppSelfMessageNotic(sender, e);
            }
        }

        /// <summary>
        /// 解析信息
        /// </summary>
        /// <param name="obj"></param>
        private void Instance_ReceiveDataHandler(string obj)
        {
            LogHelper.Instance.WirteLog("ReceiveMessage:" + obj, LogLevel.LogDebug);

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(obj);

                // 消息类型
                MsgType msgType = MsgType.AccountValidate;

                // 消息内容
                string content = xmlDoc.SelectSingleNode("hytera/cmd_content").InnerXml;

                string typeValue = xmlDoc.SelectSingleNode("hytera/cmd_name").InnerXml;

                bool enumConver = Enum.TryParse<MsgType>(typeValue, out msgType);

                if (msgType == MsgType.Heartbeat)
                {
                    // 心跳包
                    AnalyzeHeartbeat();
                    return;
                }

                if (enumConver)
                {
                    Modules.ForEach(p => p.OnMessageNotice(msgType, content));
                    EventManager.Instance.OnMessageNotice(msgType, content);
                }
                else
                {
                    Modules.ForEach(p => p.OnUnknownMessageNotice(typeValue, content));
                    EventManager.Instance.OnUnknownMessageNotice(typeValue, content);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg("Message XML Analyze Error :" + e.Message);
            }
        }

        /// <summary>
        /// 特殊字符转义
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string SpecialCharTransferred(string value)
        {
            value = value.Replace("&lt;", "<");
            value = value.Replace("&gt;", ">");
            value = value.Replace("&apos;", "'");
            value = value.Replace("&quot;", "\"");
            value = value.Replace("&amp;", "&");
            return value;
        }

        /// <summary>
        /// 检测心跳包,20秒钟检测
        /// </summary>
        private void CheckHeartbeat()
        {
            new Thread(() =>
            {
                while (!isEnd)
                {
                    Thread.Sleep(10000);
                    long currentTick = DateTime.Now.Ticks;
                    if (lastTick > 0 && currentTick - lastTick > 200000000)
                    {
                        if (AppConfigInfos.IceConnect)
                        {
                            IceHelper.Instance.Dispose();

                            Modules.ForEach(p => p.DisConnectNetWork());
                            EventManager.Instance.DisConnectNetWork();

                            LogHelper.Instance.WirteLog("Heartbeat Start Reconnection", LogLevel.LogDebug);
                        }

                        AppConfigInfos.IceConnect = IceHelper.Instance.Init();

                        // 更新状态
                        if (AppConfigInfos.IceConnect)
                        {
                            LogHelper.Instance.WirteLog("Heartbeat  Reconnection Succeed", LogLevel.LogDebug);

                            dataResponsible.SendHeartbeat();

                            Modules.ForEach(p => p.ConnectNetWork(dataResponsible));
                            EventManager.Instance.ConnectNetWork(dataResponsible);
                        }
                        else
                        {
                            LogHelper.Instance.WirteLog("Heartbeat  Reconnection Failed", LogLevel.LogDebug);
                            AppConfigInfos.AppStateInfos.ServerState = "1";
                            AppConfigInfos.AppStateInfos.DataBaseState = "1";
                            AppConfigInfos.AppStateInfos.ServerMsgCode = "1";
                            AppConfigInfos.AppStateInfos.DataBaseMsgCode = "1";
                        }
                    }
                }
            }) { IsBackground = true }.Start();
        }

        /// <summary>
        /// 解析心跳包
        /// </summary>
        private void AnalyzeHeartbeat()
        {
            lastTick = DateTime.Now.Ticks;
        }


        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            isEnd = true;
            dataResponsible.Exit();
            IceHelper.Instance.Dispose();
        }
    }
}
