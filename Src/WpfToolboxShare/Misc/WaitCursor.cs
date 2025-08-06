namespace WpfToolbox.Misc;

public class WaitCursor : IDisposable
{
    private readonly Dispatcher dispatcher = Application.Current.Dispatcher;
    private Cursor cursor = Cursors.Arrow;

    public WaitCursor()
    {
        if (dispatcher.CheckAccess())
        {
            cursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        }
        else
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                cursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            });
        }
    }

    public void Dispose()
    {
        if (dispatcher.CheckAccess())
        {
            Mouse.OverrideCursor = cursor;
        }
        else
        {
            Application.Current.Dispatcher.BeginInvoke(() => Mouse.OverrideCursor = cursor);
        }
    }
}
