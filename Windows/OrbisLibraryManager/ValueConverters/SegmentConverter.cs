using OrbisLib2.Targets;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrbisLibraryManager.ValueConverters
{
    public class SegmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var outputStr = string.Empty;
            //var segments = value as SegmentInfo[];
            //if(segments != null)
            //{
            //    foreach (var segment in segments)
            //    {
            //        if(segment.Address > 0 && segment.Size > 0)
            //        {
            //            outputStr += $"0x{segment.Address.ToString("X")}({segment.Size}) ";
            //        }
            //    }
            //}
            //
            //return string.IsNullOrEmpty(outputStr) ? "-" : outputStr;
            return "Fix me.";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
            //return new SegmentInfo[0];
        }
    }
}
