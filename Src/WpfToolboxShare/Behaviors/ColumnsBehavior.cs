using Microsoft.Xaml.Behaviors;

namespace WpfToolbox.Behaviors;

public class ColumnsBehavior : Behavior<DataGrid>
{
    private readonly ContextMenu headerContextMenu = new();

    protected override void OnAttached()
    {
        AssociatedObject.Initialized += OnInitialized;

        var headerStyle = new Style(typeof(DataGridColumnHeader));
        headerStyle.Setters.Add(new Setter(DataGridColumnHeader.ContextMenuProperty, headerContextMenu));
        AssociatedObject.ColumnHeaderStyle = headerStyle;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Initialized -= OnInitialized;
    }

    private void OnInitialized(object? sender, EventArgs e)
    {
        if (SettingsName != null)
        {
            var settings = GetApplicationSettings();
            if (settings != null && settings.Properties.Cast<System.Configuration.SettingsProperty>().Any(p => p.Name == SettingsName))
            {
                ColumnVisibility = (uint)(settings[SettingsName] ?? uint.MaxValue);
            }
        }
    }

    public string? SettingsName { get; set; } = null;


    public static readonly DependencyProperty ColumnVisibilityProperty =
        DependencyProperty.Register("ColumnVisibility", typeof(uint), typeof(ColumnsBehavior),
            new FrameworkPropertyMetadata(uint.MaxValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback((d, e) => ((ColumnsBehavior)d).CreateHeaderMenu((uint)e.NewValue))));

    public uint ColumnVisibility
    {
        get => (uint)GetValue(ColumnVisibilityProperty);
        set => SetValue(ColumnVisibilityProperty, value);
    }

    private void CreateHeaderMenu(uint flags)
    {
        uint mask = 1;
        headerContextMenu.Items.Clear();
        foreach (var column in AssociatedObject.Columns)
        {
            bool isChecked = (flags & mask) != 0;
            var menuItem = new MenuItem() { Header = column.Header, Tag = column, IsCheckable = true, IsChecked = isChecked };
            menuItem.Checked += OnHeaderMenuItemChecked;
            menuItem.Unchecked += OnHeaderMenuItemUnchecked;
            headerContextMenu.Items.Add(menuItem);
            column.Visibility = isChecked ? Visibility.Visible : Visibility.Collapsed;
            mask <<= 1;
        }
    }

    private void OnHeaderMenuItemUnchecked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.Tag is DataGridColumn column)
        {
            column.Visibility = Visibility.Collapsed;
        }
        UpdateColumnVisibility();
    }

    private void OnHeaderMenuItemChecked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.Tag is DataGridColumn column)
        {
            column.Visibility = Visibility.Visible;
        }
        UpdateColumnVisibility();
    }

    private void UpdateColumnVisibility()
    {
        uint flags = 0;
        uint mask = 1;
        foreach (var column in AssociatedObject.Columns)
        {
            if (column.Visibility == Visibility.Visible)
            {
                flags |= mask;
            }
            mask <<= 1;
        }
        ColumnVisibility = flags;

        if (SettingsName != null)
        {
            var settings = GetApplicationSettings();
            if (settings != null && settings.Properties.Cast<System.Configuration.SettingsProperty>().Any(p => p.Name == SettingsName))
            {
                settings[SettingsName] = flags;
                settings.Save();
            }
        }
    }

    private void OnHeaderContextMenuOpening(object sender, RoutedEventArgs e)
    {
        if (headerContextMenu == null) return;
        headerContextMenu.Items.Clear();
        foreach (var column in AssociatedObject.Columns)
        {
            headerContextMenu.Items.Add(new MenuItem() { Header = column.Header });
        }
    }

    private static ApplicationSettingsBase? GetApplicationSettings()
    {
        return (ApplicationSettingsBase?)System.Reflection.Assembly.GetEntryAssembly()!.GetTypes().FirstOrDefault(t => t.FullName!.EndsWith(".Properties.Settings"))?.GetProperty("Default")?.GetValue(null);
    }
}
