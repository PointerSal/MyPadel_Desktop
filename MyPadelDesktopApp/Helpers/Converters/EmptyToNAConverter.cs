using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Helpers.Converters
{
    public class EmptyToNAConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((value is string str && string.IsNullOrWhiteSpace(str)) || value == null)
                    return "N/A";
            }
            catch (Exception ex) { }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && str == "N/A")
            {
                return string.Empty;
            }
            return value;
        }
    }
}