namespace MvvmAppBase.ViewModel;

public abstract partial class AppViewModel : ObservableObject
{
    //private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;
    //private double progressValue = 0.0;
    private readonly string progressTime = string.Empty;

    public AppViewModel()
    {
        UpgradeSettings();
    }

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

    #endregion

    #region Title

    /// <summary>
    /// The main title of the application. Displayed in the main window header and in the taskbar.
    /// </summary>
    public virtual string Title
    {
        get
        {
            var ass = System.Reflection.Assembly.GetEntryAssembly()!.GetName()!;
            return $"{ass.Name} {ass.Version!.ToString(2)}";
        }
    }

    #endregion

    #region Progress

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowProgress))]
    private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;

    public Visibility ShowProgress => ProgressState == TaskbarItemProgressState.None ? Visibility.Collapsed : Visibility.Visible;

    [ObservableProperty]
    private double progressValue = 0.0;

    /// <summary>
    /// Status text in status line.
    /// </summary>
    [ObservableProperty]
    private string statusText = "Ready";

    //private void OnProgressState(ProgressStateEventArgs args)
    //{
    //    this.ProgressState = args.State;
    //}

    //private void OnProgressValue(ProgressValueEventArgs args)
    //{
    //    this.ProgressValue = args.Value;
    //}

    //private void OnProgressText(ProgressTextEventArgs args)
    //{
    //    this.StatusText = args.Text;
    //}

    public static bool IsWorking => false;

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
            Application.Current.Dispatcher.BeginInvoke(new Action(() => OnActivate()), DispatcherPriority.ContextIdle, null);
        }
    }

    //[RelayCommand]
    protected abstract void OnActivate();

    protected virtual bool OnCanRefresh() => true;
    
    [RelayCommand(CanExecute = nameof(OnCanRefresh))]
    protected virtual void OnRefresh()
    { }

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
    { }

    [RelayCommand]
    protected virtual void OnHelp()
    {
        string path = System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetEntryAssembly()!.Location, ".chm");
        if (System.IO.File.Exists(path))
        {
            System.Diagnostics.Process.Start(path);
        }
        else
        {
            MessageBox.Show(string.Format("Help file \"{0}\" not found!", path), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

}
