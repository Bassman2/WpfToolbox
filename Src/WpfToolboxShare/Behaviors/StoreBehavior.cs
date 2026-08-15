namespace WpfToolbox.Behaviors;

/// <summary>
/// Represents an abstract base behavior that can be attached to a <see cref="DependencyObject"/> and provides store-related functionality.
/// </summary>
/// <typeparam name="T">The type of <see cref="DependencyObject"/> to which this behavior can be attached.</typeparam>
public abstract class StoreBehavior<T> : Behavior<T> where T : DependencyObject
{
    private static readonly ApplicationSettingsBase applicationSettings;

    static StoreBehavior()
    {
        applicationSettings = (ApplicationSettingsBase?)System.Reflection.Assembly.GetEntryAssembly()!.GetTypes().FirstOrDefault(t => t.FullName!.EndsWith(".Properties.Settings"))?.GetProperty("Default")?.GetValue(null)!;
    }

    /// <summary>
    /// Gets or sets the application settings key used to persist column visibility.
    /// </summary>
    public string? SettingsName { get; set; } = null;

    
    /// <summary>
    /// Gets the value of the specified application setting, or returns the provided default value if the setting is not found or is of a different type.
    /// </summary>
    /// <typeparam name="TValue">The type of the setting value to retrieve.</typeparam>
    /// <param name="def">The default value to return if the setting is not found or is not of the expected type.</param>
    /// <returns>The value of the setting if found and of the correct type; otherwise, the default value.</returns>
    public TValue GetSettingsValue<TValue>(TValue def)
    {
        ThrowIfPropertyNotExists();
        return applicationSettings[SettingsName] is TValue tValue ? tValue : def;
    }

    /// <summary>
    /// Sets the value of the specified application setting and saves the settings.
    /// </summary>
    /// <param name="value">The value to set for the setting.</param>
    public void SetSettingsValue(object value)
    {
        ThrowIfPropertyNotExists();

        applicationSettings[SettingsName] = value;
        applicationSettings.Save();
    }



    /// <summary>
    /// Throws an exception if the <see cref="SettingsName"/> property is not set or does not exist in the application settings.
    /// </summary>
    protected void ThrowIfPropertyNotExists()
    {
        if (string.IsNullOrWhiteSpace(SettingsName))
        {
            throw new InvalidOperationException("SettingsName is not set.");
        }
        if (applicationSettings is null || !applicationSettings.Properties.Cast<System.Configuration.SettingsProperty>().Any(p => p.Name == SettingsName))
        {
            throw new ArgumentException($"Settings property '{SettingsName}' does not exist in application settings.");
        }
    }
}
