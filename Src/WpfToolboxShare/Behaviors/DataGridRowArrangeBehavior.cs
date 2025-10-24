namespace WpfToolbox.Behaviors;

/// <summary>
/// Behavior that enables drag-and-drop row reordering and editing state tracking for a DataGrid.
/// Handles mouse and drag events to allow users to rearrange rows interactively.
/// </summary>
public class DataGridRowArrangeBehavior : Behavior<DataGrid>
{
    /// <summary>
    /// Gets or sets a value indicating whether a cell is currently being edited.
    /// Used to prevent drag operations during editing.
    /// </summary>
    public bool IsEditing { get; set; }

    /// <summary>
    /// Stores the item that is the target of a drag-and-drop operation.
    /// </summary>
    private object? targetItem;


    /// <summary>
    /// Attaches event handlers for drag-and-drop and editing events when the behavior is attached to a DataGrid.
    /// </summary>
    protected override void OnAttached()
    {
        //AssociatedObject.Initialized += OnInitialized;
        AssociatedObject.BeginningEdit += OnBeginningEdit;
        AssociatedObject.CellEditEnding += OnCellEditEnding;
        AssociatedObject.MouseMove += OnMouseMove;
        AssociatedObject.DragEnter += OnDragEnter;
        AssociatedObject.DragLeave += OnDragLeave;
        AssociatedObject.DragOver += OnDragOver;
        AssociatedObject.Drop += OnDrop;
        AssociatedObject.AddingNewItem += OnAddingNewItem;
        AssociatedObject.AllowDrop = true;

    }

    /// <summary>
    /// Detaches event handlers when the behavior is removed from the DataGrid.
    /// </summary>
    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        //AssociatedObject.Initialized -= OnInitialized;
        AssociatedObject.BeginningEdit -= OnBeginningEdit;
        AssociatedObject.CellEditEnding -= OnCellEditEnding;
        AssociatedObject.MouseMove -= OnMouseMove;
        AssociatedObject.DragEnter -= OnDragEnter;
        AssociatedObject.DragLeave -= OnDragLeave;
        AssociatedObject.DragOver -= OnDragOver;
        AssociatedObject.Drop -= OnDrop;
        AssociatedObject.AddingNewItem -= OnAddingNewItem;
    }

    //private void OnInitialized(object? sender, EventArgs e)
    //{
    //}

    /// <summary>
    /// Sets the editing state to true when a cell edit begins.
    /// </summary>
    protected void OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e)
    {
        this.IsEditing = true;
    }

    /// <summary>
    /// Sets the editing state to false when a cell edit ends.
    /// </summary>
    protected void OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        this.IsEditing = false;
    }

    /// <summary>
    /// Gets the actual width of the DataGrid's cells panel using reflection.
    /// Used to determine if the mouse is over the cell area (not scrollbars) during drag.
    /// </summary>
    public double CellsPanelActualWidth
    {
        get
        {
            Type type = typeof(DataGrid);
            PropertyInfo? pInfo = type.GetProperty("CellsPanelActualWidth", BindingFlags.NonPublic | BindingFlags.Instance);
            var value = pInfo?.GetValue(AssociatedObject, null);
            return (double)(value ?? 0.0);
        }
    }

    /// <summary>
    /// Initiates a drag-and-drop operation for row reordering if the left mouse button is pressed and not editing.
    /// </summary>
    protected void OnMouseMove(object? sender, MouseEventArgs e)
    {
        if (sender is DataGrid dataGrid && !this.IsEditing)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // don't do if mouse is over scrollbars
                Point pos = e.GetPosition(dataGrid);
                if (pos.X < CellsPanelActualWidth /*&& pos.Y < CellsPanelActualHeight*/)
                {
                    //Debug.Print("************** {0} < {1}", pos.X, CellsPanelActualWidth);
                    object selectedItem = dataGrid.SelectedItem;

                    // check if item has errors
                    bool hasError = false;
                    if (selectedItem is IDataErrorInfo dataErrorInfo)
                    {
                        foreach (var property in selectedItem.GetType().GetProperties())
                        {
                            if (!string.IsNullOrEmpty(dataErrorInfo[property.Name]))
                            {
                                hasError = true; ;
                            }
                        }
                    }

                    if (selectedItem != null && selectedItem != CollectionView.NewItemPlaceholder && !this.IsEditing && !hasError)
                    {
                        DataGridRow dataGridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(selectedItem);
                        if (dataGridRow != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(dataGridRow, selectedItem, DragDropEffects.Move);
                            if ((finalDropEffect == DragDropEffects.Move) && (this.targetItem != null))
                            {
                                dynamic itemsSource = dataGrid.ItemsSource;
                                int oldIndex = itemsSource.IndexOf((dynamic)selectedItem);
                                int newIndex = itemsSource.IndexOf((dynamic)this.targetItem);
                                itemsSource.Move(oldIndex, newIndex);
                                this.targetItem = null;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles the DragEnter event to determine if the drag target is a valid DataGridRow.
    /// Disables drop if the target is not a valid row or is the new item placeholder.
    /// </summary>
    protected void OnDragEnter(object? sender, DragEventArgs e)
    {
        DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
        if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    /// <summary>
    /// Handles the DragLeave event to determine if the drag target is a valid DataGridRow.
    /// Disables drop if the target is not a valid row or is the new item placeholder.
    /// </summary>
    protected void OnDragLeave(object? sender, DragEventArgs e)
    {
        DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
        if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    /// <summary>
    /// Handles the DragOver event to determine if the drag target is a valid DataGridRow.
    /// Disables drop if the target is not a valid row or is the new item placeholder.
    /// </summary>
    protected void OnDragOver(object? sender, DragEventArgs e)
    {

        DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
        if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    /// <summary>
    /// Handles the Drop event to set the target item for row reordering if the drop target is valid.
    /// </summary>
    protected void OnDrop(object? sender, DragEventArgs e)
    {

        e.Effects = DragDropEffects.None;
        e.Handled = true;

        // Verify that this is a valid drop and then store the drop target
        DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
        if ((dataGridRow != null) && (dataGridRow.DataContext != CollectionView.NewItemPlaceholder))
        {
            this.targetItem = dataGridRow.DataContext;
            e.Effects = DragDropEffects.Move;
        }
    }

    #region UI Helper

    /// <summary>
    /// Finds the first visual parent of the specified type for a given UIElement.
    /// </summary>
    /// <typeparam name="T">The type of parent to search for.</typeparam>
    /// <param name="element">The starting UIElement.</param>
    /// <returns>The first parent of type T, or null if not found.</returns>
    public static T? FindVisualParent<T>(UIElement element) where T : UIElement
    {
        UIElement? parent = element;
        while (parent != null)
        {
            if (parent is T correctlyTyped)
            {
                return correctlyTyped;
            }
            parent = VisualTreeHelper.GetParent(parent) as UIElement;
        }
        return null;
    }

    #endregion

    /// <summary>
    /// Handles the AddingNewItem event to create a new instance of the item type for the DataGrid.
    /// </summary>
    protected void OnAddingNewItem(object? sender, AddingNewItemEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {

            Type listType = dataGrid.ItemsSource.GetType();
            Type itemType = listType.GenericTypeArguments[0];
            object item = itemType.GetConstructor(Type.EmptyTypes)!.Invoke(null);

            //if (this.DataContext is BindingViewModel)
            //{
            //    PropertyInfo pi = itemType.GetProperty("Parent");
            //    pi?.SetValue(item, this.DataContext);
            //}

            e.NewItem = item;
        }

    }
}
