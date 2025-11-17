namespace WpfToolboxDemo.ViewModel;

public partial class FilteredComboBoxViewModel : ObservableObject
{
    public FilteredComboBoxViewModel()
    {
        StringItems = new List<string>
        {
            "Apple",
            "Banana",
            "Cherry",
            "Date",
            "Elderberry",
            "Fig",
            "Grape",
            "Honeydew"
        };
        SelectedStringItem = StringItems[4];

        ClassItems = new List<FilteredComboBoxItem>
        {
            new FilteredComboBoxItem { Name = "Apple", Value = 1 },
            new FilteredComboBoxItem { Name = "Banana", Value = 2 },
            new FilteredComboBoxItem { Name = "Cherry", Value = 3 },
            new FilteredComboBoxItem { Name = "Date", Value = 4 },
            new FilteredComboBoxItem { Name = "Elderberry", Value = 5 },
            new FilteredComboBoxItem { Name = "Fig", Value = 6 },
            new FilteredComboBoxItem { Name = "Grape", Value = 7 },
            new FilteredComboBoxItem { Name = "Honeydew", Value = 8 }
        };
        SelectedClassItem = ClassItems[4];
    }

    [ObservableProperty]
    private List<string> stringItems;

    [ObservableProperty]
    private string? selectedStringItem;

    [ObservableProperty]
    private List<FilteredComboBoxItem> classItems;

    [ObservableProperty]
    private FilteredComboBoxItem? selectedClassItem;

    partial void OnSelectedClassItemChanged(FilteredComboBoxItem? value)
    {       
    }
}

[DebuggerDisplay("{Name} (#{Value})")]
public class FilteredComboBoxItem 
{
    public required string Name { get; init;}

    public int Value { get; init; }

    public override string ToString() => $"{Name} (#{Value})";
}

