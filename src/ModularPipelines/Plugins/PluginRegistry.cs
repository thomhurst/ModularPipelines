namespace ModularPipelines.Plugins;

/// <summary>
/// Static registry for ModularPipelines plugins.
/// Plugins register themselves via [ModuleInitializer] methods during assembly load.
/// </summary>
public static class PluginRegistry
{
    private static readonly object Lock = new();
    private static readonly List<IModularPipelinesPlugin> Registered = [];
    private static readonly AsyncLocal<List<IModularPipelinesPlugin>?> IsolatedRegistry = new();

    /// <summary>
    /// Gets all registered plugins, ordered by priority (ascending).
    /// </summary>
    public static IReadOnlyList<IModularPipelinesPlugin> Plugins
    {
        get
        {
            lock (Lock)
            {
                return GetActiveRegistry().OrderBy(p => p.Priority).ToList();
            }
        }
    }

    /// <summary>
    /// Registers a plugin. Call this from a [ModuleInitializer] method.
    /// </summary>
    /// <param name="plugin">The plugin to register.</param>
    /// <exception cref="InvalidOperationException">Thrown if a plugin with the same name is already registered.</exception>
    public static void Register(IModularPipelinesPlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        lock (Lock)
        {
            var registry = GetActiveRegistry();
            if (registry.Any(p => p.Name == plugin.Name))
            {
                throw new InvalidOperationException($"Plugin '{plugin.Name}' is already registered.");
            }

            registry.Add(plugin);
        }
    }

    /// <summary>
    /// Clears all registered plugins. For testing purposes only.
    /// </summary>
    internal static void Clear()
    {
        lock (Lock)
        {
            GetActiveRegistry().Clear();
        }
    }

    internal static IDisposable BeginIsolatedScope()
    {
        var previousRegistry = IsolatedRegistry.Value;
        IsolatedRegistry.Value = [];
        return new IsolatedRegistryScope(previousRegistry);
    }

    private static List<IModularPipelinesPlugin> GetActiveRegistry() =>
        IsolatedRegistry.Value ?? Registered;

    private sealed class IsolatedRegistryScope(List<IModularPipelinesPlugin>? previousRegistry) : IDisposable
    {
        public void Dispose()
        {
            IsolatedRegistry.Value = previousRegistry;
        }
    }
}
