using System.Collections.Generic;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 向ICE接口传递参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [XmlRoot("Conditions")]
    public class Conditions : List<Item>
    {
        public void AddItem(string key, string value)
        {
            this.Add(new Item() { Key = key, Value = value });
        }
    }

    public class Item
    {
        [XmlAttribute("Key")]
        public string Key
        {
            get;
            set;
        }

        [XmlAttribute("Value")]
        public string Value
        {
            get;
            set;
        }

    }
}
