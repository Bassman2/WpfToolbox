namespace WpfToolbox.Behaviors;

/// <summary>
/// Provides a behavior for <see cref="DataGrid"/> that enables column reordering
/// and persists the column order using application settings.
/// </summary>
public class DataGridColumnReorderBehavior : StoreBehavior<DataGrid>
{
    /// <inheritdoc/>
    protected override void OnAttached()
    {
        AssociatedObject.Initialized += OnInitialized;  
        AssociatedObject.Columns.CollectionChanged += OnColumnsCollectionChanged;
    }

    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        AssociatedObject.Initialized -= OnInitialized;
        AssociatedObject.Columns.CollectionChanged -= OnColumnsCollectionChanged;
    }

    

    private void OnInitialized(object? sender, EventArgs e)
    {
        AssociatedObject.CanUserReorderColumns = true;

       string s = GetSettingsValue<string>("");


    }

    private void OnColumnsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        
    }
}
