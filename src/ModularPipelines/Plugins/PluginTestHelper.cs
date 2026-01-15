namespace ModularPipelines.Plugins;

/// <summary>
/// Helper class for testing plugins in isolation.
/// </summary>
public static class PluginTestHelper
{
    /// <summary>
    /// Creates an isolated plugin registry scope for testing.
    /// Clears all registered plugins and restores them when disposed.
    /// </summary>
    /// <returns>A disposable scope that restores the original plugins when disposed.</returns>
    /// <example>
    /// <code>
    /// [Test]
    /// public async Task MyPlugin_Should_Register_Services()
    /// {
    ///     using var _ = PluginTestHelper.IsolatedRegistry();
    ///     PluginRegistry.Register(new MyTestPlugin());
    ///
    ///     // Test plugin behavior in isolation...
    /// }
    /// </code>
    /// </example>
    public static IDisposable IsolatedRegistry()
    {
        var snapshot = PluginRegistry.Plugins.ToList();
        PluginRegistry.Clear();
        return new RegistryRestorer(snapshot);
    }

    private sealed class RegistryRestorer : IDisposable
    {
        private readonly List<IModularPipelinesPlugin> _snapshot;

        public RegistryRestorer(List<IModularPipelinesPlugin> snapshot)
        {
            _snapshot = snapshot;
        }

        public void Dispose()
        {
            PluginRegistry.Clear();
            foreach (var plugin in _snapshot)
            {
                PluginRegistry.Register(plugin);
            }
        }
    }
}
