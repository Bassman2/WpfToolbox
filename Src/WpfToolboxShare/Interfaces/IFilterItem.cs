namespace WpfToolbox.Interfaces;

/// <summary>
/// Interface for custom filter items.
/// </summary>
public interface IFilterItem
{
    /// <summary>
    /// Gets or sets the display name of the filter item.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets the value associated with the filter item.
    /// </summary>
    int Value { get; set; }
}