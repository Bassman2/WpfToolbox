namespace WpfToolbox.Behaviors;

/// <summary>
/// A behavior for WPF DataGrid that attaches a context menu to each row.
/// The context menu can be dynamically updated by implementing the <see cref="IContextMenu"/> interface on the row's data context.
/// </summary>
public class DataGridRowContextMenuBehavior : Behavior<DataGrid>
{
    /// <summary>
    /// The shared context menu instance used for all DataGrid rows.
    /// </summary>
    private ContextMenu? rowContextMenu;

    /// <summary>
    /// Attaches the behavior to the DataGrid and subscribes to the Initialized event.
    /// </summary>
    protected override void OnAttached()
    {
        AssociatedObject.Initialized += OnInitialized;
    }

    /// <summary>
    /// Detaches the behavior from the DataGrid and unsubscribes from the Initialized event.
    /// </summary>
    protected override void OnDetaching()
    {
        AssociatedObject.Initialized -= OnInitialized;
    }

    /// <summary>
    /// Handles the DataGrid's Initialized event.
    /// Sets up a row style that assigns a context menu to each DataGridRow.
    /// </summary>
    private void OnInitialized(object? sender, EventArgs e)
    {
        rowContextMenu = new();
        rowContextMenu.Opened += OnRowContextMenuOpened;

        var rowStyle = new Style(typeof(DataGridRow));
        rowStyle.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, rowContextMenu));
        AssociatedObject.RowStyle = rowStyle;
    }

    /// <summary>
    /// Handles the ContextMenu.Opened event for a row.
    /// If the row's DataContext implements <see cref="IContextMenu"/>, calls <see cref="IContextMenu.UpdateContextMenu"/> to update the menu.
    /// </summary>
    private void OnRowContextMenuOpened(object sender, RoutedEventArgs e)
    {
        if (sender is ContextMenu contextMenu && contextMenu.DataContext is IContextMenu item)
        {
            item.UpdateContextMenu(AssociatedObject.DataContext, contextMenu);
        }
    }
}


