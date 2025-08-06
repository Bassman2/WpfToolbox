namespace WpfToolbox.Controls;

public partial class ExtendedDataGrid : DataGrid
{
    public bool IsEditing { get; set; }
    private object? targetItem;

    //public ExtendedDataGrid()
    //{
    //    this.AllowDrop = true;
    //    this.CanUserSortColumns = false;
    //}

    public static readonly DependencyProperty CanUserSortRowsProperty = 
        DependencyProperty.Register("CanUserSortRows", typeof(bool), typeof(ExtendedDataGrid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(
                (d, e) => ((ExtendedDataGrid)d).OnCanUserSortRowsPropertyChanged((bool)e.NewValue))));

    public bool CanUserSortRows
    {
        get => (bool)GetValue(CanUserSortRowsProperty);
        set => SetValue(CanUserSortRowsProperty, value);
    }

    private void OnCanUserSortRowsPropertyChanged(bool value) => this.AllowDrop = value;
    
    
    protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
    {
        this.IsEditing = true;
        base.OnBeginningEdit(e);
    }

    protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
    {
        base.OnCellEditEnding(e);
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

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (this.CanUserSortRows && !this.IsEditing)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // don't do if mouse is over scrollbars
                Point pos = e.GetPosition(this);
                if (pos.X < CellsPanelActualWidth /*&& pos.Y < CellsPanelActualHeight*/)
                {
                    //Debug.Print("************** {0} < {1}", pos.X, CellsPanelActualWidth);
                    object selectedItem = this.SelectedItem;

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
                        DataGridRow dataGridRow = (DataGridRow)this.ItemContainerGenerator.ContainerFromItem(selectedItem);
                        if (dataGridRow != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(dataGridRow, selectedItem, DragDropEffects.Move);
                            if ((finalDropEffect == DragDropEffects.Move) && (this.targetItem != null))
                            {
                                dynamic itemsSource = this.ItemsSource;
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
        base.OnMouseMove(e);
    }

    protected override void OnDragEnter(DragEventArgs e)
    {
        base.OnDragEnter(e);
        if (this.CanUserSortRows)
        {
            DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
            if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }
    }

    protected override void OnDragLeave(DragEventArgs e)
    {
        base.OnDragLeave(e);
        if (this.CanUserSortRows)
        {
            DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
            if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }
    }

    protected override void OnDragOver(DragEventArgs e)
    {
        base.OnDragOver(e);
        if (this.CanUserSortRows)
        {
            DataGridRow? dataGridRow = FindVisualParent<DataGridRow>((UIElement)e.OriginalSource);
            if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }
    }

    protected override void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);
        if (this.CanUserSortRows)
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

    protected override void OnAddingNewItem(AddingNewItemEventArgs e)
    {
        Type listType = this.ItemsSource.GetType();
        Type itemType = listType.GenericTypeArguments[0];
        object item = itemType.GetConstructor(Type.EmptyTypes)!.Invoke(null);

        //if (this.DataContext is BindingViewModel)
        //{
        //    PropertyInfo pi = itemType.GetProperty("Parent");
        //    pi?.SetValue(item, this.DataContext);
        //}

        e.NewItem = item;

        base.OnAddingNewItem(e);
    }
}
