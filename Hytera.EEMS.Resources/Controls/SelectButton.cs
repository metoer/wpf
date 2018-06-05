using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hytera.EEMS.Resources.Controls
{
    public class SelectButton : ImageButton
    {

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IsSelectProperty = DependencyProperty.Register("IsSelect", typeof(bool), typeof(SelectButton), new PropertyMetadata(false));

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelect
        {
            get
            {
                return (bool)GetValue(IsSelectProperty);
            }
            set
            {
                SetValue(IsSelectProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty SelectImageSourceProperty = DependencyProperty.Register("SelectImageSource", typeof(ImageSource), typeof(SelectButton));

        /// <summary>
        /// 选中图标
        /// </summary>
        public ImageSource SelectImageSource
        {
            get
            {
                return (ImageSource)GetValue(SelectImageSourceProperty);
            }
            set
            {
                SetValue(SelectImageSourceProperty, value);
            }
        }

    }
}
