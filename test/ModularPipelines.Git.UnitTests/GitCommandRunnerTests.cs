using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class GitCommandRunnerTests
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
}
