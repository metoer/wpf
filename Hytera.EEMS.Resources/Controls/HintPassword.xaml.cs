using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Resources.Controls
{

    /// <summary>
    /// HintTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class HintPassword : UserControl
    {
        public event RoutedEventHandler PasswordChanged;

        /// <summary>
        /// 输入值
        /// </summary>
        public string Password
        {
            get
            {
                return (string)GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(HintPassword), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 提示
        /// </summary>
        public string Hint
        {
            get
            {
                return (string)GetValue(HintProperty);
            }
            set
            {
                SetValue(HintProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(HintPassword), new PropertyMetadata(string.Empty));

        TextBlock tbk1;

        public HintPassword()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(HintPassword_Loaded);
        }

        public void SetFocus()
        {
            tbText.Focus();
        }

        void HintPassword_Loaded(object sender, RoutedEventArgs e)
        {
            tbk1 = tbText.Template.FindName("tbk1", tbText) as TextBlock;
            if (tbk1 != null)
            {
                tbk1.Visibility = string.IsNullOrEmpty(tbText.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
            tbText.PasswordChanged += tbText_PasswordChanged;
        }

        void tbText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.Password = tbText.Password;

            if (this.PasswordChanged != null)
            {
                this.PasswordChanged(this, e);
            }

            if (tbk1 != null)
            {
                tbk1.Visibility = string.IsNullOrEmpty(tbText.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
