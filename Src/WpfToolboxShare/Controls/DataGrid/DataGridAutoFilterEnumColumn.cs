namespace WpfToolbox.Controls;

public class DataGridAutoFilterEnumColumn : DataGridAutoFilterColumn
{
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

    public override bool Filter(object obj)
    {
        int value = (int)this.Binding.GetBindingValue(obj)!;
        return this.checkedFilters!.Any(i => value == (int)i.Value!);
    }
}
