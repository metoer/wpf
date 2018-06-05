using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hytera.EEMS.Resources.Controls
{
    public class ImageButton : Button
    {
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty MouseOverSourceProperty = DependencyProperty.Register("MouseOverSource", typeof(ImageSource), typeof(ImageButton));

        /// <summary>
        /// 鼠标覆盖图标
        /// </summary>
        public ImageSource MouseOverSource
        {
            get
            {
                return (ImageSource)GetValue(MouseOverSourceProperty);
            }
            set
            {
                SetValue(MouseOverSourceProperty, value); 
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty DefaultSourceProperty = DependencyProperty.Register("DefaultSource", typeof(ImageSource), typeof(ImageButton));

        /// <summary>
        /// 默认图标
        /// </summary>
        public ImageSource DefaultSource
        {
            get
            {
                return (ImageSource)GetValue(DefaultSourceProperty);
            }
            set
            {
                SetValue(DefaultSourceProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty MouseDownSourceProperty = DependencyProperty.Register("MouseDownSource", typeof(ImageSource), typeof(ImageButton));

        /// <summary>
        /// 鼠标按下图标
        /// </summary>
        public ImageSource MouseDownSource
        {
            get
            {
                return (ImageSource)GetValue(MouseDownSourceProperty);
            }
            set
            {
                SetValue(MouseDownSourceProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty NoEnableSourceProperty = DependencyProperty.Register("NoEnableSource", typeof(ImageSource), typeof(ImageButton));

        /// <summary>
        /// 不可用
        /// </summary>
        public ImageSource NoEnableSource
        {
            get
            {
                return (ImageSource)GetValue(NoEnableSourceProperty);
            }
            set
            {
                SetValue(NoEnableSourceProperty, value);
            }
        }

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register("ShowText", typeof(string), typeof(ImageButton));

        /// <summary>
        /// 显示Text
        /// </summary>
        public string ShowText
        {
            get
            {
                return (string)GetValue(ShowTextProperty);
            }
            set
            {
                SetValue(ShowTextProperty, value);
            }
        }

        public static readonly DependencyProperty IsCharacterEllipsisProperty = DependencyProperty.Register("IsCharacterEllipsis", typeof(bool), typeof(ImageButton));

        /// <summary>
        /// 显示Text
        /// </summary>
        public bool IsCharacterEllipsis
        {
            get
            {
                return (bool)GetValue(IsCharacterEllipsisProperty);
            }
            set
            {
                SetValue(IsCharacterEllipsisProperty, value);
            }
        }
    }
}
