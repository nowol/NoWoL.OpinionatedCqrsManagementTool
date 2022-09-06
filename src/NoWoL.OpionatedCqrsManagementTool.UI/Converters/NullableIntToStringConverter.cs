using System.Globalization;
using System.Text.RegularExpressions;

namespace CodeGen.UI.Converters
{
    public class NullableIntToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }

            return value.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value as string))
            {
                return null;
            }

            var v = (string)value;
            
            v = Regex.Replace(v, @"[^\d-]", string.Empty);

            if (int.TryParse(v,
                             out int result))
            {
                return result;
            }

            return null;
        }
    }
}
