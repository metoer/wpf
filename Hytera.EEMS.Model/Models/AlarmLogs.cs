using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    [XmlRoot("AlarmLog")]
    public class AlarmLogs
    {
        public AlarmLogs()
        {
            AlarmInfoList = new List<AlarmInfo>();
        }

        [XmlAttribute("PageIndex")]
        public string PageIndex
        {
            get;
            set;
        }

        [XmlElement("AlarmInfo")]
        public List<AlarmInfo> AlarmInfoList
        {
            get;
            set;
        }
    }

    public class AlarmInfo : INotifyPropertyChanged
    {
        public Int32 SequenceNum
        {
            get;
            set;
        }
        /// <summary>
        /// 告警ID
        /// </summary>
        public string AlarmID
        {
            get;
            set;
        }

        /// <summary>
        /// 告警设备ID
        /// </summary>
        public string DeviceID
        {
            get;
            set;
        }

        /// <summary>
        /// 告警设备别名
        /// </summary>
        public string DeviceAlias
        {
            get;
            set;
        }

        /// <summary>
        /// 告警设备Ip
        /// </summary>
        public string DeviceIp
        {
            get;
            set;
        }

        /// <summary>
        /// 设备类别
        /// </summary>
        public string DeviceType
        {
            get;
            set;
        }

        /// <summary>
        /// 告警模块
        /// </summary>
        public string AlarmModule
        {
            get;
            set;
        }

        /// <summary>
        /// 告警内容
        /// </summary>
        public string AlarmMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 告警时间
        /// </summary>
        public string AlarmTime
        {
            get;
            set;
        }

        /// <summary>
        /// 告警代码
        /// </summary>
        public string AlarmCode
        {
            get;
            set;
        }

        /// <summary>
        /// 告警级别
        /// </summary>
        public string AlarmLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 告警状态
        /// </summary>
        public string AlarmStatus
        {
            get;
            set;
        }
        private Visibility vis = Visibility.Hidden;
        public Visibility Vis
        {
            get
            {
                return vis;
            }
            set
            {
                vis = value;
                OnPropertyChanged("Vis");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// 解决时间
        /// </summary>
        public string SolvedTime
        {
            get;
            set;
        }

        public void ValueCloneToObject(AlarmInfo objectAlarmLogInfo)
        {
            objectAlarmLogInfo.AlarmCode = this.AlarmCode;
            objectAlarmLogInfo.AlarmID = this.AlarmID;
            objectAlarmLogInfo.AlarmLevel = this.AlarmLevel;
            objectAlarmLogInfo.AlarmMessage = this.AlarmMessage;
            objectAlarmLogInfo.AlarmModule = this.AlarmModule;
            objectAlarmLogInfo.AlarmStatus = this.AlarmStatus;
            objectAlarmLogInfo.AlarmTime = this.AlarmTime;
            objectAlarmLogInfo.DeviceAlias = this.DeviceAlias;
            objectAlarmLogInfo.DeviceID = this.DeviceID;
            objectAlarmLogInfo.DeviceIp = this.DeviceIp;
            objectAlarmLogInfo.DeviceType = this.DeviceType;
            objectAlarmLogInfo.SolvedTime = this.SolvedTime;
        }
    }

    /// <summary>
    /// 告警查询条件
    /// </summary>
    public class AlarmLogsSearch
    {
        /// <summary>
        /// 告警级别
        /// </summary>
        public string AlarmLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 告警类型
        /// </summary>
        public string AlarmCode
        {
            get;
            set;
        }
        /// <summary>
        /// 告警模块
        /// </summary>
        public string AlarmModule
        {
            get;
            set;
        }
        /// <summary>
        /// 告警状态
        /// </summary>
        public string AlarmStatus
        {
            get;
            set;
        }

        public string AlarmIp
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string AlarmStartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string AlarmtEndTime
        {
            get;
            set;
        }

        public string SearchTime
        {
            get;
            set;
        }
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 页数量
        /// </summary>
        public int PageCount
        {
            get;
            set;
        }

        public bool IsAdvanced
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 告警总数
    /// </summary>
    [XmlRoot("DataCount")]
    public class AlarmLogsCount
    {
        [XmlAttribute("Count")]
        public string DataCount
        {
            get;
            set;
        }
    }
}
