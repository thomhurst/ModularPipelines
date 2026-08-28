using ModularPipelines.Context.Domains;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class ShellContextTests
{
    [Test]
    public async Task RunAsync_With_Options_Delegates_To_Command_Context()
    {
        var expectedResult = CreateResult();
        var options = new CommandLineToolOptions("dotnet") { Arguments = ["--version"] };
        var executionOptions = new CommandExecutionOptions();
        using var cancellationTokenSource = new CancellationTokenSource();
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                options,
                executionOptions,
                cancellationTokenSource.Token))
            .ReturnsAsync(expectedResult);
        var shell = CreateShell(command.Object);

        var result = await shell.RunAsync(options, executionOptions, cancellationTokenSource.Token);

        await Assert.That(result).IsSameReferenceAs(expectedResult);
    }

    [Test]
    public async Task RunAsync_With_Raw_Command_Creates_Options()
    {
        var expectedResult = CreateResult();
        var arguments = new[] { "tool", "restore" };
        var executionOptions = new CommandExecutionOptions();
        using var cancellationTokenSource = new CancellationTokenSource();
        CommandLineToolOptions? capturedOptions = null;
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                executionOptions,
                cancellationTokenSource.Token))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (options, _, _) => capturedOptions = options)
            .ReturnsAsync(expectedResult);
        var shell = CreateShell(command.Object);

        var result = await shell.RunAsync(
            "dotnet",
            arguments,
            executionOptions,
            cancellationTokenSource.Token);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsSameReferenceAs(expectedResult);
            await Assert.That(capturedOptions!.Tool).IsEqualTo("dotnet");
            await Assert.That(capturedOptions.Arguments).IsEquivalentTo(arguments);
        }
    }

    [Test]
    public async Task RunAsync_Accepts_Cancellation_Token_As_Third_Argument()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                null,
                cancellationTokenSource.Token))
            .ReturnsAsync(CreateResult());
        var shell = CreateShell(command.Object);

        await shell.RunAsync("dotnet", ["--version"], cancellationTokenSource.Token);

        command.VerifyAll();
    }

    [Test]
    public async Task Shell_Interface_Does_Not_Expose_Command_Nesting()
    {
        await Assert.That(typeof(IShellContext).GetProperty("Command")).IsNull();
    }

    private static ShellContext CreateShell(ICommandContext command)
    {
        return new ShellContext(
            command,
            Mock.Of<IBashContext>(),
            Mock.Of<IPowerShellContext>());
    }

    private static CommandResult CreateResult()
    {
        var timestamp = DateTimeOffset.UtcNow;
        return new CommandResult(
            "command",
            Environment.CurrentDirectory,
            string.Empty,
            string.Empty,
            new Dictionary<string, string?>(),
            timestamp,
            timestamp,
            TimeSpan.Zero,
            0);
    }
}
