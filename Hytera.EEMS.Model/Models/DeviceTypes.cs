using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model.Models
{
    [XmlRoot("DeviceTypesResult")]
    public class DeviceTypes
    {
        public DeviceTypes()
        {
            DeviceTypeList = new List<DeviceType>();
        }

        [XmlElement("DeviceType")]
        public List<DeviceType> DeviceTypeList
        {
            get;
            set;
        }
    }

    public class DeviceType
    {
        public string ID
        {
            get;
            set;
        }

        public string CategoryName
        {
            get;
            set;
        }
    }
}
