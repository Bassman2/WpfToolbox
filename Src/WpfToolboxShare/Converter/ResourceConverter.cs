using WpfToolbox.Internal;

namespace WpfToolbox.Converter;

[ValueConversion(typeof(object), typeof(string))]
public class ResourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum)
        {
            return EntryAssemblyResourceManager.GetString(value) ?? value;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
