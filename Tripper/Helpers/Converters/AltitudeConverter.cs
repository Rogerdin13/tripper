using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripper.Helpers.Converters
{
    public class AltitudeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!double.TryParse(value.ToString(), out var meters)) return value;
            return (int)Math.Floor(meters);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!int.TryParse(value.ToString(), out var meters)) return value;
            return meters;
        }
    }
}
