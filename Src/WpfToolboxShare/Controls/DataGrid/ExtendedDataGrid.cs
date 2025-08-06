namespace WpfToolbox.Controls;

public partial class ExtendedDataGrid : DataGrid
{
    public ExtendedDataGrid()
    {
        // SortRows
        this.AllowDrop = true;
        this.CanUserSortColumns = false;

        // Filter
        this.Columns.CollectionChanged += OnColumnsCollectionChanged;
    }    
}
