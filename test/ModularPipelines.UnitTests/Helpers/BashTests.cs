using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
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
    private sealed class SerialCommandInterceptor : ICommandInterceptor, IDisposable
    {
        private readonly SemaphoreSlim _gate = new(1, 1);

        public int Calls { get; private set; }

        public async ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            Calls++;
            if (!await _gate.WaitAsync(TimeSpan.FromSeconds(1), cancellationToken))
            {
                throw new InvalidOperationException("Command interceptor re-entered while holding its gate.");
            }

            try
            {
                return await next(invocation, cancellationToken);
            }
            finally
            {
                _gate.Release();
            }
        }

        public void Dispose() => _gate.Dispose();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task RunFileAsync_Wsl_Conversion_Does_Not_Reenter_Interceptor_Gate()
    {
        using var interceptor = new SerialCommandInterceptor();
        var (bash, _) = await GetService<IBashContext>(services => services.AddSingleton<ICommandInterceptor>(interceptor));
        // Fail process creation deterministically without needing WSL installed or starting a distribution.
        var missingDirectory = Path.Combine(Path.GetTempPath(), $"mp missing wsl directory {Guid.NewGuid():N}");

        var exception = await Assert.ThrowsAsync<CommandException>(() => bash.RunFileAsync(
            new BashFileOptions("script.sh") { Tool = Path.Combine(Environment.SystemDirectory, "bash.exe") },
            new CommandExecutionOptions { WorkingDirectory = missingDirectory }));

        await Assert.That(exception!.Result.CommandInput).Contains("wsl.exe");
        await Assert.That(interceptor.Calls).IsEqualTo(1);
    }

    private sealed class ChangeBashEnvironmentInterceptor(string directory, bool useWsl) : ICommandInterceptor
    {
        public int Conversions { get; private set; }

        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            if (invocation.CommandLine.Tool == Path.Combine(Environment.SystemDirectory, "wsl.exe"))
            {
                Conversions++;

                return ValueTask.FromResult(CommandResult.Ok("/mnt/c/converted.sh\n"));
            }

            return next(invocation with
            {
                CommandLine = useWsl ? ChangeTool(invocation.CommandLine) : invocation.CommandLine,
                WorkingDirectory = directory,
                ExecutionOptions = invocation.ExecutionOptions with
                {
                    EnvironmentVariables = new Dictionary<string, string?> { ["PATH"] = "tools", ["PATHEXT"] = ".EXE" },
                },
            }, cancellationToken);
        }

        private static CommandLine ChangeTool(CommandLine commandLine)
        {
            var systemBash = Path.Combine(Environment.SystemDirectory, "bash.exe");
            if (commandLine.Tool != "sudo")
            {
                return new CommandLine(systemBash, commandLine.Arguments);
            }

            var arguments = commandLine.Arguments.ToList();
            arguments[arguments.IndexOf("--") + 1] = systemBash;
            return new CommandLine(commandLine.Tool, arguments);
        }
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task RunFileAsync_Resolves_Bash_After_Interceptor_Changes(bool useWsl, bool sudo)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"mp bash interceptor {Guid.NewGuid():N}");
        var initialDirectory = Path.Combine(directory, "initial");
        var finalDirectory = Path.Combine(directory, "final");
        Directory.CreateDirectory(Path.Combine(initialDirectory, "tools"));
        Directory.CreateDirectory(Path.Combine(finalDirectory, "tools"));
        try
        {
            await File.WriteAllBytesAsync(Path.Combine(initialDirectory, "tools", "bash.exe"), []);
            var finalBash = Path.Combine(finalDirectory, "tools", "bash.exe");
            await File.WriteAllBytesAsync(finalBash, []);
            var interceptor = new ChangeBashEnvironmentInterceptor(finalDirectory, useWsl);
            var (bash, _) = await GetService<IBashContext>(services => services.AddSingleton<ICommandInterceptor>(interceptor));
            var options = new BashFileOptions("nested dir\\script's $value.sh") { Arguments = ["literal argument"] };
            var executionOptions = new CommandExecutionOptions
            {
                InternalDryRun = true,
                Sudo = sudo,
                WorkingDirectory = initialDirectory,
                EnvironmentVariables = new Dictionary<string, string?> { ["PATH"] = "tools", ["PATHEXT"] = ".EXE" },
            };

            var result = await bash.RunFileAsync(options, executionOptions);

            await Assert.That(result.CommandInput).Contains(useWsl ? Path.Combine(Environment.SystemDirectory, "bash.exe") : finalBash);
            await Assert.That(result.CommandInput).Contains(useWsl ? Path.GetFullPath(options.FilePath, finalDirectory) : "nested dir/script's $value.sh");
            await Assert.That(result.CommandInput).Contains("literal argument");
            await Assert.That(interceptor.Conversions).IsEqualTo(0);

            await Assert.That(options.Tool).IsNull();
            await Assert.That(options.FilePath).IsEqualTo("nested dir\\script's $value.sh");
            await Assert.That(executionOptions.WorkingDirectory).IsEqualTo(initialDirectory);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments("bash")]
    [Arguments("BASH.EXE")]
    [Arguments("missing-bash")]
    [Arguments("tools\\bash.exe")]
    public async Task Unresolved_Bash_Prefers_Application_Directory_Before_System32(string tool)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"mp bash application {Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        var applicationBash = Path.Combine(directory, "bash.exe");
        try
        {
            await File.WriteAllBytesAsync(applicationBash, []);
            var isBareBash = tool.Equals("bash", StringComparison.OrdinalIgnoreCase)
                || tool.Equals("bash.exe", StringComparison.OrdinalIgnoreCase);

            var resolved = Bash.ResolveFallbackTool(tool, directory);

            await Assert.That(resolved).IsEqualTo(isBareBash ? applicationBash : tool);
            File.Delete(applicationBash);
            var systemBash = Path.Combine(Environment.SystemDirectory, "bash.exe");
            var expectedFallback = isBareBash && File.Exists(systemBash) ? systemBash : tool;
            await Assert.That(Bash.ResolveFallbackTool(tool, directory)).IsEqualTo(expectedFallback);
            await Assert.That(Bash.ResolveFallbackTool(tool, null)).IsEqualTo(expectedFallback);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    [WindowsOnlyTest]
    public async Task RunFileAsync_DryRun_Does_Not_Invoke_Wsl()
    {
        var directory = Path.Combine(Path.GetTempPath(), "mp bash dry run");
        var command = new Mock<ICommandContext>(MockBehavior.Strict);
        var options = new BashFileOptions("nested dir\\script.sh")
        {
            Tool = Path.Combine(Environment.SystemDirectory, "bash.exe"),
        };

        var prepared = await Bash.PrepareFileAsync(
            options,
            new CommandExecutionOptions { InternalDryRun = true },
            new PipelineWorkingDirectory(directory),
            command.Object.ExecuteCommandLineToolAsync,
            default);

        await Assert.That(prepared.Tool).IsEqualTo(options.Tool);
        await Assert.That(prepared.FilePath).IsEqualTo(Path.GetFullPath(options.FilePath, directory));
        command.VerifyNoOtherCalls();
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunFileAsync_Preserves_Option_Value_Equal_To_Script_Path(bool sudo)
    {
        const string path = "nested dir\\script.sh";
        var bash = await GetService<IBashContext>();
        var result = await bash.RunFileAsync(new BashFileOptions(path)
        {
            Tool = Path.Combine(Path.GetTempPath(), "native-bash.exe"),
            AdditionalArguments =
            [
                new("--rcfile", CommandLinePhase.EarlyOperand),
                new(path, CommandLinePhase.EarlyOperand),
            ],
        }, new CommandExecutionOptions
        {
            InternalDryRun = true,
            Sudo = sudo,
            EnvironmentVariables = new Dictionary<string, string?> { ["PATH"] = string.Empty },
        });

        await Assert.That(result.CommandInput).Contains($"--rcfile \"{path}\"");
        await Assert.That(result.CommandInput).Contains($"\"{path.Replace('\\', '/')}\"");
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments("")]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task RunFileAsync_Rejects_Empty_Wsl_Path_Conversion(string output)
    {
        var directory = Path.Combine(Path.GetTempPath(), "mp bash empty conversion");
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResult.Ok(output));
        var options = new BashFileOptions("script.sh")
        {
            Tool = Path.Combine(Environment.SystemDirectory, "bash.exe"),
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Bash.PrepareFileAsync(
                options, new CommandExecutionOptions(), new PipelineWorkingDirectory(directory), command.Object.ExecuteCommandLineToolAsync, default));

        await Assert.That(exception!.Message).IsEqualTo($"wslpath returned no path for '{Path.Combine(directory, options.FilePath)}'.");
    }

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

        var prepared = await Bash.PrepareFileAsync(
            new BashFileOptions(path) { Tool = bashPath },
            executionOptions,
            new PipelineWorkingDirectory(pipelineDirectory),
            command.Object.ExecuteCommandLineToolAsync,
            cancellationToken);

        await Assert.That(invocations).Count().IsEqualTo(1);
        await Assert.That(invocations[0].Options.Tool).IsEqualTo(Path.Combine(Environment.SystemDirectory, "wsl.exe"));
        await Assert.That(invocations[0].Options.Arguments!.ToArray()).IsEquivalentTo(
            ["--exec", "wslpath", "-a", path.StartsWith('/') ? path : Path.GetFullPath(path, expectedDirectory)],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(invocations[0].ExecutionOptions?.ExecutionTimeout).IsEqualTo(TimeSpan.FromSeconds(10));
        await Assert.That(invocations[0].ExecutionOptions?.WorkingDirectory).IsEqualTo(expectedDirectory);
        await Assert.That(invocations[0].ExecutionOptions?.EnvironmentVariables).IsSameReferenceAs(executionOptions.EnvironmentVariables);
        await Assert.That(invocations[0].CancellationToken).IsEqualTo(cancellationToken);
        await Assert.That(prepared.FilePath).IsEqualTo("/mnt/c/script.sh");
        await Assert.That(prepared.Tool).IsEqualTo(bashPath);
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
            var command = new Mock<ICommandContext>(MockBehavior.Strict);
            var prepared = await Bash.PrepareFileAsync(options, executionOptions, new PipelineWorkingDirectory(directory), command.Object.ExecuteCommandLineToolAsync, cancellationToken);

            await Assert.That(prepared.FilePath).IsEqualTo("nested dir/script's $value.sh");
            await Assert.That(prepared.Tool).IsEqualTo(bashPath);
            await Assert.That(prepared.Arguments).IsSameReferenceAs(options.Arguments);
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
    [WindowsOnlyTest]
    [Arguments(null)]
    [Arguments("bash")]
    [Arguments("BASH.EXE")]
    [Arguments("missing-bash")]
    [Arguments("tools\\bash.exe")]
    public async Task RunFileAsync_Uses_System_Bash_Fallback_Only_For_Unresolved_Bare_Bash(string? tool)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"mp bash tests {Guid.NewGuid():N}");
        var bashPath = Path.Combine(Environment.SystemDirectory, "bash.exe");
        var usesSystemBash = (tool is null || tool.Equals("bash", StringComparison.OrdinalIgnoreCase)
            || tool.Equals("bash.exe", StringComparison.OrdinalIgnoreCase)) && File.Exists(bashPath);
        var executionOptions = new CommandExecutionOptions
        {
            WorkingDirectory = directory,
            EnvironmentVariables = new Dictionary<string, string?> { ["PATH"] = string.Empty, ["PATHEXT"] = ".EXE" },
        };
        var options = new BashFileOptions("nested dir\\script's $value.sh") { Tool = tool };
        var invocations = new List<CommandLineToolOptions>();
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (actual, _, _) => invocations.Add(actual))
            .ReturnsAsync((CommandLineToolOptions actual, CommandExecutionOptions? _, CancellationToken _) =>
                actual is BashFileOptions ? CommandResult.Ok() : CommandResult.Ok("/mnt/c/script.sh\n"));

        var prepared = await Bash.PrepareFileAsync(options, executionOptions, new PipelineWorkingDirectory(directory), command.Object.ExecuteCommandLineToolAsync, default);

        await Assert.That(invocations).Count().IsEqualTo(usesSystemBash ? 1 : 0);
        if (usesSystemBash)
        {
            await Assert.That(invocations[0].Tool).IsEqualTo(Path.Combine(Environment.SystemDirectory, "wsl.exe"));
            await Assert.That(invocations[0].Arguments!.ToArray()).IsEquivalentTo(
                ["--exec", "wslpath", "-a", Path.GetFullPath(options.FilePath, directory)],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
        }

        await Assert.That(prepared.Tool).IsEqualTo(usesSystemBash ? bashPath : tool ?? "bash");
        await Assert.That(prepared.FilePath)
            .IsEqualTo(usesSystemBash ? "/mnt/c/script.sh" : "nested dir/script's $value.sh");
        await Assert.That(options.Tool).IsEqualTo(tool);
        await Assert.That(options.FilePath).IsEqualTo("nested dir\\script's $value.sh");
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
    public async Task RunFileAsync_Forwards_Options_Unchanged()
    {
        var options = new BashFileOptions("/tmp/script's $value.sh");
        var executionOptions = new CommandExecutionOptions { WorkingDirectory = "/tmp" };
        var cancellationToken = new CancellationTokenSource().Token;
        var command = new Mock<ICommandContext>(MockBehavior.Strict);
        command.Setup(context => context.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken))
            .ReturnsAsync(CommandResult.Ok());

        await new Bash(command.Object).RunFileAsync(options, executionOptions, cancellationToken);

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

        await new Bash(command.Object).RunAsync(options, executionOptions, cancellationToken);

        command.VerifyAll();
    }
}
