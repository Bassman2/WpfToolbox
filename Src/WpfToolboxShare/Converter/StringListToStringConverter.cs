namespace WpfToolbox.Converter;

/// <summary>
/// Converts an <see cref="IEnumerable{String}"/> to a single comma-separated <see cref="string"/> for display in the UI.
/// </summary>
[ValueConversion(typeof(IEnumerable<string>), typeof(string))]
public class StringListToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a list of strings to a single comma-separated string.
    /// </summary>
    /// <param name="value">The value produced by the binding source, expected to be <see cref="IEnumerable{String}"/>.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use (not used).</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// A comma-separated string if <paramref name="value"/> is an <see cref="IEnumerable{String}"/>; otherwise, an empty string.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is IEnumerable<string> list ? string.Join(", ", list) : "";

    /// <summary>
    /// Not implemented. Converts a string back to a list of strings.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>Throws <see cref="NotImplementedException"/> in all cases.</returns>
    /// <exception cref="NotImplementedException">Always thrown.</exception>

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
