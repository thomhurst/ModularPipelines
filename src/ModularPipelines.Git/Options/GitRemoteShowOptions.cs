using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

/// <summary>
/// Options for the 'git remote show' command.
/// Gives some information about the remote.
/// </summary>
/// <example>
/// <code>
/// var options = new GitRemoteShowOptions
/// {
///     Remote = "origin"
/// };
/// </code>
/// </example>
[CliSubCommand("remote", "show")]
[ExcludeFromCodeCoverage]
public record GitRemoteShowOptions : GitRemoteOptions
{
    /// <summary>
    /// Do not query remote heads (useful when offline).
    /// </summary>
    [CliFlag("-n")]
    public virtual bool? NoQuery { get; set; }

    /// <summary>
    /// The name of the remote to show information about (e.g., "origin").
    /// </summary>
    [CliArgument(0)]
    public required string Remote { get; init; }
}
