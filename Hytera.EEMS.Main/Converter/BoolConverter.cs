﻿using System;
using System.Windows.Data;

namespace Hytera.EEMS.Main.Converter
{
    /// <summary>
    /// 转为为Bool
    /// </summary>
    public class BoolConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool data = false;

            bool.TryParse((value ?? string.Empty).ToString(), out data);

            return data;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
