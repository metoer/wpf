using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{

    /// <summary>
    /// HintTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class HintTextBox : UserControl
    {
        public event TextChangedEventHandler TextChanged;

        public event RoutedEventHandler TextLostFocus;

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);            
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(HintTextBox), new PropertyMetadata(string.Empty));


        public void FocusToText()
        {
            tbText.Focus();
        }

        public TextBox TextBox
        {
            get
            {
                return tbText;
            }
        }

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
        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(HintTextBox), new PropertyMetadata(string.Empty));

        public HintTextBox()
        {

            InitializeComponent();

            Loaded += new RoutedEventHandler(HintTextBox_Loaded);
        }

        public void SetFocus()
        {
            tbText.Focus();
        }

        void HintTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tbk1 = tbText.Template.FindName("tbk1", tbText) as TextBlock;
            if (tbk1 != null)
            {
                tbk1.Visibility = string.IsNullOrEmpty(tbText.Text) ? Visibility.Visible : Visibility.Collapsed;
            }

            tbText.TextChanged += (sender1, e1) =>
            {
                this.Text = tbText.Text;
                if (TextChanged != null)
                {
                    TextChanged(this, e1);
                }

                if (tbk1 != null)
                {
                    tbk1.Visibility = string.IsNullOrEmpty(tbText.Text) ? Visibility.Visible : Visibility.Collapsed;
                }
            };

            tbText.LostFocus += tbText_LostFocus;

            new Thread(() =>
            {
                Thread.Sleep(100);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tbText.Focus();
                }));
            }) { IsBackground = true }.Start();
        }

        private void tbText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextLostFocus != null)
            {
                TextLostFocus(this, e);
            }
        }

    }
}
