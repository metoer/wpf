using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 采集操作结果
    /// </summary>
    public class DeviceCollectInfo
    {
        public int ResultCode
        {
            get;
            set;
        }

        public string DeviceID
        {
            get;
            set;
        }

        public string ResultMsg
        {
            get;
            set;
        }
    }
}
