namespace WpfToolbox.Converter;

/// <summary>
/// Converts an enum value to its <see cref="DescriptionAttribute"/> value if present; otherwise, returns the enum's string representation.
/// If the value is null or an empty string, returns an empty string.
/// </summary>
[ValueConversion(typeof(object), typeof(string))]
public class DescriptionConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to its description, or returns the value itself if no description is found.
    /// </summary>
    /// <param name="value">The value to convert, typically an enum.</param>
    /// <param name="targetType">The target binding type (unused).</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture to use in the converter (unused).</param>
    /// <returns>The description string, the value's string representation, or an empty string.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || (value is string str && string.IsNullOrEmpty(str)))
        {
            return string.Empty;
        }
        if (value is Enum)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString()!);
            return (fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description)!;
        }
        return value;
    }

    /// <summary>
    /// Not supported. Throws <see cref="NotSupportedException"/> if called.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
