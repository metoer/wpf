using System;
using System.Windows.Data;

namespace Hytera.EEMS.Main.Converter
{
    public class Int32Converter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int dataValue = 1;

            bool result = Int32.TryParse((value ?? string.Empty).ToString(), out dataValue);

            if (result)
            {
                return dataValue == 0;
            }
            else
            {
                return result;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
