namespace WpfToolbox.Controls;

public abstract class DataGrid2FilterColumn : DataGridTextColumn
{
    // Reference to the ComboBox used for filtering in the header.
    private ComboBox? filterComboBox;

    /// <summary>
    /// List of filter items displayed in the ComboBox for filtering the DataGrid column.
    /// </summary>
    protected List<DataGridFilterItem>? filters;

    // Special filter item representing the "All" option.
    private readonly DataGridFilterItem allFilter = new();

    /// <summary>
    /// Stores the list of filter items that are currently checked (selected) by the user.
    /// </summary>
    protected List<DataGridFilterItem>? checkedFilters;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGridFilterColumn"/> class.
    /// Sets up the header template with a TextBlock and a ComboBox for filtering.
    /// </summary>
    public DataGrid2FilterColumn()
    {
        Debug.WriteLine("DataGrid2FilterColumn.DataGrid2FilterColumn");

        IsReadOnly = true;

        var headerTemplate = new DataTemplate() { DataType = typeof(string) };

        var dockPanel = new FrameworkElementFactory(typeof(DockPanel));
        dockPanel.SetValue(DockPanel.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);

        var textBlock = new FrameworkElementFactory(typeof(TextBlock));
        textBlock.SetBinding(TextBlock.TextProperty, new Binding("Header") { Source = this });
        textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
        textBlock.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 4.0, 0.0));

        var comboBox = new FrameworkElementFactory(typeof(ComboBox));
        comboBox.SetValue(DockPanel.HorizontalAlignmentProperty, HorizontalAlignment.Right);
        comboBox.SetValue(DockPanel.DockProperty, Dock.Right);
        comboBox.SetValue(ComboBox.WidthProperty, 16.0);
        comboBox.SetValue(ComboBox.IsEnabledProperty, true);
        comboBox.SetBinding(ComboBox.DataContextProperty, this.Binding);
        comboBox.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoaded));

        dockPanel.AppendChild(textBlock);
        dockPanel.AppendChild(comboBox);

        headerTemplate.VisualTree = dockPanel;
        HeaderTemplate = headerTemplate;
    }

    /// <summary>
    /// Handler for the ComboBox Loaded event.
    /// Sets up the item template and populates the filter items.
    /// </summary>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DataGrid2FilterColumn.OnLoaded");

        filterComboBox = (ComboBox)sender;

        DataTemplate dataTemplate = new(typeof(ComboBox));
        FrameworkElementFactory checkBox = new(typeof(CheckBox));
        checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsChecked"));
        checkBox.SetBinding(CheckBox.ContentProperty, new Binding("Name"));
        checkBox.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(OnChecked));
        checkBox.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(OnChecked));
        checkBox.AddHandler(CheckBox.IndeterminateEvent, new RoutedEventHandler(OnChecked));
        dataTemplate.VisualTree = checkBox;
        filterComboBox.ItemTemplate = dataTemplate;

        FillFilters();

        IsReady = true;
    }

    /// <summary>
    /// Handles the Checked, Unchecked, and Indeterminate events for filter CheckBoxes.
    /// Updates the filter state and the "All" filter accordingly.
    /// </summary>
    protected virtual void OnChecked(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DataGrid2FilterColumn.OnChecked");

        DataGridFilterItem fvm = (DataGridFilterItem)((CheckBox)sender).DataContext;

        Debug.WriteLine($"OnChecked {fvm.Name}");

        if (fvm.IsAll) // if All button
        {
            switch (fvm.IsChecked)
            {
            case true:
                filters?.Where(f => f.IsChecked == false).ToList().ForEach(f => f.IsChecked = true);
                break;
            case false:
                filters?.Where(f => f.IsChecked == true).ToList().ForEach(f => f.IsChecked = false);
                break;
            case null:
                break;
            }
        }
        else
        {
            // update all button state
            if (filters?.All(f => f.IsChecked == true) ?? false)
            {
                this.allFilter!.IsChecked = true;
            }
            else if (filters?.All(f => f.IsChecked == false) ?? false)
            {
                this.allFilter!.IsChecked = false;
            }
            else
            {
                this.allFilter!.IsChecked = null;
            }
        }

        checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();

        if (this.DataGridOwner is DataGrid dataGrid)
        {
            // TODO
            //dataGrid.RefreshFilter();

            //var behavior = Interaction.GetBehaviors(dataGrid).OfType<DataGridColumnFilterBehavior>().FirstOrDefault();

            //behavior?.RefreshFilter();
        }
    }

    /// <summary>
    /// Called when the column's binding changes.
    /// Sets a DescriptionConverter for the new binding.
    /// </summary>
    protected override void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
    {
        if (newBinding != null && newBinding is Binding binding)
        {
            binding.Converter = new DescriptionConverter();
        }
        base.OnBindingChanged(oldBinding, newBinding);
    }

    /// <summary>
    /// Populates the ComboBox with filter items, including the "All" filter.
    /// </summary>
    protected void FillFilters()
    {
        Debug.WriteLine("DataGrid2FilterColumn.FillFilters");


        if (filterComboBox != null && filters != null)
        {
            filterComboBox.ItemsSource = filters?.Prepend(allFilter);
        }
    }

    public bool IsReady { get; private set; } = false;

    /// <summary>
    /// Populates the filter column with filter items based on the provided collection view.
    /// Implementations should extract unique values or filter options from the data source.
    /// </summary>
    /// <param name="items">The collection view containing the data to analyze for filter options.</param>
    public abstract void FillColumn(ICollectionView items);

    /// <summary>
    /// Determines whether the specified object passes the filter criteria defined by the checked filter items.
    /// </summary>
    /// <param name="obj">The object to test against the filter.</param>
    /// <returns>True if the object matches the filter; otherwise, false.</returns>
    public abstract bool Filter(object obj);

}
