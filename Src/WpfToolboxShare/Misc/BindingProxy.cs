namespace WpfToolbox.Misc;

/// <summary>
/// A WPF <see cref="Freezable"/> used as a binding proxy to enable data binding in scenarios where the DataContext is not accessible,
/// such as in resource dictionaries or style setters. This allows binding to data from places where standard data binding is not possible.
/// </summary>
public class BindingProxy : Freezable
{
    /// <summary>
    /// Creates a new instance of the <see cref="BindingProxy"/> class.
    /// </summary>
    /// <returns>A new <see cref="BindingProxy"/> instance.</returns>
    protected override Freezable CreateInstanceCore()
    {
        return new BindingProxy();
    }

    /// <summary>
    /// Identifies the <see cref="Data"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DataProperty = 
        DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));

    /// <summary>
    /// Gets or sets the data object to be proxied for binding.
    /// </summary>
    public object Data
    {
        get => GetValue(DataProperty); 
        set => SetValue(DataProperty, value); 
    }
}
