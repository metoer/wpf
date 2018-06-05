using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    [XmlRoot("CameraLog")]
    public class CameraLogs
    {
        public CameraLogs()
        {
            CameraLogInfoList = new List<CameraLogInfo>();
        }

        [XmlAttribute("PageIndex")]
        public string PageIndex
        {
            get;
            set;
        }

        [XmlElement("LogInfo")]
        public List<CameraLogInfo> CameraLogInfoList
        {
            get;
            set;
        }
    }

    public class CameraLogInfo : INotifyPropertyChanged
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
        /// 采集站设备序列号
        /// </summary>
        public string StationCode
        {
            get;
            set;
        }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID
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

        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollectTime
        {
            get;
            set;
        }

        /// <summary>
        /// 采集站Ip
        /// </summary>
        public string CollectIp
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

        public void ValueCloneToObject(CameraLogInfo objectCameraLogInfo)
        {
            objectCameraLogInfo.CollectIp = this.CollectIp;
            objectCameraLogInfo.CollectTime = this.CollectTime;
            objectCameraLogInfo.DeviceID = this.DeviceID;
            objectCameraLogInfo.FileName = this.FileName;
            objectCameraLogInfo.LogID = this.LogID;
            objectCameraLogInfo.OpDescription = this.OpDescription;
            objectCameraLogInfo.OpTime = this.OpTime;
            objectCameraLogInfo.OpType = this.OpType;
            objectCameraLogInfo.OperatorOrgID = this.OperatorOrgID;
            objectCameraLogInfo.OperatorOrgName = this.OperatorOrgName;
            objectCameraLogInfo.OperatorCode = this.OperatorCode;
            objectCameraLogInfo.OperatorID = this.OperatorID;
            objectCameraLogInfo.OperatorName = this.OperatorName;
            objectCameraLogInfo.StationCode = this.StationCode;
            objectCameraLogInfo.OperatedCode = this.OperatedCode;
            objectCameraLogInfo.OperatedGuid = this.OperatedGuid;
            objectCameraLogInfo.OperatedID = this.OperatedID;
            objectCameraLogInfo.OperatedName = this.OperatedName;
            objectCameraLogInfo.OperatedOrgID = this.OperatedOrgID;
            objectCameraLogInfo.OperatedOrgName = this.OperatedOrgName;
        }
    }

    /// <summary>
    /// 执法仪日志查询条件
    /// </summary>
    public class CameraLogsSerach
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
    /// 执法仪日志总数
    /// </summary>
    [XmlRoot("DataCount")]
    public class CameraLogsCount
    {
        [XmlAttribute("Count")]
        public string DataCount
        {
            get;
            set;
        }
    }
}
