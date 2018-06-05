using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    [XmlRoot("DataRefreshResult")]
    public class DataRefreshResult
    {
        public ResultCode DataCode
        {
            get;
            set;
        }

        public string ResultMsg
        {
            get;
            set;
        }

        public string ProgressValue
        {
            get;
            set;
        }
    }

    public enum ResultCode
    {
        [XmlEnum("0")]
        Success = 0,
        [XmlEnum("1")]
        Failed = 1
    }
}
