using System.ComponentModel.DataAnnotations;

namespace WpfToolboxDemo.ViewModel;

public partial class ValidationViewModel : ObservableValidator
{
    public ValidationViewModel()
    {
        this.ErrorsChanged += OnErrorChanged;
        text = "Hallo";

        List<ValidationItemViewModel> l = [
        new ValidationItemViewModel { Name = "Pete", Text = "DemoA1" },
        new ValidationItemViewModel { Name = "Paul", Text = "DemoB2" },
        new ValidationItemViewModel { Name = "Mary", Text = "DemoC3" }];
       // List.ErrorsChanged += OnErrorChanged;

        List = new ObservableCollection<ValidationItemViewModel>(l);
    }

    private void OnErrorChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasNoErrors));
    }

    

    public bool HasNoErrors => !(HasErrors);

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [CustomValidation(typeof(TextValidation), nameof(TextValidation.IdentifierValidation))]
    private string text;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [List]
    private ObservableCollection<ValidationItemViewModel> list;

    //private ObservableValidatorCollection<ValidationItemViewModel> list;
}
