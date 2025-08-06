namespace WpfToolbox.Behaviours;

public class ConverterBindableBinding : MarkupExtension
{
    public Binding? Binding { get; set; }
    public IValueConverter? Converter { get; set; }
    public Binding? ConverterParameterBinding { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        MultiBinding multiBinding = new();
        multiBinding.Bindings.Add(Binding);
        multiBinding.Bindings.Add(ConverterParameterBinding);
        MultiValueConverterAdapter adapter = new() { Converter = Converter };
        multiBinding.Converter = adapter;
        return multiBinding.ProvideValue(serviceProvider);
    }
}

[ContentProperty("Converter")]
public class MultiValueConverterAdapter : IMultiValueConverter
{
    public IValueConverter? Converter { get; set; }

    private object? lastParameter;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (Converter == null)
        {
            // Required for VS design-time
            return values[0];  
        }
        if (values.Length > 1)
        {
            lastParameter = values[1];
        }
        return Converter.Convert(values[0], targetType, lastParameter, culture);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        if (Converter == null)
        {
            // Required for VS design-time
            return [ value ]; 
        }
        return [ Converter.ConvertBack(value, targetTypes[0], lastParameter, culture) ];
    }
}
