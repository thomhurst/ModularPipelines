using ModularPipelines.Plugins;

namespace ModularPipelines.Testing;

/// <summary>
/// Provides helpers for testing plugins in isolation.
/// </summary>
public static class PluginTestHelper
{
    /// <summary>
    /// Creates an execution-context-local plugin registry scope for testing.
    /// The empty isolated registry is replaced with the previous context when disposed.
    /// </summary>
    /// <returns>A disposable scope that restores the original plugins when disposed.</returns>
    public static IDisposable IsolatedRegistry() => PluginRegistry.BeginIsolatedScope();
}
