namespace WpfToolbox.Controls;

public class DataGrid2FilterEnumColumn : DataGrid2FilterColumn
{
    /// <summary>
    /// Populates the filter items for the column based on the enum values of the bound property.
    /// Throws an exception if the bound property is not an enum.
    /// </summary>
    /// <param name="items">The collection view containing the data to analyze for filter options.</param>
    public override void FillColumn(ICollectionView items)
    {
        Debug.WriteLine("DataGrid2FilterEnumColumn.FillColumn");

        if (items is not null && items.CurrentItem is not null)
        {
            Type type = this.Binding.GetBindingType(items.CurrentItem)!;
            if (!type.IsEnum)
            {
                throw new Exception($"{nameof(DataGrid2FilterEnumColumn)} Binding object must be an Enum");
            }

            var values = Enum.GetValues(type).Cast<object>();

            this.filters = [.. values.Select(e => new DataGridFilterItem(e))];
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
    /// Determines whether the specified object matches any of the checked enum filter items.
    /// </summary>
    /// <param name="obj">The object to test against the filter.</param>
    /// <returns>True if the object's enum value is among the checked filters; otherwise, false.</returns>
    public override bool Filter(object obj)
    {
        Debug.WriteLine("DataGrid2FilterEnumColumn.Filter");

        int value = (int)this.Binding.GetBindingValue(obj)!;
        return this.checkedFilters!.Any(i => value == (int)i.Value!);
    }
}
