using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Numerics.NET;

namespace IntervalsDesktop.Utility;

public class BigFloatConverter : IValueConverter
{
    public static readonly BigFloatConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BigFloat bigFloat)
            return bigFloat.ToString("G", CultureInfo.InvariantCulture);
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            try
            {
                return BigFloat.Parse(s, CultureInfo.InvariantCulture);
            }
            catch
            {
                return new Avalonia.Data.BindingNotification(
                    new FormatException("Nieprawidłowy format."),
                    Avalonia.Data.BindingErrorType.Error);
            }
        }
        return Avalonia.Data.BindingOperations.DoNothing;
    }
}