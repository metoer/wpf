using System;
using System.Windows;
using System.Windows.Data;

namespace Hytera.EEMS.Fingerprint.Converter
{
    public class EmptyTextConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty((value ?? string.Empty).ToString()))
            {
                return "---";
            }
            else
            {
                return value;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
