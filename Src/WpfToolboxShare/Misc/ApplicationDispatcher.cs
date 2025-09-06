namespace WpfToolbox.Misc;

/// <summary>
/// Provides static helper methods to safely invoke actions or functions on the WPF application's main UI thread.
/// Ensures that the specified delegate is executed on the application's dispatcher, either directly if already on the UI thread,
/// or marshaled to the dispatcher if called from a background thread.
/// </summary>
public static class ApplicationDispatcher
{
    /// <summary>
    /// Invokes the specified <paramref name="action"/> on the application's dispatcher.
    /// If called from the UI thread, the action is executed immediately; otherwise, it is marshaled to the UI thread.
    /// </summary>
    /// <param name="action">The action to execute on the UI thread.</param>
    public static void Invoke(Action action)
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            action();
        }
        else
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }

    /// <summary>
    /// Invokes the specified <paramref name="func"/> on the application's dispatcher and returns its result.
    /// If called from the UI thread, the function is executed immediately; otherwise, it is marshaled to the UI thread.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="func">The function to execute on the UI thread.</param>
    /// <returns>The result of the function.</returns>
    public static T Invoke<T>(Func<T> func)
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            return func();
        }
        else
        {
            return Application.Current.Dispatcher.Invoke<T>(func);
        }
    }
}
