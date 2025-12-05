using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Attributes;

/// <summary>
/// Defines the CLI command structure for an options class.
/// The tool name followed by any subcommands.
/// </summary>
/// <example>
/// <code>
/// [CliCommand("helm", "install")]
/// public record HelmInstallOptions : HelmOptions { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class CliCommandAttribute : Attribute
{
    /// <summary>
    /// Gets the tool name (e.g., "helm", "kubectl", "docker").
    /// </summary>
    public string Tool { get; }

    /// <summary>
    /// Gets the subcommands that follow the tool name.
    /// </summary>
    public string[] SubCommands { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="CliCommandAttribute"/> class.
    /// Initializes a new instance of the <see cref="CliCommandAttribute"/> class.
    /// </summary>
    /// <param name="tool">The CLI tool name.</param>
    /// <param name="subCommands">The subcommands following the tool name.</param>
    public CliCommandAttribute(string tool, params string[] subCommands)
    {
        Tool = tool;
        SubCommands = subCommands;
    }

    /// <summary>
    /// Gets all command parts including the tool and subcommands.
    /// </summary>
    /// <returns></returns>
    public string[] GetAllParts() => [Tool, .. SubCommands];
}
