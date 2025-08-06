namespace WpfToolbox.Internal;

internal static class BindingHelper
{
    public static Type? GetBindingType(this BindingBase binding, object obj)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        return property?.PropertyType;
    }

    public static object? GetBindingValue(this BindingBase binding, object obj)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        return property?.GetValue(obj);
    }

    public static string? GetBindingText(this BindingBase binding, object obj)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        return property?.GetValue(obj)?.ToString();
    }

    public static void SetBindingHandler(this BindingBase binding, object obj, EventHandler handler)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        property?.AddValueChanged(obj, handler);
    }
}
