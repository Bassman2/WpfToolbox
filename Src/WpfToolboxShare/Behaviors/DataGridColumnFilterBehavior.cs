namespace WpfToolbox.Behaviors;

public class DataGridColumnFilterBehavior : Behavior<DataGrid>
{
    protected override void OnAttached()
    {
        AssociatedObject.Columns.CollectionChanged += OnColumnsCollectionChanged;
        AssociatedObject.Items.CurrentChanged += OnItemsSourceChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Columns.CollectionChanged -= OnColumnsCollectionChanged;
        AssociatedObject.Items.CurrentChanged -= OnItemsSourceChanged;
    }

    public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register("Count", typeof(int), typeof(DataGridColumnFilterBehavior),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public static readonly DependencyProperty FilteredCountProperty =
        DependencyProperty.Register("FilteredCount", typeof(int), typeof(DataGridColumnFilterBehavior),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int FilteredCount
    {
        get => (int)GetValue(FilteredCountProperty);
        set => SetValue(FilteredCountProperty, value);
    }
   
    private void OnItemsSourceChanged(object? sender, EventArgs e)
    {
        if (AssociatedObject.ItemsSource == null) return;
        this.Count = AssociatedObject.ItemsSource.Cast<object>().Count();

        // Fill columns
        FillColumns(AssociatedObject.Columns);

        // activate filtering
        if (AssociatedObject.ItemsSource is ICollectionView collectionView)
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
        if (AssociatedObject.ItemsSource is null)
        {
            return;
        }

        if (AssociatedObject.ItemsSource is not ICollectionView && columns.Any(c => c is DataGridAutoFilterColumn))
        {
            throw new ArgumentException($"ItemsSource must be of type ICollectionView for filtering!");
        }

        columns.OfType<DataGridAutoFilterColumn>().ToList().ForEach(c => c.FillColumn((ICollectionView)AssociatedObject.ItemsSource));
    }

    private bool DoFilter(object obj)
    {
        bool res = true;
        foreach (DataGridAutoFilterColumn column in AssociatedObject.Columns.Where(c => c is DataGridAutoFilterColumn).Cast<DataGridAutoFilterColumn>())
        {
            res &= column.Filter(obj);
        }
        return res;

    }

    public void RefreshFilter()
    {
        if (AssociatedObject.ItemsSource is ICollectionView collectionView)
        {
            collectionView.Refresh();
            FilteredCount = collectionView.Cast<object>().Count();
        }
    }
}
