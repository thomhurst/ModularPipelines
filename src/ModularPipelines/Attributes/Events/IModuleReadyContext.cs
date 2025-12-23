using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Context provided to module ready event receivers.
/// Provides access to module information and timing data about when dependencies were satisfied.
/// </summary>
public interface IModuleReadyContext
{
    /// <summary>
    /// Gets the type of the module.
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Gets the module instance.
    /// </summary>
    IModule ModuleInstance { get; }

    /// <summary>
    /// Gets the attributes declared on the module.
    /// </summary>
    IReadOnlyList<Attribute> ModuleAttributes { get; }

    /// <summary>
    /// Gets the time when all dependencies were satisfied and the module became ready.
    /// </summary>
    DateTimeOffset ReadyTime { get; }

    /// <summary>
    /// Gets the time the module spent waiting for dependencies since pipeline start.
    /// </summary>
    TimeSpan TimeWaitingForDependencies { get; }

    /// <summary>
    /// Gets the pipeline context.
    /// </summary>
    IPipelineContext PipelineContext { get; }

    /// <summary>
    /// Gets the cancellation token.
    /// </summary>
    CancellationToken CancellationToken { get; }

    /// <summary>
    /// Resolves a service from the DI container.
    /// </summary>
    T GetService<T>()
        where T : notnull;

    /// <summary>
    /// Resolves a service from the DI container, or null if not registered.
    /// </summary>
    T? GetServiceOrDefault<T>()
        where T : class;

    /// <summary>
    /// Gets metadata that was set during registration.
    /// </summary>
    T? GetMetadata<T>(string key);
}
