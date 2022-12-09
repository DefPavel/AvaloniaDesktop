using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaDesktop.Helpers;

public class CustomDateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ((DateTime?)value)?.ToString("dd.MM.yyyy", culture);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isSuccess = DateTime.TryParseExact((string)value!, "dd.MM.yyyy", culture, DateTimeStyles.None, out _);
        return (isSuccess ? DateTime.ParseExact((string)value!, "dd.MM.yyyy", culture) : value) 
               ?? throw new InvalidOperationException();
    }
}