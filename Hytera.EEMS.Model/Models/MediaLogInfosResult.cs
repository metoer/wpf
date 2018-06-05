using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 执法记录信息列表
    /// </summary>
   [XmlRoot("MediaList")]
    public class MediaList
    {
        public MediaList()
        {
            MediaInfoList = new List<MediaInfo>();
        }

        /// <summary>
        /// 总数量
        /// </summary>
       [XmlAttribute("Count")]
        public string Count
        {
            get;
            set;
        }

        /// <summary>
        /// 页索引
        /// </summary>
        [XmlAttribute("PageIndex")]
        public string PageIndex
        {
            get;
            set;
        }

        [XmlElement("MediaInfo")]
        public List<MediaInfo> MediaInfoList
        {
            get;
            set;
        }
    }


    public class MediaInfo : INotifyPropertyChanged
    {
        public Int32 SequenceNum
        {
            get;
            set;
        }

        /// <summary>
        /// 记录ID
        /// </summary>
        public string RecordID
        {
            get;
            set;
        }

        /// <summary>
        /// 录制者ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

        public string UserGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 录制者姓名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 录制者编号
        /// </summary>
        public string UserCode
        {
            get;
            set;
        }

        public string ShowUserNameAndCode
        {
            get
            {
                if (UserCode.Length > 0)
                    return UserName + "(" + UserCode + ")";
                else
                    return UserName;
            }
        }
        /// <summary>
        /// 录制者部门ID
        /// </summary>
        public string OrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 录制者部门名称
        /// </summary>
        public string OrgName
        {
            get;
            set;
        }

        /// <summary>
        /// 录制设备ID
        /// </summary>
        public string DeviceID
        {
            get;
            set;
        }

        /// <summary>
        /// 录制时间
        /// </summary>
        public string RecordTime
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
        /// 采集站IP地址
        /// </summary>
        public string CollectIp
        {
            get;
            set;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string MediaType
        {
            get;
            set;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        private string _FileName;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set
            {
                this._FileName = value;
            }
        }

        private string _filePicPath;

        public string ShowName
        {
            get
            {
                return FileName.Split('.')[0];
            }
            set
            {
                this._filePicPath = value;
            }
        }
        /// <summary>
        /// 文件内容
        /// </summary>
        public string FileData
        {
            get;
            set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// 时长
        /// </summary>
        public string Duration
        {
            get;
            set;
        }

        public string ShowDuration
        {
            get
            {
                TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(Duration));
                if (MediaType != null && MediaType.Equals("3"))
                    return "";
                else
                    return ts.ToString();
            }
            set { }
        }
        /// <summary>
        /// 分辨率宽度
        /// </summary>
        public string ResolutionWith
        {
            get;
            set;
        }

        /// <summary>
        /// 分辨率高度
        /// </summary>
        public string ResolutionHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 编码类型
        /// </summary>
        public string EncodeType
        {
            get;
            set;
        }

        /// <summary>
        /// 加密类型
        /// </summary>
        public string EncryType
        {
            get;
            set;
        }

        /// <summary>
        /// 算法类型
        /// </summary>
        public string ArithType
        {
            get;
            set;
        }

        /// <summary>
        /// 执法是否是重点文件
        /// </summary>
        public string CameraImp
        {
            get;
            set;
        }

        private string _cameraTag;
        /// <summary>
        /// 用户标记，执法记录仪上标记的
        /// </summary>
        public string CameraTag
        {
            get { return this._cameraTag; }
            set
            {
                this._cameraTag = value;
                this.OnPropertyChanged("CameraTag");
            }
        }

        public string _UserImp;
        /// <summary>
        /// 用户是否是重点文件
        /// </summary>
        public string UserImp
        {
            get { return this._UserImp; }
            set
            {
                this._UserImp = value;
                this.OnPropertyChanged("UserImp");
            }
        }

        public ImageBrush Img
        {
            get;
            set;
        }

        public string _thumbnail;
        public string Thumbnail
        {
            get { return _thumbnail; }
            set
            {
                this._thumbnail = value;
                if (value != null && value.Length > 1)
                {
                    byte[] msgBytesp = Convert.FromBase64String(this._thumbnail);
                    BitmapImage bitmapp = new BitmapImage();
                    bitmapp.BeginInit();
                    bitmapp.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapp.StreamSource = new MemoryStream(msgBytesp);
                    bitmapp.EndInit();
                    Img = new ImageBrush() { ImageSource = bitmapp };
                }
            }
        }
        private string _userTag;
        /// <summary>
        /// 用户标记,采集站
        /// </summary>
        public string UserTag
        {
            get { return _userTag; }
            set
            {
                this._userTag = value;
                this.OnPropertyChanged("UserTag");
            }
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

        private float _playPosition;
        public float PlayPosition
        {
            get { return _playPosition; }
            set
            {
                this._playPosition = value;
                this._playPositionper = PlayPosition > 1 ? "100%" : Convert.ToUInt32((PlayPosition * 100)).ToString() + "%";
                this.OnPropertyChanged("PlayPositionPer");
                this.OnPropertyChanged("PlayPosition");
            }
        }

        private string _playPositionper;
        public string PlayPositionPer
        {
            get { return _playPositionper; }
            set
            {
                this._playPositionper = value;
                this.OnPropertyChanged("PlayPositionPer");
            }
        }

        /// <summary>
        /// 标记信息
        /// </summary>
        public string Mark
        {
            get;
            set;
        }

        public string UpdateMark;
        

        public string UpdateUserImp;


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ValueCloneToObject(MediaInfo objectMediaInfo)
        {
            objectMediaInfo.ArithType = this.ArithType;
            objectMediaInfo.CameraImp = this.CameraImp;
            objectMediaInfo.CameraTag = this.CameraTag;
            objectMediaInfo.CollectIp = this.CollectIp;
            objectMediaInfo.CollectTime = this.CollectTime;
            objectMediaInfo.DeviceID = this.DeviceID;
            objectMediaInfo.Duration = this.Duration;
            objectMediaInfo.EncodeType = this.EncodeType;
            objectMediaInfo.EncryType = this.EncryType;
            objectMediaInfo.FileData = this.FileData;
            objectMediaInfo.FileName = this.FileName;
            objectMediaInfo.FilePath = this.FilePath;
            objectMediaInfo.FileSize = this.FileSize;
            objectMediaInfo.MediaType = this.MediaType;
            objectMediaInfo.OrgID = this.OrgID;
            objectMediaInfo.OrgName = this.OrgName;
            objectMediaInfo.RecordID = this.RecordID;
            objectMediaInfo.RecordTime = this.RecordTime;
            objectMediaInfo.ResolutionHeight = this.ResolutionHeight;
            objectMediaInfo.ResolutionWith = this.ResolutionWith;
            objectMediaInfo.UserCode = this.UserCode;
            objectMediaInfo.UserID = this.UserID;
            objectMediaInfo.UserGuid = this.UserGuid;
            objectMediaInfo.UserImp = this.UserImp;
            objectMediaInfo.UserName = this.UserName;
            objectMediaInfo.UserTag = this.UserTag;
            objectMediaInfo.Progress = this.Progress;
            objectMediaInfo.UpLoadState = this.UpLoadState;
        }
    }

    public enum UpLoadState
    {
        NoUpLoad = 0,
        UpLoading = 1,
        UpLoadFail = 2,
        UpLoaded = 3,
        UpLoadWait = 4
    }

    /// <summary>
    /// 电子证据查询条件
    /// </summary>
    public class MediaLogsSerach
    {
        /// <summary>
        /// 录制者编号
        /// </summary>
        public string UserGuid
        {
            get;
            set;
        }
        /// <summary>
        /// 录制者部门ID
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
        /// 文件名称
        /// </summary>
        public string FileName
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
        /// 文件类型
        /// </summary>
        public string FileType
        {
            get;
            set;
        }

        public string UserImp
        {
            get;
            set;
        }

        public string UploadState
        {
            get;
            set;
        }

        public string UserTag
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
    /// 电子证据日志总数
    /// </summary>
    [XmlRoot("DataCounts")]
    public class MediaLogsCount
    {
        [XmlElement("AllCount")]
        public string AllCount
        {
            get;
            set;
        }

        [XmlElement("PicCount")]
        public string PicCount
        {
            get;
            set;
        }

        [XmlElement("VideoCount")]
        public string VideoCount
        {
            get;
            set;
        }

        [XmlElement("VoiceCount")]
        public string VoiceCount
        {
            get;
            set;
        }
    }

    [XmlRoot("MediaLogEditorResult")]
    public class MediaLogEditorResult
    {
        [XmlElement("ResultCode")]
        public string ResultCode
        {
            get;
            set;
        }

        [XmlElement("ResultMessage")]
        public string ResultMessage
        {
            get;
            set;
        }
        [XmlElement("MediaLogID")]
        public string MediaLogID
        {
            get;
            set;
        }
    }
}
