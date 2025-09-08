namespace WpfToolbox.Controls;

/// <summary>
/// Represents a DataGrid column that provides automatic filtering for enum values.
/// Inherits from <see cref="DataGridAutoFilterColumn"/> and specializes filter logic for enum types.
/// </summary>
public class DataGridAutoFilterEnumColumn : DataGridAutoFilterColumn
{
    /// <summary>
    /// Populates the filter items for the column based on the enum values of the bound property.
    /// Throws an exception if the bound property is not an enum.
    /// </summary>
    /// <param name="items">The collection view containing the data to analyze for filter options.</param>
    public override void FillColumn(ICollectionView items)
    {
        if (items is not null && items.CurrentItem is not null)
        {
            Type type = this.Binding.GetBindingType(items.CurrentItem)!;
            if (!type.IsEnum)
            {
                throw new Exception($"{nameof(DataGridAutoFilterEnumColumn)} Binding object must be an Enum");
            }

            this.filters = [.. Enum.GetValues(type).Cast<object>().Select(e => new FilterItem(e))];
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
    /// Determines whether the specified object matches any of the checked enum filter items.
    /// </summary>
    /// <param name="obj">The object to test against the filter.</param>
    /// <returns>True if the object's enum value is among the checked filters; otherwise, false.</returns>
    public override bool Filter(object obj)
    {
        int value = (int)this.Binding.GetBindingValue(obj)!;
        return this.checkedFilters!.Any(i => value == (int)i.Value!);
    }
}
