using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class PowerShellTests : TestBase
{
    private class PowerShellEchoModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Shell.PowerShell.RunAsync("Write-Host \"Foo bar!\"", cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<PowerShellEchoModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<PowerShellEchoModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task RunAsync_Forwards_Options_Record_And_Execution_Options()
    {
        var options = new PowerShellScriptOptions("Write-Host test");
        var executionOptions = new CommandExecutionOptions { WorkingDirectory = "work" };
        var cancellationToken = new CancellationTokenSource().Token;
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken))
            .ReturnsAsync(CommandResult.Ok());

        await new PowerShell(command.Object).RunAsync(options, executionOptions, cancellationToken);

        command.VerifyAll();
    }

    [Test]
    public async Task RunFileAsync_Forwards_Path_And_Execution_Options()
    {
        var executionOptions = new CommandExecutionOptions { ThrowOnNonZeroExitCode = false };
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.Is<PowerShellFileOptions>(options => options.FilePath == "script.ps1"),
                executionOptions,
                CancellationToken.None))
            .ReturnsAsync(CommandResult.Ok());

        await new PowerShell(command.Object).RunFileAsync("script.ps1", executionOptions);

        command.VerifyAll();
    }
}
