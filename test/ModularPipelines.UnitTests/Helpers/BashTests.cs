using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using ModularPipelines.UnitTests.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class BashTests : TestBase
{
    private class BashCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Shell.Bash.RunAsync("echo \"Foo bar!\"", cancellationToken: cancellationToken);
        }
    }

    private class BashScriptModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var file = context.Files.GetFile(Path.Combine(
                TestContext.OutputDirectory!,
                "Data",
                "BashTest.sh"));
            return await context.Shell.Bash.RunFileAsync(file, cancellationToken: cancellationToken);
        }
    }

    [Test]
    [RequiresTool("bash")]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<BashCommandModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    [RequiresTool("bash")]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<BashCommandModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    [LinuxOnlyTest]
    [RequiresTool("bash")]
    public async Task Standard_Output_From_Script_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<BashScriptModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    [WindowsOnlyTest]
    public async Task RunFileAsync_Bounds_And_Cancels_Wsl_Path_Conversion()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var executionOptions = new CommandExecutionOptions { ThrowOnNonZeroExitCode = false };
        var invocations = new List<(CommandLineToolOptions Options, CommandExecutionOptions? ExecutionOptions, CancellationToken CancellationToken)>();
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (options, executionOptions, token) => invocations.Add((options, executionOptions, token)))
            .ReturnsAsync((CommandLineToolOptions options, CommandExecutionOptions? _, CancellationToken _) =>
                options.Tool == "wsl" ? CommandResult.Ok("/mnt/c/script.sh\n") : CommandResult.Ok());

        await new Bash(command.Object).RunFileAsync(
            new BashFileOptions("C:\\script.sh"),
            executionOptions,
            cancellationToken);

        await Assert.That(invocations).Count().IsEqualTo(2);
        await Assert.That(invocations[0].Options.Tool).IsEqualTo("wsl");
        await Assert.That(invocations[0].ExecutionOptions?.ExecutionTimeout).IsEqualTo(TimeSpan.FromSeconds(10));
        await Assert.That(invocations[0].CancellationToken).IsEqualTo(cancellationToken);
        await Assert.That(((BashFileOptions) invocations[1].Options).FilePath).IsEqualTo("/mnt/c/script.sh");
        await Assert.That(invocations[1].ExecutionOptions).IsSameReferenceAs(executionOptions);
        await Assert.That(invocations[1].CancellationToken).IsEqualTo(cancellationToken);
    }

    [Test]
    public async Task RunAsync_Forwards_Options_Record_And_Execution_Options()
    {
        var options = new BashCommandOptions("echo test");
        var executionOptions = new CommandExecutionOptions { WorkingDirectory = "work" };
        var cancellationToken = new CancellationTokenSource().Token;
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken))
            .ReturnsAsync(CommandResult.Ok());

        await new Bash(command.Object).RunAsync(options, executionOptions, cancellationToken);

        command.VerifyAll();
    }
}
