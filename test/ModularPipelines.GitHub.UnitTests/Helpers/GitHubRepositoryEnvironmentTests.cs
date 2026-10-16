using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;
using ModularPipelines.GitHub;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.GitHub.UnitTests.Helpers;

public class GitHubRepositoryEnvironmentTests
{
    [Test]
    [Arguments("https://github.example.com/remote/project.git", "remote", "project")]
    [Arguments("git@github.example.com:remote/project.git", "remote", "project")]
    [Arguments("", "fallback", "repository")]
    [Arguments("https://gitlab.com/other/project.git", "fallback", "repository")]
    public async Task Initializes_From_Enterprise_Origin_Or_Environment(string remoteUrl, string owner, string name)
    {
        using var services = CreateServices(remoteUrl);
        var info = new GitHubRepositoryInfo(services, NullLogger<GitHubRepositoryInfo>.Instance);

        await info.InitializeAsync();

        using (Assert.Multiple())
        {
            await Assert.That(info.IsInitialized).IsTrue();
            await Assert.That(info.Owner).IsEqualTo(owner);
            await Assert.That(info.RepositoryName).IsEqualTo(name);
            await Assert.That(info.Endpoint).IsEqualTo("github");
            await Assert.That(info.Url).IsEqualTo(string.IsNullOrEmpty(remoteUrl) || remoteUrl.Contains("gitlab.com")
                ? "https://github.example.com/fallback/repository"
                : remoteUrl);
        }
    }

    [Test]
    public async Task Initializes_From_Environment_When_Git_Throws()
    {
        using var services = CreateServices(null);
        var info = new GitHubRepositoryInfo(services, NullLogger<GitHubRepositoryInfo>.Instance);

        await info.InitializeAsync();

        await Assert.That(info.IsInitialized).IsTrue();
        await Assert.That(info.Owner).IsEqualTo("fallback");
    }

    [Test]
    public async Task Environment_Fallback_Defaults_To_GitHub_Com()
    {
        using var services = CreateServices("", serverUrl: null);
        var info = new GitHubRepositoryInfo(services, NullLogger<GitHubRepositoryInfo>.Instance);

        await info.InitializeAsync();

        await Assert.That(info.IsInitialized).IsTrue();
        await Assert.That(info.Url).IsEqualTo("https://github.com/fallback/repository");
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("owner")]
    [Arguments("owner/repository/extra")]
    public async Task Missing_Or_Invalid_Environment_Repository_Does_Not_Initialize(string? repository)
    {
        using var services = CreateServices("", repository);
        var info = new GitHubRepositoryInfo(services, NullLogger<GitHubRepositoryInfo>.Instance);

        await info.InitializeAsync();

        await Assert.That(info.IsInitialized).IsFalse();
    }

    private static ServiceProvider CreateServices(
        string? remoteUrl,
        string? repository = "fallback/repository",
        string? serverUrl = "https://github.example.com/")
    {
        var git = new Mock<IGit> { DefaultValue = DefaultValue.Mock };
        var remote = git.Setup(x => x.Commands.Remotes.RemoteAsync(
            It.IsAny<GitRemoteOptions>(), It.IsAny<CommandExecutionOptions>(), It.IsAny<CancellationToken>()));
        if (remoteUrl is null)
        {
            remote.ThrowsAsync(new InvalidOperationException("Git unavailable"));
        }
        else
        {
            remote.ReturnsAsync(string.IsNullOrEmpty(remoteUrl)
                ? CommandResult.Fail(2, standardError: "No origin remote")
                : CommandResult.Ok(remoteUrl));
        }

        var environment = new Mock<IGitHubEnvironmentVariables>();
        environment.SetupGet(x => x.ServerUrl).Returns(serverUrl);
        environment.SetupGet(x => x.Repository).Returns(repository);
        return new ServiceCollection()
            .AddLogging()
            .AddSingleton(git.Object)
            .AddScoped(_ => environment.Object)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
    }
}
