namespace WpfToolbox.Controls;

/// <summary>
/// A WPF numeric spin control specialized for decimal values.
/// Inherits all numeric editing, increment/decrement, and formatting behavior from <see cref="NumericSpin{Decimal}"/>.
/// </summary>
public class DecimalSpin : NumericSpin<decimal>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static DecimalSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalSpin), new FrameworkPropertyMetadata(typeof(DecimalSpin)));
    }
}

