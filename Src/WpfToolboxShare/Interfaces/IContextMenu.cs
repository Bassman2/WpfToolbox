namespace WpfToolbox.Interfaces;


/// <summary>
/// Interface for providing dynamic context menu updates for DataGrid rows.
/// Implement this interface on your row data context to customize the context menu at runtime.
/// </summary>
public interface IContextMenu
{
    /// <summary>
    /// Updates the context menu for a DataGrid row.
    /// </summary>
    /// <param name="parent">The DataGrid's data context (parent).</param>
    /// <param name="contextMenu">The context menu to update.</param>
    void UpdateContextMenu(object parent, ContextMenu contextMenu);
}
