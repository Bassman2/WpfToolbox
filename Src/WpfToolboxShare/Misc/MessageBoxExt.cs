namespace WpfToolbox.Misc;

public static class MessageBoxExt
{
    public static MessageBoxResult ShowInfo(string message)
    {
        Dispatcher dispatcher = Application.Current.Dispatcher;
        if (dispatcher.CheckAccess())
        {
            return ShowInfoIntern(message);
        }
        else
        {
            return dispatcher.Invoke(() => ShowInfoIntern(message));
        }
    }

    private static MessageBoxResult ShowInfoIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public static MessageBoxResult ShowError(string message)
    {
        Dispatcher dispatcher = Application.Current.Dispatcher;
        if (dispatcher.CheckAccess())
        {
            return ShowErrorIntern(message);
        }
        else
        {
            return dispatcher.Invoke(() => ShowErrorIntern(message));
        }
    }

    private static MessageBoxResult ShowErrorIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public static MessageBoxResult ShowWarning(string message)
    {
        Dispatcher dispatcher = Application.Current.Dispatcher;
        if (dispatcher.CheckAccess())
        {
            return ShowWarningIntern(message);
        }
        else
        {
            return dispatcher.Invoke(() => ShowWarningIntern(message));
        }
    }

    private static MessageBoxResult ShowWarningIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public static MessageBoxResult ShowQuestion(string message)
    {
        Dispatcher dispatcher = Application.Current.Dispatcher;
        if (dispatcher.CheckAccess())
        {
            return ShowQuestionIntern(message);
        }
        else
        {
            return dispatcher.Invoke(() => ShowQuestionIntern(message));
        }
    }

    private static MessageBoxResult ShowQuestionIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
