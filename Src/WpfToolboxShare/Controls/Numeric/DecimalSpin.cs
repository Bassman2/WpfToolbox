namespace WpfToolbox.Controls;

public class DecimalSpin : NumericSpin<decimal>
{
    static DecimalSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalSpin), new FrameworkPropertyMetadata(typeof(DecimalSpin)));
    }
}

