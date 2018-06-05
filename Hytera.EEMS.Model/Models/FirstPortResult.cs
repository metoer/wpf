using System.Xml.Serialization;

namespace Hytera.EEMS.Model.Models
{
    [XmlRoot("FirstPortResult")]
    public class FirstPortResult
    {
        public ResultCode ResultCode
        {
            get;
            set;
        }

        public string ResultMsg
        {
            get;
            set;
        }
    }
}
