namespace ModularPipelines.Build.Settings;

/// <summary>
/// General settings for the build pipeline.
/// </summary>
public record PipelineSettings
{
    /// <summary>
    /// The target framework for running tests. Defaults to "net10.0".
    /// </summary>
    public string TestFramework { get; init; } = "net10.0";

    /// <summary>
    /// The product header value used when creating GitHub API clients. Defaults to "ModularPipelinesBuild".
    /// </summary>
    public string GitHubProductHeader { get; init; } = "ModularPipelinesBuild";

    /// <summary>
    /// The default number of retry attempts for modules. Defaults to 3.
    /// </summary>
    public int DefaultRetryCount { get; init; } = 3;
}
