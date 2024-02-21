using System.IO;
using System.Windows.Data;

namespace OrbisLib2.Common.ValueConverters
{
    public class FilePathShortenerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Check if the value is a string (file path)
            if (value is string filePath)
            {
                // Split the file path into its components
                string[] pathParts = filePath.Split(Path.DirectorySeparatorChar);

                // Ensure there is more than 3 path levels.
                if (pathParts.Length <= 3)
                    return value;

                // Extract the last three folders, file name, and extension
                string[] shortenedParts = pathParts[^4..]; // Use slice notation

                // Join the shortened parts back together
                return $"...\\{Path.Combine(shortenedParts)}";
            }

            // If the value is not a string, return it as is
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // This converter is one-way and does not support converting back
            throw new NotImplementedException();
        }
    }
}
