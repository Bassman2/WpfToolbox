namespace WpfToolbox.Controls;

/// <summary>
/// Represents a numeric spin control for double-precision floating-point values.
/// Inherits all functionality from <see cref="NumericSpin{Double}"/> and associates the control with its default style.
/// </summary>
public class DoubleSpin : NumericSpin<double>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static DoubleSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleSpin), new FrameworkPropertyMetadata(typeof(DoubleSpin)));
    }
}

