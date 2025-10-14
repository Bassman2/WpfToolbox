namespace WpfToolbox.Controls.Primitives;

public abstract class ValueSliderControl<T> : ValueControl<T> where T : IFormattable
{
    static ValueSliderControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueSliderControl<T>), new FrameworkPropertyMetadata(typeof(ValueSliderControl<T>)));
    }
}
