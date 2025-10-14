namespace WpfToolbox.Controls;

public class IntegerSlider : ValueSliderControl<int>
{
    static IntegerSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerSlider), new FrameworkPropertyMetadata(typeof(IntegerSlider)));
    }
}