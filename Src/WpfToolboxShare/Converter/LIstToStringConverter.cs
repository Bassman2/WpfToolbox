namespace WpfToolbox.Converter;

/// <summary>
/// Converts an <see cref="IEnumerable"/> to a single comma-separated <see cref="string"/> for display in the UI.
/// </summary>
[ValueConversion(typeof(IEnumerable), typeof(string))]
public class ListToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a list to a single comma-separated string.
    /// </summary>
    /// <param name="value">The value produced by the binding source, expected to be <see cref="IEnumerable"/>.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use (not used).</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// A comma-separated string if <paramref name="value"/> is an <see cref="IEnumerable"/>; otherwise, an empty string.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is IEnumerable list ? string.Join(", ", list.Cast<object>().Where(o => o is not null).Select(o => o.ToString())) : "";

    /// <summary>
    /// Not implemented. Converts a string back to a list.
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
