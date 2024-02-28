using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ModularPipelines.Git;

namespace ModularPipelines.GitHub;

[ExcludeFromCodeCoverage]
internal class GitHubRepositoryInfo : IGitHubRepositoryInfo
{
  public string Endpoint { get; }

  public string Owner { get; }

  public string RepositoryName { get; }

  public GitHubRepositoryInfo(IGit git)
  {
    var gitDir = Path.Combine(git.RootDirectory, ".git");
    using (var repo = new LibGit2Sharp.Repository(gitDir))
    {
      var remoteUrl = repo.Network.Remotes.FirstOrDefault()?.Url;

      if (string.IsNullOrEmpty(remoteUrl))
      {
        throw new InvalidOperationException("No remote repository configured.");
      }

      // Parse owner and repository name from the remote URL
      var endpoint = "github";
      var sshPattern = $@"git@{endpoint}\.com:(?<owner>.+?)/(?<name>.+?)\.git";
      var httpsPattern = $@"https://{endpoint}\.com/(?<owner>.+?)/(?<name>.+?)\.git";

      var match = Regex.Match(remoteUrl, sshPattern);
      if (!match.Success)
      {
        match = Regex.Match(remoteUrl, httpsPattern);
      }

      if (!match.Success)
      {
        throw new InvalidOperationException(
          $"The remote repository URL is not recognized as a {endpoint} repository URL.");
      }

      Endpoint = endpoint;
      Owner = match.Groups["owner"].Value;
      RepositoryName = match.Groups["name"].Value;
    }
  }
}