namespace WpfToolbox.Converter;


[ValueConversion(typeof(object), typeof(string))]
public class DescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || (value is string str && string.IsNullOrEmpty(str)))
        {
            return string.Empty;
        }
        if (value is Enum)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString()!);
            return (fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description)!;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
