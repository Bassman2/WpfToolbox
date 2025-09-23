using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace WpfToolbox.ViewModel;

/// <summary>
/// View Model base for dialogs based on DialogView and DialogButtonsView
/// </summary>
public partial class DialogViewModel : ObservableValidator, IRootValidator
{
    /// <summary>
    /// Constructor
    /// </summary>
    //public DialogViewModel()
    //{
    //    this.ErrorsChanged += (s, e) => OnPropertyChanged(nameof(HasNoErrors));
    //}


    private readonly Dictionary<(LeafViewModel child, string property), List<ValidationResult>> childErrors = [];

    public void ChangeChildErrors(LeafViewModel child, string? propertyName, IEnumerable<ValidationResult> results)
    {
        List<ValidationResult> list = [.. results];

        if (list.Count > 0 )
        {             
            childErrors[(child, propertyName!)] = list;
        }
        else
        {
            childErrors.Remove((child, propertyName!));
        }

        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(HasNoErrors));
    }

    public new bool HasErrors => base.HasErrors || childErrors.Any(ce => ce.Value.Count > 0);

    /// <summary>
    /// Gets a value indicating whether the view model has no validation errors.
    /// </summary>
    public bool HasNoErrors => !HasErrors;

    /// <summary>
    /// true if the OK button was pressed, else flase
    /// </summary>
    [ObservableProperty]
    private bool? dialogResult = null;

    /// <summary>
    /// Should be overwritten in derived classes to update the values;
    /// </summary>
    /// <returns>true if one ore more values are changed, else false</returns>
    public virtual bool OnUpdate()
    {
        return true;
    }

    private bool OnCanOK() => HasNoErrors;

    /// <summary>
    /// Handler for the OK button
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanOK))]
    protected virtual void OnOK()      
    {
        // update current focused element
        if (Keyboard.FocusedElement is TextBox textBox)
        {
            textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        this.DialogResult = OnUpdate();
    }


    /// <summary>
    /// Handler for the Cancel button
    /// </summary>
    [RelayCommand]
    protected virtual void OnCancel()
    {
        this.DialogResult = false;
    }

    /// <summary>
    /// Handler for the Close button
    /// </summary>
    [RelayCommand]
    protected void OnClose()
    {
        if (this.DialogResult == null)
        {
            OnCancel();
        }
    }
}

