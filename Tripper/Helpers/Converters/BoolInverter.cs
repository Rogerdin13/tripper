using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripper.Helpers.Converters
{
    public class BoolInverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => !(bool)value;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => !(bool)value;
    }
}
