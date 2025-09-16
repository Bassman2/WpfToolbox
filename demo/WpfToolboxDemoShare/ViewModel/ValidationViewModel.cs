using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace WpfToolboxDemo.ViewModel;

public partial class ValidationViewModel : ObservableValidator
{
    public ValidationViewModel()
    {
        this.ErrorsChanged += (s, e) => OnPropertyChanged(nameof(HasNoErrors));
        text = "Hallo";
    }

    public bool HasNoErrors => !HasErrors;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [CustomValidation(typeof(TextValidation), nameof(TextValidation.IdentifierValidation))]
    private string text;
}
