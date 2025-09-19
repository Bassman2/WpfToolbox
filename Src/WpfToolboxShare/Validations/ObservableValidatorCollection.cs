namespace WpfToolbox.Validations;

/// <summary>
/// A collection that aggregates validation errors from its child elements.
/// Items in the collection must implement <see cref="INotifyDataErrorInfo"/>.
/// The collection raises its own <see cref="ErrorsChanged"/> event when any child item's error state changes.
/// </summary>
/// <typeparam name="T">An item type that implements <see cref="INotifyDataErrorInfo"/>.</typeparam>
public class ObservableValidatorCollection<T> : ObservableCollection<T>, INotifyDataErrorInfo where T : INotifyDataErrorInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableValidatorCollection{T}"/> class.
    /// Subscribes to the CollectionChanged event to monitor added and removed items.
    /// </summary>
    public ObservableValidatorCollection() : base()
    {
        CollectionChanged += OnCollectionChanged;
    }

    /// <summary>
    /// Handles changes in the collection.
    /// Subscribes or unsubscribes to the <see cref="INotifyDataErrorInfo.ErrorsChanged"/> event for added or removed items.
    /// Raises <see cref="ErrorsChanged"/> when the collection changes.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing event data.</param>
    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.NewItems?.OfType<INotifyDataErrorInfo>() ?? [])
        {
            item.ErrorsChanged += OnChildErrorsChanged;

        }
        foreach (var item in e.OldItems?.OfType<INotifyDataErrorInfo>() ?? [])
        {
            item.ErrorsChanged -= OnChildErrorsChanged;
        }

        OnErrorsChanged(string.Empty);
    }

    /// <summary>
    /// Handles the <see cref="INotifyDataErrorInfo.ErrorsChanged"/> event for a child item.
    /// Raises the aggregated <see cref="ErrorsChanged"/> event.
    /// </summary>
    /// <param name="sender">The child item that raised the event.</param>
    /// <param name="e">The <see cref="DataErrorsChangedEventArgs"/> instance containing event data.</param>
    private void OnChildErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        OnErrorsChanged(string.Empty);
    }

    /// <summary>
    /// Raises the <see cref="ErrorsChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property with changed errors.</param>
    protected void OnErrorsChanged(string propertyName) =>
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

    /// <summary>
    /// Occurs when the validation errors have changed for a property or for the entire entity.
    /// </summary>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>
    /// Returns the aggregated validation errors for the specified property across all items.
    /// </summary>
    /// <param name="propertyName">The property name for which errors are requested. Passing an empty string returns all errors.</param>
    /// <returns>An <see cref="IEnumerable"/> containing all validation errors.</returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        List<object> errors = [];
        foreach (var item in this)
        {
            var childErrors = item.GetErrors(propertyName);
            if (childErrors is not null)
            {
                foreach (var error in childErrors)
                {
                    errors.Add(error);
                }
            }
        }
        return errors;
    }

    /// <summary>
    /// Gets a value indicating whether any item in the collection has validation errors.
    /// </summary>
    public bool HasErrors => this.Any(item => item.HasErrors);
}
