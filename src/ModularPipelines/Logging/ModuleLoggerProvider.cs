using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides module loggers by detecting the calling module type and creating appropriate logger instances.
/// Uses stack trace analysis to automatically determine which module is requesting a logger.
/// </summary>
/// <remarks>
/// This provider coordinates between:
/// - StackTraceModuleDetector: Analyzes call stack to find module type
/// - Service provider: Creates new logger instances
/// The provider maintains thread-safe singleton behavior per module type.
/// </remarks>
internal class ModuleLoggerProvider : IInternalModuleLoggerProvider, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IStackTraceModuleDetector _stackTraceDetector;
    private readonly object _lock = new();

    private IModuleLogger? _moduleLogger;

    public ModuleLoggerProvider(
        IServiceProvider serviceProvider,
        IStackTraceModuleDetector stackTraceDetector)
    {
        _serviceProvider = serviceProvider;
        _stackTraceDetector = stackTraceDetector;
    }

    /// <summary>
    /// Gets a logger for a specific module type.
    /// Does not cache to _moduleLogger to avoid conflicts with parameterless GetLogger().
    /// </summary>
    public IModuleLogger GetLogger(Type type)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(type);
        return (IModuleLogger)_serviceProvider.GetRequiredService(loggerType);
    }

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
                return _moduleLogger = GetLogger(moduleType);
            }

            // Fallback: use stack trace inspection (for edge cases where AsyncLocal context is lost)
            var detectedType = _stackTraceDetector.DetectModuleType();

            if (detectedType == null)
            {
                throw new InvalidOperationException("Could not detect module type from call stack.");
            }

            return _moduleLogger = GetLogger(detectedType);
        }
    }

    public void Dispose()
    {
        _moduleLogger?.Dispose();
    }
}
