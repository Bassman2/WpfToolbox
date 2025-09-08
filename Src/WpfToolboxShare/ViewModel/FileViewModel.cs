namespace WpfToolbox.ViewModel;

/// <summary>
/// Abstract base view model for file-based WPF applications.
/// Provides file open/save logic, recent file management, and drag &amp; drop support.
/// </summary>
public abstract partial class FileViewModel : AppViewModel
{
    /// <summary>
    /// Message shown when prompting the user to save changes before closing or opening a file.
    /// </summary>
    private readonly string saveBefore = "Do you want to store current file before? If not all changes will be lost.";

    /// <summary>
    /// Default file extension for file dialogs (e.g., "*.txt").
    /// </summary>
    protected string? DefaultFileExt { get; set; }

    /// <summary>
    /// File filter string for file dialogs (e.g., "Text Files|*.txt|All Files|*.*").
    /// </summary>
    protected string? FileFilter { get; set; }

    /// <summary>
    /// Maximum number of recent files to keep in the list.
    /// </summary>
    protected int MaxNumOfRecentFiles { get; set; }

    /// <summary>
    /// Represents a file entry in the recent files list.
    /// </summary>
    public class FileItem(string path)
    {
        /// <summary>
        /// Gets the file name (without path).
        /// </summary>
        public string Name { get; } = System.IO.Path.GetFileName(path);

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public string Path { get; } = System.IO.Path.GetFullPath(path);
        //public ImageSource Image { get; private set; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileViewModel"/> class with default settings.
    /// </summary>
    public FileViewModel()
    {
        this.FilePath = null;
        this.FileChanged = false;
        this.DefaultFileExt = "*.*";
        this.FileFilter = "All Files|*.*";
        this.MaxNumOfRecentFiles = 10;
    }

    /// <summary>
    /// Handles application startup logic, including loading recent files and opening files from command line or autoload.
    /// </summary>
    protected override void OnStartup()
    {
        LoadRecentFileList();

        string[] cmd = Environment.GetCommandLineArgs();
        if (cmd.Length > 1 && System.IO.File.Exists(cmd[1]))
        {
            string path = System.IO.Path.GetFullPath(cmd[1]);
            OnLoad(path);
            this.FilePath = path;
        }
        else if (this.Autoload && System.IO.File.Exists(this.AutoloadFile))
        {
            OnLoad(this.AutoloadFile);
            this.FilePath = this.AutoloadFile;
        }
        base.OnStartup();
    }

    /// <summary>
    /// Gets the window title, including file name and change marker if needed.
    /// </summary>
    public override string Title => string.Format("{0}{1}{2}", base.Title, string.IsNullOrEmpty(this.FilePath) ? "" : " - " + this.FileName, this.FileChanged ? " *" : "");

    /// <summary>
    /// The full path of the current file.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FileName))]
    [NotifyPropertyChangedFor(nameof(Title))]
    private string? filePath;

    /// <summary>
    /// Gets the file name (without extension) of the current file.
    /// </summary>
    public string? FileName => System.IO.Path.GetFileNameWithoutExtension(this.FilePath);

    /// <summary>
    /// Indicates whether the current file has unsaved changes.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private bool fileChanged;

    /// <summary>
    /// Collection with the recent files.
    /// </summary>
    [ObservableProperty]
    private System.Collections.ObjectModel.ObservableCollection<FileItem> recentFiles = [];

    /// <summary>
    /// Gets whether the view model should autoload a file on startup.
    /// </summary>
    protected virtual bool Autoload { get { return false; } }

    /// <summary>
    /// Gets or sets the file path to autoload on startup.
    /// </summary>
    protected virtual string AutoloadFile { get { return string.Empty; } set { } }

    /// <summary>
    /// Called when a new file is created.
    /// </summary>
    public abstract void OnCreate();

    /// <summary>
    /// Called when a file is loaded.
    /// </summary>
    /// <param name="path">The file path to load.</param>
    public abstract void OnLoad(string path);

    /// <summary>
    /// Called when a file is stored (saved).
    /// </summary>
    /// <param name="path">The file path to store.</param>
    public abstract void OnStore(string path);

