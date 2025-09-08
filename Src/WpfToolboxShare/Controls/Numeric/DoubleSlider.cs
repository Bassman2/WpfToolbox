namespace WpfToolbox.Controls;

/// <summary>
/// A WPF slider control specialized for double-precision floating-point values.
/// Inherits numeric slider functionality (min/max, increment, formatting, etc.) from <see cref="NumericSlider{Double}"/>.
/// </summary>
public class DoubleSlider : NumericSlider<double>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static DoubleSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleSlider), new FrameworkPropertyMetadata(typeof(DoubleSlider)));
    }
}
