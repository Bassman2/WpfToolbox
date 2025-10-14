namespace WpfToolbox.Controls.Primitives;

public abstract class ValueSpinControl<T> : ValueControl<T> where T : IFormattable
{
    static ValueSpinControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueSpinControl<T>), new FrameworkPropertyMetadata(typeof(ValueSpinControl<T>)));
    }
}
