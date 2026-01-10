namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies the CLI tool name for an options hierarchy.
/// Apply to the base options class for a tool (e.g., GitOptions).
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class CliToolAttribute : Attribute
{
    /// <summary>
    /// Gets the CLI tool name (e.g., "git", "docker", "dotnet").
    /// </summary>
    public string Tool { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliToolAttribute"/> class.
    /// </summary>
    /// <param name="tool">The CLI tool name.</param>
    public CliToolAttribute(string tool)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tool);
        Tool = tool;
    }
}
