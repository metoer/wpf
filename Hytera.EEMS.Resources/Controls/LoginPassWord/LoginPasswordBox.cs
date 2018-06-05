using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Resources.Controls
{
    public class LoginPasswordBox : TextBox
    {
        PasswordBox passwordBox;

        public LoginPasswordBox()
        {
            this.Loaded += LoginPasswordBox_Loaded;
        }

        /// <summary>
        /// 是否显示明文密码
        /// </summary>
        public bool IsDisplayPassword
        {
            get
            {
                return (bool)GetValue(IsDisplayPasswordProperty);
            }
            set
            {
                SetValue(IsDisplayPasswordProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IsDisplayPasswordProperty = DependencyProperty.Register("IsDisplayPassword", typeof(bool), typeof(LoginPasswordBox), new PropertyMetadata(false));

        /// <summary>
        /// 是否聚焦显示
        /// </summary>
        public bool IsDisplayFocused
        {
            get
            {
                return (bool)GetValue(IsDisplayFocusedProperty);
            }
            set
            {
                SetValue(IsDisplayFocusedProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IsDisplayFocusedProperty = DependencyProperty.Register("IsDisplayFocused", typeof(bool), typeof(LoginPasswordBox), new PropertyMetadata(false));

        /// <summary>
        /// 前面锁的图片是否显示
        /// </summary>
        public Visibility HeadVisibility
        {
            get
            {
                return (Visibility)GetValue(HeadVisibilityProperty);
            }
            set
            {
                SetValue(HeadVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty HeadVisibilityProperty = DependencyProperty.Register("HeadVisibility", typeof(Visibility), typeof(LoginPasswordBox), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 说明
        /// </summary>
        public string MarkValue
        {
            get
            {
                return (string)GetValue(MarkValueProperty);
            }
            set
            {
                SetValue(MarkValueProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty MarkValueProperty = DependencyProperty.Register("MarkValue", typeof(string), typeof(LoginPasswordBox), new PropertyMetadata(string.Empty));

        void LoginPasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            passwordBox = this.Template.FindName("newpb", this) as PasswordBox;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (!IsDisplayPassword && passwordBox != null)
            {
                passwordBox.Focus();

                passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { this.Text.Length, 0 });
            }
            else
            {
                this.Focus();
                this.Select(this.Text.Length, 0);
            }
        }
    }
}
