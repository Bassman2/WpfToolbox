using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WpfToolbox.Interfaces;

public interface IRootValidator
{
    void ChangeChildErrors(LeafViewModel child, string? propertyName, IEnumerable<ValidationResult> results);
}
