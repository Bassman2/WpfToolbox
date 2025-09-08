namespace WpfToolboxDemo.ViewModel;

public partial class NumericViewModel : ObservableObject
{
    [ObservableProperty]
    private int spinIntValue = 0;

    [ObservableProperty]
    private int sliderIntValue = 0;

    [ObservableProperty]
    private double spinDoubleValue = 0;

    [ObservableProperty]
    private double sliderDoubleValue = 0;

    [ObservableProperty]
    private decimal spinDecimalValue = 0;

    [ObservableProperty]
    private decimal sliderDecimalValue = 0;
}
