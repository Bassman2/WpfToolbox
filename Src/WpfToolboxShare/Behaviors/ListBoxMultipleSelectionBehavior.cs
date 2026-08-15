namespace WpfToolbox.Behaviors;

/// <summary>
/// A behavior for WPF <see cref="ListBox"/> that exposes a bindable <see cref="SelectedItems"/> property and a <see cref="HasSelectedItems"/> property.
/// This behavior allows two-way binding of the selected items collection and provides a boolean indicating if any items are selected.
/// </summary>
public class ListBoxMultipleSelectionBehavior : Behavior<ListBox>
{
    /// <summary>
    /// Identifies the <see cref="SelectedItems"/> dependency property.
    /// This property allows binding to the selected items collection.
    /// </summary>
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(IList),
            typeof(ListBoxMultipleSelectionBehavior),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets the collection of selected items. This property is bindable.
    /// </summary>
    public IList? SelectedItems
    {
        get => (IList?)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="HasSelectedItems"/> dependency property.
    /// This property indicates whether any items are selected.
    /// </summary>
    public static readonly DependencyProperty HasSelectedItemsProperty =
        DependencyProperty.Register(
            nameof(HasSelectedItems),
            typeof(bool),
            typeof(ListBoxMultipleSelectionBehavior),
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
    /// Attaches the behavior to the ListBox and subscribes to the SelectionChanged event.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += OnSelectionChanged;
    }

    /// <summary>
    /// Detaches the behavior from the ListBox and unsubscribes from the SelectionChanged event.
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.SelectionChanged -= OnSelectionChanged;
    }

    /// <summary>
    /// Handles selection changes, updates the <see cref="SelectedItems"/> collection, and sets <see cref="HasSelectedItems"/>.
    /// </summary>
    /// <param name="sender">The ListBox that raised the event.</param>
    /// <param name="e">The event data.</param>
    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectedItems is not null)
        {
            SelectedItems.Clear();
            foreach (var item in AssociatedObject.SelectedItems)
            {
                SelectedItems.Add(item);
            }
        }

        // Set always after SelectedItems
        HasSelectedItems = AssociatedObject.SelectedItems.Count > 0;
    }
}
