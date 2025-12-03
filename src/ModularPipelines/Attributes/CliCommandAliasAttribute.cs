using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Attributes;

/// <summary>
/// Defines an alternative invocation form for a CLI command.
/// Use this when a command can be invoked in multiple ways.
/// </summary>
/// <example>
/// <code>
/// [CliCommand("docker", "container", "run")]
/// [CliCommandAlias("docker", "run")]
/// public record DockerRunOptions : DockerOptions { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class CliCommandAliasAttribute : Attribute
{
    /// <summary>
    /// Gets the command parts for this alias.
    /// </summary>
    public string[] CommandParts { get; }

    /// <summary>
    /// Gets or sets whether this alias is the preferred form when building the command.
    /// When true, this alias will be used instead of the primary <see cref="CliCommandAttribute"/>.
    /// </summary>
    public bool IsPreferred { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandAliasAttribute"/> class.
    /// </summary>
    /// <param name="commandParts">The command parts for this alternative invocation.</param>
    public CliCommandAliasAttribute(params string[] commandParts)
    {
        CommandParts = commandParts;
    }
}
