namespace WpfToolbox.Converter;

/// <summary>
/// Converts an enum value to a Visibility value (Visible or Collapsed) based on parameter comparison.
/// Returns Visible when the value matches the parameter, otherwise Collapsed.
/// </summary>
[ValueConversion(typeof(Enum), typeof(Visibility))]
public class EnumToVisibilityCollapsedConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to a Visibility value.
    /// </summary>
    /// <param name="value">The enum value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The enum value to compare against.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>Visibility.Visible if value equals parameter, otherwise Visibility.Collapsed.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var res = parameter.Equals(value) ? Visibility.Visible : Visibility.Collapsed;
        return res;
    }
    

    /// <summary>
    /// Not supported. Throws <see cref="NotImplementedException"/> if called.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}