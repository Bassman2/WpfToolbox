namespace WpfToolbox.Controls;

/// <summary>
/// A WPF button control that copies a specified string value to the clipboard when clicked.
/// The button is automatically disabled if the <see cref="CopyValue"/> is null or whitespace.
/// </summary>
public class CopyButton : Button
{
    static CopyButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CopyButton), new FrameworkPropertyMetadata(typeof(CopyButton)));
    }

    /// <summary>
    /// Identifies the <see cref="CopyValue"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CopyValueProperty =
       DependencyProperty.Register("CopyValue", typeof(string), typeof(CopyButton),
           new FrameworkPropertyMetadata(null,
               new PropertyChangedCallback((d, e) => ((CopyButton)d).UpdateCopyValue((string?)e.NewValue))));

    /// <summary>
    /// Gets or sets the string value to be copied to the clipboard when the button is clicked.
    /// </summary>
    public string? CopyValue
    {
        get => (string?)GetValue(CopyValueProperty);
        set => SetValue(CopyValueProperty, value);
    }

    /// <summary>
    /// Updates the enabled state of the button based on the <paramref name="value"/>.
    /// The button is enabled only if the value is not null or whitespace.
    /// </summary>
    /// <param name="value">The new value of <see cref="CopyValue"/>.</param>
    private void UpdateCopyValue(string? value)
    {
        IsEnabled = !string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Handles the Click event. Copies the <see cref="CopyValue"/> to the clipboard.
    /// </summary>
    protected override void OnClick()
    {
        base.OnClick();
        Clipboard.SetText(CopyValue);
    }
}
