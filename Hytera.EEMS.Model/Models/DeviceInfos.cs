using Hytera.EEMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 执法记录仪设备信息列表
    /// </summary>
    [XmlRoot("DeviceList")]
    public class DeviceInfos
    {
        public DeviceInfos()
        {
            DeviveInfoList = new List<DeviveInfo>();
        }

        [XmlElement("DeviceInfo")]
        public List<DeviveInfo> DeviveInfoList
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 执法记录仪设备信息
    /// </summary>
    public class DeviveInfo : NotifyPropertyChangedBase
    {
        private string electricity = string.Empty;

        private string volume = string.Empty;

        private string collectProgress = string.Empty;

        private DeviceState deviceState = DeviceState.Default;

        private IsMatchOrRegist isMatchOrRegist = IsMatchOrRegist.UnRegister;

        private string matchUserName = string.Empty;

        private string matchUserCode = string.Empty;

        private string matchOrgName = string.Empty;

        private string deviceCode = string.Empty;

        /// <summary>
        /// 物理端口编号，用来配置显示在执法记录仪面板中的位置
        /// </summary>
        public string PortCode
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

        /// <summary>
        /// 设备系列号
        /// </summary>
        public string DeviceCode
        {
            get
            {
                return deviceCode;
            }
            set
            {
                deviceCode = value;
            }
        }

        /// <summary>
        /// 设备别名
        /// </summary>
        public string DeviceAlias
        {
            get;
            set;
        }

        /// <summary>
        /// 设备图标ID
        /// </summary>
        public string DeviceIconID
        {
            get;
            set;
        }

        /// <summary>
        /// 电量
        /// </summary>
        public string Electricity
        {
            get
            {
                return electricity;
            }
            set
            {
                electricity = value;
                OnPropertyChanged("Electricity");
            }
        }

        /// <summary>
        /// 容量
        /// </summary>
        public string Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                OnPropertyChanged("Volume");
            }
        }

        private string totalVolume = string.Empty;

        /// <summary>
        /// 总容量
        /// </summary>
        public string TotalVolume
        {
            get
            {
                return totalVolume;
            }
            set
            {
                totalVolume = value;
                OnPropertyChanged("TotalVolume");
            }
        }

        private string useVolume = string.Empty;

        /// <summary>
        /// 已用容量
        /// </summary>
        public string UseVolume
        {
            get
            {
                return useVolume;
            }
            set
            {
                useVolume = value;
                OnPropertyChanged("UseVolume");
            }
        }

        /// <summary>
        /// 采集进度
        /// </summary>
        public string CollectProgress
        {
            get
            {
                return collectProgress;
            }
            set
            {
                collectProgress = value;
                OnPropertyChanged("CollectProgress");
            }
        }

        /// <summary>
        /// 所属组织ID
        /// </summary>
        public string OrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 厂家ID
        /// </summary>
        public string VenderID
        {
            get;
            set;
        }

        /// <summary>
        /// 厂家名称
        /// </summary>
        public string VenderName
        {
            get;
            set;
        }

        /// <summary>
        /// 厂家图标ID
        /// </summary>
        public string VenderIconID
        {
            get;
            set;
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegisterTime
        {
            get;
            set;
        }

        /// <summary>
        /// 注册用户ID
        /// </summary>
        public string RegisterUser
        {
            get;
            set;
        }

        /// <summary>
        /// 注册采集站ID
        /// </summary>
        public string RegisterStationID
        {
            get;
            set;
        }

        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegisterIp
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string LastUpdateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户ID
        /// </summary>
        public string MatchUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户名称
        /// </summary>
        public string MatchUserName
        {
            get
            {
                return matchUserName;
            }
            set
            {
                matchUserName = value;
                OnPropertyChanged("MatchUserName");
            }
        }

        /// <summary>
        /// 配对用户编号
        /// </summary>
        public string MatchUserCode
        {
            get
            {
                return matchUserCode;
            }
            set
            {
                matchUserCode = value;
                OnPropertyChanged("MatchUserCode");
            }
        }

        /// <summary>
        /// 配对用户所在单位ID
        /// </summary>
        public string MatchOrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户所在单位时间
        /// </summary>
        public string MatchOrgName
        {
            get
            {
                return matchOrgName;
            }
            set
            {
                matchOrgName = value;
                OnPropertyChanged("MatchOrgName");
            }
        }

        /// <summary>
        /// 配对时间
        /// </summary>
        public string MatchTime
        {
            get;
            set;
        }

        /// <summary>
        /// 操作状态
        /// </summary>
        public DeviceState DeviceState
        {
            get
            {
                return deviceState;
            }
            set
            {
                deviceState = value;
                OnPropertyChanged("DeviceState");
            }
        }

        /// <summary>
        /// 配对状态
        /// </summary>
        public IsMatchOrRegist IsMatchOrRegist
        {
            get
            {
                return isMatchOrRegist;
            }
            set
            {
                isMatchOrRegist = value;
                OnPropertyChanged("IsMatchOrRegist");
            }
        }

        /// <summary>
        /// 手动复制到指定的对象中，出发通知事件
        /// </summary>
        /// <param name="objectDevInfo"></param>

        public void ValueCloneToObject(DeviveInfo objectDevInfo)
        {
            objectDevInfo.CollectProgress = this.deviceState == DeviceState.PauseCollect || object.Equals(this.CollectProgress, null) ? objectDevInfo.CollectProgress : this.CollectProgress;
            objectDevInfo.Description = object.Equals(this.Description, null) ? objectDevInfo.Description : this.Description;
            objectDevInfo.DeviceAlias = object.Equals(this.DeviceAlias, null) ? objectDevInfo.DeviceAlias : this.DeviceAlias;
            objectDevInfo.PortCode = object.Equals(this.PortCode, null) ? objectDevInfo.PortCode : this.PortCode;
            objectDevInfo.deviceCode = object.Equals(this.deviceCode, null) ? objectDevInfo.deviceCode : this.deviceCode;
            objectDevInfo.DeviceIconID = object.Equals(this.DeviceIconID, null) ? objectDevInfo.DeviceIconID : this.DeviceIconID;
            objectDevInfo.Electricity = object.Equals(this.Electricity, null) || this.Electricity.Equals("-1") ? objectDevInfo.Electricity : this.Electricity;
            objectDevInfo.LastUpdateTime = object.Equals(this.LastUpdateTime, null) ? objectDevInfo.LastUpdateTime : this.LastUpdateTime;
            objectDevInfo.MatchOrgID = object.Equals(this.MatchOrgID, null) ? objectDevInfo.MatchOrgID : this.MatchOrgID;
            objectDevInfo.MatchOrgName = object.Equals(this.MatchOrgName, null) ? objectDevInfo.MatchOrgName : this.MatchOrgName;
            objectDevInfo.MatchTime = object.Equals(this.MatchTime, null) ? objectDevInfo.MatchTime : this.MatchTime;
            objectDevInfo.MatchUserCode = object.Equals(this.MatchUserCode, null) ? objectDevInfo.MatchUserCode : this.MatchUserCode;
            objectDevInfo.MatchUserID = object.Equals(this.MatchUserID, null) ? objectDevInfo.MatchUserID : this.MatchUserID;
            objectDevInfo.MatchUserName = object.Equals(this.MatchUserName, null) ? objectDevInfo.MatchUserName : this.MatchUserName;
            objectDevInfo.RegisterIp = object.Equals(this.RegisterIp, null) ? objectDevInfo.RegisterIp : this.RegisterIp;
            objectDevInfo.RegisterStationID = object.Equals(this.RegisterStationID, null) ? objectDevInfo.RegisterStationID : this.RegisterStationID;
            objectDevInfo.RegisterTime = object.Equals(this.RegisterTime, null) ? objectDevInfo.RegisterTime : this.RegisterTime;
            objectDevInfo.RegisterUser = object.Equals(this.RegisterUser, null) ? objectDevInfo.RegisterUser : this.RegisterUser;
            objectDevInfo.VenderID = object.Equals(this.VenderID, null) ? objectDevInfo.VenderID : this.VenderID;
            objectDevInfo.VenderName = object.Equals(this.VenderName, null) ? objectDevInfo.VenderName : this.VenderName;
            objectDevInfo.Volume = object.Equals(this.Volume, null) ? objectDevInfo.Volume : this.Volume;
            objectDevInfo.DeviceState = this.DeviceState != Model.DeviceState.UnDefine ? this.DeviceState : objectDevInfo.DeviceState;
            objectDevInfo.IsMatchOrRegist = this.IsMatchOrRegist != IsMatchOrRegist.UnDefine ? this.IsMatchOrRegist : objectDevInfo.IsMatchOrRegist;
        }
    }

    /// <summary>
    /// 执法记录仪设备信息列表
    /// </summary>
    [XmlRoot("DeviceList")]
    public class UpdateDeviceInfos
    {
        public UpdateDeviceInfos()
        {
            DeviveInfoList = new List<UpdateDeviveInfo>();
        }

        [XmlElement("DeviceInfo")]
        public List<UpdateDeviveInfo> DeviveInfoList
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 执法记录仪设备信息
    /// </summary>
    public class UpdateDeviveInfo
    {
        /// <summary>
        /// 插在面板中的位置
        /// </summary>
        public string PortCode
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

        /// <summary>
        /// 设备系列号
        /// </summary>
        public string DeviceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 设备别名
        /// </summary>
        public string DeviceAlias
        {
            get;
            set;
        }

        /// <summary>
        /// 设备图标ID
        /// </summary>
        public string DeviceIconID
        {
            get;
            set;
        }

        /// <summary>
        /// 电量
        /// </summary>
        public string Electricity
        {
            get;
            set;
        }

        /// <summary>
        /// 剩余容量
        /// </summary>
        public string Volume
        {
            get;
            set;
        }

        /// <summary>
        /// 总容量
        /// </summary>
        public string TotalVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 已用容量
        /// </summary>
        public string UseVolume
        {
            get;
            set;
        }


        /// <summary>
        /// 采集进度
        /// </summary>
        public string CollectProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 所属组织ID
        /// </summary>
        public string OrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 厂家ID
        /// </summary>
        public string VenderID
        {
            get;
            set;
        }

        /// <summary>
        /// 厂家名称
        /// </summary>
        public string VenderName
        {
            get;
            set;
        }

        /// <summary>
        /// 厂家图标ID
        /// </summary>
        public string VenderIconID
        {
            get;
            set;
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegisterTime
        {
            get;
            set;
        }

        /// <summary>
        /// 注册用户ID
        /// </summary>
        public string RegisterUser
        {
            get;
            set;
        }

        /// <summary>
        /// 注册采集站ID
        /// </summary>
        public string RegisterStationID
        {
            get;
            set;
        }

        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegisterIp
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string LastUpdateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户ID
        /// </summary>
        public string MatchUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户名称
        /// </summary>
        public string MatchUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户编号
        /// </summary>
        public string MatchUserCode
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户所在单位ID
        /// </summary>
        public string MatchOrgID
        {
            get;
            set;
        }

        /// <summary>
        /// 配对用户所在单位时间
        /// </summary>
        public string MatchOrgName
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

        public DeviceState deviceState = DeviceState.UnDefine;
        /// <summary>
        /// 操作状态
        /// </summary>
        public DeviceState DeviceState
        {
            get
            {
                return deviceState;
            }
            set
            {
                deviceState = value;
            }
        }

        public IsMatchOrRegist isMatchOrRegist = IsMatchOrRegist.UnDefine;

        /// <summary>
        /// 配对状态
        /// </summary>
        public IsMatchOrRegist IsMatchOrRegist
        {
            get
            {
                return isMatchOrRegist;
            }
            set
            {
                isMatchOrRegist = value;
            }
        }

        /// <summary>
        /// 除空值不复制
        /// </summary>
        /// <param name="objectDevInfo"></param>
        public void ValueCloneToObjectExceptNull(DeviveInfo objectDevInfo)
        {
            // 暂停时不更新采集进度
            objectDevInfo.CollectProgress = this.deviceState == DeviceState.PauseCollect || object.Equals(this.CollectProgress, null) ? objectDevInfo.CollectProgress : this.CollectProgress;
            objectDevInfo.Description = object.Equals(this.Description, null) ? objectDevInfo.Description : this.Description;
            objectDevInfo.DeviceAlias = object.Equals(this.DeviceAlias, null) ? objectDevInfo.DeviceAlias : this.DeviceAlias;
            objectDevInfo.PortCode = object.Equals(this.PortCode, null) ? objectDevInfo.PortCode : this.PortCode;
            objectDevInfo.DeviceCode = object.Equals(this.DeviceCode, null) ? objectDevInfo.DeviceCode : this.DeviceCode;
            objectDevInfo.DeviceIconID = object.Equals(this.DeviceIconID, null) ? objectDevInfo.DeviceIconID : this.DeviceIconID;
            objectDevInfo.Electricity = object.Equals(this.Electricity, null) || this.Electricity.Equals("-1") ? objectDevInfo.Electricity : this.Electricity;
            objectDevInfo.LastUpdateTime = object.Equals(this.LastUpdateTime, null) ? objectDevInfo.LastUpdateTime : this.LastUpdateTime;
            objectDevInfo.MatchOrgID = object.Equals(this.MatchOrgID, null) ? objectDevInfo.MatchOrgID : this.MatchOrgID;
            objectDevInfo.MatchOrgName = object.Equals(this.MatchOrgName, null) ? objectDevInfo.MatchOrgName : this.MatchOrgName;
            objectDevInfo.MatchTime = object.Equals(this.MatchTime, null) ? objectDevInfo.MatchTime : this.MatchTime;
            objectDevInfo.MatchUserCode = object.Equals(this.MatchUserCode, null) ? objectDevInfo.MatchUserCode : this.MatchUserCode;
            objectDevInfo.MatchUserID = object.Equals(this.MatchUserID, null) ? objectDevInfo.MatchUserID : this.MatchUserID;
            objectDevInfo.MatchUserName = object.Equals(this.MatchUserName, null) ? objectDevInfo.MatchUserName : this.MatchUserName;
            objectDevInfo.RegisterIp = object.Equals(this.RegisterIp, null) ? objectDevInfo.RegisterIp : this.RegisterIp;
            objectDevInfo.RegisterStationID = object.Equals(this.RegisterStationID, null) ? objectDevInfo.RegisterStationID : this.RegisterStationID;
            objectDevInfo.RegisterTime = object.Equals(this.RegisterTime, null) ? objectDevInfo.RegisterTime : this.RegisterTime;
            objectDevInfo.RegisterUser = object.Equals(this.RegisterUser, null) ? objectDevInfo.RegisterUser : this.RegisterUser;
            objectDevInfo.VenderID = object.Equals(this.VenderID, null) ? objectDevInfo.VenderID : this.VenderID;
            objectDevInfo.VenderName = object.Equals(this.VenderName, null) ? objectDevInfo.VenderName : this.VenderName;
            objectDevInfo.Volume = object.Equals(this.Volume, null) ? objectDevInfo.Volume : this.Volume;
            objectDevInfo.TotalVolume = object.Equals(this.TotalVolume, null) ? objectDevInfo.TotalVolume : this.TotalVolume;
            objectDevInfo.DeviceState = this.DeviceState != Model.DeviceState.UnDefine ? this.DeviceState : objectDevInfo.DeviceState;
            objectDevInfo.IsMatchOrRegist = this.IsMatchOrRegist != IsMatchOrRegist.UnDefine ? this.IsMatchOrRegist : objectDevInfo.IsMatchOrRegist;
            if (!string.IsNullOrEmpty(objectDevInfo.Volume) && !string.IsNullOrEmpty(objectDevInfo.TotalVolume))
            {
                objectDevInfo.UseVolume = (Convert.ToDouble(objectDevInfo.TotalVolume) - Convert.ToDouble(objectDevInfo.Volume)).ToString();
            }
            else
            {
                objectDevInfo.Volume = "0";
                objectDevInfo.TotalVolume = "0";
                objectDevInfo.UseVolume = "0";
            }
        }
    }
}
