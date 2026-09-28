using System.Text.Json.Serialization;
using ModularPipelines.Configuration;

namespace ModularPipelines.Modules;

/// <summary>
/// Non-generic view of every module, enabling non-generic operations.
/// </summary>
/// <remarks>
/// This interface cannot be implemented outside ModularPipelines. Derive modules from
/// <see cref="Module{T}"/> or <see cref="SyncModule{T}"/>.
/// </remarks>
public interface IModule
{
    /// <summary>
    /// Gets the result type of this module.
    /// </summary>
    [JsonIgnore]
    Type ResultType { get; }

    /// <summary>
    /// Gets the configuration for this module's execution behaviors.
    /// </summary>
    ModuleConfiguration Configuration { get; }

    /// <summary>
    /// Returns the engine view of this module. The member is internal, so only
    /// <see cref="Module{T}"/> and <see cref="SyncModule{T}"/> can implement <see cref="IModule"/>.
    /// </summary>
    internal IInternalModule AsInternalModule();
}
