namespace WpfToolbox.Controls;

/// <summary>
/// A WPF auto toggle button control that derives from <see cref="ToggleButton"/>.
/// Provides a custom style and toggling behavior for a switch-like UI element.
/// </summary>
public class AutoToggleButton : ToggleButton
{
    static AutoToggleButton()
    {
        // Override the default style key to associate the control with its style in Generic.xaml.
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoToggleButton), new FrameworkPropertyMetadata(typeof(AutoToggleButton)));
    }

    /// <summary>
    /// Handles the toggle action by inverting the <see cref="IsChecked"/> state.
    /// </summary>
    protected override void OnToggle()
    {
        IsChecked = !IsChecked; 
    }
}

