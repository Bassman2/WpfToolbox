namespace WpfToolbox.Controls.Primitives;

public class BindablePasswordBox : Decorator
{
    public BindablePasswordBox()
    {
        PasswordBox passwordBox = new();
        passwordBox.LostFocus += OnLostFocus;
        Child = passwordBox;
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        PasswordBox passwordBox = (PasswordBox)sender;
        Password = passwordBox.Password;
    }

    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(
                (d, e) => ((BindablePasswordBox)d).OnPasswordPropertyChanged((string)e.NewValue))));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    private void OnPasswordPropertyChanged(string newPassword)
    {
        PasswordBox passwordBox = (PasswordBox)Child;
        passwordBox.Password = newPassword;
    }
}
