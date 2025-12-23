using ModularPipelines.Enums;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies the execution priority of a module.
/// Higher priority modules are scheduled before lower priority modules when multiple modules are ready to execute.
/// Priority does not override dependencies - a high priority module will still wait for its dependencies.
/// </summary>
/// <example>
/// <code>
/// [Priority(ModulePriority.Critical)]
/// public class CriticalBuildModule : Module&lt;BuildResult&gt;
/// {
///     // This module will be scheduled first when ready
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class PriorityAttribute : Attribute
{
    /// <summary>
    /// Gets the priority level of the module.
    /// </summary>
    public ModulePriority Priority { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityAttribute"/> class.
    /// </summary>
    /// <param name="priority">The priority level for the module.</param>
    public PriorityAttribute(ModulePriority priority)
    {
        Priority = priority;
    }
}
