namespace WpfToolbox.Converter;

[ValueConversion(typeof(bool), typeof(Color))]
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Brushes.Black : Brushes.Gray;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

