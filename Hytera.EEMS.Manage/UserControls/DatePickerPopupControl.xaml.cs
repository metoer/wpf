using System;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// DatePickerPopupControl.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerPopupControl : UserControl
    {
        public static readonly DependencyProperty TextMarkProperty = DependencyProperty.Register("TextMark", typeof(string), typeof(DatePickerPopupControl));

        public string TextMark
        {
            get
            {
                return (string)GetValue(TextMarkProperty);
            }
            set
            {
                SetValue(TextMarkProperty, value);
            }
        }

        
        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register("TextForeground", typeof(string), typeof(DatePickerPopupControl));

        public string TextForeground
        {
            get
            {
                return (string)GetValue(TextForegroundProperty);
            }
            set
            {
                SetValue(TextForegroundProperty, value);
            }
        }
        public event Action<string> CheckTimeEvent = null;
        public DatePickerPopupControl()
        {
            InitializeComponent();
        }

        private void btnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            btnDateFilter.IsEnabled = false;
            try
            {
                ucDatePicker.SetSelectedDate(DateTime.Parse(txtDate.Text));
                popPanel.IsOpen = true;              
            }
            finally
            {
                btnDateFilter.IsEnabled = true;
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            ucDatePicker.OKClicked += new RoutedEventHandler(ucDatePicker_OKClicked);
            ucDatePicker.CancelClicked += new RoutedEventHandler(ucDatePicker_CancelClicked);
        }

        private void ucDatePicker_OKClicked(object sender, RoutedEventArgs e)
        {
            popPanel.IsOpen = false;
            if (CheckTimeEvent != null)
                CheckTimeEvent(ucDatePicker.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        void ucDatePicker_CancelClicked(object sender, RoutedEventArgs e)
        {
            popPanel.IsOpen = false;
        }
    }
}
