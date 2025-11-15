using ModularPipelines.Attributes;
using ModularPipelines.Http;

namespace ModularPipelines.GitHub.Options;

public record GitHubOptions
{
    [SecretValue]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the HTTP logging level for GitHub API requests.
    /// If not set, defaults to <see cref="ModularPipelines.Options.PipelineOptions.DefaultHttpLogging"/>.
    /// </summary>
    public HttpLoggingType? HttpLogging { get; set; }
}