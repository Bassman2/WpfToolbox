namespace WpfToolbox.Converter;

/// <summary>
/// Converts an enum value to its corresponding resource string using <see cref="EntryAssemblyResourceManager"/>.
/// If the resource string is not found, returns the enum value itself.
/// </summary>
[ValueConversion(typeof(object), typeof(string))]
public class ResourceConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to a resource string. If the value is not an enum or no resource is found, returns the value.
    /// </summary>
    /// <param name="value">The value to convert, typically an enum.</param>
    /// <param name="targetType">The target binding type (unused).</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture to use in the converter (unused).</param>
    /// <returns>The resource string if found; otherwise, the original value.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum)
        {
            return EntryAssemblyResourceManager.GetString(value) ?? value;
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
