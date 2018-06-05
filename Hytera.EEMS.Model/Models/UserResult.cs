
using Hytera.EEMS.Common;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 用户验证返回信息
    /// </summary>
    [XmlRoot("UserResult")]
    public class UserResult
    {
        public UserInfoFrom UserInfoFrom
        {
            get;
            set;
        }

        public string UserResultCode
        {
            get;
            set;
        }

        public string ResultMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 权限ID
        /// </summary>
        public string PermissionID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfos UserInfos
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 机构权限列表
    /// </summary>
    public class OrgList
    {
        public OrgList()
        {
            orgList = new List<OrgInfos>();
        }

        [XmlElement("OrgInfos")]
        public List<OrgInfos> orgList
        { get; set; }
    }

    /// <summary>
    /// 用户权限列表
    /// </summary>
    public class UserInfoList
    {
        public UserInfoList()
        {
            UserList = new List<UserInfos>();
        }

        [XmlElement("UserInfos")]
        public List<UserInfos> UserList
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 用户信息列表
    /// </summary>
    public class UserInfos : NotifyPropertyChangedBase
    {
        public UserInfos()
        {
            Users = new UserInfoList();
            Fingers = new List<Finger>();
        }

        /// <summary>
        /// 用户ID
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

        private string userName = string.Empty;

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string userCode = string.Empty;

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserCode
        {
            get
            {
                return userCode;
            }
            set
            {
                userCode = value;
                OnPropertyChanged("UserCode");
            }
        }

        /// <summary>
        /// 用户图标ID
        /// </summary>
        public string UserIconID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户头像文件名
        /// </summary>
        public string UserPortraitName
        {
            get;
            set;
        }

        /// <summary>
        /// 用户头像文件路径
        /// </summary>
        public string UserPortraitPath
        {
            get;
            set;
        }

        private string userType = string.Empty;

        /// <summary>
        /// 用户类型(1、调度员；2、普通警员)
        /// </summary>
        public string UserType
        {
            get
            {
                return userType;
            }
            set
            {
                userType = value;
                OnPropertyChanged("UserType");
            }
        }

        /// <summary>
        /// 警种类型
        /// </summary>
        public string PoliceType
        {
            get;
            set;
        }

        /// <summary>
        /// 单位ID
        /// </summary>
        public string OrgID
        {
            get;
            set;
        }

        public string OrgIDCode
        {
            get;
            set;
        }

        private string orgName = string.Empty;
        /// <summary>
        /// 单位Name
        /// </summary>
        public string OrgName
        {
            get
            {
                return orgName;
            }
            set
            {
                orgName = value;
                OnPropertyChanged("OrgName");
            }
        }

        public string OrgIDCodeStr
        {
            get;
            set;
        }

        private int fingerNumber = 0;

        /// <summary>
        /// 指纹数量
        /// </summary>
        public int FingerNumber
        {
            get
            {
                return fingerNumber;
            }
            set
            {
                fingerNumber = value;
                OnPropertyChanged("FingerNumber");
            }
        }

        [XmlElement("Fingers")]
        public List<Finger> Fingers
        {
            get;
            set;
        }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// 无线终端号码
        /// </summary>
        public string SSI
        {
            get;
            set;
        }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public string LastLoginTime
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


        [XmlElement("UserList")]
        public UserInfoList Users
        {
            get;
            set;
        }

        [XmlElement("OrgList")]
        public OrgList OrgList
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 验证信息来源
    /// </summary>
    public enum UserInfoFrom
    {
        // 指纹
        [XmlEnum("0")]
        Fingerprint = 0,
        // 登录
        [XmlEnum("1")]
        Login = 1
    }

    public class OrgInfos
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string OrgID
        {
            get;
            set;
        }

        public string OrgIDCode
        {
            get;
            set;
        }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string OrgName
        {
            get;
            set;
        }

        /// <summary>
        /// 部门编号
        /// </summary>
        public string OrgCode
        {
            get;
            set;
        }

        public string ParentID
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get;
            set;
        }
        public bool IsExpanded
        {
            get;
            set;
        }

        public bool HasItem
        {
            get;
            set;
        }
        public List<OrgInfos> Children
        {
            get;
            set;
        }
        public int Level
        {
            get;
            set;
        }
        public string LevelCode
        {
            get;
            set;
        }
    }
}
