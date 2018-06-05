
using Hytera.EEMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hytera.EEMS.Model.Models
{
    /// <summary>
    /// 采集端口配对信息
    /// </summary>
    public class PortPairInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Index
        {
            get;
            set;
        }

        /// <summary>
        /// 物理端口序号
        /// </summary>
        public string PortCode
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 执法记录仪设备信息列表
    /// </summary>
    [XmlRoot("PortInfos")]
    public class PortInfos : NotifyPropertyChangedBase
    {
        public PortInfos()
        {
            PortList = new List<string>();
        }

        [XmlElement("Port")]
        public List<string> PortList
        {
            get;
            set;
        }
    }

    public class PortIsDevice : NotifyPropertyChangedBase
    {
        public string PortName
        {
            get;
            set;
        }

        bool isDeviceInfo = false;
        public bool IsDeviceInfo
        {
            get
            {
                return isDeviceInfo;
            }
            set
            {
                isDeviceInfo = value;
                OnPropertyChanged("IsDeviceInfo");
            }
        }
    }

    /// <summary>
    /// 端口对应的是否有执法记录仪
    /// </summary>
    public class PortPariDevice : NotifyPropertyChangedBase
    {
        public PortPariDevice()
        {
            PortevList = new List<PortIsDevice>();
            //PortevList.Add(new PortIsDevice() { PortName = string.Empty, IsDeviceInfo = false });
            //PortevList.Add(new PortIsDevice() { PortName = "123", IsDeviceInfo = false });
            //PortevList.Add(new PortIsDevice() { PortName = "124", IsDeviceInfo = false });
            //PortevList.Add(new PortIsDevice() { PortName = "125", IsDeviceInfo = false });
            //PortevList.Add(new PortIsDevice() { PortName = "126", IsDeviceInfo = false });
            //PortevList.Add(new PortIsDevice() { PortName = "127", IsDeviceInfo = false });
            //PortevList.Add(new PortIsDevice() { PortName = "128", IsDeviceInfo = false });
        }

        public List<PortIsDevice> PortevList
        {
            get;
            set;
        }

        private string firstPort = string.Empty;

        /// <summary>
        /// 优先端口
        /// </summary>
        public string FirstPort
        {
            get
            {
                return firstPort;
            }
            set
            {
                firstPort = value;
                OnPropertyChanged("FirstPort");
            }
        }

        /// <summary>
        /// 优先端口
        /// </summary>
        public string FirstPortCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否修改通知
        /// </summary>
        private bool isChanged = false;

        /// <summary>
        /// 是否修改通知
        /// </summary>
        public bool IsChanged
        {
            get
            {
                return isChanged;
            }
            set
            {
                isChanged = value;
                OnPropertyChanged("IsChanged");
            }
        }
    }
}
