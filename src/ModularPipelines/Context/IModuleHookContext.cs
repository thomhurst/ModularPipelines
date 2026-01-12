using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Extended context for module-level hooks, providing module information and control flow.
/// </summary>
public interface IModuleHookContext : IPipelineHookContext
{
    /// <summary>
    /// Gets the module instance.
    /// </summary>
    IModule Module { get; }

    /// <summary>
    /// Gets the type of the module.
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string ModuleName { get; }

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
    /// Gets the module result. Null in Ready/Start hooks, populated in End/Failure/Skipped hooks.
    /// </summary>
    IModuleResult? Result { get; }

    /// <summary>
    /// Requests that the module be retried.
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

    /// <summary>
    /// Gets metadata that was set during registration.
    /// </summary>
    T? GetMetadata<T>(string key);
}
