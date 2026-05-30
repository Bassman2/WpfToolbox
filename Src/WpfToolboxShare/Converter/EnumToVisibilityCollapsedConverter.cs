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
        if (parameter.GetType().IsEnum)
        {
            var res = parameter.Equals(value) ? Visibility.Visible : Visibility.Collapsed;
            return res;
        }
        if (parameter is string parameterString)
        {
            var parameterValues = parameterString.Split('|');
            foreach (var paramValue in parameterValues)
            {
                if (Enum.TryParse(value.GetType(), paramValue, out object? enumValue) && enumValue.Equals(value))
                {
                    return Visibility.Visible;
                }
            }
        } 
        throw new ArgumentException("Parameter must be an enum value.");
    }
    

    /// <summary>
    /// Not supported. Throws <see cref="NotImplementedException"/> if called.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}