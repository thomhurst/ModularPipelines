using System.Text.Json.Serialization;
using ModularPipelines.Configuration;

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
    /// Gets the configuration for this module's execution behaviors.
    /// </summary>
    ModuleConfiguration Configuration { get; }
}
