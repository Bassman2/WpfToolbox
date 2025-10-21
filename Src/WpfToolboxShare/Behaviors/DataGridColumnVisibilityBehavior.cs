namespace WpfToolbox.Behaviors;

/// <summary>
/// A WPF behavior for <see cref="DataGrid"/> that enables users to control the visibility of columns
/// via a context menu in the column header. The visibility state is persisted using application settings,
/// allowing user preferences to be restored across sessions.
/// </summary>
/// <remarks>
/// - The context menu is dynamically generated based on the columns in the <see cref="DataGrid"/>.
/// - The <see cref="ColumnVisibility"/> property stores the visibility state as a bitmask.
/// - The <see cref="StoreBehavior{T}.SettingsName"/> property specifies the application setting key for persistence.
/// </remarks>
public class DataGridColumnVisibilityBehavior : StoreBehavior<DataGrid> //Behavior<DataGrid>
{
    /// <summary>
    /// The context menu displayed in the column header for toggling column visibility.
    /// </summary>
    private readonly ContextMenu headerContextMenu = new();

    /// <inheritdoc/>
    protected override void OnAttached()
    {
        ColumnVisibility = GetSettingsValue<uint>(uint.MaxValue);

        var headerStyle = new Style(typeof(DataGridColumnHeader));
        headerStyle.Setters.Add(new Setter(DataGridColumnHeader.ContextMenuProperty, headerContextMenu));
        AssociatedObject.ColumnHeaderStyle = headerStyle;

        AssociatedObject.Initialized += OnInitialized;
    }

    /// <inheritdoc/>
    protected override void OnDetaching()
        => AssociatedObject.Initialized -= OnInitialized;
    
    private void OnInitialized(object? sender, EventArgs e)
        => CreateHeaderMenu(ColumnVisibility);
    

    /// <summary>
    /// Identifies the <see cref="ColumnVisibility"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnVisibilityProperty =
        DependencyProperty.Register("ColumnVisibility", typeof(uint), typeof(DataGridColumnVisibilityBehavior),
            new FrameworkPropertyMetadata(uint.MaxValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback((d, e) => ((DataGridColumnVisibilityBehavior)d).CreateHeaderMenu((uint)e.NewValue))));

    /// <summary>
    /// Gets or sets the bitmask representing the visibility of each column.
    /// Each bit corresponds to a column: 1 = visible, 0 = collapsed.
    /// </summary>
    public uint ColumnVisibility
    {
        get => (uint)GetValue(ColumnVisibilityProperty);
        set => SetValue(ColumnVisibilityProperty, value);
    }

    /// <summary>
    /// Creates or updates the header context menu based on the current column visibility flags.
    /// </summary>
    /// <param name="flags">Bitmask representing column visibility.</param>
    private void CreateHeaderMenu(uint flags)
    {
        uint mask = 1;
        headerContextMenu.Items.Clear();
        foreach (var column in AssociatedObject.Columns)
        {
            bool isChecked = (flags & mask) != 0;
            var menuItem = new MenuItem() { Header = column.Header, Tag = column, IsCheckable = true, IsChecked = isChecked };
            menuItem.Checked += OnHeaderMenuItemChecked;
            menuItem.Unchecked += OnHeaderMenuItemUnchecked;
            headerContextMenu.Items.Add(menuItem);
            column.Visibility = isChecked ? Visibility.Visible : Visibility.Collapsed;
            mask <<= 1;
        }
    }

    /// <summary>
    /// Handles the Unchecked event for a column menu item, hiding the corresponding column.
    /// </summary>
    private void OnHeaderMenuItemUnchecked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.Tag is DataGridColumn column)
        {
            column.Visibility = Visibility.Collapsed;
        }
        UpdateColumnVisibility();
    }

    /// <summary>
    /// Handles the Checked event for a column menu item, showing the corresponding column.
    /// </summary>
    private void OnHeaderMenuItemChecked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.Tag is DataGridColumn column)
        {
            column.Visibility = Visibility.Visible;
        }
        UpdateColumnVisibility();
    }

    /// <summary>
    /// Updates the <see cref="ColumnVisibility"/> property and persists the state to application settings.
    /// </summary>
    private void UpdateColumnVisibility()
    {
        uint flags = 0;
        uint mask = 1;
        foreach (var column in AssociatedObject.Columns)
        {
            if (column.Visibility == Visibility.Visible)
            {
                flags |= mask;
            }
            mask <<= 1;
        }
        ColumnVisibility = flags;
        SetSettingsValue(flags);
    }
}
