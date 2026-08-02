using ModularPipelines.Context;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context.Domains.Shell;
using Moq;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Git.UnitTests;

public class GitInformationTests : TestBase
{
    [Test]
    public async Task Repository_Info_Is_Cached()
    {
        var context = await GetService<IPipelineContext>();
        var gitInformation = context.Git().Information;
        var first = await gitInformation.GetInfoAsync();
        var second = await gitInformation.GetInfoAsync();

        using (Assert.Multiple())
        {
            await Assert.That(first).IsNotNull();
            await Assert.That(ReferenceEquals(first, second)).IsTrue();
        }
    }

    [Test]
    public async Task Resolving_Service_Does_Not_Run_Git_Commands()
    {
        var command = new Mock<ICommandContext>();
        var result = await GetService<IGitInformation>((_, services) =>
            services.AddSingleton<ICommandContext>(command.Object));

        command.VerifyNoOtherCalls();
        await result.Host.DisposeAsync();
    }

    [Test]
    public async Task Unavailable_Git_Returns_Null()
    {
        var command = new Mock<ICommandContext>();
        command.Setup(x => x.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("git unavailable"));
        var result = await GetService<IGitInformation>((_, services) =>
            services.AddSingleton<ICommandContext>(command.Object));

        await Assert.That(await result.T.GetInfoAsync()).IsNull();
        await result.Host.DisposeAsync();
    }

    [Test]
    public async Task Cancelled_Load_Is_Not_Cached()
    {
        var command = new Mock<ICommandContext>();
        var observedToken = default(CancellationToken);
        command.Setup(x => x.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Returns<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (_, _, cancellationToken) =>
                {
                    observedToken = cancellationToken;
                    return Task.FromException<CommandResult>(new OperationCanceledException(cancellationToken));
                });
        var result = await GetService<IGitInformation>((_, services) =>
            services.AddSingleton<ICommandContext>(command.Object));
        using var cancellationTokenSource = new CancellationTokenSource();

        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await result.T.GetInfoAsync(cancellationTokenSource.Token));
        await Assert.That(observedToken).IsEqualTo(cancellationTokenSource.Token);

        command.Setup(x => x.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("git unavailable"));

        await Assert.That(await result.T.GetInfoAsync()).IsNull();
        await result.Host.DisposeAsync();
    }

    [Test]
    public async Task Commits_Propagates_Cancellation_Token()
    {
        var observedToken = default(CancellationToken);
        var runner = new Mock<IGitCommandRunner>();
        runner.Setup(x => x.RunCommandsOrNull(
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string?[]>()))
            .Returns<CommandExecutionOptions?, CancellationToken, string?[]>(
                (_, cancellationToken, _) =>
                {
                    observedToken = cancellationToken;
                    return Task.FromResult<string?>(null);
                });
        var result = await GetService<IGitInformation>((_, services) =>
            services.AddSingleton(runner.Object));
        using var cancellationTokenSource = new CancellationTokenSource();

        await foreach (var _ in result.T.Commits(cancellationToken: cancellationTokenSource.Token))
        {
        }

        await Assert.That(observedToken).IsEqualTo(cancellationTokenSource.Token);
        await result.Host.DisposeAsync();
    }
}
