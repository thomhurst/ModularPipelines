namespace ModularPipelines.Plugins;

/// <summary>
/// Helper class for testing plugins in isolation.
/// </summary>
public static class PluginTestHelper
{
    /// <summary>
    /// Creates an execution-context-local plugin registry scope for testing.
    /// The empty isolated registry is replaced with the previous context when disposed.
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
    public static IDisposable IsolatedRegistry() => PluginRegistry.BeginIsolatedScope();
}
