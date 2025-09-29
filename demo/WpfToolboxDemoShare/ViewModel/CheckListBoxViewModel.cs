namespace WpfToolboxDemo.ViewModel;

public partial class CheckListBoxViewModel : ObservableObject
{
    public CheckListBoxViewModel()
    {
        TextItems =
        [
            "Item_A 1",
            "Item_A 2",
            "Item_A 3",
            "Item_A 4",
            "Item_A 5"
        ];
        CheckedTextItems = [ "Item_A 2", "Item_A 4" ];
        CheckedTextItems.CollectionChanged += (c, e) => OnUpdateTextItemsText();
        OnUpdateTextItemsText();

        DataItems =
        [
            new CheckListBoxtem { Name = "Data_A 1", Value = 10 },
            new CheckListBoxtem { Name = "Data_A 2", Value = 20 },
            new CheckListBoxtem { Name = "Data_A 3", Value = 30 },
            new CheckListBoxtem { Name = "Data_A 4", Value = 40 },
            new CheckListBoxtem { Name = "Data_A 5", Value = 50 }
        ];
        CheckedDataItems = [ DataItems[1], DataItems[2] ];
    }

    #region TextItems

    private void OnUpdateTextItemsText()
    {
        TextItemsText = string.Join(", ", CheckedTextItems.Order());
    }

    public List<string> TextItems { get; set; }

    [ObservableProperty]
    private ObservableCollection<string> checkedTextItems;

    [ObservableProperty]
    private string textItemsText = "";

    #endregion
    public List<CheckListBoxtem> DataItems { get; set; }

    [ObservableProperty]
    private List<CheckListBoxtem> checkedDataItems;
}

public class CheckListBoxtem
{
    public string Name { get; set; } = null!;
    public int Value { get; set; }

    public override string ToString() => Name;

    public override int GetHashCode()
        => (Name, Value).GetHashCode();

    public override bool Equals(object? obj)
        => obj is CheckListBoxtem other && Name == other.Name && Value == other.Value;
}