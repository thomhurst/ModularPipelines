using ModularPipelines.Distributed.Abstractions;

namespace ModularPipelines.Distributed.Attributes;

/// <summary>
/// Specifies that a module requires execution on a specific operating system.
/// The distributed scheduler will only assign this module to workers with a matching OS.
/// </summary>
/// <example>
/// <code>
/// [RequiresOs(OS.Linux)]
/// public class DockerBuildModule : Module&lt;CommandResult&gt;
/// {
///     // This module will only run on Linux workers
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RequiresOsAttribute : ModuleRequirementAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequiresOsAttribute"/> class.
    /// </summary>
    /// <param name="operatingSystem">The required operating system.</param>
    public RequiresOsAttribute(OS operatingSystem)
    {
        OperatingSystem = operatingSystem;
    }

    /// <summary>
    /// Gets the required operating system.
    /// </summary>
    public OS OperatingSystem { get; }
}
