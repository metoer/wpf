using System;
using System.Windows;
using System.Windows.Data;

namespace Hytera.EEMS.Gather.Converter
{
    public class BoolVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool hascontent = false;
            bool.TryParse((value ?? String.Empty).ToString(), out hascontent);
            string data = parameter.ToString();
            if (data.Equals("0"))
            {
                return hascontent ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return hascontent ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
