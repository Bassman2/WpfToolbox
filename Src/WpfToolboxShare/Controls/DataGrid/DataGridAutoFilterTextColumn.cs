namespace WpfToolbox.Controls;

public class DataGridAutoFilterTextColumn : DataGridAutoFilterColumn
{
    public override void FillColumn(ICollectionView items)
    {
        if (items is not null && items.CurrentItem is not null)
        {
            Type type = this.Binding.GetBindingType(items.CurrentItem)!;
            if (type.Name != "String")
            {
                throw new Exception($"{nameof(DataGridAutoFilterTextColumn)} Binding object must be an string");
            }

            this.filters = items.Cast<object>().Select(o => this.Binding.GetBindingText(o) ?? string.Empty).Distinct().Order().Select(i => new FilterItem(i)).ToList();
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
        string text = this.Binding.GetBindingText(obj) ?? string.Empty;
        return this.checkedFilters!.Any(c => (string)c.Value! == text);
    }    
}
