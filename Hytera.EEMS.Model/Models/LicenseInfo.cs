using Hytera.EEMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model.Models
{
    /// <summary>
    /// license信息
    /// </summary>
    public class LicenseInfo
    {

        /// <summary>
        /// 本机机器码
        /// </summary>
        public string LocalMachineCode
        {
            get;
            set;
        }

        /// <summary>
        /// 加密狗序列号
        /// </summary>
        public string SoftdogCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用机器码控制(0是，1否)
        /// </summary>
        public string IsMachineCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用加狗密控制(0是，1否)
        /// </summary>
        public string IsSoftdogCode
        {
            get;
            set;
        }

        /// <summary>
        /// 授权机器码
        /// </summary>
        public string EmpowerMachineCode
        {
            get;
            set;
        }

        /// <summary>
        /// 授权加密狗序列号
        /// </summary>
        public string EmpowerSoftdog
        {
            get;
            set;
        }

        /// <summary>
        /// 授权文件生成时间
        /// </summary>
        public string EmpowerFileCreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 授权截止时间
        /// </summary>
        public string EmpowerFileEndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 授权总天数
        /// </summary>
        public string EmpowerCountDays
        {
            get;
            set;
        }

        /// <summary>
        /// 剩余授权天数
        /// </summary>
        public string EmpowerSurplusDays
        {
            get;
            set;
        }

        /// <summary>
        /// 授权状态
        /// </summary>
        public string EmpowerStatus
        {
            get;
            set;
        }
    }
}
