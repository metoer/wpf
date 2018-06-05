using Hytera.EEMS.Common;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Dispatcher
{
    /// <summary>
    /// App配置信息
    /// </summary>
    public class AppConfigInfos
    {
        static AppConfigInfos()
        {
            LanguageList = new List<LanguageInfos>();
            Skins = new Dictionary<string, List<string>>();
            AppStateInfos = new AppStateInfos();            
            LimitsUserInfos = new UserInfos();
            LicenseInfo = new LicenseInfo();
            PortPairInfos = new List<PortPairInfo>();
            PortDeviceList = new PortPariDevice();
            DeviceTypeList = new List<DeviceType>();
        }

        /// <summary>
        /// 是否播放视频音频文件
        /// </summary>
        public static bool IsPlay
        {
            get;
            set;
        }

        /// <summary>
        /// ICE端口是否连接成功
        /// </summary>
        public static bool IceConnect
        {
            get;
            set;
        }

        /// <summary>
        /// 当前操作用户信息
        /// </summary>
        public static UserInfos CurrentUserInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 权限查询用户信息
        /// </summary>
        public static UserInfos LimitsUserInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public static AppStateInfos AppStateInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 端口设置配对信息
        /// </summary>
        public static List<PortPairInfo> PortPairInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 端口信息
        /// </summary>
        public static PortPariDevice PortDeviceList
        {
            get;
            set;
        }

        /// <summary>
        /// license信息
        /// </summary>
        public static LicenseInfo LicenseInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 执法记录仪类型
        /// </summary>
        public static List<DeviceType> DeviceTypeList
        {
            get;
            set;
        }

        /// <summary>
        /// 语言数据
        /// </summary>
        public static List<LanguageInfos> LanguageList
        {
            get;
            set;
        }

        /// <summary>
        /// 当前皮肤key
        /// </summary>
        public static string CurrentSkinName
        {
            get;
            set;
        }

        /// <summary>
        /// 皮肤字典
        /// </summary>
        public static Dictionary<string, List<string>> Skins
        {
            get;
            set;
        }
    }
}