    /// <summary>
    /// Determines whether the New command can execute.
    /// </summary>
    protected virtual bool OnCanNew() => this.ProgressState == TaskbarItemProgressState.None;

    /// <summary>
    /// Creates a new file, prompting to save changes if needed.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanNew))]
    protected virtual void OnNew()
    {
        if (StoreChanges())
        {
            this.FilePath = null;
            this.FileChanged = true;
            OnCreate();
        }
    }

    /// <summary>
    /// Determines whether the Open command can execute.
    /// </summary>
    protected virtual bool OnCanOpen() => this.ProgressState == TaskbarItemProgressState.None;


    /// <summary>
    /// Opens a file using a file dialog, prompting to save changes if needed.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanOpen))]
    protected virtual void OnOpen()
    {
        if (StoreChanges())
        {
            Microsoft.Win32.OpenFileDialog dlg = new()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = DefaultFileExt,
                Filter = FileFilter,
                Multiselect = false,
                Title = "Open file ..."
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                this.FilePath = dlg.FileName;
                OnLoad(dlg.FileName);
                this.AutoloadFile = dlg.FileName;
                this.FileChanged = false;
                AddRecentFile(dlg.FileName);
            }
        }
    }

    /// <summary>
    /// Determines whether a file can be opened by path.
    /// </summary>
    protected virtual bool OnCanOpenFile(string path) => this.ProgressState == TaskbarItemProgressState.None;


    /// <summary>
    /// Opens a file by path, prompting to save changes if needed.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanOpenFile))]
    protected virtual void OnOpenFile(string path)
    {

        if (StoreChanges())
        {
            OnLoad(path);
            this.FilePath = path;
            this.AutoloadFile = path;
            this.FileChanged = false;
            AddRecentFile(path);
        }
    }

    /// <summary>
    /// Determines whether the Save command can execute.
    /// </summary>
    protected virtual bool OnCanSave() => this.ProgressState == TaskbarItemProgressState.None && !string.IsNullOrEmpty(this.FilePath) && this.FileChanged;


    /// <summary>
    /// Saves the current file.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanSave))]
    protected virtual void OnSave()
    {
        OnStore(this.FilePath!);
        this.FileChanged = false;
    }

    /// <summary>
    /// Determines whether the Save As command can execute.
    /// </summary>
    protected virtual bool OnCanSaveAs() => this.ProgressState == TaskbarItemProgressState.None;

    /// <summary>
    /// Saves the current file to a new location using a file dialog.
    /// </summary>
    [RelayCommand(CanExecute = nameof(OnCanSaveAs))]
    protected virtual void OnSaveAs()
    {
        Microsoft.Win32.SaveFileDialog dlg = new()
        {
            OverwritePrompt = true,
            ValidateNames = true,
            CheckPathExists = true,
            DefaultExt = DefaultFileExt,
            Filter = FileFilter,
            Title = "Save file as ..."
        };
        if (dlg.ShowDialog().GetValueOrDefault())
        {
            //this.watcher.EnableRaisingEvents = false;
            OnStore(dlg.FileName);
            this.FilePath = dlg.FileName;
            this.AutoloadFile = dlg.FileName;
            this.FileChanged = false;
            AddRecentFile(dlg.FileName);
        }
    }

    /// <summary>
    /// Opens a recent file by path.
    /// </summary>
    [RelayCommand]
    protected virtual void OnRecentFile(string path)
    {
        if (System.IO.File.Exists(path))
        {
            OnOpenFile(path);
        }
        else
        {
            //this.recentFiles.Remove(i => i.Path == path);
        }
    }

    //protected virtual void OnFileChanged(object source, FileSystemEventArgs e)
    //{
    //    if (e.ChangeType == WatcherChangeTypes.Changed)
    //    {
    //        Application.Current.Dispatcher.Invoke(new Action(() =>
    //        {
    //            if (MessageBox.Show(Application.Current.MainWindow, "File changed from outside.\r\nDo you want to reload the file and loose all changes?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
    //            {
    //                OnLoad(e.FullPath);
    //                this.FileChanged = false;
    //            }
    //        }));
    //    }
    //}

