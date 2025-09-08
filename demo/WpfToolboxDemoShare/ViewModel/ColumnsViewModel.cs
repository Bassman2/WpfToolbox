namespace WpfToolboxDemo.ViewModel;

public partial class ColumnsItemViewModel(string name, string description, Color color) : ObservableObject
{
    [ObservableProperty]
    private string name = name;

    [ObservableProperty]
    private string description = description;

    [ObservableProperty]
    private Color color = color;
    
    [ObservableProperty]
    private string password = "Demo";

    [RelayCommand]
    public static void OnItemButton(object parameter)
    {
        ColumnsItemViewModel item = (ColumnsItemViewModel)parameter;
        MessageBox.Show($"VM Item Button pressed for {item.Name}");
    }

}
public partial class ColumnsViewModel : ObservableObject
{
    public ColumnsViewModel()
    {
        Items = new ListCollectionView(new List<ColumnsItemViewModel>(
            [
            new("Peter", "Peter Description", Colors.Red),
            new("PaulS", "PaulS Description", Colors.Bisque),
            new("Susie", "Susie Description", Colors.Blue),
            new("UllyS", "UllyS Description", Colors.DarkGray),
            new("Diete", "Diete Description", Colors.Firebrick),
            new("Renat", "Renat Description", Colors.Green),
            new("Wolfg", "Wolfg Description", Colors.Yellow),
            new("Sabbe", "Sabbe Description", Colors.Maroon),
            new("AnjaS", "AnjaS Description", Colors.Navy),
            new("Felix", "Felix Description", Colors.Orange)
            ]));
    }

    [ObservableProperty]
    private ListCollectionView items;

    [RelayCommand]
    public static void OnButton(object parameter)
    {
        ColumnsItemViewModel item = (ColumnsItemViewModel)parameter;
        MessageBox.Show($"VM Button pressed for {item.Name}");
    }
}
