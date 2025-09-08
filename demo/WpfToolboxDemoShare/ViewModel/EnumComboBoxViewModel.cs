namespace WpfToolboxDemo.ViewModel;


public enum EnumName
{
    Blue,
    Red,
    Green,
    Yellow,
    Magenta,
    Pink,
    White
}

public enum EnumNameImage
{
    [Image("/Images/Blue.png")]
    Blue,
    [Image("/Images/Red.png")]
    Red,
    Green,
    [Image("/Images/Yellow.png")]
    Yellow,
    Magenta,
    Pink,
    White
}


public enum EnumDesc
{
    [Description("Blue")]
    B,
    [Description("Red")]
    R,
    [Description("Green")]
    G,
    [Description("Yellow")]
    Y,
    [Description("Magenta")]
    M,
    [Description("Pink")]
    P,
    [Description("White")]
    W
}

public enum EnumDescImage
{
    [Image("/Images/Blue.png")]
    [Description("Blue")]
    B,
    [Image("/Images/Red.png")]
    [Description("Red")]
    R,
    [Description("Green")]
    G,
    [Image("/Images/Yellow.png")]
    [Description("Yellow")]
    Y,
    [Description("Magenta")]
    M,
    [Description("Pink")]
    P,
    [Description("White")]
    W
}

public enum EnumResc
{
    [Resource("ColorBlue")]
    B,
    [Resource("ColorRed")]
    R,
    [Resource("ColorGreen")]
    G,
    [Resource("ColorYellow")]
    Y,
    [Resource("ColorMagenta")]
    M,
    [Resource("ColorPink")]
    P,
    [Resource("ColorWhite")]
    W
}

public enum EnumRescImage
{
    [Image("/Images/Blue.png")]
    [Resource("ColorBlue")]
    B,
    [Image("/Images/Red.png")]
    [Resource("ColorRed")]
    R,
    [Resource("ColorGreen")]
    G,
    [Image("/Images/Yellow.png")]
    [Resource("ColorYellow")]
    Y,
    [Resource("ColorMagenta")]
    M,
    [Resource("ColorPink")]
    P,
    [Resource("ColorWhite")]
    W
}

public partial class EnumComboBoxViewModel : ObservableObject
{
    [ObservableProperty]
    private EnumName selectedEnumName = EnumName.Red;

    [ObservableProperty]
    private EnumNameImage selectedEnumNameImage = EnumNameImage.Red;

    [ObservableProperty]
    private EnumDesc selectedEnumDesc = EnumDesc.R;

    [ObservableProperty]
    private EnumDescImage selectedEnumDescImage = EnumDescImage.R;

    [ObservableProperty]
    private EnumResc selectedEnumResc = EnumResc.W;

    [ObservableProperty]
    private EnumRescImage selectedEnumRescImage = EnumRescImage.W;
}
