using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Resources.Controls
{
    public class PortComboBoxItem : ComboBoxItem
    {
        /// <summary>
        /// 是否有设备信息
        /// </summary>
        public bool IsDeviceInfo
        {
            get
            {
                return (bool)GetValue(IsDeviceInfoProperty);
            }
            set
            {
                SetValue(IsDeviceInfoProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IsDeviceInfoProperty = DependencyProperty.Register("IsDeviceInfo", typeof(bool), typeof(PortComboBoxItem), new PropertyMetadata(false));
    }
}
