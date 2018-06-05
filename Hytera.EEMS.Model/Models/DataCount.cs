using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    [XmlRoot("DataCount")]
    public class DataCount
    {
        [XmlAttribute("Count")]
        public string Count
        {
            get;
            set;
        }
    }
}
