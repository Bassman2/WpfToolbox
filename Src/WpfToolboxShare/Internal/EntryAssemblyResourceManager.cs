namespace WpfToolbox.Internal;

internal static class EntryAssemblyResourceManager
{
    private static readonly ResourceManager? resourceManager;

    static EntryAssemblyResourceManager()
    {
        if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Assembly assembly = Assembly.GetEntryAssembly()!;
            string? name = assembly.DefinedTypes.SingleOrDefault(t => t.Name == "Resources")?.FullName!;
            if (name is not null)
            {
                resourceManager = new ResourceManager(name, assembly);
            }
        }
    }

    public static string? GetString(object item)
    {
        if (resourceManager is not null && item.GetType().GetField(item.ToString()!)?.GetCustomAttribute<ResourceAttribute>() is ResourceAttribute resourceAttribute)
        {
            return resourceManager.GetString(resourceAttribute.Name) ?? item.ToString();
        }
        return item.ToString();
    }

    public static string? GetString(string name)
    {
        return resourceManager?.GetString(name) ?? null;
    }
}
