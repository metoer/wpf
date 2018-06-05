using IRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.WarnInfo
{
    public class NMClientI : NMClientDisp_
    {
        public event Action<string> ReceiveDataHandler;

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override int SendData(string strXml, Ice.Current current__)
        {
            Hytera.EEMS.Log.LogHelper.Instance.WirteLog(string.Format("Receive Alarm or UnAlarm Message: {0}", strXml), Log.LogLevel.LogDebug);
            if (ReceiveDataHandler != null)
            {
                ReceiveDataHandler.BeginInvoke(strXml, null, null);
            }

            return 1;
        }
    }
}
