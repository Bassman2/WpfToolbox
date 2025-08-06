namespace WpfToolbox.Controls;

public class ToggleSwitch : ToggleButton
{
    static ToggleSwitch()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
    }

    protected override void OnToggle()
    {
        IsChecked = !IsChecked; 
    }
}

