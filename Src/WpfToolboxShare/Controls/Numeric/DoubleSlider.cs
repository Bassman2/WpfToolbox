namespace WpfToolbox.Controls;

public class DoubleSlider : NumericSlider<double>
{
    static DoubleSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleSlider), new FrameworkPropertyMetadata(typeof(DoubleSlider)));
    }
}
