using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WpfToolbox.Validations;

public partial class LdapPathValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value is string str && !string.IsNullOrWhiteSpace(str) && LdapPathRegex().IsMatch(str)
            ? ValidationResult.Success! : new ValidationResult($"The field {validationContext.MemberName} must be a valid LDAP path.");
    }

    [GeneratedRegex("^(?:(CN|OU|DC|O|C)=[^,]+)(?:,(?:(CN|OU|DC|O|C)=[^,]+))*$", RegexOptions.IgnoreCase)]
    private static partial Regex LdapPathRegex();
}
