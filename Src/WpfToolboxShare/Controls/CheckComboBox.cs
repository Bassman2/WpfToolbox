namespace WpfToolbox.Controls;

public class CheckComboBox : ComboBox
{
    static CheckComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckComboBox), new FrameworkPropertyMetadata(typeof(CheckComboBox)));
    }

    public static readonly DependencyProperty CheckedItemsProperty =
                DependencyProperty.Register(nameof(CheckedItems), typeof(IList), typeof(CheckComboBox),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCheckedItemsChanged));

    public IList CheckedItems
    {
        get => (IList)GetValue(CheckedItemsProperty);
        set => SetValue(CheckedItemsProperty, value);
    }

    private static void OnCheckedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CheckComboBox checkComboBox)
        {
            checkComboBox.UpdateText();
        }
    }
    
    internal void UpdateText()
    {
        Text = CheckedItems is not null ? string.Join(", ", CheckedItems.Cast<object>().Select(x => x?.ToString()).Order()) : "";
    }
}

