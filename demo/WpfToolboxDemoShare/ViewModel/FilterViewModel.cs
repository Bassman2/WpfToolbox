namespace WpfToolboxDemo.ViewModel;

[Flags]
public enum EnumFilter
{
    [Description("Active")]
    Active      /**/= 0b_00000000_00000000_00000000_00000001, // = 1,
    Deactivated /**/= 0b_00000000_00000000_00000000_00000010, // = 2,
    Reserve     /**/= 0b_00000000_00000000_00000000_00000100, // = 4,
    Removed     /**/= 0b_00000000_00000000_00000000_00001000, // = 8,
    Reused      /**/= 0b_00000000_00000000_00000000_00010000, // = 16
}

public class FilterItem //: IFilterItem
{
    private static int value = 1;
    public FilterItem(string name)
    {
        Name = name;
        Value = value;
        value = value << 1;
    }

    public string Name { get; set; }
    public int Value { get; set; }
}

public partial class FilterItemViewModel : ObservableObject
{
    public FilterItemViewModel(string name, string description, Color color, EnumFilter filterEnum, FilterItem filterList)
    {
        this.Name = name;
        this.Description = description;
        this.Color = color;
        this.FilterEnum = filterEnum;
        this.FilterList = filterList.Name;
        this.FilterListValue = filterList.Value;
    }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private Color color = Colors.Red;

    [ObservableProperty]
    private string filterList = string.Empty;

    public int FilterListValue = 0;

    [ObservableProperty]
    private EnumFilter filterEnum = EnumFilter.Active;

    [ObservableProperty]
    private string password = "xxxxxxx";

    [RelayCommand]
    public static void OnButton()
    {
        MessageBox.Show("Button Pressed");
    }
}

public partial class FilterViewModel : ObservableObject
{
    public FilterViewModel()
    {

        this.FilterListItems =
        [
            new FilterItem("Empty"),
            new FilterItem("Tree"),
            new FilterItem("Flower")
        ];
        this.Items =
        [
            new FilterItemViewModel("Peter", "X Peter X", Colors.Red, EnumFilter.Active, this.FilterListItems[0]),
            new FilterItemViewModel("Paul",  "X Paul X" , Colors.Blue, EnumFilter.Reserve, this.FilterListItems[1]),
            new FilterItemViewModel("Susi",  "X Susi X" , Colors.Pink, EnumFilter.Removed, this.FilterListItems[2])
        ];
        this.FilterItems = new(this.Items) { Filter = Filter };
        this.FilterItems.Refresh();
    }

    private bool Filter(object obj)
    {
        FilterItemViewModel item = (FilterItemViewModel)obj;
        return (((int)item.FilterEnum) & FilterEnumValue) != 0 && (((int)item.FilterListValue) & FilterListValue) != 0;

    }

    [ObservableProperty]
    private ObservableCollection<FilterItemViewModel> items;

    [ObservableProperty]
    private ListCollectionView filterItems;

    [ObservableProperty]
    public FilterItem[] filterListItems;

    [ObservableProperty]
    public int filterEnumValue = 0x7fffffff;

    partial void OnFilterEnumValueChanged(int value) => this.FilterItems.Refresh();

    [ObservableProperty]
    public int filterListValue = 0x7fffffff;

    partial void OnFilterListValueChanged(int value) => this.FilterItems.Refresh();


    [ObservableProperty]
    private int integerSliderValue = 1;

    [ObservableProperty]
    private double doubleSliderValue = 1.0;

    [ObservableProperty]
    private int integerSpinValue = 1;

    [ObservableProperty]
    private double doubleSpinValue = 1.0;
}
