using System.ComponentModel.DataAnnotations;

namespace WpfToolboxDemo.ViewModel;

public partial class ValidationItemViewModel : ObservableValidator
{
    public ValidationItemViewModel()
    {
        //this.ErrorsChanged += (s, e) => OnPropertyChanged(nameof(HasNoErrors));
        Name = "Peter";
        Text = "Hallo";
    }

    public bool HasNoErrors => !HasErrors;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(4, MinimumLength = 4)]
    private string name;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [CustomValidation(typeof(TextValidation), nameof(TextValidation.IdentifierValidation))]
    private string text;
}
