using Hytera.EEMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// App设置信息
    /// </summary>
    public class AppStateInfos : NotifyPropertyChangedBase
    {
        private int faceplateRow = 4;

        private int faceplateColumn = 5;

        // 0表示连接，1表示未连接
        private string serverState = "1";

        private string dataBaseState = "1";

        private string localIp = string.Empty;
        
        private int searchpagecount = 20;
        private int searchtimeout = 300;
        /// <summary>
        /// 面板行数
        /// </summary>
        public int FaceplateRow
        {
            get
            {
                return faceplateRow;
            }
            set
            {
                faceplateRow = value;
                OnPropertyChanged("FaceplateRow");
            }
        }

        /// <summary>
        /// 面板列数
        /// </summary>
        public int FaceplateColumn
        {
            get
            {
                return faceplateColumn;
            }
            set
            {
                faceplateColumn = value;
                OnPropertyChanged("FaceplateColumn");
            }
        }

        /// <summary>
        /// 采集存储路径
        /// </summary>
        public string MemoryPath
        {
            get;
            set;
        }

        private string serverIp = string.Empty;

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIp
        {
            get
            {
                return serverIp;
            }
            set
            {
                serverIp = value;
                OnPropertyChanged("ServerIp");
            }
        }

        /// <summary>
        /// 本机IP
        /// </summary>
        public string LocalIp
        {
            get
            {
                return localIp;
            }
            set
            {
                localIp = value;
                OnPropertyChanged("LocalIp");
            }
        }

        /// <summary>
        /// 服务器连接状态
        /// </summary>
        public string ServerState
        {
            get
            {
                return serverState;
            }
            set
            {
                serverState = value;
                OnPropertyChanged("ServerState");
            }
        }

        /// <summary>
        /// 数据库连接状态
        /// </summary>
        public string DataBaseState
        {
            get
            {
                return dataBaseState;
            }
            set
            {
                dataBaseState = value;
                OnPropertyChanged("DataBaseState");
            }
        }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string DataBaseMsgCode
        {
            get;
            set;
        }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string ServerMsgCode
        {
            get;
            set;
        }

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
        /// 定时提示未取消的告警时间（单位:秒)
        /// </summary>
        public int WarnCanelTime
        {
            get;
            set;
        }
        
        public int SearchPageCount
        {
            get
            {
                return searchpagecount;
            }
            set
            {
                searchpagecount = value;
            }
        }

        public int SearchTimeOut
        {
            get
            {
                return searchtimeout;
            }
            set
            {
                searchtimeout = value;
            }
        }

        public string TakeSnapshotPath
        {
            get;
            set;
        }
    }
}
