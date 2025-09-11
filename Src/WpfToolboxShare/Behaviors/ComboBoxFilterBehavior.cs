namespace WpfToolbox.Behaviors;

/// <summary>
/// A behavior for WPF ComboBox that enables multi-value filtering using checkboxes for each item.
/// Supports flag enums and custom filter items, and provides an "All" option for quick selection.
/// The filter value is represented as an integer bitmask of selected flags.
/// </summary>
public class ComboBoxFilterBehavior : Behavior<ComboBox>
{
    /// <summary>
    /// Attaches the behavior to the ComboBox, sets up the item template with checkboxes, and initializes the filter list.
    /// </summary>
    protected override void OnAttached()
    {
        AssociatedObject.IsEditable = true;
        AssociatedObject.IsReadOnly = true;
        AssociatedObject.Text = "All";
        DataTemplate dataTemplate = new (typeof(ComboBox));
        FrameworkElementFactory factory = new (typeof(CheckBox));
        factory.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsChecked"));
        factory.SetBinding(CheckBox.ContentProperty, new Binding("Name"));
        factory.SetBinding(CheckBox.IsThreeStateProperty, new Binding("IsThreeState"));
        dataTemplate.VisualTree = factory;
        AssociatedObject.ItemTemplate = dataTemplate;
        Update();        
    }

    /// <summary>
    /// Detaches the behavior from the ComboBox. (No-op in current implementation.)
    /// </summary>
    protected override void OnDetaching()
    {
        //AssociatedObject.TextChanged -= OnAssociatedObjectTextChanged;
    }

    /// <summary>
    /// The enum type to use for filtering (optional).
    /// </summary>
    public static readonly DependencyProperty EnumTypeProperty =
        DependencyProperty.Register("EnumType", typeof(Type), typeof(ComboBoxFilterBehavior),
        new PropertyMetadata(null));


