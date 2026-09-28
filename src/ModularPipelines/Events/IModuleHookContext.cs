using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Extended context for module-level hooks, providing read-only module information.
/// </summary>
/// <remarks>
/// This interface extends <see cref="IPipelineContext"/> with module-specific information
/// for use in module hooks (Ready, Start, End, Failure, Skipped). Hooks observe modules:
/// retries, skips and failure handling are configured through module configuration and conditions.
/// </remarks>
public interface IModuleHookContext : IPipelineContext
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
    /// Gets metadata that was set during registration.
    /// </summary>
    T? GetMetadata<T>(string key);
}
