namespace WpfToolbox.ViewModel;

/// <summary>
/// Abstract base view model for WPF applications, providing common functionality such as
/// application title, progress reporting, drag &amp; drop, settings upgrade, and command handling.
/// Inherits from <see cref="ObservableValidator"/> for property change notification and validation.
/// </summary>
public abstract partial class AppViewModel : ObservableValidator
{
    //private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;
    //private double progressValue = 0.0;
    private readonly string progressTime = string.Empty;

    /// <summary>
    /// Reference to the main application window.
    /// </summary>
    protected readonly Window mainWindow;

    /// <summary>
    /// Reference to the main UI dispatcher.
    /// </summary>
    protected readonly Dispatcher mainDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppViewModel"/> class.
    /// Upgrades application settings and sets references to the main window and dispatcher.
    /// </summary>
    public AppViewModel()
    {
        this.ErrorsChanged += (s, e) => OnPropertyChanged(nameof(HasNoErrors));
        UpgradeSettings();
        mainWindow = Application.Current.MainWindow;
        mainDispatcher = Application.Current.MainWindow.Dispatcher;
    }

    /// <summary>
    /// Gets a value indicating whether the view model has no validation errors.
    /// </summary>
    public bool HasNoErrors => !HasErrors;

    /// <summary>
    /// Gets the path to the application's local app data folder.
    /// </summary>
    public static string LocalAppDataPath 
        => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().ProcessName));

    #region Title

    /// <summary>
    /// The main title of the application. Displayed in the main window header and in the taskbar.
    /// </summary>
    public virtual string Title
    {
        get
        {
            Assembly app = Assembly.GetEntryAssembly()!;
            string title = app.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
            string product = app.GetCustomAttribute<AssemblyProductAttribute>()!.Product;
            var ver = app.GetName()!.Version!;
            string version = ver.Build > 0 ? ver.ToString(3) : ver.ToString(2);
            return $"{product ?? title} {version}";
        }
    }

    #endregion

    #region Drag & Drop

    /// <summary>
    /// Handles drag events. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand]
    protected virtual void OnDrag(DragEventArgs args)
    { }

    /// <summary>
    /// Handles drop events. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand]
    protected virtual void OnDrop(DragEventArgs args)
    { }

    #endregion

    #region Settings

    /// <summary>
    /// Gets the application settings instance using reflection.
    /// </summary>
    protected static ApplicationSettingsBase? GetApplicationSettings()
    {
        return (ApplicationSettingsBase?)System.Reflection.Assembly.GetEntryAssembly()!.GetTypes().FirstOrDefault(t => t.FullName!.EndsWith(".Properties.Settings"))?.GetProperty("Default")?.GetValue(null);
    }

    /// <summary>
    /// Upgrades application settings if the NeedsUpgrade property is set.
    /// </summary>
    private static void UpgradeSettings()
    {
        const string upgradeProperty = "NeedsUpgrade";

        // upgrade Application settings

        var settings = GetApplicationSettings();
        if (settings != null) 
        {
            SettingsProperty? property = settings?.Properties.Cast<SettingsProperty>().FirstOrDefault(p => p.Name == upgradeProperty);
            if (property != null && property.PropertyType == typeof(bool))
            {
                if ((bool)settings![upgradeProperty])
                {
                    settings.Upgrade();
                    settings.Reload();
                    settings[upgradeProperty] = false;
                    settings.Save();
                }
            }
            else
            {
                //throw new Exception($"No App setttings property {upgradeProperty} found");
            }
        }
        //else
        //{
        //    throw new Exception("No App settings found!");
        //}
    }

    /// <summary>
    /// Reference to the application settings instance.
    /// </summary>
    protected ApplicationSettingsBase? settings = null;

    //private void UpgradeSettings()
    // {
    //        // for upgrade add settings
    //        // NeedsUpgrade | bool | User | true
    //        const string upgradeProperty = "NeedsUpgrade";

    // upgrade settings
    //        if (settings != null && (bool)settings[upgradeProperty])
    //        {
    //            settings.Upgrade();
    //            settings.Reload();
    //            settings[upgradeProperty] = false;
    //            settings.Save();
    //    }
    //}

    #endregion

    #region Progress

    /// <summary>
    /// Status text in status line.
    /// </summary>
    [ObservableProperty]
    private string statusText = "Ready";

    /// <summary>
    /// The current progress state for the taskbar item.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowProgress))]
    private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;

    /// <summary>
    /// Gets the visibility of the progress indicator based on the progress state.
    /// </summary>
    public Visibility ShowProgress => ProgressState == TaskbarItemProgressState.None ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    /// The current progress value (0.0 to 1.0).
    /// </summary>
    [ObservableProperty]
    private double progressValue = 0.0;

    /// <summary>
    /// Iterates over a list, performing an action for each item and updating progress.
    /// </summary>
    public void ProgressForEach<T>(List<T> list, Action<T> action, double start = 0.0, double size = 1.0)
    {
        if (list.Count == 0) return;
        ProgressState = TaskbarItemProgressState.Normal;
        ProgressValue = start;
        double step = size / list.Count;
        foreach (var item in list)
        {
            action(item);
            ProgressValue += step;
        }
        ProgressState = TaskbarItemProgressState.None;
    }

    private double progressStep = 0.0;

    /// <summary>
    /// Starts progress reporting for a given number of steps.
    /// </summary>
    public void ProgressStart(int count)
    {
        if (count <= 0) return;
        ProgressState = TaskbarItemProgressState.Normal;
        ProgressValue = 0.0;
        progressStep = 1.0 / count;
    }

    /// <summary>
    /// Advances the progress by one step.
    /// </summary>
    public void ProgressStep() => ProgressValue += progressStep;

    /// <summary>
    /// Stops progress reporting.
    /// </summary>
    public void ProgressStop() => ProgressState = TaskbarItemProgressState.None;

    #endregion

    #region command methods

    /// <summary>
    /// Handles the Loaded event. Invokes <see cref="OnStartup"/> on the UI thread.
    /// </summary>
    [RelayCommand]
    public void OnLoaded()
    {
        if (Application.Current == null)
        {
            // for testing
            OnStartup();
        }
        else
        {
            Application.Current.Dispatcher.Invoke(OnStartup, DispatcherPriority.ContextIdle, null);
        }
    }

    /// <summary>
    /// Called during application startup. Can be overridden in derived classes.
    /// </summary>
    protected virtual void OnStartup() { }

    /// <summary>
    /// Determines whether the Refresh command can execute.
    /// </summary>
    protected virtual bool OnCanRefresh() => true;

    /// <summary>
    /// Handles the Refresh command. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanRefresh))]
    protected virtual async Task OnRefresh()
    {
        await Task.Run(() => { });
    }

    /// <summary>
    /// Determines whether the Import command can execute.
    /// </summary>
    protected virtual bool OnCanImport() => this.ProgressState == TaskbarItemProgressState.None;

    /// <summary>
    /// Handles the Import command. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanImport))]
    protected virtual void OnImport()
    { }

    /// <summary>
    /// Determines whether the Export command can execute.
    /// </summary>
    protected virtual bool OnCanExport()
    {
        return this.ProgressState == TaskbarItemProgressState.None;
    }

    /// <summary>
    /// Handles the Export command. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanExport))]
    protected virtual void OnExport()
    { }

    /// <summary>
    /// Determines whether the Undo command can execute.
    /// </summary>
    protected virtual bool OnCanUndo => false;

    /// <summary>
    /// Handles the Undo command. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanUndo))]
    protected virtual void OnUndo()
    { }

    /// <summary>
    /// Determines whether the Redo command can execute.
    /// </summary>
    protected virtual bool OnCanRedo => false;

    /// <summary>
    /// Handles the Redo command. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanRedo))]
    protected virtual void OnRedo()
    { }

    /// <summary>
    /// Determines whether the Options command can execute.
    /// </summary>
    protected virtual bool OnCanOptions => true;

    /// <summary>
    /// Handles the Options command. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanOptions))]
    protected virtual void OnOptions()
    { }

    /// <summary>
    /// Handles the Options command. Can be overridden in derived classes.
    /// </summary>
    [RelayCommand]
    protected virtual void OnAbout()
    {

        Assembly app = Assembly.GetEntryAssembly()!;
        string title = app.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
        string product = app.GetCustomAttribute<AssemblyProductAttribute>()!.Product;
        string company = app.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "";
        string copyright = app.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? "";
        string description = app.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";
        var ver = app.GetName()!.Version!;
        string version = ver.Build > 0 ? ver.ToString(3) : ver.ToString(2);

        string info = $"{product ?? title} {version}\n{copyright} {company}\n{description}";
        MessageBox.Show(Application.Current.MainWindow, info, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// The help page to open for the application.
    /// </summary>
    protected string HelpPage { get; set; } = null!;

    /// <summary>
    /// Handles the Help command. Opens the help page.
    /// </summary>
    [RelayCommand]
    protected virtual void OnHelp()
    {
        if (string.IsNullOrWhiteSpace(HelpPage)) return;
        try
        {
            Process myProcess = new();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = HelpPage;
            myProcess.Start();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Determines whether the Exit command can execute.
    /// </summary>
    protected virtual bool OnCanExit => true;

    /// <summary>
    /// Handles the Exit command. Closes the main window.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanExit))]
    protected virtual void OnExit()
    {
        Application.Current.MainWindow.Close();
    }

    /// <summary>
    /// Handles the Closing event. Cancels closing if <see cref="OnClosing()"/> returns false.
    /// </summary>
    [RelayCommand]
    private void OnClosing(CancelEventArgs e)
    {
        //if (e != null)
        {
            e.Cancel = !OnClosing();
        }
    }

    /// <summary>
    /// Called during window closing. Can be overridden to prevent closing.
    /// </summary>
    protected virtual bool OnClosing() => true;

    #endregion

    #region error message box

    /// <summary>
    /// Executes an action and shows an error message box if an exception occurs.
    /// </summary>
    public static void HandleErrorMessage(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Exception e = ex is AggregateException && ex.InnerException is not null ? ex.InnerException : ex;
            Debug.WriteLine(e);
            ApplicationDispatcher.Invoke(() => MessageBox.Show(Application.Current.MainWindow, e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }
    }

    /// <summary>
    /// Executes an action and shows an error message box with a custom caption if an exception occurs.
    /// </summary>
    public static void HandleErrorMessage(Action action, string caption)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Exception e = ex is AggregateException && ex.InnerException is not null ? ex.InnerException : ex;
            Debug.WriteLine(e);
            ApplicationDispatcher.Invoke(() => MessageBox.Show(Application.Current.MainWindow, e.Message, caption, MessageBoxButton.OK, MessageBoxImage.Error));
        }
    }
    #endregion
}
