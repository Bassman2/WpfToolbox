namespace WpfToolbox.Behaviors;

public class BindingProxy : Freezable
{
    protected override Freezable CreateInstanceCore()
    {
        return new BindingProxy();
    }

    public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));

    public object Data
    {
        get => GetValue(DataProperty); 
        set => SetValue(DataProperty, value); 
    }
}
