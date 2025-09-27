namespace WpfToolbox.Controls;

public class CheckComboBox : ComboBox
{
    static CheckComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckComboBox), new FrameworkPropertyMetadata(typeof(CheckComboBox)));
    }

    public CheckComboBox()
    {
        this.DefaultStyleKey = typeof(CheckComboBox);

        //SelectedItems.CollectionChanged += (s, e) => UpdateText();
        // Allow the text area to show the comma-delimited list.
        IsEditable = true;
        IsReadOnly = true;
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
        if (d is CheckComboBox control)
        {
            control.UpdateText();
        }
    }

    private void UpdateText()
    {
        Text = string.Join(", ", CheckedItems.Cast<object>().Select(x => x?.ToString()));
    }
}

public class MultiSelectConverter : IValueConverter
{
    // Checks if the item (passed as ConverterParameter) is in the SelectedItems list.
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var selectedItems = value as IList;
        if (selectedItems != null && parameter != null)
        {
            return selectedItems.Contains(parameter);
        }
        return false;
    }

    // This converter does not currently update the SelectedItems list.
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // In a full implementation, you would update the collection.
        return Binding.DoNothing;
    }
}

public class MultiSelectMultiConverter : IMultiValueConverter
{
    // values[0] is SelectedItems, values[1] is the current item.
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is IList selectedItems && values[1] != null)
        {
            return selectedItems.Contains(values[1]);
        }
        return false;
    }

    // For simplicity, ConvertBack is not implemented; you might need to handle updating the collection.
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        // Return DoNothing for both bindings.
        return new object[] { Binding.DoNothing, Binding.DoNothing };
    }
}