namespace WpfToolbox.Controls;

public partial class DataGridFilterTextColumn : DataGridFilterColumn
{
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

    public static readonly DependencyProperty FilterEnumProperty =
        DependencyProperty.Register("FilterEnum", typeof(Type), typeof(DataGridFilterTextColumn),
        new PropertyMetadata(null, (d, e) => ((DataGridFilterTextColumn)d).OnFilterEnumChanged((Type)e.NewValue)));

    public Type FilterEnum
    {
        get => (Type)GetValue(FilterEnumProperty);
        set => SetValue(FilterEnumProperty, value);
    }

    private void OnFilterEnumChanged(Type filterEnum)
    {
        this.filters = Enum.GetValues(filterEnum).Cast<object>().Select(e => new FilterItem(e)).ToList();
        FillFilters();
    }

    public static readonly DependencyProperty FilterItemsProperty =
        DependencyProperty.Register("FilterItems", typeof(IEnumerable<IFilterItem>), typeof(DataGridFilterTextColumn),
        new PropertyMetadata(null, (d, e) => ((DataGridFilterTextColumn)d).OnFilterItemsChanged((IEnumerable<IFilterItem>?)e.NewValue)));

    public IEnumerable<IFilterItem>? FilterItems
    {
        get => (IEnumerable<IFilterItem>?)GetValue(FilterItemsProperty);
        set => SetValue(FilterItemsProperty, value);
    }

    private void OnFilterItemsChanged(IEnumerable<IFilterItem>? filterItems)
    {
        this.filters = filterItems!.Select(i => new FilterItem(i)).ToList();
        FillFilters();
    }

    public static readonly DependencyProperty FilterValueProperty =
        DependencyProperty.Register("FilterValue", typeof(int), typeof(DataGridFilterTextColumn),
        new FrameworkPropertyMetadata(0x7fffffff, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
       
    public int FilterValue
    {
        get => (int)GetValue(FilterValueProperty); 
        set => SetValue(FilterValueProperty, value); 
    }
}
