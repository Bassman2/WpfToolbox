using Microsoft.Xaml.Behaviors;

namespace WpfToolbox.Behaviors;

public class ContextMenuBehavior : Behavior<DataGrid>
{
    private ContextMenu? rowContextMenu;

    protected override void OnAttached()
    {
        AssociatedObject.Initialized += OnInitialized;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Initialized -= OnInitialized;
    }

    private void OnInitialized(object? sender, EventArgs e)
    {
        rowContextMenu = new();
        rowContextMenu.Opened += OnRowContextMenuOpened;

        var rowStyle = new Style(typeof(DataGridRow));
        rowStyle.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, rowContextMenu));
        AssociatedObject.RowStyle = rowStyle;
    }

    private void OnRowContextMenuOpened(object sender, RoutedEventArgs e)
    {
        if (sender is ContextMenu contextMenu && contextMenu.DataContext is IContextMenu item)
        {
            item.UpdateContextMenu(AssociatedObject.DataContext, contextMenu);
        }
    }
}

public interface IContextMenu
{
    void UpdateContextMenu(object parent, ContextMenu contextMenu);
}
