namespace WpfToolbox.Controls;

public class IntegerSpin : NumericSpin<int>
{
    static IntegerSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerSpin), new FrameworkPropertyMetadata(typeof(IntegerSpin)));
    }
}