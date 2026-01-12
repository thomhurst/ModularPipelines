using Octokit;

namespace ModularPipelines.GitHub;

public interface IGitHub
{
    IGitHubClient Client { get; }

    IGitHubRepositoryInfo RepositoryInfo { get; }

    IGitHubEnvironmentVariables EnvironmentVariables { get; }

    /// <summary>
    /// Writes a message to the console output.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void WriteLine(string message);

    /// <summary>
    /// Begins a collapsible section in CI output (GitHub Actions group).
    /// </summary>
    /// <param name="name">The section name.</param>
    /// <returns>A disposable that ends the section when disposed.</returns>
    IDisposable BeginSection(string name);
}
