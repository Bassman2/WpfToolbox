namespace WpfToolbox.Controls;

public class CheckListBox : ListBox
{
    static CheckListBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckListBox), new FrameworkPropertyMetadata(typeof(CheckListBox)));
    }

    public static readonly DependencyProperty CheckedItemsProperty =
                DependencyProperty.Register(nameof(CheckedItems), typeof(IList), typeof(CheckListBox),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public IList CheckedItems
    {
        get => (IList)GetValue(CheckedItemsProperty);
        set => SetValue(CheckedItemsProperty, value);
    }
}
