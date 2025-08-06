namespace WpfToolbox.Controls;

public class DecimalSlider : NumericSlider<decimal>
{
    static DecimalSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalSlider), new FrameworkPropertyMetadata(typeof(DecimalSlider)));
    }
}
