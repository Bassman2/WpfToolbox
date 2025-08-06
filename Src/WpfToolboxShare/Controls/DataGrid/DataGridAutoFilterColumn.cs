namespace WpfToolbox.Controls;

public abstract class DataGridAutoFilterColumn : DataGridFilterColumn
{
    protected List<FilterItem>? checkedFilters;

    protected override void OnChecked(object sender, RoutedEventArgs e)
    {
        base.OnChecked(sender, e);

        checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();

        if (this.DataGridOwner is ExtendedDataGrid dataGrid)
        {
            dataGrid.RefreshFilter();
        }
    }
    
    public abstract void FillColumn(ICollectionView items);

    public abstract bool Filter(object obj);
}
