using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;

// ReSharper disable ConvertToPrimaryConstructor
namespace ModularPipelines.GitHub;

[ExcludeFromCodeCoverage]
internal class GitHubRepositoryInfo : IGitHubRepositoryInfo, IInitializer
{
  private readonly IServiceProvider _serviceProvider;

  public bool IsInitialized { get; private set; }
  
  public string? Url { get; private set; }
  
  public string? Endpoint { get; private set; }

  public string? Owner { get; private set; }

  public string? RepositoryName { get; private set; }

  public GitHubRepositoryInfo(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }
  
  public async Task InitializeAsync()
  {
    if (IsInitialized)
    {
      return;
    }
    
    await using var scope = _serviceProvider.CreateAsyncScope();
    var git = scope.ServiceProvider.GetRequiredService<IGit>();
    
    var options = new GitRemoteOptions
    {
      Arguments = ["get-url", "origin"],
      ThrowOnNonZeroExitCode = false,
    };
    var remote = await git.Commands.Remote(options);
    var remoteUrl = remote.StandardOutput;
    
    if (string.IsNullOrEmpty(remoteUrl))
    {
      // Will not initialize as git repo is not setup
      return;
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
      // Will not initialize as git repo is not setup
      return;
    }

    Url = remoteUrl;
    Endpoint = endpoint;
    Owner = match.Groups["owner"].Value;
    RepositoryName = match.Groups["name"].Value;

    IsInitialized = true;
  }
}