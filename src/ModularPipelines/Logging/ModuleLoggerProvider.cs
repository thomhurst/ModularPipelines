using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides module loggers by detecting the calling module type and creating appropriate logger instances.
/// Uses stack trace analysis to automatically determine which module is requesting a logger.
/// </summary>
/// <remarks>
/// This provider coordinates between:
/// - StackTraceModuleDetector: Analyzes call stack to find module type
/// - ModuleLoggerContainer: Caches existing loggers
/// - Service provider: Creates new logger instances
/// The provider maintains thread-safe singleton behavior per module type.
/// </remarks>
internal class ModuleLoggerProvider : IModuleLoggerProvider, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleLoggerContainer _moduleLoggerContainer;
    private readonly IStackTraceModuleDetector _stackTraceDetector;

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

    public IModuleLogger GetLogger(Type type) => MakeLogger(type);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IModuleLogger GetLogger()
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
            return MakeLogger(moduleType);
        }

        // Fallback: use stack trace inspection (for edge cases where AsyncLocal context is lost)
        var detectedType = _stackTraceDetector.DetectModuleType();

        if (detectedType == null)
        {
            throw new InvalidOperationException("Could not detect module type from call stack.");
        }

        return MakeLogger(detectedType);
    }

    public void Dispose()
    {
        _moduleLogger?.Dispose();
    }

    private IModuleLogger MakeLogger(Type module)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(module);

        return _moduleLogger ??= _moduleLoggerContainer.GetLogger(loggerType)
            ?? (IModuleLogger) _serviceProvider.GetRequiredService(loggerType);
    }
}