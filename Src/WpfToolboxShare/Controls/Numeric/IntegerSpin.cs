namespace WpfToolbox.Controls;

/// <summary>
/// A WPF numeric spin control specialized for integer values.
/// Inherits all numeric editing, increment/decrement, and formatting behavior from <see cref="NumericSpin{Integer}"/>.
/// </summary>
public class IntegerSpin : NumericSpin<int>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static IntegerSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerSpin), new FrameworkPropertyMetadata(typeof(IntegerSpin)));
    }
}