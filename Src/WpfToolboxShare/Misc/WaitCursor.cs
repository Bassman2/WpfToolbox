namespace WpfToolbox.Misc;

public sealed class WaitCursor : IDisposable
{
    private readonly Dispatcher dispatcher = Application.Current.Dispatcher;

    public WaitCursor() => SetOverrideCursor(Cursors.Wait);

    public void Dispose() => SetOverrideCursor(null);

    private void SetOverrideCursor(Cursor? cursor)
    {
        if (dispatcher.CheckAccess())
        {
            Mouse.OverrideCursor = cursor;
        }
        else
        {
            dispatcher.Invoke(() => Mouse.OverrideCursor = cursor);
        }
    }
}
