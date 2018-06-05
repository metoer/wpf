using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 采集状态
    /// </summary>
    public enum DeviceState
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [XmlEnum("-1")]
        UnDefine = -1,

        [XmlEnum("0")]
        Default = 0,
        /// <summary>
        /// 正在采集
        /// </summary>
        [XmlEnum("1")]
        Collecting = 1,
        /// <summary>
        /// 暂停采集
        /// </summary>
        [XmlEnum("2")]
        PauseCollect = 2,
        /// <summary>
        /// 采集失败
        /// </summary>
        [XmlEnum("3")]
        CollectFailed = 3,
        /// <summary>
        /// 采集完成
        /// </summary>
        [XmlEnum("4")]
        CollectFinish = 4
    }

    /// <summary>
    /// 配对状态
    /// </summary>
    public enum IsMatchOrRegist
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [XmlEnum("-1")]
        UnDefine = -1,

        /// <summary>
        /// 未注册
        /// </summary>
        [XmlEnum("0")]
        UnRegister = 0,

        /// <summary>
        /// 已注册
        /// </summary>
        [XmlEnum("1")]
        Registered = 1,

        /// <summary>
        /// 已配对
        /// </summary>
        [XmlEnum("2")]
        Matched = 2
    }
}
