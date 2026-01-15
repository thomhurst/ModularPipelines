namespace ModularPipelines.Plugins;

/// <summary>
/// Static registry for ModularPipelines plugins.
/// Plugins register themselves via [ModuleInitializer] methods during assembly load.
/// </summary>
public static class PluginRegistry
{
    private static readonly List<IModularPipelinesPlugin> Registered = [];

    /// <summary>
    /// Gets all registered plugins, ordered by priority (ascending).
    /// </summary>
    public static IReadOnlyList<IModularPipelinesPlugin> Plugins =>
        Registered.OrderBy(p => p.Priority).ToList();

    /// <summary>
    /// Registers a plugin. Call this from a [ModuleInitializer] method.
    /// </summary>
    /// <param name="plugin">The plugin to register.</param>
    /// <exception cref="InvalidOperationException">Thrown if a plugin with the same name is already registered.</exception>
    public static void Register(IModularPipelinesPlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        if (Registered.Any(p => p.Name == plugin.Name))
        {
            throw new InvalidOperationException($"Plugin '{plugin.Name}' is already registered.");
        }

        Registered.Add(plugin);
    }

    /// <summary>
    /// Clears all registered plugins. For testing purposes only.
    /// </summary>
    internal static void Clear() => Registered.Clear();
}
