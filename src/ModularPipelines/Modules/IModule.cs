using System.Text.Json.Serialization;
using ModularPipelines.Configuration;
using ModularPipelines.Models;

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
    ModuleRunType ModuleRunType => Configuration.AlwaysRun ? ModuleRunType.AlwaysRun : ModuleRunType.OnSuccessfulDependencies;

    /// <summary>
    /// Gets the configuration for this module's execution behaviors.
    /// </summary>
    ModuleConfiguration Configuration { get; }
}