namespace WpfToolbox.Interfaces;

public interface IRootValidator
{
    void ChildHasErrors(LeafViewModel child, string? propertyName);
}
