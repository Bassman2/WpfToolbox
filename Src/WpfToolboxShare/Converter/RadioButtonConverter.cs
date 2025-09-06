namespace WpfToolbox.Converter;

/// <summary>
/// Converts an object value to a boolean for use with radio buttons in WPF.
/// Returns true if the value equals the parameter, enabling radio button selection binding.
/// The ConvertBack method returns the parameter, allowing two-way binding from the radio button to the bound value.
/// </summary>
[ValueConversion(typeof(object), typeof(bool))]
public class RadioButtonConverter : IValueConverter
{
    /// <summary>
    /// Returns true if the bound value equals the parameter; otherwise, false.
    /// Used to determine if a radio button should be checked.
    /// </summary>
    /// <param name="value">The current bound value.</param>
    /// <param name="targetType">The target type (unused).</param>
    /// <param name="parameter">The radio button's value to compare against.</param>
    /// <param name="culture">The culture to use in the converter (unused).</param>
    /// <returns>True if value equals parameter; otherwise, false.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return parameter.Equals(value);
    }

    /// <summary>
    /// Returns the parameter, which represents the value associated with the radio button.
    /// Used to update the bound value when a radio button is checked.
    /// </summary>
    /// <param name="value">The value from the target (unused).</param>
    /// <param name="targetType">The target type (unused).</param>
    /// <param name="parameter">The radio button's value.</param>
    /// <param name="culture">The culture to use in the converter (unused).</param>
    /// <returns>The parameter value.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return parameter;
    }
}
