namespace WpfToolbox.Controls;

public class DecimalSlider : ValueSliderControl<decimal>
{
    static DecimalSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalSlider), new FrameworkPropertyMetadata(typeof(DecimalSlider)));
    }
}
