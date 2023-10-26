using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OrbisLibraryManager.ValueConverters
{
    public class HexNumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ulong num)
            {
                return $"0x{num.ToString("X")}";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            if (!ulong.TryParse(str.Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var val))
            {
                return 0;
            }

            return val;
        }
    }
}
