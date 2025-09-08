namespace WpfToolbox.Internal;

/// <summary>
/// Provides extension methods for retrieving custom attributes from enum fields or objects.
/// </summary>
internal static class AttributeHelper
{
    /// <summary>
    /// Gets the <see cref="FieldInfo"/> for the specified object's value.
    /// Typically used with enum values.
    /// </summary>
    /// <param name="item">The object (usually an enum value).</param>
    /// <returns>The <see cref="FieldInfo"/> corresponding to the object's value.</returns>
    public static FieldInfo? GetFieldInfo(this object item) => item.GetType().GetField(item.ToString()!);

    /// <summary>
    /// Retrieves a custom attribute of type <typeparamref name="T"/> from the specified object's field.
    /// </summary>
    /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
    /// <param name="item">The object (usually an enum value).</param>
    /// <returns>The attribute of type <typeparamref name="T"/>, or null if not found.</returns>
    public static T? GetCustomAttribute<T>(this object item) where T : Attribute
    {
        var fieldInfo = item.GetType().GetField(item.ToString()!);

        if (fieldInfo == null) return null;

        var res = Attribute.GetCustomAttribute(fieldInfo, typeof(T));
        return (T?)res; 
        //return (T?)Attribute.GetCustomAttribute(item.GetFieldInfo(), typeof(T));
    }

    /// <summary>
    /// Attempts to retrieve a custom attribute of type <typeparamref name="T"/> from the specified object's field.
    /// </summary>
    /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
    /// <param name="item">The object (usually an enum value).</param>
    /// <param name="res">When this method returns, contains the attribute if found; otherwise, a new instance of <typeparamref name="T"/>.</param>
    /// <returns><c>true</c> if the attribute was found; otherwise, <c>false</c>.</returns>
    public static bool TryGetCustomAttribute<T>(this object item, out T res) where T : Attribute, new()
    {
        var fieldInfo = item.GetFieldInfo();
        if (fieldInfo == null)
        {
            res = new T();
            return false;
        }
        T? attribute = (T?)Attribute.GetCustomAttribute(fieldInfo, typeof(T));
        res = attribute ?? new T();
        return attribute is not null;
    }
}
