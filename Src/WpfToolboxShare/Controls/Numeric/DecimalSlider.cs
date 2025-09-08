namespace WpfToolbox.Controls;

/// <summary>
/// A WPF slider control specialized for decimal values.
/// Inherits all numeric slider functionality (min/max, increment, formatting, etc.) from <see cref="NumericSlider{Decimal}"/>.
/// </summary>
public class DecimalSlider : NumericSlider<decimal>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static DecimalSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalSlider), new FrameworkPropertyMetadata(typeof(DecimalSlider)));
    }
}