    //protected virtual void OnFileRenamed(object source, RenamedEventArgs e)
    //{
    //    if (e.ChangeType == WatcherChangeTypes.Renamed)
    //    {
    //        Application.Current.Dispatcher.Invoke(new Action(() =>
    //        {
    //            if (MessageBox.Show(Application.Current.MainWindow, "File renamed from outside.\r\nDo you want to rename your file too?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
    //            {
    //                this.FilePath = e.FullPath;
    //                this.AutoloadFile = e.FullPath;
    //            }
    //        }));
    //    }
    //}

    #region Drag & Drop

    /// <summary>
    /// Handles drag events for file drop support.
    /// </summary>
    /// <param name="args">The drag event arguments.</param>
    protected override void OnDrag(DragEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args, nameof(args));

        bool isCorrect = true;
        if (args.Data.GetDataPresent(DataFormats.FileDrop, true))
        {
            string[] files = args.Data.GetData(DataFormats.FileDrop, true) as string[] ?? [];
            foreach (string file in files)
            {
                if (!System.IO.File.Exists(file) || !OnCanOpenFile(file))
                {
                    isCorrect = false;
                }
            }
        }
        args.Effects = isCorrect ? DragDropEffects.All : DragDropEffects.None;
        args.Handled = true;
    }

    /// <summary>
    /// Handles drop events for file drop support.
    /// </summary>
    /// <param name="args">The drop event arguments.</param>
    protected override void OnDrop(DragEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args, nameof(args));

        if (args.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = args.Data.GetData(DataFormats.FileDrop, true) as string[] ?? [];
            foreach (string file in files)
            {
                OnOpenFile(file);
            }
        }
        args.Handled = true;
    }

    #endregion

    /// <summary>
    /// Closing handler to store changed files on exit.
    /// </summary>
    /// <example>
    /// &lt;Window x:Class="InternalInvoiceManager.MainView" xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"&gt;
    /// &lt;i:Interaction.Triggers&gt;
    /// &lt;i:EventTrigger EventName="Closing"&gt;
    ///     &lt;EventToCommand Command="{Binding ClosingCommand}" PassEventArgsToCommand="True"/&gt;
    /// &lt;/i:EventTrigger&gt;
    /// </example>
    protected override bool OnClosing()
    {
        return StoreChanges();
    }

    /// <summary>
    /// Prompts the user to store changes if the file has been modified.
    /// </summary>
    /// <returns>True if the operation can continue, false if canceled.</returns>
    protected virtual bool StoreChanges()
    {
        if (this.FileChanged)
        {
            switch (MessageBox.Show(saveBefore, "Info", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
            case MessageBoxResult.Yes:
                if (string.IsNullOrEmpty(this.FilePath))
                {
                    OnSaveAs();
                }
                else
                {
                    OnSave();
                }
                break;
            case MessageBoxResult.No:
                break;
            case MessageBoxResult.Cancel:
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Loads the recent file list from application settings.
    /// </summary>
    private void LoadRecentFileList()
    {
        ApplicationSettingsBase? settings = GetApplicationSettings();

        if (settings != null && settings[nameof(RecentFiles)] != null)
        {
            foreach (string? path in (System.Collections.Specialized.StringCollection)settings[nameof(RecentFiles)])
            {
                if (System.IO.File.Exists(path))
                {
                    this.RecentFiles.Add(new FileItem(path));
                }
            }

            //settings["RecentFiles"] = new(((StringCollection)settings["RecentFiles"])?.Cast<string>().Where(p => System.IO.File.Exists(p)).Select(p => new FileItem(p)));
        }
    }

    /// <summary>
    /// Adds a file to the recent files list and updates application settings.
    /// </summary>
    /// <param name="path">The file path to add.</param>
    protected void AddRecentFile(string path)
    {
        if (this.RecentFiles.Count <= this.MaxNumOfRecentFiles && !this.RecentFiles.Any(i => i.Name == System.IO.Path.GetFileName(path)))
        {
            this.RecentFiles.Add(new FileItem(path));

            // store recent file list
            System.Collections.Specialized.StringCollection col = [];
            col.AddRange([.. this.RecentFiles.Select(f => f.Path)]);

            ApplicationSettingsBase? settings = GetApplicationSettings();
            if (settings != null)
            {
                settings[nameof(RecentFiles)] = col;
                settings.Save();
            }
        }
    }
}
