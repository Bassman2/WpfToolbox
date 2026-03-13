namespace WpfToolbox.Controls;

public class DataGrid2FilterTextColumn : DataGrid2FilterColumn
{
    /// <summary>
    /// Populates the filter items for the column based on the unique string values
    /// found in the bound property of the items in the collection view.
    /// Throws an exception if the bound property is not a string.
    /// </summary>
    /// <param name="items">The collection view containing the data to analyze for filter options.</param>
    public override void FillColumn(ICollectionView items)
    {
        Debug.WriteLine("DataGrid2FilterTextColumn.FillColumn");

        if (items is not null && items.CurrentItem is not null)
        {
            Type type = this.Binding.GetBindingType(items.CurrentItem)!;
            if (type != typeof(string))
            {
                throw new Exception($"{nameof(DataGrid2FilterTextColumn)} Binding object must be an string");
            }

            var values = items.Cast<object>().Select(o => this.Binding.GetBindingText(o) ?? string.Empty).Distinct().Order();
            this.filters = [.. values.Select(i => new DataGridFilterItem(i))];
            this.checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();
        }
        else
        {
            this.filters = null;
            this.checkedFilters = null;
        }
        FillFilters();
    }

    /// <summary>
    /// Determines whether the specified object matches any of the checked string filter items.
    /// </summary>
    /// <param name="obj">The object to test against the filter.</param>
    /// <returns>True if the object's string value is among the checked filters; otherwise, false.</returns>
    public override bool Filter(object obj)
    {
        Debug.WriteLine("DataGrid2FilterTextColumn.Filter");

        string text = this.Binding.GetBindingText(obj) ?? string.Empty;
        return this.checkedFilters!.Any(c => (string)c.Value! == text);
    }
}
