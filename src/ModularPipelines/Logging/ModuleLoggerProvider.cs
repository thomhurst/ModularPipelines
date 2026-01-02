using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides module loggers by detecting the calling module type and creating appropriate logger instances.
/// Uses stack trace analysis to automatically determine which module is requesting a logger.
/// </summary>
/// <remarks>
/// <para>
/// <b>DI Lifetime:</b> This class is registered as Scoped, so each module scope gets its own instance.
/// However, within a module scope, multiple async continuations may access the same instance concurrently,
/// so thread-safety is still required for the parameterless <see cref="GetLogger()"/> method.
/// </para>
/// <para>
/// <b>Caching Strategy:</b> The two <c>GetLogger</c> methods have different caching behaviors by design:
/// </para>
/// <list type="bullet">
/// <item><see cref="GetLogger(Type)"/>: Does NOT cache to <c>_moduleLogger</c> field. This method is
/// for getting a logger for a SPECIFIC type and relies on the <see cref="IModuleLoggerContainer"/>
/// for caching. This prevents incorrect cache pollution when called with different types.</item>
/// <item><see cref="GetLogger()"/>: Caches to <c>_moduleLogger</c> field with lock protection.
/// This method auto-detects the module type and caches the result since within a scope the
/// auto-detected type will always be the same.</item>
/// </list>
/// </remarks>
internal class ModuleLoggerProvider : IModuleLoggerProvider, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleLoggerContainer _moduleLoggerContainer;
    private readonly IStackTraceModuleDetector _stackTraceDetector;
    private readonly object _lock = new();

    // Cached logger for parameterless GetLogger() only.
    // Not used by GetLogger(Type) to avoid type conflicts.
    private IModuleLogger? _moduleLogger;

    public ModuleLoggerProvider(
        IServiceProvider serviceProvider,
        IModuleLoggerContainer moduleLoggerContainer,
        IStackTraceModuleDetector stackTraceDetector)
    {
        _serviceProvider = serviceProvider;
        _moduleLoggerContainer = moduleLoggerContainer;
        _stackTraceDetector = stackTraceDetector;
    }

    /// <summary>
    /// Gets a logger for a specific module type.
    /// </summary>
    /// <remarks>
    /// This method intentionally does NOT cache to the instance-level <c>_moduleLogger</c> field.
    /// Caching is handled by <see cref="IModuleLoggerContainer"/> which provides proper
    /// per-type caching without risking type conflicts between different module types.
    /// </remarks>
    public IModuleLogger GetLogger(Type type)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(type);

        return _moduleLoggerContainer.GetLogger(loggerType)
            ?? (IModuleLogger)_serviceProvider.GetRequiredService(loggerType);
    }

    /// <summary>
    /// Gets a logger for the current module by auto-detecting the module type.
    /// </summary>
    /// <remarks>
    /// <para>This method uses the following detection strategy (in priority order):</para>
    /// <list type="number">
    /// <item>Check cached <c>_moduleLogger</c> (fastest path)</item>
    /// <item>Check <see cref="ModuleLogger.Values"/> AsyncLocal (pre-set during module execution)</item>
    /// <item>Check <see cref="ModuleLogger.CurrentModuleType"/> AsyncLocal</item>
    /// <item>Fall back to stack trace inspection (slowest, for edge cases)</item>
    /// </list>
    /// <para>
    /// The result is cached to <c>_moduleLogger</c> for subsequent calls within the same scope.
    /// Lock is required because multiple async continuations may race to initialize the cached logger.
    /// </para>
    /// </remarks>
    public IModuleLogger GetLogger()
    {
        lock (_lock)
        {
            if (_moduleLogger != null)
            {
                return _moduleLogger;
            }

            // Fast path: check if logger is already set in AsyncLocal
            if (ModuleLogger.Values.Value != null)
            {
                return _moduleLogger = ModuleLogger.Values.Value;
            }

            // Fast path: check if module type is set in AsyncLocal (avoids stack trace inspection)
            var moduleType = ModuleLogger.CurrentModuleType.Value;
            if (moduleType != null)
            {
                return _moduleLogger = CreateLogger(moduleType);
            }

            // Fallback: use stack trace inspection (for edge cases where AsyncLocal context is lost)
            var detectedType = _stackTraceDetector.DetectModuleType();

            if (detectedType == null)
            {
                throw new InvalidOperationException("Could not detect module type from call stack.");
            }

            return _moduleLogger = CreateLogger(detectedType);
        }
    }

    public void Dispose()
    {
        _moduleLogger?.Dispose();
    }

    private IModuleLogger CreateLogger(Type module)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(module);

        return _moduleLoggerContainer.GetLogger(loggerType)
            ?? (IModuleLogger)_serviceProvider.GetRequiredService(loggerType);
    }
}
