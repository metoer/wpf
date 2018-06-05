using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Resources.Controls
{
    /// <summary>
    /// WindowHead.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindowHead : UserControl
    {
        private Window parentWindow;
        public MainWindowHead()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return (string)GetValue(WindowTitleProperty);
            }
            set
            {
                SetValue(WindowTitleProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.Register("WindowTitle", typeof(string), typeof(MainWindowHead), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 标题是否显示
        /// </summary>
        public Visibility WindowTitleVisibility
        {
            get
            {
                return (Visibility)GetValue(WindowTitleVisibilityProperty);
            }
            set
            {
                SetValue(WindowTitleVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty WindowTitleVisibilityProperty = DependencyProperty.Register("WindowTitleVisibility", typeof(Visibility), typeof(MainWindowHead), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 关闭按钮的显示
        /// </summary>
        public Visibility HeadCloseVisibility
        {
            get
            {
                return (Visibility)GetValue(HeadCloseVisibilityProperty);
            }
            set
            {
                SetValue(HeadCloseVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty HeadCloseVisibilityProperty = DependencyProperty.Register("HeadCloseVisibility", typeof(Visibility), typeof(MainWindowHead), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 最大化按钮的显示
        /// </summary>
        public Visibility HeadMaxVisibility
        {
            get
            {
                return (Visibility)GetValue(HeadMaxVisibilityProperty);
            }
            set
            {
                SetValue(HeadMaxVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty HeadMaxVisibilityProperty = DependencyProperty.Register("HeadMaxVisibility", typeof(Visibility), typeof(MainWindowHead), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 最小化按钮的显示
        /// </summary>
        public Visibility HeadMinVisibility
        {
            get
            {
                return (Visibility)GetValue(HeadMinVisibilityProperty);
            }
            set
            {
                SetValue(HeadMinVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty HeadMinVisibilityProperty = DependencyProperty.Register("HeadMinVisibility", typeof(Visibility), typeof(MainWindowHead), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 最小化按钮的显示
        /// </summary>
        public Visibility HeadKeyVisibility
        {
            get
            {
                return (Visibility)GetValue(HeadKeyVisibilityProperty);
            }
            set
            {
                SetValue(HeadKeyVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty HeadKeyVisibilityProperty = DependencyProperty.Register("HeadKeyVisibility", typeof(Visibility), typeof(MainWindowHead), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 控件加载过程寻找父窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void GetWindow()
        {
            FrameworkElement element = GetParent(this);
            while (element != null)
            {
                if (element is Window)
                {
                    parentWindow = element as Window;
                    break;
                }

                element = GetParent(element);
            }
        }

        private FrameworkElement GetParent(FrameworkElement element)
        {
            return element.Parent as FrameworkElement;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (parentWindow == null)
            {
                GetWindow();
            }

            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            if (parentWindow == null)
            {
                GetWindow();
            }

            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (parentWindow == null)
            {
                GetWindow();
            }

            if (parentWindow != null)
            {
                if (parentWindow.WindowState == WindowState.Maximized)
                {
                    parentWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    parentWindow.WindowState = WindowState.Maximized;
                }
            }
        }

        private void btnKey_Click(object sender, RoutedEventArgs e)
        {
            if (parentWindow == null)
            {
                GetWindow();
            }

            AppHelper.StartKey(parentWindow);
        }

    }
}
