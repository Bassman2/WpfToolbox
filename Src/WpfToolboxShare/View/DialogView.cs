using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfToolbox.View;

/// <summary>
/// A WPF dialog window base class that provides attached properties and helpers for dialog result binding
/// and dynamic window style manipulation (system menu, minimize, maximize).
/// </summary>
public partial class DialogView : Window
{
    /// <summary>
    /// Sets up dialog ownership, startup location, and disables taskbar icon.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        this.Owner = Application.Current.MainWindow;
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        this.ShowInTaskbar = false;

        //string uriStr = @"Mvvm;component/Themes/Validate.xaml";
        //ResourceDictionary dict = new ResourceDictionary();
        //dict.Source = new Uri(uriStr, UriKind.Relative);
        //this.Resources.MergedDictionaries.Add(dict);
    }

    #region DialogResult

    /// <summary>
    /// Attached property for binding a dialog's result to a view model.
    /// </summary>
    public static readonly DependencyProperty DialogResultProperty =
        DependencyProperty.RegisterAttached(
            "DialogResult",
            typeof(bool?),
            typeof(DialogView),
            new PropertyMetadata(DialogResultChanged));

    private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            window.DialogResult = e.NewValue as bool?;
        }
    }

    /// <summary>
    /// Sets the DialogResult attached property.
    /// </summary>
    public static void SetDialogResult(Window target, bool? value)
    {
        target.SetValue(DialogResultProperty, value);
    }

    /// <summary>
    /// Bind DialogResultProperty to DialogResult from DialogViewModel
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == DataContextProperty)
        {
            this.SetBinding(DialogResultProperty, "DialogResult");
        }

        base.OnPropertyChanged(e);
    }

    #endregion

    #region Window Styles

    const int GWL_STYLE = (-16);

    /// <summary>
    /// Window style flags for Win32 interop.
    /// </summary>
    [Flags]
    private enum WindowStyles : uint
    {
        WS_SYSMENU = 0x80000,
        WS_MINIMIZEBOX = 0x20000,
        WS_MAXIMIZEBOX = 0x10000,
    }

    public const int SWP_FRAMECHANGED = 0x0020;
    public const int SWP_NOACTIVATE = 0x0010;
    public const int SWP_NOMOVE = 0x0002;
    public const int SWP_NOOWNERZORDER = 0x0200;
    public const int SWP_NOREPOSITION = 0x0200;
    public const int SWP_NOSIZE = 0x0001;
    public const int SWP_NOZORDER = 0x0004;

    /// <summary>
    /// Adds a window style flag to the specified window.
    /// </summary>
    private static void AddWindowStyle(Window window, WindowStyles styleToAdd)
    {
        WindowInteropHelper wih = new(window);
        WindowStyles style = (WindowStyles)NativeMethods.GetWindowLong(wih.EnsureHandle(), GWL_STYLE);
        style |= styleToAdd;
        var _ = NativeMethods.SetWindowLong(wih.Handle, GWL_STYLE, (uint)style);
        var __ = NativeMethods.SetWindowPos(wih.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOREPOSITION | SWP_NOSIZE | SWP_NOZORDER);
    }

    /// <summary>
    /// Removes a window style flag from the specified window.
    /// </summary>
    private static void RemoveWindowStyle(Window window, WindowStyles styleToRemove)
    {
        WindowInteropHelper wih = new(window);
        WindowStyles style = (WindowStyles)NativeMethods.GetWindowLong(wih.EnsureHandle(), GWL_STYLE);
        style &= ~styleToRemove;
        var _ = NativeMethods.SetWindowLong(wih.Handle, GWL_STYLE, (uint)style);
        var __ = NativeMethods.SetWindowPos(wih.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOREPOSITION | SWP_NOSIZE | SWP_NOZORDER);
    }

    /// <summary>
    /// Gets the value of the ShowMinimize attached property.
    /// </summary>
    public static bool GetShowMinimize(DependencyObject obj)
    {
        return (bool)obj.GetValue(ShowMinimizeProperty);
    }

    /// <summary>
    /// Sets the value of the ShowMinimize attached property.
    /// </summary>
    public static void SetShowMinimize(DependencyObject obj, bool value)
    {
        obj.SetValue(ShowMinimizeProperty, value);
    }

    /// <summary>
    /// Attached property to control the visibility of the minimize button.
    /// </summary>
    public static readonly DependencyProperty ShowMinimizeProperty =
        DependencyProperty.RegisterAttached("ShowMinimize", typeof(bool), typeof(DialogView), new UIPropertyMetadata(true, OnShowMinimizeChanged));

    private static void OnShowMinimizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Window? window = d as Window;
        if (window == null)
        {
            if ((bool)e.NewValue)
            {
                AddWindowStyle(window!, WindowStyles.WS_MINIMIZEBOX);
            }
            else
            {
                RemoveWindowStyle(window!, WindowStyles.WS_MINIMIZEBOX);
            }
        }
    }

    /// <summary>
    /// Gets the value of the ShowMaximize attached property.
    /// </summary>
    public static bool GetShowMaximize(DependencyObject obj)
    {
        return (bool)obj.GetValue(ShowMaximizeProperty);
    }

    /// <summary>
    /// Sets the value of the ShowMaximize attached property.
    /// </summary>
    public static void SetShowMaximize(DependencyObject obj, bool value)
    {
        obj.SetValue(ShowMaximizeProperty, value);
    }

    /// <summary>
    /// Attached property to control the visibility of the maximize button.
    /// </summary>
    public static readonly DependencyProperty ShowMaximizeProperty =
        DependencyProperty.RegisterAttached("ShowMaximize", typeof(bool), typeof(DialogView), new UIPropertyMetadata(true, OnShowMaximizeChanged));

    private static void OnShowMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            if ((bool)e.NewValue)
            {
                AddWindowStyle(window, WindowStyles.WS_MAXIMIZEBOX);
            }
            else
            {
                RemoveWindowStyle(window, WindowStyles.WS_MAXIMIZEBOX);
            }
        }
    }

    /// <summary>
    /// Gets the value of the ShowSystemMenu attached property.
    /// </summary>
    public static bool GetShowHasSystemMenu(DependencyObject obj)
    {
        return (bool)obj.GetValue(ShowSystemMenuProperty);
    }

    /// <summary>
    /// Sets the value of the ShowSystemMenu attached property.
    /// </summary>
    public static void SetShowSystemMenu(DependencyObject obj, bool value)
    {
        obj.SetValue(ShowSystemMenuProperty, value);
    }

    /// <summary>
    /// Attached property to control the visibility of the system menu.
    /// </summary>

    public static readonly DependencyProperty ShowSystemMenuProperty =
        DependencyProperty.RegisterAttached("ShowSystemMenu", typeof(bool), typeof(DialogView), new UIPropertyMetadata(true, OnShowSystemMenuChanged));

    private static void OnShowSystemMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            if ((bool)e.NewValue)
            {
                AddWindowStyle(window, WindowStyles.WS_SYSMENU);
            }
            else
            {
                RemoveWindowStyle(window, WindowStyles.WS_SYSMENU);
            }
        }
    }

    /// <summary>
    /// Gets or sets whether the system menu is shown.
    /// </summary>
    public bool ShowSystemMenu
    {
        set { SetValue(ShowSystemMenuProperty, value); }
        get { return (bool)GetValue(ShowSystemMenuProperty); }
    }

    #endregion

    private static partial class NativeMethods
    {
        [LibraryImport("user32.dll")]
        internal static partial UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [LibraryImport("user32.dll")]
        internal static partial int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
    }
}