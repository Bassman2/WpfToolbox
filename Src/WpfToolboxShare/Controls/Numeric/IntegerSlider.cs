namespace WpfToolbox.Controls;

/// <summary>
/// A WPF slider control specialized for integer values.
/// Inherits numeric slider functionality (min/max, increment, formatting, etc.) from <see cref="NumericSlider{Integer}"/>.
/// </summary>
public class IntegerSlider : NumericSlider<int>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static IntegerSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerSlider), new FrameworkPropertyMetadata(typeof(IntegerSlider)));
    }
}