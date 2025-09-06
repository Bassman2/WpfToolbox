using System.Windows.Media.Imaging;

namespace WpfToolbox.Converter;

[ValueConversion(typeof(bool), typeof(ImageSource))]
public class WorkingToImageBallConverter : IValueConverter
{
    static readonly ImageSource workingImage;
    static readonly ImageSource notworkImage;

    static WorkingToImageBallConverter()
    {
        notworkImage = new BitmapImage(new Uri("pack://application:,,,/WpfToolbox;component/Images/BallGrey16.png", UriKind.Absolute));
        workingImage = new BitmapImage(new Uri("pack://application:,,,/WpfToolbox;component/Images/BallBlue16.png", UriKind.Absolute));
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? workingImage : notworkImage;
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

