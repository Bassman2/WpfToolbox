using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WpfToolbox.Validations;

/// <summary>
/// Specifies that a data field value is a valid HTTP or HTTPS URL.
/// </summary>
public sealed class UrlValidationAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UrlAttribute"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property to validate.</param>
    public UrlValidationAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }   

    /// <summary>
    /// Gets the name of the property to validate.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Determines whether the specified value is a valid HTTP or HTTPS URL.
    /// </summary>
    /// <param name="value">The value of the object to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    /// An instance of <see cref="ValidationResult"/> indicating whether validation succeeded.
    /// </returns>
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        return value is string str && Uri.TryCreate(str, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
            ? ValidationResult.Success! : new ValidationResult($"The field {validationContext.MemberName} must be a valid URL (http, https).");
    }
}
