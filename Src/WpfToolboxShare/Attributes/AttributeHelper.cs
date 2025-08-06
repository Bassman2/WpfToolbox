namespace WpfToolbox.Attributes;

internal static class AttributeHelper
{
    public static FieldInfo GetFieldInfo(this object item) => item.GetType().GetField(item.ToString()!)!;

    public static T? GetCustomAttribute<T>(this object item) where T : Attribute
    {
        return (T?)Attribute.GetCustomAttribute(item.GetFieldInfo(), typeof(T));
    }

    public static bool TryGetCustomAttribute<T>(this object item, out T res) where T : Attribute, new()
    {
        T? attribute = (T?)Attribute.GetCustomAttribute(item.GetFieldInfo(), typeof(T));
        res = attribute ?? new T();
        return attribute is not null;
    }
}
