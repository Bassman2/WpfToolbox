namespace WpfToolbox.Controls;

public class DoubleSlider : ValueSliderControl<double>
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static DoubleSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleSlider), new FrameworkPropertyMetadata(typeof(DoubleSlider)));
    }
}
