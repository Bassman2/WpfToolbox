namespace WpfToolbox.ViewModel;

public abstract partial class AppViewModel : ObservableValidator
{
    //private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;
    //private double progressValue = 0.0;
    private readonly string progressTime = string.Empty;

    protected readonly Window mainWindow;
    protected readonly Dispatcher mainDispatcher;

    public AppViewModel()
    {
        UpgradeSettings();
        mainWindow = Application.Current.MainWindow;
        mainDispatcher = Application.Current.MainWindow.Dispatcher;
    }


    public static string LocalAppDataPath => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().ProcessName));

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
            var ver = app.GetName()!.Version!;
            string version = ver.Build > 0 ? ver.ToString(3) : ver.ToString(2);
            return $"{title} {version}";
        }
    }

    #endregion

    #region Drag & Drop

    [RelayCommand]
    protected virtual void OnDrag(DragEventArgs args)
    { }

    [RelayCommand]
    protected virtual void OnDrop(DragEventArgs args)
    { }

    #endregion

    #region Settings

    protected static ApplicationSettingsBase? GetApplicationSettings()
    {
        return (ApplicationSettingsBase?)System.Reflection.Assembly.GetEntryAssembly()!.GetTypes().FirstOrDefault(t => t.FullName!.EndsWith(".Properties.Settings"))?.GetProperty("Default")?.GetValue(null);
    }

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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowProgress))]
    private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;

    public Visibility ShowProgress => ProgressState == TaskbarItemProgressState.None ? Visibility.Collapsed : Visibility.Visible;

    [ObservableProperty]
    private double progressValue = 0.0;

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

    public void ProgressStart(int count)
    {
        if (count <= 0) return;
        ProgressState = TaskbarItemProgressState.Normal;
        ProgressValue = 0.0;
        progressStep = 1.0 / count;
    }

    public void ProgressStep() => ProgressValue += progressStep;

    public void ProgressStop() => ProgressState = TaskbarItemProgressState.None;
    
    #endregion

    #region command methods

    [RelayCommand]
    public virtual void OnStartup()
    {
        if (Application.Current == null)
        {
            // for testing
            OnActivate();
        }
        else
        {
            Application.Current.Dispatcher.Invoke(OnActivate, DispatcherPriority.ContextIdle, null);
        }
    }

    //[RelayCommand]
    protected abstract void OnActivate();

    protected virtual bool OnCanRefresh() => true;
    
    [RelayCommand(CanExecute = nameof(OnCanRefresh))]
    protected virtual async Task OnRefresh()
    {
        await Task.Run(() => { });
    }

    protected virtual bool OnCanImport() => this.ProgressState == TaskbarItemProgressState.None;

    
    [RelayCommand(CanExecute = nameof(OnCanImport))]
    protected virtual void OnImport()
    { }

    protected virtual bool OnCanExport()
    {
        return this.ProgressState == TaskbarItemProgressState.None;
    }

    [RelayCommand(CanExecute = nameof(OnCanExport))]
    protected virtual void OnExport()
    { }

    protected virtual bool OnCanUndo => false;
    
    [RelayCommand(CanExecute = nameof(OnCanUndo))]
    protected virtual void OnUndo()
    { }


    protected virtual bool OnCanRedo => false;
    
    [RelayCommand(CanExecute = nameof(OnCanRedo))]
    protected virtual void OnRedo()
    { }

    protected virtual bool OnCanOptions => true;

    [RelayCommand(CanExecute = nameof(OnCanOptions))]
    protected virtual void OnOptions()
    { }


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

    protected string HelpPage { get; set; } = null!;

    [RelayCommand]
    protected virtual void OnHelp()
    {
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

    protected virtual bool OnCanExit => true;
    

    [RelayCommand(CanExecute = nameof(OnCanExit))]
    protected virtual void OnExit()
    {
        Application.Current.MainWindow.Close();
    }
    
    [RelayCommand]
    private void OnClosing(CancelEventArgs e)
    {
        //if (e != null)
        {
            e.Cancel = !OnClosing();
        }
    }
            
    protected virtual bool OnClosing() => true;
    
    #endregion

    #region error message box

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
