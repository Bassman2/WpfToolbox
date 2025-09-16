using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WpfToolbox.Validations;

/// <summary>
/// Provides static methods for validating URLs, file paths, and combinations thereof.
/// </summary>
public static partial class TextValidation
{
    /// <summary>
    /// Validates that the specified path is a valid HTTP or HTTPS URL.
    /// </summary>
    /// <param name="path">The URL string to validate.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the URL is valid.</returns>
    public static ValidationResult UrlValidation(string path, ValidationContext context)
    {
        return Uri.TryCreate(path, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
            ? ValidationResult.Success! : new ValidationResult($"The field {context.MemberName} must be a valid URL (http, https).");
    }

    /// <summary>
    /// Validates that the specified path is a valid HTTP, HTTPS, or FILE URL.
    /// </summary>
    /// <param name="path">The URL string to validate.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the URL is valid.</returns>
    public static ValidationResult UrlOrFileValidation(string path, ValidationContext context)
    {
        return Uri.TryCreate(path, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeFile)
            ? ValidationResult.Success! : new ValidationResult($"The field {context.MemberName} must be a valid URL (http, https, file).");
    }

    /// <summary>
    /// Validates that the specified path is a valid HTTP, HTTPS, or FILE URL, or an absolute file or folder path.
    /// </summary>
    /// <param name="path">The path or URL string to validate.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the path or URL is valid.</returns>
    public static ValidationResult UrlFileOrPathValidation(string path, ValidationContext context)
    {
        return
            string.IsNullOrWhiteSpace(path) ||
            (Uri.TryCreate(path, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeFile)) ||
            System.IO.Path.IsPathRooted(path)
            ? ValidationResult.Success! : new ValidationResult($"The field {context.MemberName} must be a valid URL (http, https, file) or an absolute file or folder path.");
    }

    /// <summary>
    /// Validates that the specified string is a valid identifier (letters, digits, or underscores, and does not start with a digit).
    /// </summary>
    /// <param name="text">The identifier string to validate.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the identifier is valid.</returns>
    public static ValidationResult IdentifierValidation(string? text, ValidationContext context)
    {
        // An identifier must not be null, empty, or whitespace
        if (string.IsNullOrWhiteSpace(text))
            return new ValidationResult($"The field {context.MemberName} must not be empty.");

        // Must start with a letter or underscore, and contain only letters, digits, or underscores
        if (!IdentifierRegex().IsMatch(text))
            return new ValidationResult($"The field {context.MemberName} must be a valid identifier (start with a letter or underscore, contain only letters, digits, or underscores).");

        return ValidationResult.Success!;
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"^[A-Za-z_][A-Za-z0-9_]*$")]
    private static partial System.Text.RegularExpressions.Regex IdentifierRegex();
}
