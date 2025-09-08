namespace WpfToolbox.Controls;

/// <summary>
/// A WPF control that wraps a <see cref="PasswordBox"/> and enables two-way data binding for the password value.
/// This control synchronizes the <see cref="Password"/> dependency property with the inner <see cref="PasswordBox"/>.
/// </summary>
public class BindablePasswordBox : Decorator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BindablePasswordBox"/> class.
    /// Sets up the internal <see cref="PasswordBox"/> and event handlers.
    /// </summary>
    public BindablePasswordBox()
    {
        PasswordBox passwordBox = new();
        passwordBox.LostFocus += OnLostFocus;
        Child = passwordBox;
    }

    /// <summary>
    /// Handles the LostFocus event to update the <see cref="Password"/> property.
    /// </summary>
    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        PasswordBox passwordBox = (PasswordBox)sender;
        Password = passwordBox.Password;
    }

    /// <summary>
    /// Identifies the <see cref="Password"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(
                (d, e) => ((BindablePasswordBox)d).OnPasswordPropertyChanged((string)e.NewValue))));

    /// <summary>
    /// Gets or sets the password value. This property is synchronized with the inner <see cref="PasswordBox"/>.
    /// </summary>
    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    /// <summary>
    /// Updates the inner <see cref="PasswordBox"/> when the <see cref="Password"/> property changes.
    /// </summary>
    /// <param name="newPassword">The new password value.</param>
    private void OnPasswordPropertyChanged(string newPassword)
    {
        PasswordBox passwordBox = (PasswordBox)Child;
        passwordBox.Password = newPassword;
    }
}
