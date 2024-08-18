using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripper.Helpers.Converters
{
    public class DistanceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!double.TryParse(value.ToString(), out var rawDistance)) return value;
            return ((int)Math.Floor(rawDistance)).ToString("##,#");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!double.TryParse(value.ToString(), out var distance)) return value;
            return distance;
        }
    }
}
