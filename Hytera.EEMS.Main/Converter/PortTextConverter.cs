using System;
using System.Windows.Data;

namespace Hytera.EEMS.Main.Converter
{
    /// <summary>
    /// 转为为Bool
    /// </summary>
    public class PortTextConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return App.Current.TryFindResource("appMainInventedPort").ToString() + (value.ToString().Length == 1 ? value + " " : value);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
