using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandTests : TestBase
{
    private sealed class RegisterEnvironmentSecretInterceptor(ISecretRegistry secretRegistry)
        : ICommandInterceptor
    {
        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default)
        {
            secretRegistry.AddSecret(invocation.EnvironmentVariables["MP_DYNAMIC_SECRET"]!);
            return ValueTask.FromResult<CommandResult?>(CommandResult.Ok());
        }
    }

    private sealed class StubCommandInterceptor(CommandResult? result) : ICommandInterceptor
    {
        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default) =>
            ValueTask.FromResult(result);
    }

    private sealed class RegisterEnvironmentSecretAndContinueInterceptor(
        ISecretRegistry secretRegistry) : ICommandInterceptor
    {
        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default)
        {
            secretRegistry.AddSecret(invocation.EnvironmentVariables["MP_DYNAMIC_SECRET"]!);
            return ValueTask.FromResult<CommandResult?>(null);
        }
    }

    private sealed class CaptureInvocationInterceptor : ICommandInterceptor
    {
        public string? EnvironmentValue { get; private set; }

        public string? CommandInput { get; private set; }

        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default)
        {
            EnvironmentValue = invocation.EnvironmentVariables["MP_DYNAMIC_SECRET"];
            CommandInput = invocation.CommandInput;
            return ValueTask.FromResult<CommandResult?>(CommandResult.Ok());
        }
    }

    [Test]
    public async Task Command_Execution_Default_Timeout_Is_Thirty_Minutes()
    {
        var executionOptions = new CommandExecutionOptions();

        await Assert.That(CommandExecutionOptions.DefaultExecutionTimeout)
            .IsEqualTo(TimeSpan.FromMinutes(30));
        await Assert.That(executionOptions.ExecutionTimeout)
            .IsEqualTo(CommandExecutionOptions.DefaultExecutionTimeout);
        await Assert.That(executionOptions.MaxCapturedOutputLength)
            .IsEqualTo(CommandExecutionOptions.DefaultMaxCapturedOutputLength);
    }

    [Test]
    public async Task Command_Execution_Timeout_Can_Be_Overridden()
    {
        var timeout = TimeSpan.FromMinutes(5);
        var executionOptions = new CommandExecutionOptions { ExecutionTimeout = timeout };

        await Assert.That(executionOptions.ExecutionTimeout).IsEqualTo(timeout);
    }

    [Test]
    public async Task Command_Registers_Credential_Password_As_Secret()
    {
        const string password = "command-credential-password";
        var (command, pipeline) = await GetService<ICommandContext>(_ => { });
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("unused"),
                new CommandExecutionOptions
                {
                    CommandLineCredentials = new CommandLineCredentials
                    {
                        Password = password,
                    },
                },
                cancellationTokenSource.Token));

        var secretProvider = pipeline.Services.GetRequiredService<ISecretProvider>();
        await Assert.That(secretProvider.Secrets).Contains(password);
    }

    [Test]
    public async Task Invalid_Command_Options_Do_Not_Increment_Command_Count()
    {
        var (command, pipeline) = await GetService<ICommandContext>(_ => { });
        var counter = pipeline.Services.GetRequiredService<ICommandExecutionCounter>();

        await Assert.ThrowsAsync<CommandOptionsValidationException>(() =>
            command.ExecuteCommandLineToolAsync(new InvalidCountedCommandOptions()));

        await Assert.That(counter.TotalCount).IsEqualTo(0);
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task Command_Execution_Caps_Captured_Output_With_Head_And_Tail()
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            new PowershellScriptOptions("Write-Output '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'"),
            new CommandExecutionOptions { MaxCapturedOutputLength = 10 });

        using (Assert.Multiple())
        {
            await Assert.That(result.StandardOutput).StartsWith("01234");
            await Assert.That(result.StandardOutput).Contains("truncated");
            await Assert.That(result.StandardOutput).Contains("XYZ");
            await Assert.That(result.StandardOutput).EndsWith(Environment.NewLine);
            await Assert.That(result.StandardOutput.Length).IsLessThan(100);
        }
    }

    private class CommandEchoModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-Command", "echo 'Foo bar!'"],
                },
                cancellationToken: cancellationToken);
        }
    }

    [CliTool("tool")]
    private sealed record InvalidCountedCommandOptions : CommandLineToolOptions
    {
        [Range(1, 1)]
        [CliOption("--value")]
        public int Value { get; init; }
    }

    private class CommandEchoTimeoutModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return TestConstants.TestString;
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<CommandEchoModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<CommandEchoModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar_With_Timeout()
    {
        var moduleResult = await await RunModule<CommandEchoTimeoutModule>();

        await Assert.That(moduleResult.ValueOrDefault!.Trim()).IsEqualTo(TestConstants.TestString);
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task Failed_Command_Exposes_Obfuscated_Result()
    {
        const string secret = "command-result-secret-value";
        var (command, pipeline) = await GetService<ICommandContext>(_ => { });
        pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(secret);

        var exception = await Assert.ThrowsAsync<CommandException>(() =>
            command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments =
                    [
                        "-NoProfile",
                        "-Command",
                        $"Write-Output '{secret}'; [Console]::Error.WriteLine('{secret}'); exit 42",
                    ],
                },
                new CommandExecutionOptions
                {
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["MP_TEST_SECRET"] = secret,
                    },
                }));

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Result.ExitCode).IsEqualTo(42);
            await Assert.That(exception.Result.CommandInput).DoesNotContain(secret);
            await Assert.That(exception.Result.StandardOutput).DoesNotContain(secret);
            await Assert.That(exception.Result.StandardError).DoesNotContain(secret);
            await Assert.That(exception.Result.EnvironmentVariables["MP_TEST_SECRET"]).DoesNotContain(secret);
            await Assert.That(exception.Message).Contains(exception.Result.CommandInput);
            await Assert.That(exception.Message).DoesNotContain(secret);
        }
    }

    [Test]
    public async Task Missing_Executable_Throws_Actionable_Exception()
    {
        var executable = $"modular-pipelines-missing-{Guid.NewGuid():N}";
        var command = await GetService<ICommandContext>();

        var exception = await Assert.ThrowsAsync<ToolNotFoundException>(() =>
            command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions(executable)));

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Executable).IsEqualTo(executable);
            await Assert.That(exception.Result.ExitCode).IsEqualTo(-1);
            await Assert.That(exception.Result.CommandInput).Contains(executable);
            await Assert.That(exception.Message).Contains($"Executable '{executable}' was not found on PATH");
            await Assert.That(exception.Message).Contains("context.Installers");
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task Successful_Command_Exposes_Obfuscated_Input()
    {
        const string secret = "successful-command-input-secret";
        const string logOnlyInput = "manipulated-log-input";
        var (command, pipeline) = await GetService<ICommandContext>(_ => { });
        pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(secret);

        var result = await command.ExecuteCommandLineToolAsync(
            new GenericCommandLineToolOptions("pwsh")
            {
                Arguments = ["-NoProfile", "-Command", $"Write-Output '{secret}'"],
            },
            new CommandExecutionOptions
            {
                InputLoggingManipulator = _ => logOnlyInput,
            });

        await Assert.That(result.CommandInput).DoesNotContain(secret);
        await Assert.That(result.CommandInput).DoesNotContain(logOnlyInput);
        await Assert.That(result.CommandInput).Contains("Write-Output");
    }

    [Test]
    public Task Dry_Run_Command_Exposes_Obfuscated_Environment_Variables() =>
        AssertCommandExposesObfuscatedEnvironmentVariables(dryRun: true);

    [Test]
    [RequiresTool("pwsh")]
    public Task Successful_Command_Exposes_Obfuscated_Environment_Variables() =>
        AssertCommandExposesObfuscatedEnvironmentVariables(dryRun: false);

    [Test]
    public async Task Command_ObfuscatesEnvironmentVariablesOncePerInvocation()
    {
        const string environmentValue = "unique-environment-value";
        var environmentObfuscationCount = 0;
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? input, object? _) =>
            {
                if (input == environmentValue)
                {
                    Interlocked.Increment(ref environmentObfuscationCount);
                }

                return input ?? string.Empty;
            });
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.RemoveAll<ISecretObfuscator>();
            services.AddSingleton<ISecretObfuscator>(obfuscator.Object);
        });

        var result = await command.ExecuteCommandLineToolAsync(
            new GenericCommandLineToolOptions("unused"),
            new CommandExecutionOptions
            {
                InternalDryRun = true,
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["MP_TEST_VALUE"] = environmentValue,
                },
            });

        using (Assert.Multiple())
        {
            await Assert.That(environmentObfuscationCount).IsEqualTo(1);
            await Assert.That(result.EnvironmentVariables["MP_TEST_VALUE"])
                .IsEqualTo(environmentValue);
        }
    }

    [Test]
    public async Task Command_ReobfuscatesEnvironmentVariablesAfterDynamicSecretRegistration()
    {
        const string secret = "dynamically-registered-environment-secret";
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor, RegisterEnvironmentSecretInterceptor>());

        var result = await command.ExecuteCommandLineToolAsync(
            new GenericCommandLineToolOptions("unused"),
            new CommandExecutionOptions
            {
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["MP_DYNAMIC_SECRET"] = secret,
                },
            });

        await Assert.That(result.EnvironmentVariables["MP_DYNAMIC_SECRET"])
            .IsEqualTo("**********");
    }

    [Test]
    public async Task Command_ObfuscatesEnvironmentVariablesOnceForAllInterceptors()
    {
        const string environmentValue = "multi-interceptor-environment-value";
        var environmentObfuscationCount = 0;
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? input, object? _) =>
            {
                if (input == environmentValue)
                {
                    Interlocked.Increment(ref environmentObfuscationCount);
                }

                return input ?? string.Empty;
            });
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.RemoveAll<ISecretObfuscator>();
            services.AddSingleton<ISecretObfuscator>(obfuscator.Object);
            services.AddSingleton<ICommandInterceptor>(new StubCommandInterceptor(null));
            services.AddSingleton<ICommandInterceptor>(new StubCommandInterceptor(CommandResult.Ok()));
        });

        var result = await command.ExecuteCommandLineToolAsync(
            new GenericCommandLineToolOptions("unused"),
            new CommandExecutionOptions
            {
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["MP_TEST_VALUE"] = environmentValue,
                },
            });

        using (Assert.Multiple())
        {
            // One public interceptor snapshot, then one fresh result snapshot.
            await Assert.That(environmentObfuscationCount).IsEqualTo(2);
            await Assert.That(result.EnvironmentVariables["MP_TEST_VALUE"])
                .IsEqualTo(environmentValue);
        }
    }

    [Test]
    public async Task Command_RefreshesInterceptorMetadataAfterSecretRegistration()
    {
        const string secret = "interceptor-registered-environment-secret";
        var captureInterceptor = new CaptureInvocationInterceptor();
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.AddSingleton<
                ICommandInterceptor,
                RegisterEnvironmentSecretAndContinueInterceptor>();
            services.AddSingleton<ICommandInterceptor>(captureInterceptor);
        });

        var result = await command.ExecuteCommandLineToolAsync(
            new GenericCommandLineToolOptions("unused")
            {
                Arguments = [secret],
            },
            new CommandExecutionOptions
            {
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["MP_DYNAMIC_SECRET"] = secret,
                },
            });

        using (Assert.Multiple())
        {
            await Assert.That(captureInterceptor.EnvironmentValue).IsEqualTo("**********");
            await Assert.That(captureInterceptor.CommandInput).DoesNotContain(secret);
            await Assert.That(result.CommandInput).DoesNotContain(secret);
            await Assert.That(result.EnvironmentVariables["MP_DYNAMIC_SECRET"])
                .IsEqualTo("**********");
        }
    }

    private async Task AssertCommandExposesObfuscatedEnvironmentVariables(bool dryRun)
    {
        const string secret = "command-result-secret-value";
        var (command, pipeline) = await GetService<ICommandContext>(_ => { });
        pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(secret);

        var result = await command.ExecuteCommandLineToolAsync(
            new GenericCommandLineToolOptions("pwsh")
            {
                Arguments = ["-NoProfile", "-Command", "exit 0"],
            },
            new CommandExecutionOptions
            {
                InternalDryRun = dryRun,
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["MP_TEST_SECRET"] = secret,
                    ["MP_TEST_PUBLIC"] = "public-value",
                    ["MP_TEST_NULL"] = null,
                },
            });

        using (Assert.Multiple())
        {
            await Assert.That(result.EnvironmentVariables["MP_TEST_SECRET"]).DoesNotContain(secret);
            await Assert.That(result.EnvironmentVariables["MP_TEST_PUBLIC"]).IsEqualTo("public-value");
            await Assert.That(result.EnvironmentVariables["MP_TEST_NULL"]).IsNull();
            await Assert.That(result.EnvironmentVariables.Keys.Any(key =>
                key.StartsWith("MODULAR_PIPELINES_CMD_", StringComparison.OrdinalIgnoreCase))).IsFalse();
        }
    }

    [Test]
    public Task Dry_Run_Command_Preserves_Unix_Environment_Name_Casing() =>
        AssertCommandPreservesUnixEnvironmentNameCasing(dryRun: true);

    [Test]
    [RequiresTool("pwsh")]
    public Task Successful_Command_Preserves_Unix_Environment_Name_Casing() =>
        AssertCommandPreservesUnixEnvironmentNameCasing(dryRun: false);

    private async Task AssertCommandPreservesUnixEnvironmentNameCasing(bool dryRun)
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            new GenericCommandLineToolOptions("pwsh")
            {
                Arguments = ["-NoProfile", "-Command", "exit 0"],
            },
            new CommandExecutionOptions
            {
                InternalDryRun = dryRun,
                EnvironmentVariables = new Dictionary<string, string?>(StringComparer.Ordinal)
                {
                    ["FOO"] = "upper",
                    ["foo"] = "lower",
                },
            });

        using (Assert.Multiple())
        {
            await Assert.That(result.EnvironmentVariables["FOO"]).IsEqualTo("upper");
            await Assert.That(result.EnvironmentVariables["foo"]).IsEqualTo("lower");
        }
    }

    [Test]
    public async Task ExecuteCommandLineToolAsync_Resolves_Windows_Command_Scripts_From_Path()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var scriptPath = Path.Combine(tempDirectory, "mp-runtime-test.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %~1\r\n");
            var command = await GetService<ICommandContext>();

            var result = await command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("mp-runtime-test")
                {
                    Arguments = ["hello world"],
                },
                new CommandExecutionOptions
                {
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = tempDirectory,
                        ["PATHEXT"] = ".COM;.EXE;.BAT;.CMD",
                    },
                });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo("hello world");
            await Assert.That(result.CommandInput).Contains(scriptPath);
            await Assert.That(result.CommandInput).Contains("hello world");
            await Assert.That(result.CommandInput).DoesNotContain("MODULAR_PIPELINES_CMD_");
            await Assert.That(result.EnvironmentVariables["PATH"]).IsEqualTo(tempDirectory);
            await Assert.That(result.EnvironmentVariables.Keys.Any(key =>
                key.StartsWith("MODULAR_PIPELINES_CMD_", StringComparison.OrdinalIgnoreCase))).IsFalse();
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExecuteCommandLineToolAsync_Rejects_Newlines_In_Windows_Command_Script_Arguments()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var scriptPath = Path.Combine(tempDirectory, "mp-runtime-newline-test.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\n");
            var command = await GetService<ICommandContext>();

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                command.ExecuteCommandLineToolAsync(
                    new GenericCommandLineToolOptions(scriptPath)
                    {
                        Arguments = ["first line\r\nsecond line"],
                    }));

            await Assert.That(exception!.Message).Contains("cannot contain CR or LF");
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExecuteCommandLineToolAsync_Preserves_Windows_Command_Script_Metacharacters()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var scriptPath = Path.Combine(tempDirectory, "mp-runtime-metachar-test.cmd");
        const string argument = "value & echo injected | more < input > output %PATH% ^ !PATH!";

        try
        {
            await File.WriteAllTextAsync(
                scriptPath,
                "@echo off\r\nsetlocal DisableDelayedExpansion\r\nset \"arg=%~1\"\r\nsetlocal EnableDelayedExpansion\r\necho(!arg!\r\n");
            var command = await GetService<ICommandContext>();

            var result = await command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions(scriptPath)
                {
                    Arguments = [argument],
                });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo(argument);
            await Assert.That(result.StandardError).IsEmpty();
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExecuteCommandLineToolAsync_Resolves_Extensionless_Relative_Windows_Command_Script()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        var scriptDirectory = Path.Combine(tempDirectory, "scripts");
        Directory.CreateDirectory(tempDirectory);
        Directory.CreateDirectory(scriptDirectory);
        var scriptPath = Path.Combine(scriptDirectory, "mp-runtime-relative-test.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %CD%\r\n");
            var command = await GetService<ICommandContext>();
            var relativeToolPath = Path.ChangeExtension(Path.Combine("scripts", "mp-runtime-relative-test.cmd"), null);

            var resolvedScript = WindowsCommandResolver.Resolve(
                relativeToolPath,
                tempDirectory,
                pathExtensions: ".COM;.EXE;.BAT;.CMD",
                isWindows: true);

            var result = await command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions(relativeToolPath),
                new CommandExecutionOptions
                {
                    WorkingDirectory = tempDirectory,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATHEXT"] = ".COM;.EXE;.BAT;.CMD",
                    },
                });

            await Assert.That(resolvedScript).IsEqualTo(Path.GetFullPath(scriptPath));
            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo(tempDirectory);
            await Assert.That(result.StandardError).IsEmpty();
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExecuteCommandLineToolAsync_Resolves_Relative_Path_Entries_From_Working_Directory()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var workingDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        var relativeScriptDirectory = $"mp-runtime-path-{Guid.NewGuid():N}";
        var scriptDirectory = Path.Combine(workingDirectory, relativeScriptDirectory);
        Directory.CreateDirectory(workingDirectory);
        Directory.CreateDirectory(scriptDirectory);
        var scriptPath = Path.Combine(scriptDirectory, "mp-runtime-path-test.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %CD%\r\n");
            var command = await GetService<ICommandContext>();

            var result = await command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("mp-runtime-path-test"),
                new CommandExecutionOptions
                {
                    WorkingDirectory = workingDirectory,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = relativeScriptDirectory,
                        ["PATHEXT"] = ".COM;.EXE;.BAT;.CMD",
                    },
                });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo(workingDirectory);
            await Assert.That(result.StandardError).IsEmpty();
        }
        finally
        {
            Directory.Delete(workingDirectory, recursive: true);
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_KillsDescendantProcesses()
    {
        var pidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-child-{Guid.NewGuid():N}.pid");
        Process? childProcess = null;

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var script = string.Join(
                "; ",
                "$child = Start-Process pwsh -ArgumentList '-NoProfile', '-Command', 'Start-Sleep -Seconds 60' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(pidFile)}' -Value $child.Id",
                "Wait-Process -Id $child.Id");

            var executionTask = command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", script],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(50),
                },
                cancellationTokenSource.Token);

            using var pidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var childProcessId = await WaitForProcessIdAsync(pidFile, pidFileTimeout.Token);
            childProcess = Process.GetProcessById(childProcessId);
            cancellationTokenSource.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await executionTask);

            var childExited = await WaitForExitAsync(childProcess, TimeSpan.FromSeconds(2));
            await Assert.That(childExited).IsTrue();
        }
        finally
        {
            if (childProcess is { HasExited: false })
            {
                childProcess.Kill(entireProcessTree: true);
                await childProcess.WaitForExitAsync();
            }

            childProcess?.Dispose();
            File.Delete(pidFile);
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_KillsDescendantAfterParentExits()
    {
        var fileSuffix = Guid.NewGuid().ToString("N");
        var pidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-child-{fileSuffix}.pid");
        var parentExitFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-parent-exit-{fileSuffix}");
        Process? childProcess = null;

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var script = string.Join(
                "; ",
                "$child = Start-Process pwsh -ArgumentList '-NoProfile', '-Command', 'Start-Sleep -Seconds 60' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(pidFile)}' -Value $child.Id",
                $"while (-not (Test-Path -LiteralPath '{EscapePowerShellLiteral(parentExitFile)}')) {{ Start-Sleep -Milliseconds 10 }}");

            var executionTask = command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", script],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(100),
                },
                cancellationTokenSource.Token);

            using var pidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var childProcessId = await WaitForProcessIdAsync(pidFile, pidFileTimeout.Token);
            childProcess = Process.GetProcessById(childProcessId);
            cancellationTokenSource.Cancel();
            await File.WriteAllTextAsync(parentExitFile, string.Empty);

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await executionTask);

            var childExited = await WaitForExitAsync(childProcess, TimeSpan.FromSeconds(2));
            await Assert.That(childExited).IsTrue();
        }
        finally
        {
            if (childProcess is { HasExited: false })
            {
                childProcess.Kill(entireProcessTree: true);
                await childProcess.WaitForExitAsync();
            }

            childProcess?.Dispose();
            File.Delete(pidFile);
            File.Delete(parentExitFile);
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task ExecuteCommandLineToolAsync_GracefulExit_DoesNotWaitForForcefulTimeout()
    {
        var parentExitFile = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-parent-exit-{Guid.NewGuid():N}");

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var script =
                $"while (-not (Test-Path -LiteralPath '{EscapePowerShellLiteral(parentExitFile)}')) " +
                "{ Start-Sleep -Milliseconds 10 }";

            var executionTask = command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", script],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromSeconds(10),
                },
                cancellationTokenSource.Token);

            await Task.Delay(100);
            var stopwatch = Stopwatch.StartNew();
            cancellationTokenSource.Cancel();
            await File.WriteAllTextAsync(parentExitFile, string.Empty);

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await executionTask);
            await Assert.That(stopwatch.Elapsed).IsLessThan(TimeSpan.FromSeconds(5));
        }
        finally
        {
            File.Delete(parentExitFile);
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task ExecuteCommandLineToolAsync_ExecutionTimeout_ThrowsTimeoutException()
    {
        var command = await GetService<ICommandContext>();

        var exception = await Assert.ThrowsAsync<TimeoutException>(() =>
            command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", "Start-Sleep -Seconds 60"],
                },
                new CommandExecutionOptions
                {
                    ExecutionTimeout = TimeSpan.FromMilliseconds(100),
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(50),
                }));

        await Assert.That(exception!.Message).Contains("timed out after");
    }

    [Test]
    public async Task ScheduleForcefulCancellationAsync_ArmsTimerWhenReadinessFaults()
    {
        using var forcefulCancellationToken = new CancellationTokenSource();
        var readinessFailure = Task.FromException(
            new InvalidOperationException("readiness failed"));

        await Command.ScheduleForcefulCancellationAsync(
            forcefulCancellationToken,
            TimeSpan.Zero,
            readinessFailure);

        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            await Task.Delay(TimeSpan.FromSeconds(1), forcefulCancellationToken.Token));
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_CapturesDescendantSpawnedDuringGrace()
    {
        var fileSuffix = Guid.NewGuid().ToString("N");
        var triggerFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-trigger-{fileSuffix}");
        var intermediatePidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-intermediate-{fileSuffix}.pid");
        var intermediateReadyFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-intermediate-{fileSuffix}.ready");
        var grandchildPidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-grandchild-{fileSuffix}.pid");
        Process? intermediateProcess = null;
        Process? grandchildProcess = null;
        Task<CommandResult>? executionTask = null;
        var forcefulCancellationReady = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var intermediateScript = string.Join(
                "; ",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(intermediateReadyFile)}' -Value 'ready'",
                $"while (-not (Test-Path -LiteralPath '{EscapePowerShellLiteral(triggerFile)}')) {{ Start-Sleep -Milliseconds 10 }}",
                "$grandchild = Start-Process pwsh -ArgumentList '-NoProfile', '-Command', 'Start-Sleep -Seconds 60' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(grandchildPidFile)}' -Value $grandchild.Id",
                "Start-Sleep -Milliseconds 500");
            var encodedIntermediateScript =
                Convert.ToBase64String(Encoding.Unicode.GetBytes(intermediateScript));
            var parentScript = string.Join(
                "; ",
                $"$intermediate = Start-Process pwsh -ArgumentList '-NoProfile', '-EncodedCommand', '{encodedIntermediateScript}' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(intermediatePidFile)}' -Value $intermediate.Id",
                "Wait-Process -Id $intermediate.Id");

            executionTask = command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", parentScript],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromSeconds(1),
                    InternalForcefulCancellationReady = forcefulCancellationReady.Task,
                },
                cancellationTokenSource.Token);

            using var intermediatePidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var intermediateProcessId = await WaitForProcessIdAsync(
                intermediatePidFile,
                intermediatePidFileTimeout.Token);
            intermediateProcess = Process.GetProcessById(intermediateProcessId);
            await WaitForFileAsync(intermediateReadyFile, intermediatePidFileTimeout.Token);
            cancellationTokenSource.Cancel();
            await File.WriteAllTextAsync(triggerFile, string.Empty);

            using var grandchildPidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var grandchildProcessId = await WaitForProcessIdAsync(
                grandchildPidFile,
                grandchildPidFileTimeout.Token);
            grandchildProcess = Process.GetProcessById(grandchildProcessId);
            forcefulCancellationReady.SetResult();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await executionTask);

            var grandchildExited = await WaitForExitAsync(grandchildProcess, TimeSpan.FromSeconds(2));
            await Assert.That(grandchildExited).IsTrue();
        }
        finally
        {
            forcefulCancellationReady.TrySetResult();
            if (executionTask is not null)
            {
                try
                {
                    await executionTask.WaitAsync(TimeSpan.FromSeconds(5));
                }
                catch (OperationCanceledException)
                {
                    // Expected after the fixture requests command cancellation.
                }
                catch (TimeoutException)
                {
                    // Process handles below provide the final cleanup fallback.
                }
            }

            grandchildProcess ??= TryGetPublishedProcess(grandchildPidFile);

            foreach (var process in new[] { intermediateProcess, grandchildProcess })
            {
                if (process is { HasExited: false })
                {
                    process.Kill(entireProcessTree: true);
                    await process.WaitForExitAsync();
                }

                process?.Dispose();
            }

            File.Delete(triggerFile);
            File.Delete(intermediatePidFile);
            File.Delete(intermediateReadyFile);
            File.Delete(grandchildPidFile);
        }
    }

    private static string EscapePowerShellLiteral(string value) => value.Replace("'", "''");

    private static Process? TryGetPublishedProcess(string pidFile)
    {
        try
        {
            return int.TryParse(File.ReadAllText(pidFile), out var processId)
                ? Process.GetProcessById(processId)
                : null;
        }
        catch (Exception exception) when (exception is IOException or ArgumentException)
        {
            return null;
        }
    }

    private static async Task<int> WaitForProcessIdAsync(string pidFile, CancellationToken cancellationToken)
    {
        while (true)
        {
            if (File.Exists(pidFile))
            {
                try
                {
                    var processId = await File.ReadAllTextAsync(pidFile, cancellationToken);
                    if (int.TryParse(processId.Trim(), out var parsedProcessId))
                    {
                        return parsedProcessId;
                    }
                }
                catch (IOException)
                {
                    // The shell may still be creating or writing the PID file.
                }
            }

            await Task.Delay(20, cancellationToken);
        }
    }

    private static async Task WaitForFileAsync(string path, CancellationToken cancellationToken)
    {
        while (!File.Exists(path))
        {
            await Task.Delay(20, cancellationToken);
        }
    }

    private static async Task<bool> WaitForExitAsync(Process process, TimeSpan timeout)
    {
        try
        {
            await process.WaitForExitAsync().WaitAsync(timeout);
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }
}
