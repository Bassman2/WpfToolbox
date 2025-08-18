using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;

namespace MvvmAppBase.View;

public class AppWindowView : Window
{
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
        this.SetEventBinding("Loaded", "StartupCommand");
        this.SetEventBinding("Closing", "ClosingCommand", true);

        // set AllowDrop = true to enable drag & drop
        this.SetEventBinding("PreviewDragEnter", "DragCommand", true);
        this.SetEventBinding("PreviewDragOver", "DragCommand", true);
        this.SetEventBinding("PreviewDrop", "DropCommand", true);

    }

    protected void SetKeyBinding(Key key, string commandName)
    {
        KeyBinding keyBinding = new () { Key = key };
        BindingOperations.SetBinding(keyBinding, KeyBinding.CommandProperty, new Binding(commandName));
        this.InputBindings.Add(keyBinding);
    }

    protected void SetEventBinding(string eventName, string commandName, bool passEventArgsToCommand = false)
    {
        Microsoft.Xaml.Behaviors.EventTrigger trigger = new(eventName);
        Microsoft.Xaml.Behaviors.InvokeCommandAction action = new() { PassEventArgsToCommand = passEventArgsToCommand };
        BindingOperations.SetBinding(action, Microsoft.Xaml.Behaviors.InvokeCommandAction.CommandProperty, new Binding(commandName));
        trigger.Actions.Add(action);
        Microsoft.Xaml.Behaviors.Interaction.GetTriggers(this).Add(trigger);
    }
}
