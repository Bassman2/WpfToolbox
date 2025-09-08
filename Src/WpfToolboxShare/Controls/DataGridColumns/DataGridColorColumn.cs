namespace WpfToolbox.Controls;

// https://github.com/dotnet/wpf/tree/main/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls

/// <summary>
/// Represents a DataGrid column that displays and edits color values.
/// Shows a color swatch and name in display mode, and a ComboBox with color selection in edit mode.
/// </summary>
public class DataGridColorColumn : DataGridBoundColumn
{
    /// <summary>
    /// Helper class to associate a color with its name and a SolidColorBrush.
    /// Used for populating the ComboBox and for display.
    /// </summary>
    [DebuggerDisplay("ColorName {Color} {Name}")]
    private class ColorName 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorName"/> class from a PropertyInfo.
        /// </summary>
        /// <param name="propertyInfo">The property info representing a color in <see cref="Colors"/>.</param>
        public ColorName(PropertyInfo propertyInfo) 
        {
            Name = propertyInfo.Name;
            Color = (Color)propertyInfo.GetValue(null)!;
            Brush = new SolidColorBrush(Color);
        }

        /// <summary>
        /// The name of the color.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The color value.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// The brush representing the color.
        /// </summary>
        public Brush Brush { get; } 
    }

    /// <summary>
    /// Static list of all named colors from the <see cref="Colors"/> class.
    /// </summary>
    private readonly static List<ColorName> colorNames = [.. typeof(Colors).GetProperties().Select(p => new ColorName(p))];

    /// <summary>
    /// Generates the display element for the cell, showing a color swatch and the color name.
    /// </summary>
    /// <param name="cell">The parent DataGridCell.</param>
    /// <param name="dataItem">The data item for the row.</param>
    /// <returns>A StackPanel containing a Rectangle (color swatch) and a TextBlock (color name).</returns>
    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        Rectangle rectangle = new() { Width = 14, Height = 14, Margin = new Thickness(6, 0, 2, 0) };
        Binding rectBinding = new() { Path = ((Binding)this.Binding).Path, Converter = new ColorBrushConverter() };
        BindingOperations.SetBinding(rectangle, Rectangle.FillProperty, rectBinding);

        // Color Name
        TextBlock textBlock = new();
        Binding textBinding = new() { Path = ((Binding)this.Binding).Path, Converter = new ColorNameConverter() };
        BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, textBinding);

        StackPanel stackPanel = new() { Orientation = Orientation.Horizontal };
        stackPanel.Children.Add(rectangle);
        stackPanel.Children.Add(textBlock);
        return stackPanel;
    }

    /// <summary>
    /// Generates the editing element for the cell, providing a ComboBox for color selection.
    /// </summary>
    /// <param name="cell">The parent DataGridCell.</param>
    /// <param name="dataItem">The data item for the row.</param>
    /// <returns>A ComboBox for selecting a color.</returns>
    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        ComboBox comboBox = new() { ItemsSource = colorNames };
        comboBox.SetBinding(ComboBox.SelectedValueProperty, this.Binding);
        comboBox.SetValue(ComboBox.SelectedValuePathProperty, "Color");


        DataTemplate dataTemplate = new(typeof(ComboBox));

        FrameworkElementFactory stackPanel = new(typeof(StackPanel));
        stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

        FrameworkElementFactory rectangle = new(typeof(System.Windows.Shapes.Rectangle));
        rectangle.SetBinding(System.Windows.Shapes.Rectangle.FillProperty, new Binding("Brush"));
        rectangle.SetValue(System.Windows.Shapes.Rectangle.WidthProperty, 14.0);
        rectangle.SetValue(System.Windows.Shapes.Rectangle.HeightProperty, 14.0);
        rectangle.SetValue(System.Windows.Shapes.Rectangle.MarginProperty, new Thickness(0, 0, 2, 0));
        stackPanel.AppendChild(rectangle);

        FrameworkElementFactory textBlock = new(typeof(TextBlock));
        textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name")); // { Converter = new ColorToNameConverter() });

        stackPanel.AppendChild(textBlock);

        dataTemplate.VisualTree = stackPanel;

        comboBox.ItemTemplate = dataTemplate;
        return comboBox;
    }

    /// <summary>
    /// Converts a Color value to a SolidColorBrush for display in the UI.
    /// </summary>
    public class ColorBrushConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return colorNames.First(c => c.Color == color).Brush;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// Converts a Color value to its name for display in the UI.
    /// </summary>
    public class ColorNameConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return colorNames.First(c => c.Color == color).Name;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}


