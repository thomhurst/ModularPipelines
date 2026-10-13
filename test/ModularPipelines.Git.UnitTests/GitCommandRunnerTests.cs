using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Git.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class GitCommandRunnerTests : TestBase
{
    [Test]
    public async Task Raw_Command_Preserves_Caller_Execution_Options()
    {
        CommandExecutionOptions? observedOptions = null;
        var shell = new Mock<IShellContext>();
        shell.Setup(x => x.RunAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Returns<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (_, options, _) =>
                {
                    observedOptions = options;
                    return Task.FromResult(CommandResult.Ok(" raw output "));
                });
        var context = new Mock<IPipelineContext>();
        context.SetupGet(x => x.Shell).Returns(shell.Object);
        var runner = new GitCommandRunner(
            context.Object,
            Mock.Of<ILogger<GitCommandRunner>>());
        var requestedOptions = new CommandExecutionOptions
        {
            MaxCapturedOutputLength = 0,
            WorkingDirectory = "repository-root",
        };

        var output = await ((IRawGitCommandRunner) runner).RunCommandsUntrimmed(
            requestedOptions,
            CancellationToken.None,
            "diff");

        using (Assert.Multiple())
        {
            await Assert.That(output).IsEqualTo(" raw output ");
            await Assert.That(observedOptions?.MaxCapturedOutputLength).IsEqualTo(0);
            await Assert.That(observedOptions?.WorkingDirectory).IsEqualTo("repository-root");
            await Assert.That(observedOptions?.Logging).IsEqualTo(CommandLoggingOptions.Silent);
        }
    }

    [Test]
    public async Task Default_Typed_Runner_Registers_Configuration_Secrets_When_Forwarding()
    {
        var services = await GetService<IGitCommandRunner>((Action<IServiceCollection>?) null);
        IGitCommandRunner runner = new ForwardingGitCommandRunner(services.T);
        var options = new GitOptions
        {
            CommandParts = ["log"],
            Arguments = ["-1", "--format=%H"],
            Configuration = [("http.extraHeader", "Authorization: Bearer test-secret")],
            NoPager = true,
        };
        var execution = new CommandExecutionOptions { MaxCapturedOutputLength = 0 };

        var output = await runner.RunCommandsOrNull(options, execution, CancellationToken.None);

        await Assert.That(output).IsNotNull();
        await Assert.That(output!.Length).IsEqualTo(40);
        var secrets = services.Pipeline.Services.GetRequiredService<ISecretProvider>();
        await Assert.That(secrets.GetSnapshot().Secrets).Contains("Authorization: Bearer test-secret");
        await Assert.That(execution.MaxCapturedOutputLength).IsEqualTo(0);
        await services.Pipeline.DisposeAsync();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Default_Typed_Runner_Preserves_Failure_And_Cancellation_Behavior(bool cancel)
    {
        IGitCommandRunner runner = new FailingGitCommandRunner(
            cancel ? new OperationCanceledException() : new InvalidOperationException("git failed"));
        var options = new GitOptions { CommandParts = ["log"] };

        if (cancel)
        {
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                runner.RunCommandsOrNull(options, null, CancellationToken.None));
        }
        else
        {
            await Assert.That(await runner.RunCommandsOrNull(options, null, CancellationToken.None)).IsNull();
        }
    }

    private sealed class FailingGitCommandRunner(Exception exception) : IGitCommandRunner
    {
        public Task<string> RunCommands(CommandExecutionOptions? execution, params string?[] commands) =>
            Task.FromException<string>(exception);

        public Task<string?> RunCommandsOrNull(CommandExecutionOptions? execution, params string?[] commands) =>
            Task.FromException<string?>(exception);
    }

    private sealed class ForwardingGitCommandRunner(IGitCommandRunner runner) : IGitCommandRunner
    {
        public Task<string> RunCommands(CommandExecutionOptions? execution, params string?[] commands) =>
            runner.RunCommands(execution, commands);

        public Task<string?> RunCommandsOrNull(CommandExecutionOptions? execution, params string?[] commands) =>
            runner.RunCommandsOrNull(execution, commands);

        public Task<string?> RunCommandsOrNull(
            CommandExecutionOptions? execution, CancellationToken cancellationToken, params string?[] commands) =>
            runner.RunCommandsOrNull(execution, cancellationToken, commands);
    }
}
