using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

/// <summary>
/// Defines the execution priority of a module.
/// Higher priority modules are scheduled before lower priority modules when multiple modules are ready to execute.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ModulePriority>))]
public enum ModulePriority
{
    /// <summary>
    /// Low priority. Module will be scheduled after Normal, High, and Critical priority modules.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Normal priority. This is the default priority for modules.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// High priority. Module will be scheduled before Normal and Low priority modules.
    /// </summary>
    High = 2,

    /// <summary>
    /// Critical priority. Module will be scheduled first when ready.
    /// Use for modules on the critical path that block many dependents.
    /// </summary>
    Critical = 3,
}
