namespace WpfToolbox.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ImageAttribute(string source = "") : Attribute
{
    public static readonly ImageAttribute Default = new();

    public virtual string Source { get => SourceValue; }

    protected string SourceValue { get; set; } = source;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ImageAttribute other && other.Source == Source;
    public override int GetHashCode() => Source?.GetHashCode() ?? 0;

    public override bool IsDefaultAttribute() => Equals(Default);
}
