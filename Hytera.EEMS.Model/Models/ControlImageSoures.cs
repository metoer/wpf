using System.Windows;
using System.Windows.Media;

namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 控件导航说明集合
    /// </summary>
    public class ControlNavigable : DependencyObject
    {
        /// <summary>
        /// 默认图标
        /// </summary>
        private ImageSource defaultSource;

        /// <summary>
        /// 鼠标覆盖图标
        /// </summary>
        private ImageSource mouseOverSource;

        /// <summary>
        /// 鼠标按下图标
        /// </summary>
        private ImageSource mouseDownSource;

        /// <summary>
        /// 功能名称
        /// </summary>
        private string name;

        /// <summary>
        /// 功能索引
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// 权限ID值
        /// </summary>
        public string PermissionCode
        {
            get;
            set;
        }

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
        public static readonly DependencyProperty DefaultSourceProperty = DependencyProperty.Register("DefaultSource", typeof(ImageSource), typeof(ControlNavigable));

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
        public static readonly DependencyProperty MouseOverSourceProperty = DependencyProperty.Register("MouseOverSource", typeof(ImageSource), typeof(ControlNavigable));

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
        public static readonly DependencyProperty MouseDownSourceProperty = DependencyProperty.Register("MouseDownSource", typeof(ImageSource), typeof(ControlNavigable));

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name
        {
            get
            {
                return (string)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(ControlNavigable), new PropertyMetadata(string.Empty));
    }
}
