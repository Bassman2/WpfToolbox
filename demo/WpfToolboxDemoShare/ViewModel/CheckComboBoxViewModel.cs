namespace WpfToolboxDemo.ViewModel;

public partial class CheckComboBoxViewModel : ObservableObject
{
    public CheckComboBoxViewModel()
    {
        TextItems =
        [
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5"
        ];
        CheckedTextItems = ["Item 2", "Item 4"];

        DataItems =
        [
            new DataItem { Name = "Data 1", Value = 10 },
            new DataItem { Name = "Data 2", Value = 20 },
            new DataItem { Name = "Data 3", Value = 30 },
            new DataItem { Name = "Data 4", Value = 40 },
            new DataItem { Name = "Data 5", Value = 50 }
        ];
        CheckedDataItems = [ DataItems[1], DataItems[2] ];
    }

    public List<string> TextItems { get; set; }

    [ObservableProperty]
    private List<string> checkedTextItems;

    public List<DataItem> DataItems { get; set; }

    [ObservableProperty]
    private List<DataItem> checkedDataItems;

    public class DataItem
    {
        public string Name { get; set; } = null!;
        public int Value { get; set; }

        public override string ToString() => Name;
    }
}
