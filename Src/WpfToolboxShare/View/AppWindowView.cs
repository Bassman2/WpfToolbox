namespace WpfToolbox.View;

/// <summary>
/// A WPF <see cref="Window"/> that provides application-level bindings and command/event wiring.
/// Binds the window title and taskbar progress properties to the data context, and sets up key and event bindings for common commands.
/// Also includes drag-and-drop event bindings and a helper for hyperlink navigation.
/// </summary>
public class AppWindowView : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppWindowView"/> class.
    /// Sets up bindings for the window title, taskbar progress, and common commands/events.
    /// </summary>
    public AppWindowView()
    {
        ResizeMode = ResizeMode.CanResizeWithGrip;
        SetBinding(TitleProperty, new Binding("Title"));

        var taskbarItemInfo = new TaskbarItemInfo();
        BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.DescriptionProperty, new Binding("Title"));
        BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressStateProperty, new Binding("ProgressState"));
        BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressValueProperty, new Binding("ProgressValue"));
        TaskbarItemInfo = taskbarItemInfo;

        this.SetKeyBinding(Key.F5, "RefreshCommand");
        this.SetEventBinding("Loaded", "LoadedCommand");
        this.SetEventBinding("Closing", "ClosingCommand", true);

        // set AllowDrop = true to enable drag & drop
        this.SetEventBinding("PreviewDragEnter", "DragCommand", true);
        this.SetEventBinding("PreviewDragOver", "DragCommand", true);
        this.SetEventBinding("PreviewDrop", "DropCommand", true);

    }

    /// <summary>
    /// Sets up a key binding for the specified key and command name.
    /// </summary>
    /// <param name="key">The key to bind.</param>
    /// <param name="commandName">The name of the command property in the data context.</param>
    protected void SetKeyBinding(Key key, string commandName)
    {
        var keyBinding = new KeyBinding() { Key = key };
        BindingOperations.SetBinding(keyBinding, KeyBinding.CommandProperty, new Binding(commandName));
        this.InputBindings.Add(keyBinding);
    }

    /// <summary>
    /// Sets up an event binding to a command using behaviors.
    /// </summary>
    /// <param name="eventName">The name of the event to bind.</param>
    /// <param name="commandName">The name of the command property in the data context.</param>
    /// <param name="passEventArgsToCommand">Whether to pass event arguments to the command.</param>
    protected void SetEventBinding(string eventName, string commandName, bool passEventArgsToCommand = false)
    {
        Microsoft.Xaml.Behaviors.EventTrigger trigger = new(eventName);
        Microsoft.Xaml.Behaviors.InvokeCommandAction action = new() { PassEventArgsToCommand = passEventArgsToCommand };
        //Microsoft.Xaml.Behaviors.InvokeCommandAction action = new Microsoft.Xaml.Behaviors.InvokeCommandAction() { PassEventArgsToCommand = passEventArgsToCommand };
        BindingOperations.SetBinding(action, Microsoft.Xaml.Behaviors.InvokeCommandAction.CommandProperty, new Binding(commandName));
        trigger.Actions.Add(action);
        Microsoft.Xaml.Behaviors.Interaction.GetTriggers(this).Add(trigger);
    }

    /// <summary>
    /// Handles hyperlink click events and opens the target URL in the default browser.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected void OnHyperlinkClick(object sender, RoutedEventArgs e)
    {
        Hyperlink link = (Hyperlink)e.OriginalSource;
        try
        {
            Process myProcess = new();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = link.NavigateUri.AbsoluteUri;
            myProcess.Start();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
