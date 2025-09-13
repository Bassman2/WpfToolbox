namespace WpfToolbox.ViewModel;

/// <summary>
/// View Model base for dialogs based on DialogView and DialogButtonsView
/// </summary>
public partial class DialogViewModel : ObservableValidator
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DialogViewModel()
    { }

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

    private bool OnCanOK() => !HasErrors;

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

