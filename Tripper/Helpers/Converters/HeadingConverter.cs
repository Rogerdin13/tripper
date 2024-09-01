using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripper.Helpers.Converters
{
    public class HeadingConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!int.TryParse(Math.Floor((double)value).ToString(), out var headingValue)) return value;
            if (0   <= headingValue && headingValue <= 20) { return $"{headingValue}° N"; }
            if (21  <= headingValue && headingValue <= 70) { return $"{headingValue}° NO"; }
            if (71  <= headingValue && headingValue <= 110) { return $"{headingValue}° O"; }
            if (111 <= headingValue && headingValue <= 160) { return $"{headingValue}° SO"; }
            if (161 <= headingValue && headingValue <= 200) { return $"{headingValue}° S"; }
            if (201 <= headingValue && headingValue <= 250) { return $"{headingValue}° SW"; }
            if (251 <= headingValue && headingValue <= 290) { return $"{headingValue}° W"; }
            if (291 <= headingValue && headingValue <= 340) { return $"{headingValue}° NW"; }
            if (341 <= headingValue && headingValue <= 360) { return $"{headingValue}° N"; }
            return "0° N";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!double.TryParse(((string)value).Split("°")[0], out var result)) return value;
            return result;
        }
    }
}
