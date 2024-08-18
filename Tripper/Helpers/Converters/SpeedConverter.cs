using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripper.Helpers.Converters
{
    public class SpeedConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!double.TryParse(value.ToString(), out var meterPerSeconds)) return value;
            var kilometersPerHour = meterPerSeconds * 3.6;
            return (int)Math.Floor(kilometersPerHour);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!int.TryParse(value.ToString(), out var kilometersPerHour)) return value;
            var meterPerSeconds = kilometersPerHour / 3.6;
            return meterPerSeconds;
        }
    }
}
