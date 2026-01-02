namespace ModularPipelines.Logging;

/// <summary>
/// Encapsulates the management of AsyncLocal context for module logging.
/// </summary>
/// <remarks>
/// This scope wrapper sets the AsyncLocal values on creation and clears them on disposal,
/// ensuring proper cleanup even when exceptions occur. The IAsyncDisposable pattern
/// makes the lifetime management explicit and less error-prone.
/// </remarks>
internal sealed class ModuleLoggerScope : IAsyncDisposable
{
    /// <summary>
    /// Creates a new ModuleLoggerScope that sets the logger and module type in AsyncLocal storage.
    /// </summary>
    /// <param name="logger">The module logger to set in ambient context.</param>
    /// <param name="moduleType">The module type to set in ambient context.</param>
    public ModuleLoggerScope(IModuleLogger logger, Type moduleType)
    {
        ModuleLogger.Values.Value = logger;
        ModuleLogger.CurrentModuleType.Value = moduleType;
    }

    /// <summary>
    /// Clears the AsyncLocal context values.
    /// </summary>
    public ValueTask DisposeAsync()
    {
        // Clear AsyncLocal to prevent potential leaks in edge cases (thread pool reuse, long-running contexts)
        ModuleLogger.Values.Value = null;
        ModuleLogger.CurrentModuleType.Value = null;
        return ValueTask.CompletedTask;
    }
}
