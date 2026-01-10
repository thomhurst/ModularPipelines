using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

/// <summary>
/// Options for the 'git remote set-url' command.
/// Changes URLs for the remote.
/// </summary>
/// <example>
/// <code>
/// var options = new GitRemoteSetUrlOptions
/// {
///     Remote = "origin",
///     NewUrl = "https://github.com/user/repo.git"
/// };
/// </code>
/// </example>
[CliSubCommand("remote", "set-url")]
[ExcludeFromCodeCoverage]
public record GitRemoteSetUrlOptions : GitRemoteOptions
{
    /// <summary>
    /// Add URL instead of changing existing URLs.
    /// </summary>
    [CliFlag("--add")]
    public virtual bool? Add { get; set; }

    /// <summary>
    /// Delete all URLs matching regex from remote.
    /// </summary>
    [CliFlag("--delete")]
    public virtual bool? Delete { get; set; }

    /// <summary>
    /// Manipulate push URLs instead of fetch URLs.
    /// </summary>
    [CliFlag("--push")]
    public virtual bool? Push { get; set; }

    /// <summary>
    /// The name of the remote to configure (e.g., "origin").
    /// </summary>
    [CliArgument(0)]
    public required string Remote { get; init; }

    /// <summary>
    /// The new URL to set for the remote.
    /// </summary>
    [CliArgument(1)]
    public required string NewUrl { get; init; }

    /// <summary>
    /// Optional old URL pattern to match when changing URLs.
    /// </summary>
    [CliArgument(2)]
    public string? OldUrl { get; set; }
}
