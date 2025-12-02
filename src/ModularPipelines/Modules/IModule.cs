using System.Text.Json.Serialization;
using ModularPipelines.Models;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Modules;

/// <summary>
/// Marker interface for all modules, enabling non-generic operations.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the result type of this module.
    /// </summary>
    [JsonIgnore]
    Type ResultType { get; }

    /// <summary>
    /// Gets whether this module should always run, even when the pipeline fails.
    /// </summary>
    ModuleRunType ModuleRunType => this is IAlwaysRun ? ModuleRunType.AlwaysRun : ModuleRunType.OnSuccessfulDependencies;
}