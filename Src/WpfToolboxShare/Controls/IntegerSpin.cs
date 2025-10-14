namespace WpfToolbox.Controls;

public class IntegerSpin : ValueSpinControl<int>
{
    static IntegerSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerSpin), new FrameworkPropertyMetadata(typeof(IntegerSpin)));
    }
}