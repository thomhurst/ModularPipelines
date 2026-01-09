namespace ModularPipelines.Logging;

/// <summary>
/// Encapsulates the management of AsyncLocal context for module logging.
/// </summary>
/// <remarks>
/// This scope wrapper saves the previous AsyncLocal values on creation and restores them on disposal,
/// ensuring proper cleanup even when exceptions occur. This pattern supports nested scopes correctly
/// by preserving and restoring the previous context rather than clearing it.
/// The IAsyncDisposable pattern makes the lifetime management explicit and less error-prone.
/// </remarks>
internal sealed class ModuleLoggerScope : IAsyncDisposable
{
    private readonly IModuleLogger? _previousLogger;
    private readonly Type? _previousType;
    private bool _disposed;

    /// <summary>
    /// Creates a new ModuleLoggerScope that sets the logger and module type in AsyncLocal storage,
    /// preserving any previous values for restoration on dispose.
    /// </summary>
    /// <param name="logger">The module logger to set in ambient context.</param>
    /// <param name="moduleType">The module type to set in ambient context.</param>
    public ModuleLoggerScope(IModuleLogger logger, Type moduleType)
    {
        // Save previous values for restoration on dispose
        _previousLogger = ModuleLogger.Values.Value;
        _previousType = ModuleLogger.CurrentModuleType.Value;

        // Set new values
        ModuleLogger.Values.Value = logger;
        ModuleLogger.CurrentModuleType.Value = moduleType;
    }

    /// <summary>
    /// Restores the previous AsyncLocal context values.
    /// </summary>
    public ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return ValueTask.CompletedTask;
        }

        _disposed = true;

        // Restore previous values (may be null if there was no previous context)
        ModuleLogger.Values.Value = _previousLogger;
        ModuleLogger.CurrentModuleType.Value = _previousType;

        return ValueTask.CompletedTask;
    }
}
