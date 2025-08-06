namespace WpfToolbox.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class ResourceAttribute(string name = "") : Attribute
{    
    public static readonly ResourceAttribute Default = new();

    public virtual string Name { get => NameValue; }

    protected string NameValue { get; set; } = name;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ResourceAttribute other && other.Name == Name;
    public override int GetHashCode() => Name?.GetHashCode() ?? 0;
    
    public override bool IsDefaultAttribute() => Equals(Default);
}