    /// <summary>
    /// The collection of filter items to display in the ComboBox.
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable<FilterItem>), typeof(ComboBoxFilterBehavior),
        new PropertyMetadata(null, (d, e) => ((ComboBoxFilterBehavior)d).OnItemsSource(e.NewValue)));

    /// <summary>
    /// The integer bitmask representing the selected filter values.
    /// </summary>
    public static readonly DependencyProperty FilterValueProperty =
        DependencyProperty.Register("FilterValue", typeof(int), typeof(ComboBoxFilterBehavior), new FrameworkPropertyMetadata()
        {
            BindsTwoWayByDefault = true,
            //DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

    /// <summary>
    /// Gets or sets the enum type for filtering.
    /// </summary>
    public Type EnumType
    {
        get { return (Type)GetValue(EnumTypeProperty); }
        set { SetValue(EnumTypeProperty, value); }
    }

    /// <summary>
    /// Gets or sets the collection of filter items.
    /// </summary>
    public IEnumerable<FilterItem>? ItemsSource
    {
        get { return (IEnumerable<FilterItem>?)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// Gets or sets the filter value as an integer bitmask.
    /// </summary>
    public int FilterValue
    {
        get { return (int)GetValue(FilterValueProperty); }
        set { SetValue(FilterValueProperty, value); }
    }


    //private void OnEnumType(Type newValue)
    //{
    //    //Type type = (Type)newValue;
    //    //AssociatedObject.ItemsSource = Enum.GetNames(type);
    //}

    /// <summary>
    /// Called when the ItemsSource property changes. Triggers an update of the filter list.
    /// </summary>    
    private void OnItemsSource(object _)
    {
        Update();   
    }
    

    private List<FilterViewModel>? filters;

    /// <summary>
    /// Updates the ComboBox items based on the EnumType or ItemsSource.
    /// Adds an "All" filter option at the top.
    /// </summary>
    private void Update()
    {
        if (AssociatedObject == null) return;
        
        if (EnumType != null)
        {
            this.filters = Enum.GetValues(EnumType).Cast<object>().Select(e => new FilterViewModel(this, e)).ToList();
            AssociatedObject.ItemsSource = filters.Prepend(new AllFilterViewModel(this.filters));
        }
        else if (ItemsSource != null)
        {
            this.filters = ItemsSource.Select(i => new FilterViewModel(this, i)).ToList();
            AssociatedObject.ItemsSource = filters.Prepend(new AllFilterViewModel(this.filters));
        }
    }

    /// <summary>
    /// Updates the FilterValue property and ComboBox text based on the current selection.
    /// </summary>
    public void UpdateValue()
    {
        if (this.filters != null)
        {
            this.FilterValue = this.filters!.Where(f => f.IsChecked!.Value).Select(f => f.Value).Aggregate(0, (a, b) => a | b);

            if (this.filters!.All(f => f.IsChecked == true))
            {
                AssociatedObject.Text = "All";
            }
            else if (this.filters!.All(f => f.IsChecked == false))
            {
                AssociatedObject.Text = "None";
            }
            else
            {
                AssociatedObject.Text = "Selection"; //  this.filters!.Where(f => f.IsChecked!.Value).Select(f => f.Name).Aggregate("", (a, b) => $"{a}, {b}").Trim(',', ' ');
            }
        }
    }
}

/// <summary>
/// ViewModel for a single filter item in the ComboBoxFilterBehavior.
/// </summary>
public partial class FilterViewModel : ObservableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FilterViewModel"/> class.
    /// </summary>
    public FilterViewModel()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterViewModel"/> class using an enum value.
    /// </summary>
    /// <param name="behavior">The parent <see cref="ComboBoxFilterBehavior"/> instance.</param>
    /// <param name="e">The enum value to represent as a filter item.</param>
    public FilterViewModel(ComboBoxFilterBehavior behavior, object e)
    {
        this.behavior = behavior;
        Name = e.ToString();
        IsChecked = true;
        Value = (int)e;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterViewModel"/> class using a <see cref="FilterItem"/>.
    /// </summary>
    /// <param name="behavior">The parent <see cref="ComboBoxFilterBehavior"/> instance.</param>
    /// <param name="item">The <see cref="FilterItem"/> to represent as a filter item.</param>
    public FilterViewModel(ComboBoxFilterBehavior behavior, FilterItem item)
    {
        this.behavior = behavior;
        Name = item.Name;
        IsChecked = true;
        Value = (int)item.Flag;
    }

    private readonly ComboBoxFilterBehavior? behavior;

    /// <summary>
    /// Gets or sets the reference to the "All" filter view model associated with this filter item.
    /// </summary>
    public AllFilterViewModel? AllFilter;

    /// <summary>
    /// Gets or sets the name of the filter item.
    /// </summary>
    [ObservableProperty]
    public string? name;

    /// <summary>
    /// Gets or sets a value indicating whether this filter item is checked.
    /// </summary>
    [ObservableProperty]
    public bool? isChecked;

    /// <summary>
    /// Gets or sets the integer value representing the filter flag for this item.
    /// </summary>
    [ObservableProperty]
    public int value;

    /// <summary>
    /// Gets or sets a value indicating whether the filter item supports three-state (checked, unchecked, indeterminate).
    /// </summary>
    [ObservableProperty]
    public bool isThreeState = false;

    partial void OnIsCheckedChanged(bool? value) => IsCheckedChanged(value);

    /// <summary>
    /// Called when the IsChecked property changes. Updates the "All" filter and notifies the behavior.
    /// </summary>
    /// <param name="value">The new checked state.</param>
    protected virtual void IsCheckedChanged(bool? value)
    {
        AllFilter?.Update();
        this.behavior?.UpdateValue();
    }
}

/// <summary>
/// Represents a filter item for use in ComboBoxFilterBehavior.
/// </summary>
public class FilterItem
{
    /// <summary>
    /// Gets or sets the name of the filter item.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the flag value representing this filter item. Each flag is a unique power of two, used for bitmask operations.
    /// </summary>
    public uint Flag { get; private set; }

    /// <summary>
    /// Assigns unique flag values to each filter item in the collection.
    /// </summary>
    public static void DefineFlags(IEnumerable<FilterItem>? items)
    {
        if (items == null) return;

        int index = 0;
        foreach (var item in items)
        {
            item.Flag = 1u << index++;
        }
    }
}

/// <summary>
/// ViewModel for the "All" filter option, which manages the tri-state logic for all filter items.
/// </summary>
public partial class AllFilterViewModel : FilterViewModel
{
    private readonly List<FilterViewModel> filters;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllFilterViewModel"/> class.
    /// </summary>
    /// <param name="filters">The collection of <see cref="FilterViewModel"/> items to manage with the "All" filter.</param>
    public AllFilterViewModel(IEnumerable<FilterViewModel> filters)
    {
        this.filters = filters.ToList();
        this.filters.ForEach(f => f.AllFilter = this);
        this.Name = "All";
        IsChecked = true;
        Value = this.filters.Select(f => f.Value).Aggregate(0, (a, b) => a | b);
        IsThreeState = false;
    }

    /// <summary>
    /// Updates all filter items when the "All" filter is checked or unchecked.
    /// </summary>
    protected override void IsCheckedChanged(bool? value)
    {
        if (value.HasValue)
        {
            filters.ForEach(f => f.IsChecked = value);
        }
    }

    /// <summary>
    /// Updates the "All" filter's checked state based on the state of all filter items.
    /// </summary>
    public void Update()
    {
        if (this.filters.All(f => f.IsChecked == true))
        {
            this.IsChecked = true;
        }
        else if (this.filters.All(f => f.IsChecked == false))
        {
            this.IsChecked = false;
        }
        else
        {
            this.IsChecked = null;
        }
    }
}




