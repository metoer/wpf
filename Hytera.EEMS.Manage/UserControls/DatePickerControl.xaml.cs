using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// DatePickerControl.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerControl : UserControl
    {
        private DateTime _SelectedDate;
        internal event RoutedEventHandler OKClicked;
        internal event RoutedEventHandler CancelClicked;
        private int Year ;
        private int Month ;
        private int Day ;
        private int Hour ;
        private int Min ;
        private int Sec ;

        public DateTime SelectedDate
        {
            get
            {
                DateTime dt = new DateTime(_SelectedDate.Year, _SelectedDate.Month, _SelectedDate.Day, _SelectedDate.Hour, _SelectedDate.Minute, _SelectedDate.Second);
                return dt;
            }
            set { _SelectedDate = value; }
        }

        private bool isLoad=false;
        public DatePickerControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoad)
            {
                List<string> yvalues = new List<string>();
                for (int i = 2000; i < 2101; i++)
                {
                    yvalues.Add(i.ToString());
                }
                rollYear.Values = yvalues;

                List<string> mvalues = new List<string>();
                for (int i = 1; i < 13; i++)
                {
                    mvalues.Add(i.ToString().PadLeft(2, '0'));
                }
                rollMonth.Values = mvalues;

                List<string> dvalues = new List<string>();
                for (int i = 1; i < 32; i++)
                {
                    dvalues.Add(i.ToString().PadLeft(2, '0'));
                }
                rollDay.Values = dvalues;

                List<string> hvalues = new List<string>();
                for (int i = 0; i < 24; i++)
                {
                    hvalues.Add(i.ToString().PadLeft(2, '0'));
                }
                rollHour.Values = hvalues;

                List<string> svalues = new List<string>();
                for (int i = 0; i < 60; i++)
                {
                    svalues.Add(i.ToString().PadLeft(2,'0'));
                }
                rollSec.Values = svalues;
                rollMin.Values = svalues;
                isLoad = true;
            }
        }
        
        private void rollMonth_MouseLeave(object sender, MouseEventArgs e)
        {
            int year = 2010;
            Int32.TryParse(rollYear.Text.ToString(), out year);
            int month = 1;
            Int32.TryParse(rollMonth.Text.ToString(), out month);

            //当前选择的日期所在月的天数
            int days = DateTime.DaysInMonth(year, month);
            rollDay.Values.Clear();
            List<string> values = new List<string>();
            for (int i = 1; i < days + 1; i++)
            {
                values.Add(i.ToString().PadLeft(2, '0'));
            }

            rollDay.Values = values;

            Int32.TryParse(rollDay.Text, out Day);
            if (Day > days)
            {
                rollDay.Text = "01";
                rollDay.Focus();
            }
        }

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(rollYear.Text, out Year);
            Int32.TryParse(rollMonth.Text, out Month);
            Int32.TryParse(rollDay.Text, out Day);
            Int32.TryParse(rollHour.Text, out Hour);
            Int32.TryParse(rollMin.Text, out Min);
            Int32.TryParse(rollSec.Text, out Sec);

            _SelectedDate = new DateTime(Year, Month, Day, Hour, Min, Sec);

            if (OKClicked != null)
            {
                OKClicked(sender, e);
            }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClicked != null)
            {
                CancelClicked(sender, e);
            }
        }

        public void SetSelectedDate(DateTime dt)
        {
            rollYear.Text = dt.Year.ToString();
            rollMonth.Text = dt.Month.ToString().PadLeft(2, '0');
            rollDay.Text = dt.Day.ToString().PadLeft(2, '0');
            rollHour.Text = dt.Hour.ToString().PadLeft(2, '0');
            rollMin.Text = dt.Minute.ToString().PadLeft(2, '0');
            rollSec.Text = dt.Second.ToString().PadLeft(2, '0');
        }
    }
}
