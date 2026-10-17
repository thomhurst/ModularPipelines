using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class GitCommitPagerTests
{
    [Test]
    [Arguments(false, 0, false)]
    [Arguments(true, 0, false)]
    [Arguments(false, 1, false)]
    [Arguments(true, 1, false)]
    [Arguments(false, 0, true)]
    [Arguments(true, 0, true)]
    [Arguments(false, 1, true)]
    [Arguments(true, 1, true)]
    public async Task Page_Failures_Propagate(bool useOptions, int successfulPages, bool timeout)
    {
        Exception failure = timeout
            ? new TimeoutException("git log timed out")
            : new CommandException(CommandResult.Fail(128));
        var page = 0;
        var runner = CreateRunner((_, execution, _) =>
        {
            if (execution?.ThrowOnNonZeroExitCode != true || execution.MaxCapturedOutputLength != 0)
            {
                throw new InvalidOperationException("Pages must capture complete output and throw on nonzero exits.");
            }

            return page++ < successfulPages
                ? Task.FromResult(CommandResult.Ok(string.Concat(Enumerable.Repeat("record\0", 50))))
                : Task.FromException<CommandResult>(failure);
        });
        var mapper = new Mock<IGitCommitMapper>();
        mapper.Setup(x => x.Map("record")).Returns(new GitCommit());
        await using var commits = GitCommitPager.EnumerateAsync(
            runner, mapper.Object, "main", useOptions ? new GitOptions { NoPager = true } : null,
            new CommandExecutionOptions { ThrowOnNonZeroExitCode = false, MaxCapturedOutputLength = 1 },
            CancellationToken.None).GetAsyncEnumerator();

        for (var i = 0; i < successfulPages * 50; i++)
        {
            await Assert.That(await commits.MoveNextAsync()).IsTrue();
        }

        if (timeout)
        {
            await Assert.ThrowsAsync<TimeoutException>(async () => await commits.MoveNextAsync());
        }
        else
        {
            await Assert.ThrowsAsync<CommandException>(async () => await commits.MoveNextAsync());
        }

        await Assert.That(page).IsEqualTo(successfulPages + 1);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Default_History_Failure_With_Existing_Revisions_Propagates(bool useOptions)
    {
        var calls = 0;
        var failure = new CommandException(CommandResult.Fail(128));
        var runner = CreateRunner((_, _, _) => ++calls == 1
            ? Task.FromException<CommandResult>(failure)
            : Task.FromResult(CommandResult.Ok(new string('1', 40))));
        await using var commits = GitCommitPager.EnumerateAsync(
            runner, Mock.Of<IGitCommitMapper>(), null, useOptions ? new GitOptions() : null,
            null, CancellationToken.None).GetAsyncEnumerator();

        await Assert.ThrowsAsync<CommandException>(async () => await commits.MoveNextAsync());
        await Assert.That(calls).IsEqualTo(2);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task Cancellation_Propagates_During_First_And_Later_Pages(bool useOptions, bool laterPage)
    {
        using var cancellation = new CancellationTokenSource();
        var page = 0;
        var runner = CreateRunner((_, _, token) =>
        {
            if (laterPage && page++ == 0)
            {
                return Task.FromResult(CommandResult.Ok(string.Concat(Enumerable.Repeat("record\0", 50))));
            }

            cancellation.Cancel();
            return Task.FromCanceled<CommandResult>(token);
        });
        var mapper = new Mock<IGitCommitMapper>();
        mapper.Setup(x => x.Map("record")).Returns(new GitCommit());
        await using var commits = GitCommitPager.EnumerateAsync(
            runner, mapper.Object, null, useOptions ? new GitOptions() : null,
            null, cancellation.Token).GetAsyncEnumerator();
        if (laterPage)
        {
            for (var i = 0; i < 50; i++)
            {
                await Assert.That(await commits.MoveNextAsync()).IsTrue();
            }
        }

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await commits.MoveNextAsync());
    }

    private static GitCommandRunner CreateRunner(
        Func<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken, Task<CommandResult>> run)
    {
        var shell = new Mock<IShellContext>(MockBehavior.Strict);
        shell.Setup(x => x.RunAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Returns(run);
        var context = new Mock<IPipelineContext>();
        context.SetupGet(x => x.Shell).Returns(shell.Object);
        return new GitCommandRunner(context.Object, Mock.Of<ILogger<GitCommandRunner>>());
    }
}
