namespace WpfToolbox.Controls;


//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Ribbon;
//using System.Windows.Data;

public class RibbonComboBoxEx : RibbonComboBox
{
    public RibbonComboBoxEx()
    {
        this.Loaded += OnLoaded;
    }

    private RibbonGallery? ribbonGallery;

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

    public GridLength LabelWidth { get; set; }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(object), typeof(RibbonComboBoxEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedItemChanged)));

    public object SelectedItem
    {
        get { return (object)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RibbonComboBoxEx ribbonComboBoxEx && ribbonComboBoxEx.ribbonGallery != null)
        {
            ribbonComboBoxEx.ribbonGallery.SelectedItem = e.NewValue;
        }
    }

    public static new readonly DependencyProperty ItemsSourceProperty =
       DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(RibbonComboBoxEx), new FrameworkPropertyMetadata(null));

    public new IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }
}
