using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;
using ModularPipelines.GitHub;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.GitHub.UnitTests.Helpers;

public class GitHubRepositoryInfoInitializationTests
{
    [Test]
    [Arguments("https://x-access-token:ghs_test@github.com/owner/repository.git\r\n", "https://github.com/owner/repository.git")]
    [Arguments("https://ghs_test@github.com/owner/repository.git", "https://github.com/owner/repository.git")]
    [Arguments("https://user%40example.com:token%3Asecret@github.com/owner/repository.git", "https://github.com/owner/repository.git")]
    [Arguments("  https://github.com/owner/repository.git\n", "https://github.com/owner/repository.git")]
    [Arguments("https://github.com/owner/repository", "https://github.com/owner/repository")]
    [Arguments("  git@github.com:owner/repository.git\r\n", "git@github.com:owner/repository.git")]
    [Arguments("ssh://git@github.com/owner/repository.git", "ssh://git@github.com/owner/repository.git")]
    public async Task Url_Is_Trimmed_And_Does_Not_Expose_Https_Credentials(string remoteUrl, string expectedUrl)
    {
        var remotes = new Mock<IGitRemoteCommands>();
        remotes.Setup(x => x.RemoteAsync(It.IsAny<GitRemoteOptions>(), It.IsAny<CommandExecutionOptions?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResult.Ok(remoteUrl));
        await using var services = CreateServices(remotes.Object);
        var info = new GitHubRepositoryInfo(services, NullLogger<GitHubRepositoryInfo>.Instance);

        await info.InitializeAsync();

        using (Assert.Multiple())
        {
            await Assert.That(info.IsInitialized).IsTrue();
            await Assert.That(info.Url).IsEqualTo(expectedUrl);
            await Assert.That(info.Owner).IsEqualTo("owner");
            await Assert.That(info.RepositoryName).IsEqualTo("repository");
        }
    }

    [Test]
    public async Task Initialization_Failure_Is_Logged_As_A_Warning()
    {
        var exception = new InvalidOperationException("git unavailable");
        var remotes = new Mock<IGitRemoteCommands>();
        remotes.Setup(x => x.RemoteAsync(It.IsAny<GitRemoteOptions>(), It.IsAny<CommandExecutionOptions?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        await using var services = CreateServices(remotes.Object);
        var logger = new RecordingLogger();
        var info = new GitHubRepositoryInfo(services, logger);

        await info.InitializeAsync();

        await Assert.That(info.IsInitialized).IsFalse();
        await Assert.That(info.Url).IsNull();
        await Assert.That(logger.Entries).Count().IsEqualTo(1);
        await Assert.That(logger.Entries[0].Level).IsEqualTo(LogLevel.Warning);
        await Assert.That(logger.Entries[0].Exception).IsSameReferenceAs(exception);
        await Assert.That(logger.Entries[0].Message).IsEqualTo("Error when detecting GitHub git repository");
    }

    private static ServiceProvider CreateServices(IGitRemoteCommands remotes)
    {
        var git = Mock.Of<IGit>(x => x.Commands.Remotes == remotes);
        return new ServiceCollection()
            .AddLogging()
            .AddSingleton(git)
            .AddSingleton(Mock.Of<IGitHubEnvironmentVariables>())
            .BuildServiceProvider();
    }

    private sealed class RecordingLogger : ILogger<GitHubRepositoryInfo>
    {
        public List<(LogLevel Level, Exception? Exception, string Message)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Entries.Add((logLevel, exception, formatter(state, exception)));
        }
    }
}
