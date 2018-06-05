using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Hytera.EEMS.Manage.Converter
{
    public class LeafIndentConverter : IValueConverter
    {
        public double Indent { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value as OrgInfos;
            if (item == null)
            {
                return 0;
            }
            return Indent * item.Level;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
