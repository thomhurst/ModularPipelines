using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies the sub-commands for a command-line tool where the root tool
/// is defined by a base options class (e.g., inheriting from CommandLineToolOptions).
/// </summary>
/// <example>
/// <code>
/// // Tool is "git" from GitOptions base class
/// [CliSubCommand("rev-parse")]
/// public record GitRevParseOptions : GitOptions { }
///
/// // Multiple subcommands
/// [CliSubCommand("remote", "add")]
/// public record GitRemoteAddOptions : GitOptions { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class CliSubCommandAttribute : Attribute
{
    /// <summary>
    /// Gets the subcommands that follow the tool name defined in the base class.
    /// </summary>
    public string[] SubCommands { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="CliSubCommandAttribute"/> class.
    /// Initializes a new instance of the <see cref="CliSubCommandAttribute"/> class.
    /// </summary>
    /// <param name="subCommands">The subcommands following the tool name from the base class.</param>
    public CliSubCommandAttribute(params string[] subCommands)
    {
        SubCommands = subCommands ?? Array.Empty<string>();
    }
}
