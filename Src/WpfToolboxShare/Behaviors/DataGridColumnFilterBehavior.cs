namespace WpfToolbox.Behaviors;

/// <summary>
/// A behavior for WPF DataGrid that enables column-based filtering using DataGridAutoFilterColumn.
/// Tracks the total and filtered item counts, and manages filter logic for the grid.
/// </summary>
public class DataGridColumnFilterBehavior : Behavior<DataGrid>
{
    /// <summary>
    /// Attaches the behavior to the DataGrid, subscribing to column and item changes.
    /// </summary>
    protected override void OnAttached()
    {
        AssociatedObject.Columns.CollectionChanged += OnColumnsCollectionChanged;
        AssociatedObject.Items.CurrentChanged += OnItemsSourceChanged;
    }

    /// <summary>
    /// Detaches the behavior from the DataGrid, unsubscribing from events.
    /// </summary>
    protected override void OnDetaching()
    {
        AssociatedObject.Columns.CollectionChanged -= OnColumnsCollectionChanged;
        AssociatedObject.Items.CurrentChanged -= OnItemsSourceChanged;
    }

    /// <summary>
    /// Dependency property for the total item count in the DataGrid.
    /// </summary>
    public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register("Count", typeof(int), typeof(DataGridColumnFilterBehavior),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Gets or sets the total item count in the DataGrid.
    /// </summary>
    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    /// <summary>
    /// Dependency property for the filtered item count in the DataGrid.
    /// </summary>
    public static readonly DependencyProperty FilteredCountProperty =
        DependencyProperty.Register("FilteredCount", typeof(int), typeof(DataGridColumnFilterBehavior),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Gets or sets the filtered item count in the DataGrid.
    /// </summary>
    public int FilteredCount
    {
        get => (int)GetValue(FilteredCountProperty);
        set => SetValue(FilteredCountProperty, value);
    }

    /// <summary>
    /// Handles changes to the DataGrid's items source.
    /// Updates item counts, fills columns, and activates filtering.
    /// </summary>
    private void OnItemsSourceChanged(object? sender, EventArgs e)
    {
        if (AssociatedObject.ItemsSource == null) return;
        this.Count = AssociatedObject.ItemsSource.Cast<object>().Count();

        // Fill columns
        FillColumns(AssociatedObject.Columns);

        // activate filtering
        if (AssociatedObject.ItemsSource is ICollectionView collectionView)
        {
            collectionView.Filter = DoFilter;
            FilteredCount = collectionView.Cast<object>().Count();
        }
    }

    /// <summary>
    /// Handles changes to the DataGrid's columns collection.
    /// Ensures new columns are initialized for filtering.
    /// </summary>
    private void OnColumnsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            FillColumns(e.NewItems!.Cast<DataGridColumn>());
        }

    }

    /// <summary>
    /// Initializes DataGridAutoFilterColumn columns for filtering.
    /// Throws if ItemsSource is not an ICollectionView when filtering is required.
    /// </summary>
    private void FillColumns(IEnumerable<DataGridColumn> columns)
    {
        if (AssociatedObject.ItemsSource is null)
        {
            return;
        }

        if (AssociatedObject.ItemsSource is not ICollectionView && columns.Any(c => c is DataGridAutoFilterColumn))
        {
            throw new ArgumentException($"ItemsSource must be of type ICollectionView for filtering!");
        }

        columns.OfType<DataGridAutoFilterColumn>().ToList().ForEach(c => c.FillColumn((ICollectionView)AssociatedObject.ItemsSource));
    }

    /// <summary>
    /// Applies all column filters to a given item.
    /// </summary>
    /// <param name="obj">The item to filter.</param>
    /// <returns>True if the item passes all filters; otherwise, false.</returns>
    private bool DoFilter(object obj)
    {
        bool res = true;
        foreach (DataGridAutoFilterColumn column in AssociatedObject.Columns.Where(c => c is DataGridAutoFilterColumn).Cast<DataGridAutoFilterColumn>())
        {
            res &= column.Filter(obj);
        }
        return res;

    }

    /// <summary>
    /// Refreshes the filter on the DataGrid and updates the filtered item count.
    /// </summary>
    public void RefreshFilter()
    {
        if (AssociatedObject.ItemsSource is ICollectionView collectionView)
        {
            collectionView.Refresh();
            FilteredCount = collectionView.Cast<object>().Count();
        }
    }
}
