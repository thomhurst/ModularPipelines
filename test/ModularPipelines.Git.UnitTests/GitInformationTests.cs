using ModularPipelines.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context.Domains;
using ModularPipelines.Context.Domains.Shell;
using Moq;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
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
        var result = await GetService<IGitInformation>(services =>
            services.AddSingleton<ICommandContext>(command.Object));

        command.VerifyNoOtherCalls();
        await result.Pipeline.DisposeAsync();
    }

    [Test]
    public async Task RunCommandsOrNull_Forces_Nonzero_Exit_Detection()
    {
        CommandExecutionOptions? observedOptions = null;
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Returns<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (_, options, _) =>
                {
                    observedOptions = options;
                    return Task.FromResult(CommandResult.Ok());
                });
        var shell = new Mock<IShellContext>();
        shell.SetupGet(context => context.Command).Returns(command.Object);
        var pipelineContext = new Mock<IPipelineContext>();
        pipelineContext.SetupGet(context => context.Shell).Returns(shell.Object);
        var runner = new GitCommandRunner(
            pipelineContext.Object,
            Mock.Of<ILogger<GitCommandRunner>>());

        _ = await runner.RunCommandsOrNull(
            new CommandExecutionOptions { ThrowOnNonZeroExitCode = false },
            "status");

        await Assert.That(observedOptions).IsNotNull();
        await Assert.That(observedOptions!.ThrowOnNonZeroExitCode).IsTrue();
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
        var result = await GetService<IGitInformation>(services =>
            services.AddSingleton<ICommandContext>(command.Object));

        await Assert.That(await result.T.GetInfoAsync()).IsNull();
        await result.Pipeline.DisposeAsync();
    }

    [Test]
    public async Task Default_Branch_Uses_Local_Origin_Head_Without_Remote_Query()
    {
        var command = CreateRepositoryCommand((options, _) => options switch
        {
            GenericCommandLineToolOptions { Tool: "git" } => CommandResult.Ok("origin/main\n"),
            _ => CommandResult.Ok(),
        });
        var result = await GetGitInformation(command);

        var repository = await result.T.GetInfoAsync();

        await Assert.That(repository?.DefaultBranchName).IsEqualTo("main");
        command.Verify(context => context.ExecuteCommandLineToolAsync(
            It.IsAny<GitRemoteShowOptions>(),
            It.IsAny<CommandExecutionOptions?>(),
            It.IsAny<CancellationToken>()), Times.Never());
        await result.Pipeline.DisposeAsync();
    }

    [Test]
    public async Task Default_Branch_Bounds_Remote_Fallback_When_Local_Head_Is_Unavailable()
    {
        CommandExecutionOptions? remoteExecutionOptions = null;
        var command = CreateRepositoryCommand((options, executionOptions) => options switch
        {
            GenericCommandLineToolOptions { Tool: "git" } => CommandResult.Ok(),
            GitRemoteShowOptions => CaptureRemoteOptions(executionOptions, out remoteExecutionOptions),
            _ => CommandResult.Ok(),
        });
        var result = await GetGitInformation(command);

        var repository = await result.T.GetInfoAsync();

        await Assert.That(repository?.DefaultBranchName).IsEqualTo("trunk");
        await Assert.That(remoteExecutionOptions?.ThrowOnNonZeroExitCode).IsFalse();
        await Assert.That(remoteExecutionOptions?.ExecutionTimeout).IsEqualTo(TimeSpan.FromSeconds(10));
        await result.Pipeline.DisposeAsync();
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
        var result = await GetService<IGitInformation>(services =>
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
        await result.Pipeline.DisposeAsync();
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
        var result = await GetService<IGitInformation>(services =>
            services.AddSingleton(runner.Object));
        using var cancellationTokenSource = new CancellationTokenSource();

        await foreach (var _ in result.T.Commits(cancellationToken: cancellationTokenSource.Token))
        {
        }

        await Assert.That(observedToken).IsEqualTo(cancellationTokenSource.Token);
        await result.Pipeline.DisposeAsync();
    }

    [Test]
    public async Task Commits_Reads_Multiple_Records_With_One_Git_Process()
    {
        CommandExecutionOptions? observedOptions = null;
        string?[]? observedCommands = null;
        var runner = new Mock<IGitCommandRunner>();
        runner.Setup(x => x.RunCommandsOrNull(
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string?[]>()))
            .Returns<CommandExecutionOptions?, CancellationToken, string?[]>((options, _, commands) =>
            {
                observedOptions = options;
                observedCommands = commands;
                return Task.FromResult<string?>(
                    $"{CreateCommitOutput("second commit", '2')}\0{CreateCommitOutput("first commit", '1')}\0");
            });
        var result = await GetService<IGitInformation>(services =>
            services.AddSingleton(runner.Object));
        var commits = new List<GitCommit>();

        await foreach (var commit in result.T.Commits())
        {
            commits.Add(commit);
        }

        using (Assert.Multiple())
        {
            await Assert.That(commits).Count().IsEqualTo(2);
            await Assert.That(commits[0].Message?.Subject).IsEqualTo("second commit");
            await Assert.That(commits[1].Message?.Subject).IsEqualTo("first commit");
            await Assert.That(observedOptions).IsNotNull();
            await Assert.That(observedCommands!).Contains("--skip=0");
            await Assert.That(observedCommands!).Contains("--max-count=50");
            await Assert.That(observedOptions!.MaxCapturedOutputLength).IsLessThanOrEqualTo(0);
            await Assert.That(observedCommands!.Single(command =>
                    command?.StartsWith("--format=", StringComparison.Ordinal) == true))
                .EndsWith("%x00");
        }

        runner.Verify(x => x.RunCommandsOrNull(
            It.IsAny<CommandExecutionOptions?>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<string?[]>()), Times.Once());
        await result.Pipeline.DisposeAsync();
    }

    [Test]
    public async Task Git_Command_Runner_Preserves_Output_Capture_Limit()
    {
        CommandExecutionOptions? observedOptions = null;
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Returns<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (_, options, _) =>
                {
                    observedOptions = options;
                    return Task.FromResult(CommandResult.Ok("output"));
                });
        var shell = new Mock<IShellContext>();
        shell.SetupGet(context => context.Command).Returns(command.Object);
        var pipelineContext = new Mock<IPipelineContext>();
        pipelineContext.SetupGet(context => context.Shell).Returns(shell.Object);
        var runner = new GitCommandRunner(
            pipelineContext.Object,
            Mock.Of<ILogger<GitCommandRunner>>());

        var output = await runner.RunCommands(
            new CommandExecutionOptions { MaxCapturedOutputLength = 0 },
            "status");

        using (Assert.Multiple())
        {
            await Assert.That(output).IsEqualTo("output");
            await Assert.That(observedOptions).IsNotNull();
            await Assert.That(observedOptions!.MaxCapturedOutputLength).IsLessThanOrEqualTo(0);
        }
    }

    [Test]
    public async Task Commits_Throws_When_Cancelled_Between_Records()
    {
        var runner = new Mock<IGitCommandRunner>();
        runner.Setup(x => x.RunCommandsOrNull(
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string?[]>()))
            .ReturnsAsync($"{CreateCommitOutput("second commit", '2')}\0{CreateCommitOutput("first commit", '1')}\0");
        var result = await GetService<IGitInformation>(services =>
            services.AddSingleton(runner.Object));
        using var cancellationTokenSource = new CancellationTokenSource();
        await using var commits = result.T.Commits(
            cancellationToken: cancellationTokenSource.Token).GetAsyncEnumerator();

        await Assert.That(await commits.MoveNextAsync()).IsTrue();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await commits.MoveNextAsync());
        await result.Pipeline.DisposeAsync();
    }

    [Test]
    public async Task Previous_Commit_Uses_First_Parent()
    {
        CommandExecutionOptions? observedOptions = null;
        string?[]? observedCommands = null;
        var command = CreateRepositoryCommand((_, _) => CommandResult.Ok());
        var runner = new Mock<IGitCommandRunner>();
        runner.Setup(x => x.RunCommandsOrNull(
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string?[]>()))
            .Returns<CommandExecutionOptions?, CancellationToken, string?[]>((options, _, commands) =>
            {
                observedOptions = options;
                observedCommands = commands;
                return Task.FromResult<string?>(CreateCommitOutput("previous commit", '1'));
            });
        var result = await GetService<IGitInformation>(services =>
        {
            services.AddSingleton<ICommandContext>(command.Object);
            services.AddSingleton(runner.Object);
        });

        var repository = await result.T.GetInfoAsync();

        using (Assert.Multiple())
        {
            await Assert.That(repository?.PreviousCommit?.Message?.Subject).IsEqualTo("previous commit");
            await Assert.That(observedOptions).IsNotNull();
            await Assert.That(observedOptions!.MaxCapturedOutputLength).IsLessThanOrEqualTo(0);
            await Assert.That(observedCommands!).Contains("HEAD^1");
            await Assert.That(observedCommands!).DoesNotContain("--skip=1");
        }

        await result.Pipeline.DisposeAsync();
    }

    private static Mock<ICommandContext> CreateRepositoryCommand(
        Func<CommandLineToolOptions, CommandExecutionOptions?, CommandResult> responseFactory)
    {
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CommandLineToolOptions options, CommandExecutionOptions? executionOptions, CancellationToken _) =>
                options is GitRevParseOptions { ShowToplevel: true }
                    ? CommandResult.Ok(Environment.CurrentDirectory)
                    : responseFactory(options, executionOptions));
        return command;
    }

    private async Task<(IGitInformation T, IPipeline Pipeline)> GetGitInformation(
        Mock<ICommandContext> command)
    {
        var runner = new Mock<IGitCommandRunner>();
        runner.Setup(x => x.RunCommandsOrNull(
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string?[]>()))
            .ReturnsAsync((string?) null);
        return await GetService<IGitInformation>(services =>
        {
            services.AddSingleton<ICommandContext>(command.Object);
            services.AddSingleton(runner.Object);
        });
    }

    private static CommandResult CaptureRemoteOptions(
        CommandExecutionOptions? executionOptions,
        out CommandExecutionOptions? capturedOptions)
    {
        capturedOptions = executionOptions;
        return CommandResult.Ok("HEAD branch: trunk\n");
    }

    private static string CreateCommitOutput(string subject, char hashCharacter) =>
        string.Join(
            "%\n%",
            "Author",
            "author@example.com",
            "2026-07-31T13:00:45Z",
            "Committer",
            "committer@example.com",
            "2026-07-31T12:15:30Z",
            new string(hashCharacter, 40),
            new string(hashCharacter, 7),
            subject,
            string.Empty);
}
