namespace WpfToolbox.Misc;

/// <summary>
/// Provides thread-safe static helper methods for displaying common message boxes in WPF applications.
/// Ensures message boxes are shown on the UI thread and uses the application's main window as the owner.
/// </summary>
public static class MessageBoxExt
{
    /// <summary>
    /// Shows an informational message box with an "Info" caption and information icon.
    /// Ensures the message box is displayed on the UI thread.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <returns>The <see cref="MessageBoxResult"/> selected by the user.</returns>
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

    /// <summary>
    /// Internal method to show an informational message box.
    /// </summary>
    private static MessageBoxResult ShowInfoIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// Shows an error message box with an "Error" caption and error icon.
    /// Ensures the message box is displayed on the UI thread.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    /// <returns>The <see cref="MessageBoxResult"/> selected by the user.</returns>
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

    /// <summary>
    /// Internal method to show an error message box.
    /// </summary>
    private static MessageBoxResult ShowErrorIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// Shows a warning message box with a "Warning" caption and warning icon.
    /// Ensures the message box is displayed on the UI thread.
    /// </summary>
    /// <param name="message">The warning message to display.</param>
    /// <returns>The <see cref="MessageBoxResult"/> selected by the user.</returns>
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

    /// <summary>
    /// Internal method to show a warning message box.
    /// </summary>
    private static MessageBoxResult ShowWarningIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    /// <summary>
    /// Shows a question message box with a "Warning" caption and warning icon.
    /// Ensures the message box is displayed on the UI thread.
    /// </summary>
    /// <param name="message">The question message to display.</param>
    /// <returns>The <see cref="MessageBoxResult"/> selected by the user.</returns>
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

    /// <summary>
    /// Internal method to show a question message box.
    /// Note: Uses "Warning" caption and icon, which may be intentional or could be changed to "Question".
    /// </summary>
    private static MessageBoxResult ShowQuestionIntern(string message)
    {
        return MessageBox.Show(Application.Current.MainWindow, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
