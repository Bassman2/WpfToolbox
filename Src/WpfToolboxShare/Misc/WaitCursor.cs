namespace WpfToolbox.Misc;

/// <summary>
/// A helper class for managing the wait cursor (busy indicator) in a WPF application.
/// When an instance is created, the cursor is set to <see cref="Cursors.Wait"/>.
/// When disposed, the cursor is restored to its previous state.
/// Usage: wrap code in a <c>using</c> statement to automatically manage the wait cursor.
/// </summary>
public sealed class WaitCursor : IDisposable
{
    private readonly Dispatcher dispatcher = Application.Current.Dispatcher;

    /// <summary>
    /// Sets the wait cursor when the object is constructed.
    /// </summary>
    public WaitCursor() => SetOverrideCursor(Cursors.Wait);

    /// <summary>
    /// Restores the cursor to its previous state when disposed.
    /// </summary>
    public void Dispose() => SetOverrideCursor(null);

    /// <summary>
    /// Sets the <see cref="Mouse.OverrideCursor"/> property on the application's dispatcher thread.
    /// </summary>
    /// <param name="cursor">The cursor to set, or null to restore the default.</param>
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
