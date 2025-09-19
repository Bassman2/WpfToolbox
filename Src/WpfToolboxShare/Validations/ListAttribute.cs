using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WpfToolbox.Validations;

/// <summary>
/// ValidationAttribute for validating items within an observable collection.
/// Iterates through the collection, validates each item using data annotations,
/// and aggregates error messages if any validation fails.
/// </summary>
public sealed class ListAttribute : ValidationAttribute
{
    private string? propertyName = null;
    private ObservableObject? parent = null; 
    private INotifyCollectionChanged? notifyCollectionChanged = null;

    /// <summary>
    /// Determines whether each item in the specified collection is valid.
    /// </summary>
    /// <param name="value">The collection to validate.</param>
    /// <param name="validationContext">Context in which the validation is performed.</param>
    /// <returns>
    /// An instance of <see cref="ValidationResult"/> indicating whether validation succeeded.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // If value is null, consider it valid.
        // Use [Required] attribute on the property if null should be considered an error.
        if (value is null)
        {
            return ValidationResult.Success;
        }

        // Ensure the value is IEnumerable
        if (value is not IEnumerable enumerable)
        {
            return new ValidationResult($"The {validationContext.DisplayName} field is not a valid collection.");
        }

        // Store parent for error notification.
        if (validationContext.ObjectInstance is not ObservableObject parent) //ObservableValidator
        {
            return new ValidationResult($"The {validationContext.ObjectInstance.GetType().Name} is not based on ObservableObject.");
        }
        this.parent = parent;

        // Handle collection changed events to re-validate the collection.
        if (value is null && notifyCollectionChanged is not null)
        {
            notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
            notifyCollectionChanged = null;
        }
        else if (value is INotifyCollectionChanged notify)
        {
            if (notifyCollectionChanged is not null)
                notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
            notifyCollectionChanged = notify;
            if (notifyCollectionChanged is not null)
                notifyCollectionChanged.CollectionChanged += OnCollectionChanged; 
        }

        foreach (var item in enumerable.OfType<INotifyDataErrorInfo>())
        {
            item.ErrorsChanged += OnErrorsChanged;
        }

        foreach (var item in enumerable)
        {
            if (item is null)
            {
                return new ValidationResult($"The {validationContext.DisplayName} collection contains a null item.");
            }
            var itemContext = new ValidationContext(item);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(item, itemContext, results, validateAllProperties: true);
            if (!isValid)
            {
                string itemErrors = string.Join(", ", results.Select(r => r.ErrorMessage));
                return new ValidationResult($"The {validationContext.DisplayName} collection contains invalid items: {itemErrors}");
            }
        }

        return ValidationResult.Success;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var item in e.OldItems.OfType<INotifyDataErrorInfo>())
            {
                item.ErrorsChanged -= OnErrorsChanged;
            }
        }
        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems.OfType<INotifyDataErrorInfo>())
            {
                item.ErrorsChanged += OnErrorsChanged;
            }
        }
    }

    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        if (parent is not null && propertyName is not null)
        {
            // Use the public/protected OnPropertyChanged(string? propertyName) overload instead
            //parent.OnPropertyChanged(propertyName);

            MethodInfo? onPropertyChangedMethod = parent.GetType().GetMethod("OnPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            onPropertyChangedMethod?.Invoke(parent, new object?[] { propertyName });
        }

        //parent.ErrorsChanged.?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        ////var itemContext = new ValidationContext(sender!);
        ////var results = new List<ValidationResult>();
        ////Validator.TryValidateObject(e.PropertyName!, itemContext, results, validateAllProperties: true);
        //////if (parent is ObservableValidator observableValidator)
        //////{
        //////    // Notify parent that the collection has changed.
        //////    parent.ErrorsChanged.I.observableValidator.
        //////}
        
    }
}
