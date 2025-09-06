

namespace WpfToolbox.Behaviors;

public class ComboBoxFilterBehavior : Behavior<ComboBox>
{ 
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

    protected override void OnDetaching()
    {
        //AssociatedObject.TextChanged -= OnAssociatedObjectTextChanged;
    }

    
    public static readonly DependencyProperty EnumTypeProperty =
        DependencyProperty.Register("EnumType", typeof(Type), typeof(ComboBoxFilterBehavior),
        new PropertyMetadata(null));


    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable<FilterItem>), typeof(ComboBoxFilterBehavior),
        new PropertyMetadata(null, (d, e) => ((ComboBoxFilterBehavior)d).OnItemsSource(e.NewValue)));


    public static readonly DependencyProperty FilterValueProperty =
        DependencyProperty.Register("FilterValue", typeof(int), typeof(ComboBoxFilterBehavior), new FrameworkPropertyMetadata()
        {
            BindsTwoWayByDefault = true,
            //DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
    
    public Type EnumType
    {
        get { return (Type)GetValue(EnumTypeProperty); }
        set { SetValue(EnumTypeProperty, value); }
    }

    public IEnumerable<FilterItem>? ItemsSource
    {
        get { return (IEnumerable<FilterItem>?)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

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
    
    private void OnItemsSource(object newValue)
    {
        Update();   
    }
    

    private List<FilterViewModel>? filters;

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

public partial class FilterViewModel : ObservableObject
{
    public FilterViewModel()
    { }

    public FilterViewModel(ComboBoxFilterBehavior behavior, object e)
    {
        this.behavior = behavior;
        Name = e.ToString();
        IsChecked = true;
        Value = (int)e;
    }

    public FilterViewModel(ComboBoxFilterBehavior behavior, FilterItem item)
    {
        this.behavior = behavior;
        Name = item.Name;
        IsChecked = true;
        Value = (int)item.Flag;
    }

    private readonly ComboBoxFilterBehavior? behavior;
    public AllFilterViewModel? AllFilter;

    [ObservableProperty]
    public string? name;

    [ObservableProperty]
    public bool? isChecked;

    [ObservableProperty]
    public int value;

    [ObservableProperty]
    public bool isThreeState = false;

    partial void OnIsCheckedChanged(bool? value) => IsCheckedChanged(value);

    protected virtual void IsCheckedChanged(bool? value)
    {
        AllFilter?.Update();
        this.behavior?.UpdateValue();
    }
}

public class FilterItem
{
    public string? Name { get; set; }

    public uint Flag { get; private set; }

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

public partial class AllFilterViewModel : FilterViewModel
{
    private readonly List<FilterViewModel> filters;

    public AllFilterViewModel(IEnumerable<FilterViewModel> filters)
    {
        this.filters = filters.ToList();
        this.filters.ForEach(f => f.AllFilter = this);
        this.Name = "All";
        IsChecked = true;
        Value = this.filters.Select(f => f.Value).Aggregate(0, (a, b) => a | b);
        IsThreeState = false;
    }


    protected override void IsCheckedChanged(bool? value)
    {
        if (value.HasValue)
        {
            filters.ForEach(f => f.IsChecked = value);
        }
    }

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




