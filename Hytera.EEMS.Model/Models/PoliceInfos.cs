using Hytera.EEMS.Common;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    [XmlRoot("FingerInfos")]
    public class FingerInfos
    {
        public FingerInfos()
        {
            UserInfoList = new ThreadSafeList<UserInfos>();
        }

        [XmlElement("UserInfos")]
        public ThreadSafeList<UserInfos> UserInfoList
        {
            get;
            set;
        }


        [XmlAttribute("PageIndex")]
        public int PageIndex
        {
            get;
            set;
        }

        [XmlAttribute("ResultCode")]
        public int ResultCode
        {
            get;
            set;
        }
    }

}
