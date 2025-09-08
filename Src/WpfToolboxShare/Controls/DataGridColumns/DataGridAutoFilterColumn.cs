namespace WpfToolbox.Controls;

/// <summary>
/// Abstract base class for DataGrid columns that support automatic filtering.
/// Provides infrastructure for managing checked filter items and defines the contract for filtering logic.
/// </summary>
public abstract class DataGridAutoFilterColumn : DataGridFilterColumn
{
    /// <summary>
    /// Stores the list of filter items that are currently checked (selected) by the user.
    /// </summary>
    protected List<FilterItem>? checkedFilters;

    /// <summary>
    /// Handles the Checked event for filter items.
    /// Updates the <see cref="checkedFilters"/> list based on the current selection
    /// and can trigger a filter refresh on the owning DataGrid.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">Event arguments.</param>

    protected override void OnChecked(object sender, RoutedEventArgs e)
    {
        base.OnChecked(sender, e);

        checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();

        if (this.DataGridOwner is DataGrid dataGrid)
        {
            //dataGrid.RefreshFilter();
        }
    }

    /// <summary>
    /// Populates the filter column with filter items based on the provided collection view.
    /// Implementations should extract unique values or filter options from the data source.
    /// </summary>
    /// <param name="items">The collection view containing the data to analyze for filter options.</param>
    public abstract void FillColumn(ICollectionView items);

    /// <summary>
    /// Determines whether the specified object passes the filter criteria defined by the checked filter items.
    /// </summary>
    /// <param name="obj">The object to test against the filter.</param>
    /// <returns>True if the object matches the filter; otherwise, false.</returns>
    public abstract bool Filter(object obj);
}
