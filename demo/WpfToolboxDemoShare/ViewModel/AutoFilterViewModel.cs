namespace WpfToolboxDemo.ViewModel;

public enum Enum1
{
    Empty, Active, Inactive, Removed
}

public enum Enum2
{
    [Description("--Transparent--")]
    Transparent,
    [Description("--Red--")]
    Red,
    [Description("--Green--")]
    Green,
    [Description("--Blue--")]
    Blue,
    [Description("--White--")]
    White,
    [Description("--Black--")]
    Black
}

public partial class AutoFilterItemViewModel(string name, string description, Enum1 enum1, Enum2 enum2, string group) : ObservableObject
{
    [ObservableProperty]
    private string name = name;

    [ObservableProperty]
    private string description = description;

    [ObservableProperty]
    private Enum1 enum1 = enum1;

    [ObservableProperty]
    private Enum2 enum2 = enum2;

    [ObservableProperty]
    private string group = group;
}

public partial class AutoFilterViewModel : ObservableObject
{
    public AutoFilterViewModel()
    {
        Items = new ListCollectionView(new List<AutoFilterItemViewModel>(
            [
            new("Peter", "Peter Description", Enum1.Active,   Enum2.Green, "Group D"),
            new("PaulS", "PaulS Description", Enum1.Inactive, Enum2.Green, "Group D"),
            new("Susie", "Susie Description", Enum1.Active,   Enum2.Blue,  "Group B"),
            new("UllyS", "UllyS Description", Enum1.Removed,  Enum2.Blue,  "Group B"),
            new("Diete", "Diete Description", Enum1.Active,   Enum2.Green, "Group C"),
            new("Renat", "Renat Description", Enum1.Inactive, Enum2.Green, "Group C"),
            new("Wolfg", "Wolfg Description", Enum1.Removed,  Enum2.Blue,  "Group C"),
            new("Sabbe", "Sabbe Description", Enum1.Active,   Enum2.Green, "Group A"),
            new("AnjaS", "AnjaS Description", Enum1.Active,   Enum2.Blue,  "Group A"),
            new("Felix", "Felix Description", Enum1.Active,   Enum2.Blue,  ""),
            ]));       
    }
    
    [ObservableProperty]
    private ListCollectionView items;    
}
