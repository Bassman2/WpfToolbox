namespace WpfToolbox.Converter;

/// <summary>
/// Converts a boolean value to an <see cref="ImageSource"/> representing a working or not-working state.
/// Returns a blue ball image for true (working) and a grey ball image for false (not working).
/// </summary>
[ValueConversion(typeof(bool), typeof(ImageSource))]
public class BoolToImageBallConverter : IValueConverter
{
    /// <summary>
    /// The image shown when the value is true (working).
    /// </summary>
    private static readonly ImageSource workingImage;

    /// <summary>
    /// The image shown when the value is false (not working).
    /// </summary>
    private static readonly ImageSource notworkImage;

    static BoolToImageBallConverter()
    {
        notworkImage = new BitmapImage(new Uri("pack://application:,,,/WpfToolbox;component/Images/BallGrey16.png", UriKind.Absolute));
        workingImage = new BitmapImage(new Uri("pack://application:,,,/WpfToolbox;component/Images/BallBlue16.png", UriKind.Absolute));
    }

    /// <summary>
    /// Converts a boolean value to the corresponding image.
    /// </summary>
    /// <param name="value">The value to convert (should be a boolean).</param>
    /// <param name="targetType">The target type (unused).</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture to use in the converter (unused).</param>
    /// <returns>The working or not-working image.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? workingImage : notworkImage;

    /// <summary>
    /// Not supported. Throws <see cref="NotImplementedException"/> if called.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

