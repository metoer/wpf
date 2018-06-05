using Hytera.EEMS.Common;
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Gather.Lib
{
    /// <summary>
    ///  采集模块的实体模型对象
    /// </summary>
    public class GatherViewModel
    {
        static GatherViewModel()
        {
            DeviveInfoList = new ThreadSafeObservable<DeviveInfo>();//{
            //  new DeviveInfo()
            //{ PortCode = "123", Volume="50", TotalVolume="100",  Electricity="60", DeviceID = "dsfsd", DeviceState = DeviceState.Default, IsMatchOrRegist = IsMatchOrRegist.UnRegister },
            //new DeviveInfo()
            //{ PortCode = "124", DeviceID = "sdfsd", DeviceCode="Dtsd56fsdfsd", DeviceState = DeviceState.Collecting,CollectProgress="50", IsMatchOrRegist = IsMatchOrRegist.Matched },
            // new DeviveInfo()
            //{ PortCode = "125", DeviceID = "sdfx",DeviceCode="Dtsd5d6fsdfsd", DeviceState = DeviceState.CollectFinish, IsMatchOrRegist = IsMatchOrRegist.Matched },
            //    new DeviveInfo() { PortCode = "126", DeviceID = "sdddfx", DeviceState = DeviceState.CollectFinish,CollectProgress="100", IsMatchOrRegist = IsMatchOrRegist.Registered },
            //  new DeviveInfo() { PortCode = "127", DeviceID = "cv", DeviceState = DeviceState.Default, IsMatchOrRegist = IsMatchOrRegist.Registered }};
        }

        /// <summary>
        /// 执法记录仪设备列表
        /// </summary>
        public static ThreadSafeObservable<DeviveInfo> DeviveInfoList
        {
            get;
            set;
        }


    }
}
