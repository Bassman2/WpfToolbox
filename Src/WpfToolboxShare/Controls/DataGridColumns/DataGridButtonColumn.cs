namespace WpfToolbox.Controls;

/// <summary>
/// Represents a DataGrid column that displays a button in each cell.
/// The button can display static text, a data-bound value, or a custom content template.
/// </summary>
public class DataGridButtonColumn : DataGridBoundColumn
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataGridButtonColumn"/> class.
    /// Sets the column to be read-only by default.
    /// </summary>
    public DataGridButtonColumn()
    {
        this.IsReadOnly = true;
    }

    /// <summary>
    /// Identifies the Text dependency property.
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(DataGridButtonColumn));

    /// <summary>
    /// Gets or sets the static text to display on the button.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Identifies the ContentTemplate dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentTemplateProperty =
        DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(DataGridButtonColumn));

    /// <summary>
    /// Gets or sets the data template used to display the button's content.
    /// </summary>
    public DataTemplate ContentTemplate
    {
        get => (DataTemplate)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the binding for the button's command.
    /// </summary>
    public BindingBase? CommandBinding { get; set; }

    /// <summary>
    /// Generates the display element (button) for the cell.
    /// The button's content and command are set based on the column's properties.
    /// </summary>
    /// <param name="cell">The parent DataGridCell.</param>
    /// <param name="dataItem">The data item for the row.</param>
    /// <returns>A Button configured for the cell.</returns>
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

    /// <summary>
    /// Editing is not supported for this column; this method is not implemented.
    /// </summary>
    /// <param name="cell">The parent DataGridCell.</param>
    /// <param name="dataItem">The data item for the row.</param>
    /// <returns>Not implemented.</returns>
    /// <exception cref="NotImplementedException">Always thrown.</exception>
    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        throw new NotImplementedException();
    }
}