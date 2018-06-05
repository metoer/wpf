using System;
using System.Windows;
using System.Windows.Data;

namespace Hytera.EEMS.Gather.Converter
{
    public class VolumeValueConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            long strSize = 0;
            string result = string.Empty;
            long.TryParse((value ?? String.Empty).ToString(), out strSize);
            if (strSize <= 0)
            {
                return result = "---";
            }
            else
            {
                if (strSize < 1024.00)
                    result = strSize.ToString("F2") + " M";
                else if (strSize >= 1024)
                    result = (strSize / 1024.00).ToString("F2") + " G";
            }

            return result;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
