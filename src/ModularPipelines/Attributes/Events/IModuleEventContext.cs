using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Context provided to runtime event receivers (start, end, failure, skipped).
/// Provides access to module information, services, and control flow.
/// </summary>
public interface IModuleEventContext
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
    /// Gets the time when the module started executing.
    /// </summary>
    DateTimeOffset StartTime { get; }

    /// <summary>
    /// Gets the elapsed time since the module started.
    /// </summary>
    TimeSpan ElapsedTime { get; }

    /// <summary>
    /// Gets the current status of the module.
    /// </summary>
    Status Status { get; }

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
    /// <returns></returns>
    T GetService<T>()
        where T : notnull;

    /// <summary>
    /// Resolves a service from the DI container, or null if not registered.
    /// </summary>
    /// <returns></returns>
    T? GetServiceOrDefault<T>()
        where T : class;

    /// <summary>
    /// Gets metadata that was set during registration.
    /// </summary>
    /// <returns></returns>
    T? GetMetadata<T>(string key);

    /// <summary>
    /// Requests that the module be retried.
    /// Only works if the module supports retry.
    /// </summary>
    void RequestRetry(TimeSpan? delay = null);

    /// <summary>
    /// Marks dependent modules to be skipped.
    /// </summary>
    void SkipDependentModules(string reason);

    /// <summary>
    /// Requests that the pipeline fail after current modules complete.
    /// </summary>
    void FailPipeline(string reason);
}
