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

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private ObservableCollection<ValidationItemViewModel> list = [
        new ValidationItemViewModel { Name = "Peter", Text = "DemoA1" },
        new ValidationItemViewModel { Name = "Paul", Text = "DemoB2" },
        new ValidationItemViewModel { Name = "Mary", Text = "DemoC3" }];
}
