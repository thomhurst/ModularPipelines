using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

/// <summary>
/// Options for the 'git remote get-url' command.
/// Retrieves the URLs for a remote.
/// </summary>
/// <example>
/// <code>
/// var options = new GitRemoteGetUrlOptions
/// {
///     Remote = "origin"
/// };
/// </code>
/// </example>
[CliSubCommand("remote", "get-url")]
[ExcludeFromCodeCoverage]
public record GitRemoteGetUrlOptions : GitRemoteOptions
{
    /// <summary>
    /// Query push URLs rather than fetch URLs.
    /// </summary>
    [CliFlag("--push")]
    public virtual bool? Push { get; set; }

    /// <summary>
    /// Return all URLs instead of the first one.
    /// </summary>
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    /// <summary>
    /// The name of the remote to query (e.g., "origin").
    /// </summary>
    [CliArgument(0)]
    public required string Remote { get; init; }
}
