using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WpfToolbox.Validations;

public partial class LdpaFilterValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value is string str && !string.IsNullOrWhiteSpace(str) && LdapFilterRegex().IsMatch(str)
            ? ValidationResult.Success! : new ValidationResult($"The field {validationContext.MemberName} must be a valid LDAP filter.");
    }

    private const string reg1 = @"^\([&|!](\(.*\))*\)$";

    [GeneratedRegex(reg1, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline)]
    private static partial Regex LdapFilterRegex();
}
