namespace WpfToolbox.Controls;

public class DataGridButtonColumn : DataGridBoundColumn
{
    public DataGridButtonColumn()
    {
        this.IsReadOnly = true;
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(DataGridButtonColumn));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty ContentTemplateProperty =
        DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(DataGridButtonColumn));

    public DataTemplate ContentTemplate
    {
        get => (DataTemplate)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public BindingBase? CommandBinding { get; set; }

    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        Button button = new();
        if (this.ContentTemplate is not null)
        {
            button.ContentTemplate = this.ContentTemplate;
        }
        else if(this.Text is not null)
        {
            button.Content = this.Text;
        }
        else if (this.Binding is not null)
        {
            button.SetBinding(Button.ContentProperty, this.Binding);
        }
        
        if (this.CommandBinding is not null)
        {
            button.SetBinding(Button.CommandProperty, this.CommandBinding);
            button.CommandParameter = dataItem;
        }
        return button;
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        throw new NotImplementedException();
    }
}