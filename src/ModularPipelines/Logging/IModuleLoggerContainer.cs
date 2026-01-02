using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Logging;

/// <summary>
/// Interface for managing the lifecycle of module loggers.
/// </summary>
internal interface IModuleLoggerContainer
{
    /// <summary>
    /// Cache for constructed generic logger types.
    /// MakeGenericType is expensive, so we cache the result per module type.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Type> LoggerTypeCache = new();

    /// <summary>
    /// Gets an existing logger or creates one from the scoped service provider.
    /// </summary>
    /// <param name="moduleType">The module type to get/create a logger for.</param>
    /// <param name="scopedServiceProvider">The scoped service provider to resolve from if not cached.</param>
    /// <returns>The module logger instance.</returns>
    IModuleLogger GetOrCreateLogger(Type moduleType, IServiceProvider scopedServiceProvider)
    {
        var loggerType = LoggerTypeCache.GetOrAdd(
            moduleType,
            static type => typeof(ModuleLogger<>).MakeGenericType(type));

        return GetLogger(loggerType)
            ?? (IModuleLogger)scopedServiceProvider.GetRequiredService(loggerType);
    }

    /// <summary>
    /// Flushes all buffered log events and disposes all registered loggers.
    /// Loggers are processed in order of last log written time.
    /// </summary>
    void FlushAndDisposeAll();

    /// <summary>
    /// Gets an existing logger for the specified type.
    /// </summary>
    /// <param name="type">The logger type to retrieve.</param>
    /// <returns>The logger instance, or null if not found.</returns>
    IModuleLogger? GetLogger(Type type);

    /// <summary>
    /// Adds a logger to the container registry.
    /// </summary>
    /// <param name="logger">The logger to add.</param>
    void AddLogger(ModuleLogger logger);
}