namespace ModularPipelines.GitHub;

public interface IGitHubRepositoryInfo
{
    /// <summary>
    /// Gets a value indicating whether <see cref="IGitHubRepositoryInfo"/> has been initialized.
    /// </summary>
    /// <remarks>
    /// If neither the origin remote nor the GitHub Actions environment identifies a repository,
    /// the repository information remains uninitialized.
    /// </remarks>
    bool IsInitialized { get; }

    /// <summary>
    /// Gets the trimmed URL of a GitHub repository without HTTPS credentials.
    /// </summary>
    string? Url { get; }

    /// <summary>
    /// Gets an endpoint for accessing a remote repository.
    /// </summary>
    string? Endpoint { get; }

    /// <summary>
    /// Gets the owner of a repository.
    /// </summary>
    string? Owner { get; }

    /// <summary>
    /// Gets the name of the repository.
    /// </summary>
    /// <value>The name of the repository.</value>
    string? RepositoryName { get; }

    /// <summary>
    /// Gets the identifier of a GitHub repository.
    /// </summary>
    string? Identifier => $"{Owner}/{RepositoryName}";

    /// <summary>
    /// Gets a value indicating whether the endpoint is GitHub.
    /// </summary>
    /// <value><c>true</c> if the endpoint is GitHub; otherwise, <c>false</c>.</value>
    bool IsGitHub => Endpoint?.Equals("github", StringComparison.OrdinalIgnoreCase) == true;
}
