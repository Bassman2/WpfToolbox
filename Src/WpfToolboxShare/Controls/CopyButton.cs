namespace WpfToolbox.Controls;

public class CopyButton : Button
{
    static CopyButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CopyButton), new FrameworkPropertyMetadata(typeof(CopyButton)));
    }

    public static readonly DependencyProperty CopyValueProperty =
       DependencyProperty.Register("CopyValue", typeof(string), typeof(CopyButton),
           new FrameworkPropertyMetadata(null,
               new PropertyChangedCallback((d, e) => ((CopyButton)d).UpdateCopyValue((string?)e.NewValue))));

    public string? CopyValue
    {
        get => (string?)GetValue(CopyValueProperty);
        set => SetValue(CopyValueProperty, value);
    }

    private void UpdateCopyValue(string? value)
    {
        IsEnabled = !string.IsNullOrWhiteSpace(value);
    }

    protected override void OnClick()
    {
        base.OnClick();
        Clipboard.SetText(CopyValue);
    }
}
