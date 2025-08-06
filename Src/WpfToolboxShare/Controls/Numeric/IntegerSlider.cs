namespace WpfToolbox.Controls;

public class IntegerSlider : NumericSlider<int>
{
    static IntegerSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerSlider), new FrameworkPropertyMetadata(typeof(IntegerSlider)));
    }
}