namespace WpfToolbox.Controls;

public class AutoGrayableImage : Image
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoGrayableImage"/> class.
    /// </summary>
    static AutoGrayableImage()
    {
        // Override the metadata of the IsEnabled and Source property.
        IsEnabledProperty.OverrideMetadata(typeof(AutoGrayableImage), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAutoGreyScaleImageIsEnabledPropertyChanged)));
        SourceProperty.OverrideMetadata(typeof(AutoGrayableImage), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAutoGreyScaleImageSourcePropertyChanged)));
    }

    protected static AutoGrayableImage? GetImageWithSource(DependencyObject source)
    {
        if (source is not AutoGrayableImage image || image.Source == null)
        {
            return null;
        }
        return image;
    }

    /// <summary>
    /// Called when [auto grey scale image source property changed].
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    protected static void OnAutoGreyScaleImageSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs ars)
    {
        var image = GetImageWithSource(source);
        if (image != null)
        {
            ApplyGreyScaleImage(image, image.IsEnabled);
        }
    }

    /// <summary>
    /// Called when [auto grey scale image is enabled property changed].
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    protected static void OnAutoGreyScaleImageIsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
    {
        var image = GetImageWithSource(source);
        if (image != null)
        {
            var isEnabled = Convert.ToBoolean(args.NewValue);
            ApplyGreyScaleImage(image, isEnabled);
        }
    }

    protected static void ApplyGreyScaleImage(AutoGrayableImage autoGreyScaleImg, Boolean isEnabled)
    {
        try
        {
            if (!isEnabled)
            {
                BitmapSource? bitmapImage = null;

                if (autoGreyScaleImg.Source is FormatConvertedBitmap)
                {
                    // Already gray !
                    return;
                }
                else if (autoGreyScaleImg.Source is BitmapSource)
                {
                    bitmapImage = (BitmapSource)autoGreyScaleImg.Source;
                }
                else // trying string 
                {
                    bitmapImage = new BitmapImage(new Uri(autoGreyScaleImg.Source.ToString()));
                }
                FormatConvertedBitmap conv = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                autoGreyScaleImg.Source = conv;

                // Create Opacity Mask for grayscale image as FormatConvertedBitmap does not keep transparency info
                autoGreyScaleImg.OpacityMask = new ImageBrush(((FormatConvertedBitmap)autoGreyScaleImg.Source).Source); //equivalent to new ImageBrush(bitmapImage)
            }
            else
            {
                if (autoGreyScaleImg.Source is FormatConvertedBitmap bitmap)
                {
                    autoGreyScaleImg.Source = bitmap.Source;
                }
                else if (autoGreyScaleImg.Source is BitmapSource)
                {
                    // Should be full color already.
                    return;
                }

                // Reset the Opcity Mask
                autoGreyScaleImg.OpacityMask = null;
            }
        }
        catch (Exception)
        {
            // nothin'
        }

    }
}
