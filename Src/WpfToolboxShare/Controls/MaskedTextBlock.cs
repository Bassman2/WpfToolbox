namespace WpfToolbox.Controls;

/// <summary>
/// A WPF <see cref="TextBlock"/> that displays a masked version of the input text.
/// The text is replaced with bullet characters (●) to hide its content, commonly used for password or sensitive data display.
/// </summary>
public class MaskedTextBlock : TextBlock
{
    /// <summary>
    /// Identifies the <see cref="MaskedText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaskedTextProperty =
        DependencyProperty.Register("MaskedText", typeof(string), typeof(MaskedTextBlock),
        new PropertyMetadata(null, (d, e) => ((MaskedTextBlock)d).OnMaskedTextsChanged((string)e.NewValue)));

    /// <summary>
    /// Gets or sets the text to be masked. The actual displayed text will be replaced with bullet characters.
    /// </summary>
    public string MaskedText
    {
        get => (string)GetValue(MaskedTextProperty);
        set => SetValue(MaskedTextProperty, value);
    }

    /// <summary>
    /// Updates the displayed text to a string of bullet characters matching the length of the new value.
    /// </summary>
    /// <param name="newValue">The new text value to be masked.</param>
    private void OnMaskedTextsChanged(string newValue)
    {
        int length = newValue.Length;
        Text = new string('●', length);
    }
}
