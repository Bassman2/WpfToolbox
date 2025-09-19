namespace WpfToolbox.Controls;

/// <summary>
/// A WPF ComboBox that displays the values of a specified enum type.
/// Supports displaying descriptions, localized resources, and images for each enum value using custom attributes.
/// </summary>
public class EnumComboBox : ComboBox
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumComboBox"/> class.
    /// Sets the <see cref="Selector.SelectedValuePath"/> to "Value".
    /// </summary>
    public EnumComboBox()
    {
        SelectedValuePath = "Value";
    }

    /// <summary>
    /// Identifies the <see cref="EnumType"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EnumTypeProperty =
        DependencyProperty.Register("EnumType", typeof(Type), typeof(EnumComboBox),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback((o, e) => ((EnumComboBox)o).OnEnumTypePropertyChanged(e))));

        /// <summary>
    /// Gets or sets the enum type to display in the ComboBox.
    /// </summary>
    public Type EnumType
    {
        get => (Type)GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }
    
    /// <summary>
    /// Handles changes to the <see cref="EnumType"/> property.
    /// Sets up the ComboBox items and item template based on the enum's attributes.
    /// </summary>
    /// <param name="e">The event data.</param>
    private void OnEnumTypePropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue == null)
        {
            this.ItemsSource = null;
            this.ItemTemplate = null;
            return;
        }

        Type enumType = (Type)e.NewValue;
        if (enumType.IsEnum == false)
        {
            throw new ArgumentException("Type must be an Enum.");
        }

        var enumValues = Enum.GetValues(enumType).Cast<object>();

        // check if any enum value has the ImageAttribute
        if (enumValues.Any(item => item.GetFieldInfo()!.GetCustomAttribute<ImageAttribute>() is not null))
        {
            // Create ItemTemplate for Images and Text
            var template = new DataTemplate() { DataType = typeof(string) };
            var dockPanel = new FrameworkElementFactory(typeof(DockPanel));
            dockPanel.SetValue(DockPanel.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);

            var image = new FrameworkElementFactory(typeof(Image));
            image.SetBinding(Image.SourceProperty, new Binding("Image"));
            image.SetValue(Image.WidthProperty, 16.0);
            image.SetValue(Image.HeightProperty, 16.0);
            image.SetValue(Image.MarginProperty, new Thickness(0, 0, 4.0, 0.0));

            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("Description"));
            textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            //textBlock.SetValue(TextBlock.BackgroundProperty, Brushes.Yellow);

            dockPanel.AppendChild(image);
            dockPanel.AppendChild(textBlock);
            template.VisualTree = dockPanel;

            this.ItemTemplate = template;
        }
        else
        {
            DisplayMemberPath = "Description";
        }

        this.ItemsSource = enumValues.Select(i => new EnumerationMember(i)).ToList();
    }

    /// <summary>
    /// Represents a single enum value with its associated description and image.
    /// Used as the item type for the ComboBox.
    /// </summary>
    private class EnumerationMember 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationMember"/> class.
        /// Extracts description and image information from the enum value's attributes.
        /// </summary>
        /// <param name="item">The enum value.</param>
        public EnumerationMember(object item)
        {
            Value = item;

            FieldInfo? fieldInfo = item.GetFieldInfo();
            if (fieldInfo?.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute descriptionAttribute)
            {
                Description = descriptionAttribute.Description;
            }
            else if (fieldInfo?.GetCustomAttribute<ResourceAttribute>() is ResourceAttribute resourceAttribute)
            {
                Description = EntryAssemblyResourceManager.GetString(resourceAttribute.Name) ?? "";
            }            
            else
            {
                Description = fieldInfo?.Name ?? "";
            }

            if (fieldInfo?.GetCustomAttribute<ImageAttribute>() is ImageAttribute imageAttribute)
            {
                Image = imageAttribute.Source;
            }
        }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets the description to display for the enum value.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the image source associated with the enum value, if any.
        /// </summary>
        public string Image { get; } = string.Empty;
    }
}
