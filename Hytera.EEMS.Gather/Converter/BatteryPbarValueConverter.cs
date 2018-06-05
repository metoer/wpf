using System;
using System.Windows;
using System.Windows.Data;

namespace Hytera.EEMS.Gather.Converter
{
    public class BatteryPbarValueConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double pbValue = 0;
            double.TryParse((value ?? String.Empty).ToString(), out pbValue);
            if (pbValue <= 0)
            {
                return 0;
            }
            else if (pbValue > 0 && pbValue <= 29)
            {
                return 29;
            }

            else if (pbValue > 29 && pbValue <= 50)
            {
                return 42;
            }
            else if (pbValue > 50 && pbValue <= 65)
            {
                return 55;
            }
            else if (pbValue > 60 && pbValue <= 80)
            {
                return 70;
            }
            else
            {
                return 100;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
