namespace WpfToolbox.Controls;

public class DoubleSpin : ValueSpinControl<double>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static DoubleSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleSpin), new FrameworkPropertyMetadata(typeof(DoubleSpin)));
    }
}

