namespace WpfToolboxDemo.ViewModel;

public partial class CheckComboBoxViewModel : ObservableObject
{
    public CheckComboBoxViewModel()
    {
        TextItems =
        [
            "Item_A 1",
            "Item_A 2",
            "Item_A 3",
            "Item_A 4",
            "Item_A 5"
        ];
        CheckedTextItems = ["Item_A 2", "Item_A 4"];
        CheckedTextItems.CollectionChanged += (c, e) => OnUpdateTextItemsText();
        OnUpdateTextItemsText();

        DataItems =
        [
            new DataItem { Name = "Data_A 1", Value = 10 },
            new DataItem { Name = "Data_A 2", Value = 20 },
            new DataItem { Name = "Data_A 3", Value = 30 },
            new DataItem { Name = "Data_A 4", Value = 40 },
            new DataItem { Name = "Data_A 5", Value = 50 }
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
