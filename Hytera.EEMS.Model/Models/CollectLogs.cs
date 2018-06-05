using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 采集站操作日志
    /// </summary>
    [XmlRoot("CollectLog")]
    public class CollectLogs
    {
        public CollectLogs()
        {
            CollectLogList = new List<CollectLogInfo>();
        }

        [XmlAttribute("PageIndex")]
        public string PageIndex
        {
            get;
            set;
        }

        [XmlElement("LogInfo")]
        public List<CollectLogInfo> CollectLogList
        {
            get;
            set;
        }
    }

    public class CollectLogInfo : INotifyPropertyChanged
    {

        public Int32 SequenceNum
        {
            get;
            set;
        }
        /// <summary>
        /// 日志ID
        /// </summary>
        public string LogID
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
        /// 采集站Ip
        /// </summary>
        public string StationIp
        {
            get;
            set;
        }

        /// <summary>
        /// 使用者ID
        /// </summary>
        public string OperatorID
        {
            get;
            set;
        }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }

        /// <summary>
        /// 使用者编号
        /// </summary>
        public string OperatorCode
        {
            get;
            set;
        }

        public string OperatorGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 使用者部门ID
        /// </summary>
        public string OperatorOrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 使用者部门名称
        /// </summary>
        public string OperatorOrgName
        {
            get;
            set;
        }

        /// <summary>
        /// 日志时间
        /// </summary>
        public string OpTime
        {
            get;
            set;
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OpType
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OpDescription
        {
            get;
            set;
        }
        
        /// <summary>
        /// 执法记录仪使用者ID
        /// </summary>
        public string OperatedID
        {
            get;
            set;
        }

        /// <summary>
        /// 执法记录仪使用者名称
        /// </summary>
        public string OperatedName
        {
            get;
            set;
        }

        /// <summary>
        /// 执法记录仪使用者编号
        /// </summary>
        public string OperatedCode
        {
            get;
            set;
        }

        public string OperatedGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 执法记录仪使用者部门
        /// </summary>
        public string OperatedOrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 执法记录仪使用者部门名称
        /// </summary>
        public string OperatedOrgName
        {
            get;
            set;
        }

        public string OpObjectType
        {
            get;
            set;
        }

        public string ObjectID
        {
            get;
            set;
        }

        public string UploadTime
        {
            get;
            set;
        }

        public string UploadProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 上传进度
        /// </summary>
        public string Progress
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

        private string _upLoadState;
        /// <summary>
        /// 是否已上传
        /// </summary>
        public string UpLoadState
        {
            get { return _upLoadState; }
            set
            {
                this._upLoadState = value;
                this.OnPropertyChanged("UpLoadState");
            }
        }

        private bool _isCheched;
        public bool IsChecked
        {
            get { return _isCheched; }
            set
            {
                this._isCheched = value;
                this.OnPropertyChanged("IsChecked");
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

        public void ValueCloneToObject(CollectLogInfo objectCollectLogInfo)
        {
            objectCollectLogInfo.OperatedOrgID = this.OperatedOrgID;
            objectCollectLogInfo.OperatedOrgName = this.OperatedOrgName;
            objectCollectLogInfo.OperatedGuid = this.OperatedGuid;
            objectCollectLogInfo.OperatedID = this.OperatedID;
            objectCollectLogInfo.OperatorCode = this.OperatorCode;
            objectCollectLogInfo.OperatedName = this.OperatedName;
            objectCollectLogInfo.StationCode = this.StationCode;
            objectCollectLogInfo.FileName = this.FileName;
            objectCollectLogInfo.LogID = this.LogID;
            objectCollectLogInfo.OpDescription = this.OpDescription;
            objectCollectLogInfo.OpTime = this.OpTime;
            objectCollectLogInfo.OpType = this.FileName;
            objectCollectLogInfo.OperatorOrgID = this.OperatorOrgID;
            objectCollectLogInfo.OperatorOrgName = this.OperatorOrgName;
            objectCollectLogInfo.OperatorName = this.OperatorName;
            objectCollectLogInfo.OperatorGuid = this.OperatorGuid;
            objectCollectLogInfo.OperatorID = this.OperatorID;
            objectCollectLogInfo.StationID = this.StationID;
            objectCollectLogInfo.StationIp = this.StationIp;
            objectCollectLogInfo.OpObjectType = this.OpObjectType;
            objectCollectLogInfo.UploadProcess = this.UploadProcess;
            objectCollectLogInfo.UploadTime = this.UploadTime;
            objectCollectLogInfo.UpLoadState = this.UpLoadState;
        }
    }

    public enum OpType
    {
        Other = 0,
        Login = 1,
        Search = 2,
        Paly = 3,
        Collect = 4,
        UpLoad = 5
    }

    /// <summary>
    /// 采集站日志查询条件
    /// </summary>
    public class CollectLogsSerach
    {
        /// <summary>
        /// 警号
        /// </summary>
        public string UserGuid
        {
            get;
            set;
        }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string OrgID
        {
            get;
            set;
        }

        public string OrgName
        {
            get;
            set;
        }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceID
        {
            get;
            set;
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType
        {
            get;
            set;
        }

        public string UploadState
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string CollectStartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string CollectEndTime
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
    /// 采集站日志总数
    /// </summary>
    [XmlRoot("DataCount")]
    public class CollectLogsCount
    {
        [XmlAttribute("Count")]
        public string DataCount
        {
            get;
            set;
        }
    }
}
