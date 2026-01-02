using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

public record GitHubSettings
{
    [SecretValue]
    public string? StandardToken { get; init; }

    [SecretValue]
    public string? AdminToken { get; init; }

    /// <summary>
    /// Git user name for commits. Defaults to "github-actions[bot]" if not specified.
    /// </summary>
    public string GitUserName { get; init; } = "github-actions[bot]";

    /// <summary>
    /// Git user email for commits. Defaults to "github-actions[bot]@users.noreply.github.com" if not specified.
    /// </summary>
    public string GitUserEmail { get; init; } = "github-actions[bot]@users.noreply.github.com";

    /// <summary>
    /// GitHub repository owner/organization. Defaults to "thomhurst" if not specified.
    /// </summary>
    public string RepositoryOwner { get; init; } = "thomhurst";

    /// <summary>
    /// GitHub repository name. Defaults to "ModularPipelines" if not specified.
    /// </summary>
    public string RepositoryName { get; init; } = "ModularPipelines";
}