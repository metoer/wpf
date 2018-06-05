using Hytera.EEMS.Common;
using Hytera.EEMS.Resources.Controls;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Hytera.EEMS.Resources.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWindow : BaseWindow
    {
        private MessageBoxButton messageBoxButton;

        int millisecond;

        /// <summary>
        /// 构造
        /// </summary>

        public MessageBoxWindow()
        {
            InitializeComponent();
        }

        public MessageBoxWindow(string messageBoxText)
            : this()
        {
            Text = messageBoxText;
        }

        public MessageBoxWindow(string messageBoxText, int millisecond = 0)
            : this(messageBoxText)
        {
            this.millisecond = millisecond;
        }

        public MessageBoxWindow(string messageBoxText, MessageBoxButton button)
            : this(messageBoxText)
        {
            messageBoxButton = button;
        }

        public MessageBoxWindow(string messageBoxText, string caption)
            : this(messageBoxText)
        {
            Title = caption;
        }

        public MessageBoxWindow(string messageBoxText, string caption, MessageBoxButton button)
            : this(messageBoxText, caption)
        {
            messageBoxButton = button;
        }

        public MessageBoxWindow(string messageBoxText, MessageBoxButton button, string content1)
            : this(messageBoxText, button)
        {
            ButtonContent1 = content1;
        }

        public MessageBoxWindow(string messageBoxText, string caption, MessageBoxButton button, string content1)
            : this(messageBoxText, caption, button)
        {
            ButtonContent1 = content1;
        }


        /// <summary>
        /// 操作结果
        /// </summary>
        public MessageBoxResult MessageBoxResult
        {
            get;
            private set;
        }


        /// <summary>
        /// 显示内容
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
                SetContentHeight();
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 显示标题
        /// </summary>
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata("提示"));

        /// <summary>
        /// 第一个按钮内容
        /// </summary>
        public string ButtonContent1
        {
            get
            {
                return (string)GetValue(ButtonContent1Property);
            }
            set
            {
                SetValue(ButtonContent1Property, value);
                SetContentHeight();
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty ButtonContent1Property = DependencyProperty.Register("ButtonContent1", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 第二个按钮内容
        /// </summary>
        public string ButtonContent2
        {
            get
            {
                return (string)GetValue(ButtonContent2Property);
            }
            set
            {
                SetValue(ButtonContent2Property, value);
                SetContentHeight();
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty ButtonContent2Property = DependencyProperty.Register("ButtonContent2", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata(string.Empty));

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string btnContent1 = string.Empty;
            string btnContent2 = string.Empty;
            Title = TryFindResource("appHint").ToString();
            switch (messageBoxButton)
            {
                case MessageBoxButton.OKCancel:
                    MessageBoxResult = MessageBoxResult.Cancel;
                    btnContent1 = TryFindResource("appSure").ToString();
                    btnContent2 = TryFindResource("appCancel").ToString();
                    break;
                case MessageBoxButton.YesNo:
                    btnContent1 = TryFindResource("appYes").ToString();
                    btnContent2 = TryFindResource("appNo").ToString();
                    MessageBoxResult = MessageBoxResult.No;
                    break;

                default:
                    MessageBoxResult = MessageBoxResult.OK;
                    btnContent1 = TryFindResource("appSure").ToString();
                    btn1.Visibility = Visibility.Collapsed;
                    btn2.Visibility = Visibility.Collapsed;
                    btn3.Visibility = Visibility.Visible;
                    break;
            }

            ButtonContent1 = btnContent1;
            ButtonContent2 = btnContent2;

            if (millisecond != 0)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(millisecond);
                timer.Tick += new EventHandler(timer_Tick);

                timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 设置内容高度
        /// </summary>
        private void SetContentHeight()
        {
            double txtWidth = WindowsHelper.MeasureTextWidth(Text, txtMsg.FontSize, txtMsg.FontFamily.ToString());
            int row = (int)(txtWidth / txtMsg.Width);
            txtMsg.Height = (row + 1) * 30;
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            switch (messageBoxButton)
            {
                case MessageBoxButton.YesNo:
                    MessageBoxResult = MessageBoxResult.Yes;
                    break;
                case MessageBoxButton.OKCancel:
                default:
                    MessageBoxResult = MessageBoxResult.OK;
                    break;
            }
            this.Close();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            switch (messageBoxButton)
            {
                case MessageBoxButton.YesNo:
                    MessageBoxResult = MessageBoxResult.No;
                    break;
                case MessageBoxButton.OKCancel:
                default:
                    MessageBoxResult = MessageBoxResult.Cancel;
                    break;
            }
            this.Close();
        }
    }
}
