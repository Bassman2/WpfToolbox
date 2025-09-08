namespace WpfToolbox.Controls;

/// <summary>
/// Represents a DataGrid column that supports filtering using a set of filter items or an enum.
/// The filter value is computed as a bitmask of selected filter items.
/// </summary>
public partial class DataGridFilterTextColumn : DataGridFilterColumn
{
    /// <summary>
    /// Handles the Checked event for filter items.
    /// Updates the FilterValue property based on the selected filter items.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected override void OnChecked(object sender, RoutedEventArgs e)
    {
        base.OnChecked(sender, e);

        // set filter value to trigger new filtering
        int filterValue = filters?.Where(f => f.IsChecked == true).Select(f => f.Value).Aggregate((int)0, (a, b) => (int)(((int)a) | ((int)b!))) ?? 0;
        if (filterValue != this.FilterValue)
        {
            Debug.WriteLine($"FilterValue {filterValue}");
            this.FilterValue = filterValue;
        }
    }

    /// <summary>
    /// Identifies the FilterEnum dependency property.
    /// Used to specify an enum type for generating filter items.
    /// </summary>
    public static readonly DependencyProperty FilterEnumProperty =
        DependencyProperty.Register("FilterEnum", typeof(Type), typeof(DataGridFilterTextColumn),
        new PropertyMetadata(null, (d, e) => ((DataGridFilterTextColumn)d).OnFilterEnumChanged((Type)e.NewValue)));

    /// <summary>
    /// Gets or sets the enum type used to generate filter items.
    /// </summary>
    public Type FilterEnum
    {
        get => (Type)GetValue(FilterEnumProperty);
        set => SetValue(FilterEnumProperty, value);
    }

    /// <summary>
    /// Called when the FilterEnum property changes.
    /// Populates the filter items from the enum values.
    /// </summary>
    /// <param name="filterEnum">The new enum type.</param>
    private void OnFilterEnumChanged(Type filterEnum)
    {
        this.filters = [.. Enum.GetValues(filterEnum).Cast<object>().Select(e => new FilterItem(e))];
        FillFilters();
    }

    /// <summary>
    /// Identifies the FilterItems dependency property.
    /// Used to specify a custom collection of filter items.
    /// </summary>
    public static readonly DependencyProperty FilterItemsProperty =
        DependencyProperty.Register("FilterItems", typeof(IEnumerable<IFilterItem>), typeof(DataGridFilterTextColumn),
        new PropertyMetadata(null, (d, e) => ((DataGridFilterTextColumn)d).OnFilterItemsChanged((IEnumerable<IFilterItem>?)e.NewValue)));

    /// <summary>
    /// Gets or sets the collection of custom filter items.
    /// </summary>
    public IEnumerable<IFilterItem>? FilterItems
    {
        get => (IEnumerable<IFilterItem>?)GetValue(FilterItemsProperty);
        set => SetValue(FilterItemsProperty, value);
    }

    /// <summary>
    /// Called when the FilterItems property changes.
    /// Populates the filter items from the provided collection.
    /// </summary>
    /// <param name="filterItems">The new collection of filter items.</param>
    private void OnFilterItemsChanged(IEnumerable<IFilterItem>? filterItems)
    {
        this.filters = [.. filterItems!.Select(i => new FilterItem(i))];
        FillFilters();
    }

    /// <summary>
    /// Identifies the FilterValue dependency property.
    /// Represents the current filter value as a bitmask of selected items.
    /// </summary>
    public static readonly DependencyProperty FilterValueProperty =
        DependencyProperty.Register("FilterValue", typeof(int), typeof(DataGridFilterTextColumn),
        new FrameworkPropertyMetadata(0x7fffffff, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Gets or sets the current filter value as a bitmask of selected filter items.
    /// </summary>
    public int FilterValue
    {
        get => (int)GetValue(FilterValueProperty); 
        set => SetValue(FilterValueProperty, value); 
    }
}
