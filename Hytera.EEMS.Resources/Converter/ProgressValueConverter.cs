using System;
using System.Windows;
using System.Windows.Data;

namespace Hytera.EEMS.Resources.Converter
{
    public class ProgressValueConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double pbValue = 0;
            double.TryParse((value ?? String.Empty).ToString(), out pbValue);
            if (pbValue <= 0 || pbValue == 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
