namespace WpfToolbox.Behaviors;

public class DataGridRowArrangeBehavior : Behavior<DataGrid>
{
    public bool IsEditing { get; set; }
    private object? targetItem;


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

    protected void OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e)
    {
        this.IsEditing = true;
    }

    protected void OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        this.IsEditing = false;
    }

    public double CellsPanelActualWidth
    {
        get
        {
            Type type = typeof(DataGrid);
            PropertyInfo? pInfo = type.GetProperty("CellsPanelActualWidth", BindingFlags.NonPublic | BindingFlags.Instance);
            return (double)(pInfo?.GetValue(this, null) ?? 0.0);
        }
    }

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

    protected void OnDragEnter(object? sender, DragEventArgs e)
    {
        DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
        if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    protected void OnDragLeave(object? sender, DragEventArgs e)
    {
        DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
        if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }
    protected void OnDragOver(object? sender, DragEventArgs e)
    {

        DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
        if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

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
