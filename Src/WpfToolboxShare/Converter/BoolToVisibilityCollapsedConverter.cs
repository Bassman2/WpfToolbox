namespace WpfToolbox.Converter;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class BoolToVisibilityCollapsedConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to the corresponding image.
    /// </summary>
    /// <param name="value">The value to convert (should be a boolean).</param>
    /// <param name="targetType">The target type (unused).</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture to use in the converter (unused).</param>
    /// <returns>The working or not-working image.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Visibility.Visible : Visibility.Collapsed;

    /// <summary>
    /// Not supported. Throws <see cref="NotImplementedException"/> if called.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

