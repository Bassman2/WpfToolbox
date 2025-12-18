namespace WpfToolbox.Converter;

[ValueConversion(typeof(bool), typeof(FontWeight))]
public class BoolToBoldConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? FontWeights.ExtraBold : FontWeights.Normal;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
