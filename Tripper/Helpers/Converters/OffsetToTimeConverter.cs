using System.Globalization;

namespace Tripper.Helpers.Converters;

/// <summary>
///     takes a DateTimeOffset and just returns the Time as string
/// </summary>
public class OffsetToTimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ((DateTimeOffset)value).ToString("HH:mm:ss");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
