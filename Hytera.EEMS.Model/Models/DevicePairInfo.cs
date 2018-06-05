using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model
{
    public class DevicePairInfo
    {
        /// <summary>
        /// 执法记录仪Code
        /// </summary>
        public string DeviceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 配对结果
        /// </summary>
        public int ResultCode
        {
            get;
            set;
        }

        /// <summary>
        /// 配对消息
        /// </summary>
        public string ResultMsg
        {
            get;
            set;
        }
        
        /// <summary>
        /// 执法记录仪ID
        /// </summary>

        public string DeviceID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserCode
        {
            get;
            set;
        }

        /// <summary>
        /// 用人单位ID
        /// </summary>
        public string OrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 用人单位名
        /// </summary>
        public string OrgName
        {
            get;
            set;
        }

        /// <summary>
        /// 配对时间
        /// </summary>
        public string MatchTime
        {
            get;
            set;
        }
    }
}
