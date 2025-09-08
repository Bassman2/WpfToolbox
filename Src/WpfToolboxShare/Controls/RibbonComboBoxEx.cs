namespace WpfToolbox.Controls;

/// <summary>
/// An extended RibbonComboBox control that integrates a RibbonGallery for advanced item presentation.
/// Supports custom item templates, item sources, and label width configuration.
/// </summary>
public class RibbonComboBoxEx : RibbonComboBox
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RibbonComboBoxEx"/> class.
    /// Subscribes to the Loaded event.
    /// </summary>
    public RibbonComboBoxEx()
    {
        this.Loaded += OnLoaded;
    }

    private RibbonGallery? ribbonGallery;

    /// <summary>
    /// Applies the control template and sets up the RibbonGallery and its bindings.
    /// Also configures the label width if specified.
    /// </summary>
    public override void OnApplyTemplate()
    {
        ribbonGallery = new RibbonGallery();
        ribbonGallery.SetBinding(RibbonGallery.SelectedItemProperty, new Binding("SelectedItem") { Source = this });

        RibbonGalleryCategory ribbonGalleryCategory = new();

        FrameworkElementFactory factoryPanel = new(typeof(StackPanel));
        factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
        ribbonGalleryCategory.ItemsPanel = new ItemsPanelTemplate() { VisualTree = factoryPanel }; 
        ribbonGalleryCategory.SetBinding(RibbonGalleryCategory.ItemTemplateProperty, new Binding("ItemTemplate") { Source = this });
        ribbonGalleryCategory.SetBinding(RibbonGalleryCategory.ItemsSourceProperty, new Binding("ItemsSource") { Source = this });

        ribbonGallery.Items.Add(ribbonGalleryCategory);

        this.AddChild(ribbonGallery);

        base.OnApplyTemplate();

        // set label width
        var temp = this.Template;
        if (temp != null)
        {
            if (temp.FindName("TwoLineText", this) is RibbonTwoLineText rtlt && this.LabelWidth.IsAbsolute)
            {
                rtlt.Width = this.LabelWidth.Value;
                rtlt.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }
    }

    /// <summary>
    /// Handles the Loaded event to ensure the RibbonGallery's selected item is synchronized.
    /// </summary>
    private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is RibbonComboBoxEx cb && cb.ribbonGallery != null)
        {
            // re-trigger first setting
            var bindingExpression = BindingOperations.GetBindingExpression(cb, RibbonComboBoxEx.SelectedItemProperty);
            bindingExpression?.UpdateTarget();

            cb.ribbonGallery.SelectedItem = this.SelectedItem;
        }
    }

    /// <summary>
    /// Gets or sets the width of the label area.
    /// </summary>
    public GridLength LabelWidth { get; set; }

    /// <summary>
    /// Identifies the <see cref="SelectedItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(object), typeof(RibbonComboBoxEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedItemChanged)));

    /// <summary>
    /// Gets or sets the selected item in the RibbonComboBoxEx.
    /// </summary>
    public object SelectedItem
    {
        get { return (object)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    /// <summary>
    /// Handles changes to the <see cref="SelectedItem"/> property and updates the RibbonGallery selection.
    /// </summary>
    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RibbonComboBoxEx ribbonComboBoxEx && ribbonComboBoxEx.ribbonGallery != null)
        {
            ribbonComboBoxEx.ribbonGallery.SelectedItem = e.NewValue;
        }
    }

    /// <summary>
    /// Identifies the <see cref="ItemsSource"/> dependency property.
    /// </summary>
    public static new readonly DependencyProperty ItemsSourceProperty =
       DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(RibbonComboBoxEx), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets the collection used to generate the content of the RibbonComboBoxEx.
    /// </summary>
    public new IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }
}
