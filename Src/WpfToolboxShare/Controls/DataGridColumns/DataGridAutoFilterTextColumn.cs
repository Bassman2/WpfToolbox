namespace WpfToolbox.Controls;

/// <summary>
/// Represents a DataGrid column that provides automatic filtering for string values.
/// Inherits from <see cref="DataGridAutoFilterColumn"/> and specializes filter logic for string types.
/// </summary>
public class DataGridAutoFilterTextColumn : DataGridAutoFilterColumn
{
    /// <summary>
    /// Populates the filter items for the column based on the unique string values
    /// found in the bound property of the items in the collection view.
    /// Throws an exception if the bound property is not a string.
    /// </summary>
    /// <param name="items">The collection view containing the data to analyze for filter options.</param>
    public override void FillColumn(ICollectionView items)
    {
        if (items is not null && items.CurrentItem is not null)
        {
            Type type = this.Binding.GetBindingType(items.CurrentItem)!;
            if (type.Name != "String")
            {
                throw new Exception($"{nameof(DataGridAutoFilterTextColumn)} Binding object must be an string");
            }

            this.filters = [.. items.Cast<object>().Select(o => this.Binding.GetBindingText(o) ?? string.Empty).Distinct().Order().Select(i => new FilterItem(i))];
            this.checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();
            FillFilters();
        }
        else
        {
            this.filters = null;
            this.checkedFilters = null;
            FillFilters();
        }
    }

    /// <summary>
    /// Determines whether the specified object matches any of the checked string filter items.
    /// </summary>
    /// <param name="obj">The object to test against the filter.</param>
    /// <returns>True if the object's string value is among the checked filters; otherwise, false.</returns>
    public override bool Filter(object obj)
    {
        string text = this.Binding.GetBindingText(obj) ?? string.Empty;
        return this.checkedFilters!.Any(c => (string)c.Value! == text);
    }    
}
