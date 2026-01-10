using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

/// <summary>
/// Settings for NuGet package publishing.
/// </summary>
public record NuGetSettings
{
    /// <summary>
    /// API key for authenticating with the NuGet feed. Required for publishing packages.
    /// </summary>
    [SecretValue]
    public string? ApiKey { get; init; }

    /// <summary>
    /// The NuGet feed URL to publish packages to. Defaults to the official NuGet.org feed.
    /// </summary>
    public string FeedUrl { get; init; } = "https://api.nuget.org/v3/index.json";
}