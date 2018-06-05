using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 告警信息类型
    /// </summary>
    public enum AlarmType
    {
        alert,
        alertcancel
    }

    public class AlertInfo
    {
        public string AlertCode
        {
            get;
            set;
        }

        public string AlertMessage
        {
            get;
            set;
        }

        public string Level
        {
            get;
            set;
        }

        /// <summary>
        /// 暂不处理稍后提示
        /// </summary>
        public bool IsMute
        {
            get;
            set;
        }
    }
}
