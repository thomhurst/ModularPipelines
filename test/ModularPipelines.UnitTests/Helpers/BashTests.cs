using ModularPipelines.Context;
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
    [Arguments("C:\\script's $value.sh", null)]
    [Arguments("nested dir\\script's $value.sh", null)]
    [Arguments("nested dir\\script's $value.sh", "work")]
    [Arguments("/tmp/script's $value.sh", "work")]
    public async Task RunFileAsync_Bounds_And_Cancels_Wsl_Path_Conversion(string path, string? workingDirectory)
    {
        var pipelineDirectory = Path.Combine(Path.GetTempPath(), "mp bash pipeline");
        var expectedDirectory = workingDirectory is null
            ? pipelineDirectory
            : Path.GetFullPath(workingDirectory, pipelineDirectory);
        var cancellationToken = new CancellationTokenSource().Token;
        var executionOptions = new CommandExecutionOptions
        {
            ThrowOnNonZeroExitCode = false,
            WorkingDirectory = workingDirectory,
            EnvironmentVariables = new Dictionary<string, string?> { ["WSLENV"] = "TEST_VALUE" },
        };
        var invocations = new List<(CommandLineToolOptions Options, CommandExecutionOptions? ExecutionOptions, CancellationToken CancellationToken)>();
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (options, executionOptions, token) => invocations.Add((options, executionOptions, token)))
            .ReturnsAsync((CommandLineToolOptions options, CommandExecutionOptions? _, CancellationToken _) =>
                options is BashFileOptions ? CommandResult.Ok() : CommandResult.Ok("/mnt/c/script.sh\n"));

        var bashPath = Path.Combine(Environment.SystemDirectory, "bash.exe");

        await new Bash(command.Object, new PipelineWorkingDirectory(pipelineDirectory)).RunFileAsync(
            new BashFileOptions(path) { Tool = bashPath },
            executionOptions,
            cancellationToken);

        await Assert.That(invocations).Count().IsEqualTo(2);
        await Assert.That(invocations[0].Options.Tool).IsEqualTo(Path.Combine(Environment.SystemDirectory, "wsl.exe"));
        await Assert.That(invocations[0].Options.Arguments!.ToArray()).IsEquivalentTo(
            ["--exec", "wslpath", "-a", path.StartsWith('/') ? path : Path.GetFullPath(path, expectedDirectory)],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(invocations[0].ExecutionOptions?.ExecutionTimeout).IsEqualTo(TimeSpan.FromSeconds(10));
        await Assert.That(invocations[0].ExecutionOptions?.WorkingDirectory).IsEqualTo(expectedDirectory);
        await Assert.That(invocations[0].ExecutionOptions?.EnvironmentVariables).IsSameReferenceAs(executionOptions.EnvironmentVariables);
        await Assert.That(invocations[0].CancellationToken).IsEqualTo(cancellationToken);
        await Assert.That(((BashFileOptions) invocations[1].Options).FilePath).IsEqualTo("/mnt/c/script.sh");
        await Assert.That(invocations[1].Options.Tool).IsEqualTo(bashPath);
        await Assert.That(invocations[1].ExecutionOptions).IsEqualTo(executionOptions with { WorkingDirectory = expectedDirectory });
        await Assert.That(invocations[1].CancellationToken).IsEqualTo(cancellationToken);
        await Assert.That(executionOptions.WorkingDirectory).IsEqualTo(workingDirectory);
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments(false, null)]
    [Arguments(false, "work")]
    [Arguments(true, "work")]
    public async Task RunFileAsync_Uses_Native_Bash_Without_Wsl(bool explicitTool, string? workingDirectory)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"mp bash tests {Guid.NewGuid():N}");
        var expectedDirectory = workingDirectory is null ? directory : Path.Combine(directory, workingDirectory);
        var toolDirectory = Path.Combine(expectedDirectory, "tools");
        Directory.CreateDirectory(toolDirectory);
        var bashPath = Path.Combine(toolDirectory, "bash.exe");

        try
        {
            await File.WriteAllBytesAsync(bashPath, []);
            var executionOptions = new CommandExecutionOptions
            {
                WorkingDirectory = workingDirectory,
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["Path"] = "tools",
                    ["PathExt"] = ".EXE",
                },
            };
            var cancellationToken = new CancellationTokenSource().Token;
            var options = new BashFileOptions("nested dir\\script's $value.sh")
            {
                Tool = explicitTool ? bashPath : null,
                Arguments = ["argument's $value"],
            };
            var expectedExecutionOptions = executionOptions with { WorkingDirectory = expectedDirectory };
            var command = new Mock<ICommandContext>(MockBehavior.Strict);
            command.Setup(context => context.ExecuteCommandLineToolAsync(
                    It.Is<BashFileOptions>(actual => actual.FilePath == "nested dir/script's $value.sh"
                        && actual.Tool == bashPath && actual.Arguments == options.Arguments),
                    expectedExecutionOptions,
                    cancellationToken))
                .ReturnsAsync(CommandResult.Ok());

            await new Bash(command.Object, new PipelineWorkingDirectory(directory)).RunFileAsync(options, executionOptions, cancellationToken);

            command.VerifyAll();
            command.VerifyNoOtherCalls();
            await Assert.That(options.FilePath).IsEqualTo("nested dir\\script's $value.sh");
            await Assert.That(executionOptions.WorkingDirectory).IsEqualTo(workingDirectory);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    [RequiresTool("bash")]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunFileAsync_Executes_Script_With_Literal_Path(bool absolutePath)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"mp bash tests {Guid.NewGuid():N}");
        var nestedDirectory = Path.Combine(directory, "nested dir");
        Directory.CreateDirectory(nestedDirectory);
        var path = Path.Combine(nestedDirectory, "script's $value.sh");

        try
        {
            await File.WriteAllTextAsync(path, "printf '%s' \"$1\"\n");
            var bash = await GetService<IBashContext>();
            var result = await bash.RunFileAsync(
                new BashFileOptions(absolutePath ? path : Path.GetRelativePath(directory, path))
                {
                    Arguments = ["argument's $value"],
                },
                new CommandExecutionOptions { WorkingDirectory = directory });

            await Assert.That(result.StandardOutput).IsEqualTo("argument's $value");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    [LinuxOnlyTest]
    public async Task RunFileAsync_Forwards_Unix_Path_Unchanged()
    {
        var options = new BashFileOptions("/tmp/script's $value.sh");
        var executionOptions = new CommandExecutionOptions { WorkingDirectory = "/tmp" };
        var cancellationToken = new CancellationTokenSource().Token;
        var command = new Mock<ICommandContext>(MockBehavior.Strict);
        command.Setup(context => context.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken))
            .ReturnsAsync(CommandResult.Ok());

        await new Bash(command.Object, new PipelineWorkingDirectory(Environment.CurrentDirectory)).RunFileAsync(options, executionOptions, cancellationToken);

        command.VerifyAll();
        command.VerifyNoOtherCalls();
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

        await new Bash(command.Object, new PipelineWorkingDirectory(Environment.CurrentDirectory)).RunAsync(options, executionOptions, cancellationToken);

        command.VerifyAll();
    }
}
