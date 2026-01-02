namespace ModularPipelines.GitHub;

/// <summary>
/// Provides constants for GitHub HttpClient names used with IHttpClientFactory.
/// </summary>
internal static class GitHubHttpClientNames
{
    /// <summary>
    /// The name used to register and retrieve the GitHub HttpClient.
    /// </summary>
    public const string GitHub = "ModularPipelines_GitHub";
}
