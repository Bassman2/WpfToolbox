namespace WpfToolbox.Attributes;

/// <summary>
/// Specifies an image source for a field, typically used with enum fields to associate an image resource.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ImageAttribute(string source = "") : Attribute
{
    /// <summary>
    /// The default instance of <see cref="ImageAttribute"/> with an empty source.
    /// </summary>
    public static readonly ImageAttribute Default = new();

    /// <summary>
    /// Gets the image source associated with the field.
    /// </summary>
    public virtual string Source { get => SourceValue; }

    /// <summary>
    /// The backing value for the <see cref="Source"/> property.
    /// </summary>
    protected string SourceValue { get; set; } = source;

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="ImageAttribute"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current attribute.</param>
    /// <returns><c>true</c> if the specified object is an <see cref="ImageAttribute"/> with the same source; otherwise, <c>false</c>.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) 
        => obj is ImageAttribute other && other.Source == Source;

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => Source?.GetHashCode() ?? 0;

    /// <summary>
    /// Determines whether this instance is the default attribute.
    /// </summary>
    /// <returns><c>true</c> if this instance is the default; otherwise, <c>false</c>.</returns>
    public override bool IsDefaultAttribute() => Equals(Default);
}
