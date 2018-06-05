using Hytera.EEMS.Log;
using IRPC;
using System;
using System.Threading;

namespace Hytera.EEMS.WarnInfo
{
    public class WarnHelper : IDisposable
    {
        public event Action<string> ReceiveDataHandler;

        /// <summary>
        /// ICE对象
        /// </summary>
        private Ice.Communicator ic;

        /// <summary>
        /// 数据发送
        /// </summary>
        private NMServerPrx client;

        string clientID = Guid.NewGuid().ToString("N");

        private static WarnHelper instance;

        private object lockData = new object();

        bool isThreadStart = false;

        private WarnHelper()
        {
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnect
        {
            get;
            private set;
        }

        /// <summary>
        /// 单实例
        /// </summary>
        public static WarnHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    Interlocked.CompareExchange(ref instance, new WarnHelper(), null);
                }

                return instance;
            }
        }

        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                lock (lockData)
                {
                    Ice.InitializationData intiData = new Ice.InitializationData();

                    Ice.Properties properties = Ice.Util.createProperties();

                    properties.setProperty("Ice.MessageSizeMax", "104857600");

                    intiData.properties = properties;

                    ic = Ice.Util.initialize(intiData);

                    // 连接
                    Ice.ObjectPrx objectPrx = ic.stringToProxy(string.Format("{0}:tcp -h {1} -p {2} -t 5000", NMCommunicationRPCId.value,
                                                                 EEMSConfigHelper.GetValueByCommomConfig("config/CommonConfig/netagent_ip", "127.0.0.1"),
                                                                 EEMSConfigHelper.GetValueByCommomConfig("config/CommonConfig/netagent_port", "40050")));

                    client = NMServerPrxHelper.checkedCast(objectPrx);

                    // 代理
                    Ice.ObjectAdapter adapter = ic.createObjectAdapterWithEndpoints(NMCommunicationRPCId.value, string.Format("tcp -h {0} -p {1} -t 5000",
                                                      EEMSConfigHelper.GetValueByCommomConfig("config/CollectClient/WarnInfoLocalIp", "127.0.0.1"),
                                                      EEMSConfigHelper.GetValueByCommomConfig("config/CollectClient/WarnInfoClentPort", "42050")));

                    NMClientI clientI = new NMClientI();

                    clientI.ReceiveDataHandler += clientI_ReceiveDataHandler;

                    Ice.Object obj = clientI;

                    adapter.add(obj, ic.stringToIdentity(NMCommunicationRPCId.value));

                    adapter.activate();

                    NMClientPrx call = NMClientPrxHelper.uncheckedCast(adapter.createProxy(ic.stringToIdentity(NMCommunicationRPCId.value)));

                    client.NMRegistClient(clientID, call);

                    IsConnect = true;
                }

                return true;
            }
            catch (Exception e)
            {
                // 初始化失败
                client = null;
                LogHelper.Instance.WirteErrorMsg(e.Message);
                return false;
            }
            finally
            {
                if (!isThreadStart)
                {
                    isThreadStart = true;
                    CheckConnectThread();
                }
            }
        }

        /// <summary>
        /// 重复检测连接
        /// </summary>
        private void CheckConnectThread()
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (!IsConnect)
                        {
                            Dispose();
                            Init();
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Instance.WirteErrorMsg(e.Message);
                    }
                    finally
                    {
                        Thread.Sleep(5000);
                    }
                }
            }) { IsBackground = true }.Start();
        }

        private void clientI_ReceiveDataHandler(string obj)
        {
            if (ReceiveDataHandler != null)
            {
                ReceiveDataHandler(obj);
            }
        }

        public void Dispose()
        {
            try
            {
                if (ic != null)
                {
                    ic.shutdown();
                    ic.Dispose();
                    ic = null;
                }

                client = null;
            }
            catch
            {

            }
        }
    }
}
