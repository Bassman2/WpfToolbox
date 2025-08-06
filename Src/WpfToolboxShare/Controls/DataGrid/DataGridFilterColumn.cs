namespace WpfToolbox.Controls;

public abstract class DataGridFilterColumn : DataGridTextColumn
{
    private ComboBox? filterComboBox;
    protected List<FilterItem>? filters;
    private readonly FilterItem allFilter = new();

    public DataGridFilterColumn()
    {
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

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
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
    }

    protected virtual void OnChecked(object sender, RoutedEventArgs e)
    {
        FilterItem fvm = (FilterItem)((CheckBox)sender).DataContext;

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
    }

    protected override void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
    {
        if (newBinding != null && newBinding is Binding binding)
        {
            binding.Converter = new DescriptionConverter();
        }
        base.OnBindingChanged(oldBinding, newBinding);
    }

    protected void FillFilters()
    {
        if (filterComboBox != null && filters != null)
        {
            filterComboBox.ItemsSource = filters?.Prepend(allFilter);
        }
    }

    [DebuggerDisplay("FilterItem {Name}")]
    protected class FilterItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor for 'All' filter item
        /// </summary>
        public FilterItem()
        {
            this.IsAll = true;
            this.Name = "All";
            this.IsChecked = true;
        }

        /// <summary>
        /// Constructor for flag enum filter item
        /// </summary>
        public FilterItem(object item)
        {
            FieldInfo? fieldInfo = item.GetType().GetField(item.ToString()!);
            this.Name = fieldInfo!.GetCustomAttribute<DescriptionAttribute>()?.Description ?? item.ToString();
            this.Value = (int)item;
            this.IsChecked = true;
        }

        /// <summary>
        /// Constructor for text filter item
        /// </summary>
        public FilterItem(string item)
        {
            this.Name = item;
            this.Value = item;
            this.IsChecked = true;
        }

        /// <summary>
        /// Constructor for IFilterItem filter item
        /// </summary>
        public FilterItem(IFilterItem item)
        {
            this.Name = item.Name;
            this.Value = item.Value;
            this.IsChecked = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsAll { get; } = false;

        public object? Value { get; }

        public string? Name { get; }

        private bool? isChecked;
        public bool? IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
                }
            }
        }
    }
}
