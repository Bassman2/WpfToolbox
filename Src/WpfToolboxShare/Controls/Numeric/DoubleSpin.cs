namespace WpfToolbox.Controls;

public class DoubleSpin : NumericSpin<double>
{
    static DoubleSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleSpin), new FrameworkPropertyMetadata(typeof(DoubleSpin)));
    }
}

