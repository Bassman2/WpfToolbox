namespace WpfToolbox.Controls;

/// <summary>
/// A WPF <see cref="ListBox"/> that exposes a bindable <see cref="SelectedItems"/> property and a <see cref="HasSelectedItems"/> property.
/// This control allows two-way binding of the selected items collection and provides a boolean indicating if any items are selected.
/// </summary>
public class MultipleSelectionListBox : ListBox
{
    /// <summary>
    /// Identifies the <see cref="SelectedItems"/> dependency property.
    /// This property allows binding to the selected items collection.
    /// </summary>
    public new static readonly DependencyProperty SelectedItemsProperty =
      DependencyProperty.Register(
          "SelectedItems",
          typeof(IList), 
          typeof(MultipleSelectionListBox),
          new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets the collection of selected items. This property is bindable.
    /// </summary>
    public new IList SelectedItems
    {
        get => (IList)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="HasSelectedItems"/> dependency property.
    /// This property indicates whether any items are selected.
    /// </summary>
    public static readonly DependencyProperty HasSelectedItemsProperty =
       DependencyProperty.Register(
           "HasSelectedItems",
           typeof(bool), 
           typeof(MultipleSelectionListBox),
           new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Gets or sets a value indicating whether any items are selected.
    /// </summary>
    public bool HasSelectedItems
    {
        get => (bool)GetValue(HasSelectedItemsProperty);
        set => SetValue(HasSelectedItemsProperty, value);
    }

    /// <summary>
    /// Handles selection changes, updates the <see cref="SelectedItems"/> collection, and sets <see cref="HasSelectedItems"/>.
    /// </summary>
    /// <param name="e">The event data.</param>
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
