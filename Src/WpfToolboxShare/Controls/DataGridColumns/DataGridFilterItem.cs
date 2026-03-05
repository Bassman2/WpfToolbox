namespace WpfToolbox.Controls;


 /// <summary>
 /// Represents a filter item for the filter ComboBox.
 /// Supports different constructors for various filter types.
 /// </summary>
[DebuggerDisplay("DataGridFilterItem {Name}")]
public class DataGridFilterItem : INotifyPropertyChanged
{
    /// <summary>
    /// Constructor for 'All' filter item
    /// </summary>
    public DataGridFilterItem()
    {
        this.IsAll = true;
        this.Name = "All";
        this.IsChecked = true;
    }

    /// <summary>
    /// Constructor for flag enum filter item
    /// </summary>
    public DataGridFilterItem(object item)
    {
        FieldInfo? fieldInfo = item.GetType().GetField(item.ToString()!);
        this.Name = fieldInfo?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? item?.ToString() ?? "";
        this.Value = (int)(item ?? 0);
        this.IsChecked = true;
    }

    /// <summary>
    /// Constructor for text filter item
    /// </summary>
    public DataGridFilterItem(string item)
    {
        this.Name = item;
        this.Value = item;
        this.IsChecked = true;
    }

    /// <summary>
    /// Constructor for IFilterItem filter item
    /// </summary>
    public DataGridFilterItem(IFilterItem item)
    {
        this.Name = item.Name;
        this.Value = item.Value;
        this.IsChecked = true;
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets a value indicating whether this filter item is the "All" filter.
    /// </summary>
    public bool IsAll { get; } = false;

    /// <summary>
    /// Gets the value associated with this filter item.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets or sets the display name of the filter item.
    /// </summary>
    public string Name { get; set; }

    private bool? isChecked;

    /// <summary>
    /// Gets or sets whether this filter item is checked (selected).
    /// </summary>
    public bool? IsChecked
    {
        get => isChecked;
        set
        {
            if (isChecked != value)
            {
                isChecked = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }
    }
}
