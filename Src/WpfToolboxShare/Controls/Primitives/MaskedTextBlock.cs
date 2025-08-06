namespace WpfToolbox.Controls.Primitives;

public class MaskedTextBlock : TextBlock
{
    public static readonly DependencyProperty MaskedTextProperty =
        DependencyProperty.Register("MaskedText", typeof(string), typeof(MaskedTextBlock),
        new PropertyMetadata(null, (d, e) => ((MaskedTextBlock)d).OnMaskedTextsChanged((string)e.NewValue)));

    public string MaskedText
    {
        get => (string)GetValue(MaskedTextProperty);
        set => SetValue(MaskedTextProperty, value);
    }

    private void OnMaskedTextsChanged(string newValue)
    {
        int length = newValue.Length;
        Text = new string('●', length);
    }
}
