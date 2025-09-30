namespace ModularPipelines.Distributed.Attributes;

/// <summary>
/// Specifies that a module requires a specific tool to be installed on the worker.
/// The distributed scheduler will only assign this module to workers that have the tool in their InstalledTools list.
/// </summary>
/// <example>
/// <code>
/// [RequiresTool("docker")]
/// [RequiresTool("git")]
/// public class DockerBuildModule : Module&lt;CommandResult&gt;
/// {
///     // This module will only run on workers with both docker and git installed
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RequiresToolAttribute : ModuleRequirementAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequiresToolAttribute"/> class.
    /// </summary>
    /// <param name="toolName">The name of the required tool (e.g., "docker", "git", "node").</param>
    public RequiresToolAttribute(string toolName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(toolName);
        ToolName = toolName;
    }

    /// <summary>
    /// Gets the name of the required tool.
    /// </summary>
    public string ToolName { get; }
}
