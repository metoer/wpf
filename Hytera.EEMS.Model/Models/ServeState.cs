using Hytera.EEMS.Common;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// PC状态
    /// </summary>
    public class PcState
    {
        /// <summary>
        /// 采集站ID
        /// </summary>
        public string StationID 
        {
            get;
            set;
        }

        /// <summary>
        /// 采集站设备序列号
        /// </summary>
        public string StationCode
        {
            get;
            set;
        }

        /// <summary>
        /// 采集存储路径
        /// </summary>
        public string MemoryPath
        {
            get;
            set;
        }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIp
        {
            get;
            set;
        }

        /// <summary>
        /// 服务器连接状态
        /// </summary>
        public string ServerState
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库连接状态
        /// </summary>
        public string DataBaseState
        {
            get;
            set;
        }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string DataBaseMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string ServerMsg
        {
            get;
            set;
        }
    }
}
