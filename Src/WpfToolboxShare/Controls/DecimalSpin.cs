namespace WpfToolbox.Controls;

public class DecimalSpin : ValueSpinControl<decimal>
{
    static DecimalSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalSpin), new FrameworkPropertyMetadata(typeof(DecimalSpin)));
    }
}

