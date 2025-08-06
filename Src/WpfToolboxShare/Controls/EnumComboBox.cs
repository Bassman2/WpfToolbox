namespace WpfToolbox.Controls;

public class EnumComboBox : ComboBox
{
    public EnumComboBox()
    {
        SelectedValuePath = "Value";
    }

    public static readonly DependencyProperty EnumTypeProperty =
        DependencyProperty.Register("EnumType", typeof(Type), typeof(EnumComboBox),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback((o, e) => ((EnumComboBox)o).OnEnumTypePropertyChanged(e))));

    public Type EnumType
    {
        get => (Type)GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }
    
    private void OnEnumTypePropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        Type enumType = (Type)e.NewValue;
        if (enumType.IsEnum == false)
        {
            throw new ArgumentException("Type must be an Enum.");
        }

        var enumValues = Enum.GetValues(enumType).Cast<object>();

        // check if any enum value has the ImageAttribute
        if (enumValues.Any(item => item.GetFieldInfo().GetCustomAttribute<ImageAttribute>() is not null))
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
            textBlock.SetValue(TextBlock.BackgroundProperty, Brushes.Yellow);

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

    public class EnumerationMember 
    {
        public EnumerationMember(object item)
        {
            Value = item;

            FieldInfo fieldInfo = item.GetFieldInfo();
            if (fieldInfo.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute descriptionAttribute)
            {
                Description = descriptionAttribute.Description;
            }
            else if (fieldInfo.GetCustomAttribute<ResourceAttribute>() is ResourceAttribute resourceAttribute)
            {
                Description = EntryAssemblyResourceManager.GetString(resourceAttribute.Name) ?? "";
            }            
            else
            {
                Description = fieldInfo.Name;
            }

            if (fieldInfo.GetCustomAttribute<ImageAttribute>() is ImageAttribute imageAttribute)
            {
                Image = imageAttribute.Source;
            }
        }

        public object Value { get; }

        public string Description { get; }

        public string Image { get; } = string.Empty;
    }
}
