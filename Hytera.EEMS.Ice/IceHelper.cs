using Hytera.EEMS.Log;
using System;
using System.Threading;

namespace IRPC
{
    public class IceHelper : IDisposable
    {

        /// <summary>
        /// ICE对象
        /// </summary>
        private Ice.Communicator ic;

        /// <summary>
        /// 数据发送
        /// </summary>
        public CSTransDataFromClientPrx Client
        {
            get;
            private set;
        }

        public string ClientID
        {
            get;
            set;
        }

        public event Action<string> ReceiveDataHandler;

        private static IceHelper instance;


        /// <summary>
        /// 单实例
        /// </summary>
        public static IceHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    Interlocked.CompareExchange(ref instance, new IceHelper(), null);
                }

                return instance;
            }
        }

        private IceHelper()
        {
            ClientID = "client";
        }

        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                Ice.InitializationData intiData = new Ice.InitializationData();

                Ice.Properties properties = Ice.Util.createProperties();

                properties.setProperty("Ice.MessageSizeMax", "104857600");
                properties.setProperty("Ice.ThreadPool.SizeMax", "100");
                intiData.properties = properties;

                ic = Ice.Util.initialize(intiData);

                // 连接
                Ice.ObjectPrx objectPrx = ic.stringToProxy(string.Format("ID_TransDataFromClientRPC:tcp -h {0} -p {1} -t 5000",
                                                                     EEMSConfigHelper.GetValueByCommomConfig("config/CollectClient/DataServerIp", "127.0.0.1"),
                                                                      EEMSConfigHelper.GetValueByCommomConfig("config/CollectClient/DataServerPort", "40010")));

                Client = CSTransDataFromClientPrxHelper.checkedCast(objectPrx);
                // 代理
                Ice.ObjectAdapter adapter = ic.createObjectAdapterWithEndpoints("ID_TransDataFromClientRPC", string.Format("tcp -h {0} -p {1} -t 5000",
                       EEMSConfigHelper.GetValueByCommomConfig("config/CollectClient/DataClientIp", "127.0.0.1"),
                         EEMSConfigHelper.GetValueByCommomConfig("config/CollectClient/DataClientPort", "42000")));
               
                CallbackClientI callbackClientI = new CallbackClientI();

                callbackClientI.TransDataToClientHandler += callbackClientI_TransDataToClientHandler;

                Ice.Object obj = callbackClientI;

                adapter.add(obj, ic.stringToIdentity("ID_TransDataFromClientRPC"));

                adapter.activate();

                CallbackClientPrx call = CallbackClientPrxHelper.uncheckedCast(adapter.createProxy(ic.stringToIdentity("ID_TransDataFromClientRPC")));
             
                Client.RegistClient(ClientID, call, 1000);
                
                return true;
            }
            catch (Exception e)
            {
                // 初始化失败
                Dispose();
                LogHelper.Instance.WirteErrorMsg(e.Message);
                return false;
            }
        }

        /// <summary>
        /// ICE接口消息
        /// </summary>
        /// <param name="obj"></param>
        private void callbackClientI_TransDataToClientHandler(string obj)
        {
            if (ReceiveDataHandler != null)
            {
                ReceiveDataHandler.BeginInvoke(obj, null, null);
            }
        }

        public void Dispose()
        {
            try
            {
                Client = null;
                if (ic != null)
                {
                    ic.shutdown();
                    ic.Dispose();
                    ic = null;
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }
    }
}
