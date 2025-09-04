namespace WpfToolbox.Controls;

public class MultipleSelectionListBox : ListBox
{
    public new static readonly DependencyProperty SelectedItemsProperty =
      DependencyProperty.Register(
          "SelectedItems",
          typeof(IList), 
          typeof(MultipleSelectionListBox),
          new FrameworkPropertyMetadata(null));

    public new IList SelectedItems
    {
        get => (IList)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    public static readonly DependencyProperty HasSelectedItemsProperty =
       DependencyProperty.Register(
           "HasSelectedItems",
           typeof(bool), 
           typeof(MultipleSelectionListBox),
           new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public bool HasSelectedItems
    {
        get => (bool)GetValue(HasSelectedItemsProperty);
        set => SetValue(HasSelectedItemsProperty, value);
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);

        if (SelectedItems != null)
        {
            SelectedItems.Clear();
            foreach (var item in base.SelectedItems)
            {
                SelectedItems.Add(item);
            }
        }

        // set always after SelectedItems
        HasSelectedItems = base.SelectedItems.Count > 0;
    }
}
