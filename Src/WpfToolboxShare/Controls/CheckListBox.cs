namespace WpfToolbox.Controls;

public class CheckListBox : ListBox
{
    static CheckListBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckListBox), new FrameworkPropertyMetadata(typeof(CheckListBox)));

        //ListBoxItem x;
        //ComboBoxItem y;
    }

    public static readonly DependencyProperty CheckedItemsProperty =
                DependencyProperty.Register(nameof(CheckedItems), typeof(IList), typeof(CheckListBox),
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
            //control.UpdateText();
        }
    }

}
