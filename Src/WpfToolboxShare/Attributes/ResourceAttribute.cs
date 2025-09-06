namespace WpfToolbox.Attributes;

/// <summary>
/// Specifies a resource name for a target element.
/// Can be applied to any program element to associate it with a named resource.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class ResourceAttribute(string name = "") : Attribute
{
    /// <summary>
    /// The default instance of <see cref="ResourceAttribute"/> with an empty name.
    /// </summary>
    public static readonly ResourceAttribute Default = new();

    /// <summary>
    /// Gets the resource name associated with the target element.
    /// </summary>
    public virtual string Name { get => NameValue; }

    /// <summary>
    /// The backing value for the <see cref="Name"/> property.
    /// </summary>
    protected string NameValue { get; set; } = name;

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="ResourceAttribute"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current attribute.</param>
    /// <returns><c>true</c> if the specified object is a <see cref="ResourceAttribute"/> with the same name; otherwise, <c>false</c>.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) 
        => obj is ResourceAttribute other && other.Name == Name;

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => Name?.GetHashCode() ?? 0;

    /// <summary>
    /// Determines whether this instance is the default attribute.
    /// </summary>
    /// <returns><c>true</c> if this instance is the default; otherwise, <c>false</c>.</returns>
    public override bool IsDefaultAttribute() => Equals(Default);
}
