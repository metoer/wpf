using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Resources.Controls
{
    /// <summary>
    /// PasswordControl.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordControl : UserControl
    {
        PasswordBox passwordBox;

        public event TextChangedEventHandler TextChanged;

        public PasswordControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 前面锁的图片是否显示
        /// </summary>
        public Visibility HeadVisibility
        {
            get
            {
                return loginPw.HeadVisibility;
            }
            set
            {
                loginPw.HeadVisibility = value;
            }
        }

        /// <summary>
        /// 设置说明
        /// </summary>
        public string MarkValue
        {
            get
            {
                return loginPw.MarkValue;
            }
            set
            {
                loginPw.MarkValue = value;
            }
        }

        /// <summary>
        /// 输入字符长度
        /// </summary>
        public int MaxLength
        {
            get
            {
                return loginPw.MaxLength;
            }
            set
            {
                loginPw.MaxLength = value;
            }
        }

        public string Text
        {
            get
            {
                return loginPw.Text;
            }
        }

        /// <summary>
        /// 是否设置获取焦点
        /// </summary>
        public bool IsSetFocus
        {
            get;
            set;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            passwordBox = loginPw.Template.FindName("newpb", loginPw) as PasswordBox;
            Button button = loginPw.Template.FindName("btn", loginPw) as Button;
            if (passwordBox != null)
            {
                passwordBox.GotFocus += passwordBox_GotFocus;
                passwordBox.PasswordChanged += passwordBox_PasswordChanged;
                passwordBox.LostFocus += passwordBox_LostFocus;
            }
            if (button != null)
            {
                button.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(button_MouseLeftButtonDown), true);
                button.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(button_MouseLeftButtonUp), true);
            }

            if (IsSetFocus && passwordBox != null)
            {
                passwordBox.Focus();
            }
        }

        void button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loginPw.IsDisplayPassword = true;
            loginPw.Focus();
        }

        void button_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            loginPw.IsDisplayPassword = false;
            passwordBox.Focus();
        }

        void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (loginPw != null)
            {
                loginPw.TextChanged -= loginPw_TextChanged;
                loginPw.Text = passwordBox.Password;
                loginPw.TextChanged += loginPw_TextChanged;

                if (TextChanged != null)
                {
                    TextChanged(this, null);
                }
            }
        }


        private void loginPw_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passwordBox != null)
            {
                passwordBox.Password = loginPw.Text;
            }

            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            loginPw.IsDisplayFocused = true;
        }

        void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            loginPw.IsDisplayFocused = false;
        }

    }
}
