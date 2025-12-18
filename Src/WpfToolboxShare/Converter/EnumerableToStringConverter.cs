namespace WpfToolbox.Converter;

[ValueConversion(typeof(IEnumerable), typeof(string))]
public class EnumerableToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        => value is IEnumerable items ? string.Join(Environment.NewLine, items.Cast<object>().Select(g => $"* {g.ToString()}")) : null;

    public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        =>  throw new NotImplementedException();
}
