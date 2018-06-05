using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model.Models
{
    /// <summary>
    /// 指纹结果返回
    /// </summary>
    public class FingerResult
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 执行结果:0成功、1失败
        /// </summary>
        public int ResultCode
        {
            get;
            set;
        }

        public string FingerImage
        {
            get;
            set;
        }

        /// <summary>
        /// 编辑结果：指纹ID与指纹名,可以传递多个，格式:ID:Name,ID,Name 
        /// </summary>
        public string FingersEditor
        {
            get;
            set;
        }

        /// <summary>
        ///  删除结果：指纹ID,可以传递多个，格式:ID,ID 
        /// </summary>
        public string FingersDelete
        {
            get;
            set;
        }

        /// <summary>
        /// 采集指纹ID
        /// </summary>
        public string FingerID
        {
            get;
            set;
        }

        /// <summary>
        /// 采集指纹名
        /// </summary>
        public string FingerName
        {
            get;
            set;
        }
    }
}
