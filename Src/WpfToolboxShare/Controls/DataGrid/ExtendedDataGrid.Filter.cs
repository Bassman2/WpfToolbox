namespace WpfToolbox.Controls;

public partial class ExtendedDataGrid : DataGrid
{
    public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register("Count", typeof(int), typeof(ExtendedDataGrid),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public static readonly DependencyProperty FilteredCountProperty = 
        DependencyProperty.Register("FilteredCount", typeof(int), typeof(ExtendedDataGrid),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int FilteredCount
    {
        get => (int)GetValue(FilteredCountProperty);
        set => SetValue(FilteredCountProperty, value);
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);

        this.Count = newValue.Cast<object>().Count();

        // Fill columns
        FillColumns(this.Columns);

        // activate filtering
        if (newValue is ICollectionView collectionView)
        {
            collectionView.Filter = DoFilter;
            FilteredCount = collectionView.Cast<object>().Count();
        }
    }

    private void OnColumnsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            FillColumns(e.NewItems!.Cast<DataGridColumn>());
        }
        
    }

    private void FillColumns(IEnumerable<DataGridColumn> columns)
    {
        if (this.ItemsSource is null)
        {
            return;
        }

        if (this.ItemsSource is not ICollectionView && columns.Any(c => c is DataGridAutoFilterColumn))
        {
            throw new ArgumentException($"ItemsSource must be of type ICollectionView for filtering!");
        }

        columns.OfType<DataGridAutoFilterColumn>().ToList().ForEach(c => c.FillColumn((ICollectionView)this.ItemsSource));
    }

    private bool DoFilter(object obj)
    {
        bool res = true;
        foreach (DataGridAutoFilterColumn column in this.Columns.Where(c => c is DataGridAutoFilterColumn).Cast<DataGridAutoFilterColumn>())
        {
            res &= column.Filter(obj);
        }
        return res;

    }

    public void RefreshFilter()
    {
        if (this.ItemsSource is ICollectionView collectionView)
        {
            collectionView.Refresh();
            FilteredCount = collectionView.Cast<object>().Count();
        }
    }
}
