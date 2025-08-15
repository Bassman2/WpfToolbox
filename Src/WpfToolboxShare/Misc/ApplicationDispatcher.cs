namespace WpfToolbox.Misc;

public static class ApplicationDispatcher
{
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
