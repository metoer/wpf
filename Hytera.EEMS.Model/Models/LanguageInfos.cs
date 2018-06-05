using System.Collections.Generic;

namespace Hytera.EEMS.Model
{
    public class LanguageInfos
    {
        /// <summary>
        /// 语言资源ID
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// 语言资源名称
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// 语言资源文件名称
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get;
            set;
        }
    }
}
