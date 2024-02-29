using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Initialization.Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;
using Octokit;

// ReSharper disable ConvertToPrimaryConstructor
namespace ModularPipelines.GitHub;

[ExcludeFromCodeCoverage]
internal class GitHubRepositoryInfo : IGitHubRepositoryInfo, IInitializer
{
  private readonly IGit _git;

  public string Url { get; private set; } = string.Empty;
  
  public string Endpoint { get; private set; } = string.Empty;

  public string Owner { get; private set; } = string.Empty;

  public string RepositoryName { get; private set; } = string.Empty;
  
  public AccountType AccountType { get; private set; } = default!;

  public GitHubRepositoryInfo(IGit git)
  {
    _git = git;
  }
  
  public async Task InitializeAsync()
  {
    var options = new GitRemoteOptions
    {
      Arguments = ["get-url", "origin"],
    };
    var remote = await _git.Commands.Remote(options);
    var remoteUrl = remote.StandardOutput;
    
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

    Url = remoteUrl;
    Endpoint = endpoint;
    Owner = match.Groups["owner"].Value;
    RepositoryName = match.Groups["name"].Value;
  }
}