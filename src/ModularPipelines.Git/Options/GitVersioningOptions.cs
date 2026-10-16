namespace ModularPipelines.Git.Options;

/// <summary>
/// Configures the GitVersion.Tool installation used to compute repository versions.
/// </summary>
public sealed class GitVersioningOptions
{
    /// <summary>
    /// Gets or sets the GitVersion.Tool package version. Defaults to the exact version 6.8.2.
    /// </summary>
    /// <remarks>
    /// Use an exact version to keep version calculation reproducible. The tool and any fallback
    /// configuration are installed in a temporary folder and deleted after calculation.
    /// Existing repository configuration takes precedence over the fallback configuration.
    /// </remarks>
    public string ToolVersion { get; set; } = "6.8.2";
}
