using ModularPipelines.Secrets;
using ModularPipelines.Context;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using ModularPipelines.UnitTests.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandTests : TestBase
{
    private sealed class RegisterEnvironmentSecretInterceptor(ISecretRegistry secretRegistry)
        : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            secretRegistry.AddSecret(invocation.EnvironmentVariables["MP_DYNAMIC_SECRET"]!);
            return ValueTask.FromResult(CommandResult.Ok());
        }
    }

    private sealed class StubCommandInterceptor(CommandResult? result) : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken) =>
            result is null ? next(invocation, cancellationToken) : ValueTask.FromResult(result);
    }

    private sealed class RegisterEnvironmentSecretAndContinueInterceptor(
        ISecretRegistry secretRegistry) : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            secretRegistry.AddSecret(invocation.EnvironmentVariables["MP_DYNAMIC_SECRET"]!);
            return next(invocation, cancellationToken);
        }
    }

    private sealed class CaptureInvocationInterceptor : ICommandInterceptor
    {
        public string? EnvironmentValue { get; private set; }

        public string? CommandInput { get; private set; }

        public CommandInvocation? Invocation { get; private set; }

        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            Invocation = invocation;
            invocation.EnvironmentVariables.TryGetValue("MP_DYNAMIC_SECRET", out var environmentValue);
            EnvironmentValue = environmentValue;
            CommandInput = invocation.CommandInput;
            return ValueTask.FromResult(CommandResult.Ok());
        }
    }

    private sealed class RewriteArgumentsInterceptor(params string[] arguments) : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken) =>
            next(
                invocation with { CommandLine = new CommandLine(invocation.CommandLine.Tool, arguments) },
                cancellationToken);
    }

    private sealed class ResultRewritingInterceptor(string output) : ICommandInterceptor
    {
        public CommandResult? ObservedResult { get; private set; }

        public async ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            ObservedResult = await next(invocation, cancellationToken);
            return ObservedResult with { StandardOutput = output };
        }
    }

    private sealed class DropCancellationInterceptor : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken) =>
            next(invocation, CancellationToken.None);
    }

    private sealed class OrderRecordingInterceptor(string name, List<string> log) : ICommandInterceptor
    {
        public async ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            log.Add($"{name}:before");
            var result = await next(invocation, cancellationToken);
            log.Add($"{name}:after");
            return result;
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
                new CommandLineToolOptions("unused"),
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
            new PowerShellScriptOptions("Write-Output '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'"),
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
            return await context.Shell.RunAsync(
                new CommandLineToolOptions("pwsh")
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
                new CommandLineToolOptions("pwsh")
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
    [RequiresTool("pwsh")]
    [Timeout(30_000)]
    public async Task Command_Reading_Standard_Input_Sees_End_Of_Input(CancellationToken cancellationToken)
    {
        var command = await GetService<ICommandContext>();

        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("pwsh")
            {
                Arguments = ["-NoProfile", "-Command", "[Console]::In.ReadToEnd().Length"],
            },
            cancellationToken: cancellationToken);

        await Assert.That(result.StandardOutput.Trim()).IsEqualTo("0");
    }

    [Test]
    public async Task Missing_Executable_Throws_Actionable_Exception()
    {
        var executable = $"modular-pipelines-missing-{Guid.NewGuid():N}";
        var command = await GetService<ICommandContext>();

        var exception = await Assert.ThrowsAsync<ToolNotFoundException>(() =>
            command.ExecuteCommandLineToolAsync(new CommandLineToolOptions(executable)));

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
            new CommandLineToolOptions("pwsh")
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
            new CommandLineToolOptions("unused"),
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
            new CommandLineToolOptions("unused"),
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
            new CommandLineToolOptions("unused"),
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
            new CommandLineToolOptions("unused")
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

    [Test]
    public async Task Interceptor_ModifiedInvocation_ReachesProcess()
    {
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(new RewriteArgumentsInterceptor("--version")));

        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("dotnet")
            {
                Arguments = ["--not-a-real-dotnet-option"],
            });

        using (Assert.Multiple())
        {
            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.CommandInput).Contains("--version");
            await Assert.That(result.CommandInput).DoesNotContain("--not-a-real-dotnet-option");
        }
    }

    [Test]
    public async Task Interceptor_ModifiedInvocation_IsSeenByNextInterceptor()
    {
        var capture = new CaptureInvocationInterceptor();
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.AddSingleton<ICommandInterceptor>(new RewriteArgumentsInterceptor("rewritten"));
            services.AddSingleton<ICommandInterceptor>(capture);
        });

        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("unused")
            {
                Arguments = ["original"],
            });

        using (Assert.Multiple())
        {
            await Assert.That(capture.Invocation!.CommandLine.Arguments).IsEquivalentTo(["rewritten"]);
            await Assert.That(capture.CommandInput).Contains("rewritten");
            await Assert.That(capture.CommandInput).DoesNotContain("original");
            await Assert.That(result.CommandInput).Contains("rewritten");
        }
    }

    [Test]
    public async Task Interceptor_CanObserveAndReplaceResultOfNext()
    {
        var outer = new ResultRewritingInterceptor("outer");
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.AddSingleton<ICommandInterceptor>(outer);
            services.AddSingleton<ICommandInterceptor>(new StubCommandInterceptor(CommandResult.Ok("inner")));
        });

        var result = await command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("unused"));

        using (Assert.Multiple())
        {
            await Assert.That(outer.ObservedResult!.StandardOutput).IsEqualTo("inner");
            await Assert.That(outer.ObservedResult.CommandInput).Contains("unused");
            await Assert.That(result.StandardOutput).IsEqualTo("outer");
        }
    }

    [Test]
    public async Task Interceptors_RunInRegistrationOrder()
    {
        var log = new List<string>();
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.AddSingleton<ICommandInterceptor>(new OrderRecordingInterceptor("first", log));
            services.AddSingleton<ICommandInterceptor>(new OrderRecordingInterceptor("second", log));
            services.AddSingleton<ICommandInterceptor>(new StubCommandInterceptor(CommandResult.Ok()));
        });

        await command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("unused"));

        await Assert.That(log).IsEquivalentTo(
            ["first:before", "second:before", "second:after", "first:after"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Interceptor_ShortCircuitedNonZeroResult_Throws()
    {
        var log = new List<string>();
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.AddSingleton<ICommandInterceptor>(new OrderRecordingInterceptor("outer", log));
            services.AddSingleton<ICommandInterceptor>(
                new StubCommandInterceptor(CommandResult.Ok() with { ExitCode = 3 }));
        });

        var exception = await Assert.ThrowsAsync<CommandException>(async () =>
            await command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("unused")));

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Result.ExitCode).IsEqualTo(3);
            await Assert.That(log).IsEquivalentTo(["outer:before"]);
        }
    }

    [Test]
    public async Task AddCommandInterceptor_Generic_AddsTypeOnce()
    {
        var builder = TestPipelineBuilder.Create();

        builder.AddCommandInterceptor<CaptureInvocationInterceptor>()
            .AddCommandInterceptor<CaptureInvocationInterceptor>();

        await Assert.That(builder.Services.Count(descriptor =>
                descriptor.ServiceType == typeof(ICommandInterceptor)))
            .IsEqualTo(1);
    }

    [Test]
    public async Task AddCommandInterceptor_Instance_AddsEachInstanceAndRejectsNull()
    {
        var builder = TestPipelineBuilder.Create();

        builder.AddCommandInterceptor(new CaptureInvocationInterceptor())
            .AddCommandInterceptor(new CaptureInvocationInterceptor());

        using (Assert.Multiple())
        {
            await Assert.That(builder.Services.Count(descriptor =>
                    descriptor.ServiceType == typeof(ICommandInterceptor)))
                .IsEqualTo(2);
            await Assert.That(() => builder.AddCommandInterceptor(null!))
                .Throws<ArgumentNullException>();
        }
    }

    private async Task AssertCommandExposesObfuscatedEnvironmentVariables(bool dryRun)
    {
        const string secret = "command-result-secret-value";
        var (command, pipeline) = await GetService<ICommandContext>(_ => { });
        pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(secret);

        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("pwsh")
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

    [Test]
    [WindowsOnlyTest]
    [RequiresTool("pwsh")]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Command_Uses_Last_Windows_Environment_Name_Variant(bool dryRun)
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("pwsh")
            {
                Arguments = ["-NoProfile", "-Command", "[Console]::Write($env:MP_TEST_CASE)"],
            },
            new CommandExecutionOptions
            {
                InternalDryRun = dryRun,
                EnvironmentVariables = new Dictionary<string, string?>(StringComparer.Ordinal)
                {
                    ["MP_TEST_CASE"] = "first",
                    ["mp_test_case"] = "last",
                },
            });

        await Assert.That(result.EnvironmentVariables.Count).IsEqualTo(1);
        await Assert.That(result.EnvironmentVariables["MP_TEST_CASE"]).IsEqualTo("last");
        if (!dryRun)
        {
            await Assert.That(result.StandardOutput).IsEqualTo("last");
        }
    }

    private async Task AssertCommandPreservesUnixEnvironmentNameCasing(bool dryRun)
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("pwsh")
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
        var workingDirectory = Directory.CreateDirectory(Path.Combine(tempDirectory, "checkout")).FullName;

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %~1\r\n");
            await File.WriteAllTextAsync(
                Path.Combine(workingDirectory, "mp-runtime-test.cmd"),
                "@echo off\r\necho working-directory command\r\n");
            var command = await GetService<ICommandContext>();

            var result = await command.ExecuteCommandLineToolAsync(
                new CommandLineToolOptions("mp-runtime-test")
                {
                    Arguments = ["hello world"],
                },
                new CommandExecutionOptions
                {
                    WorkingDirectory = workingDirectory,
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
    [TUnit.Core.NotInParallel]
    [Arguments(".cmd")]
    [Arguments(".bat")]
    public async Task ExecuteCommandLineToolAsync_Uses_System_Script_Host_When_Comspec_Is_Unset(string extension)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var directory = Directory.CreateTempSubdirectory("mp script host tests ");
        var scriptDirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, "bin")).FullName;
        var originalComspec = Environment.GetEnvironmentVariable("COMSPEC");

        try
        {
            Environment.SetEnvironmentVariable("COMSPEC", null);
            await File.WriteAllTextAsync(
                Path.Combine(scriptDirectory, "mp-script-host-test" + extension),
                "@echo off\r\necho trusted host\r\n");
            // An invalid executable makes accidental selection of the working-directory host fail.
            await File.WriteAllBytesAsync(Path.Combine(directory.FullName, "cmd.exe"), []);
            var command = await GetService<ICommandContext>();

            var result = await command.ExecuteCommandLineToolAsync(
                new CommandLineToolOptions("mp-script-host-test"),
                new CommandExecutionOptions
                {
                    WorkingDirectory = directory.FullName,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = scriptDirectory,
                        ["PATHEXT"] = extension,
                    },
                });

            using (Assert.Multiple())
            {
                await Assert.That(result.ExitCode).IsEqualTo(0);
                await Assert.That(result.StandardOutput.Trim()).IsEqualTo("trusted host");
                await Assert.That(result.StandardError).IsEmpty();
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("COMSPEC", originalComspec);
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(".cmd", true)]
    [Arguments(".bat", true)]
    [Arguments(".cmd", false)]
    [Arguments(".bat", false)]
    public async Task ExecuteCommandLineToolAsync_Clears_Windows_Command_Script_Internal_Environment(
        string extension,
        bool withArguments)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var scriptPath = Path.Combine(tempDirectory, $"mp-runtime-environment-test{extension}");
        const string argument = "secret & echo injected | more < input > output %PATH% ^ !PATH!";

        try
        {
            await File.WriteAllTextAsync(
                scriptPath,
                "@echo off\r\nsetlocal DisableDelayedExpansion\r\n"
                + "set \"first=%~1\"\r\nset \"second=%~2\"\r\nset \"third=%~3\"\r\n"
                + "setlocal EnableDelayedExpansion\r\n"
                + "echo first:!first!\r\necho second:!second!\r\necho third:!third!\r\n"
                + "echo script environment:\r\nset MODULAR_PIPELINES_CMD_ 2>nul\r\n"
                + "echo descendant environment:\r\n\"%ComSpec%\" /d /v:off /c \"set MODULAR_PIPELINES_CMD_ 2>nul\"\r\n"
                + "echo public:!MP_TEST_PUBLIC!\r\nexit /b 23\r\n");
            var command = await GetService<ICommandContext>();

            var result = await command.ExecuteCommandLineToolAsync(
                new CommandLineToolOptions(scriptPath)
                {
                    Arguments = withArguments ? [argument, "", "trailing value"] : [],
                },
                new CommandExecutionOptions
                {
                    ThrowOnNonZeroExitCode = false,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["MP_TEST_PUBLIC"] = "public-value",
                    },
                });

            var expectedOutput = $"first:{(withArguments ? argument : "")}\r\nsecond:\r\n"
                + $"third:{(withArguments ? "trailing value" : "")}\r\n"
                + "script environment:\r\ndescendant environment:\r\npublic:public-value";

            using (Assert.Multiple())
            {
                await Assert.That(result.StandardOutput.Trim()).IsEqualTo(expectedOutput);
                await Assert.That(result.StandardError).IsEmpty();
                await Assert.That(result.ExitCode).IsEqualTo(23);
            }
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
                    new CommandLineToolOptions(scriptPath)
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
                new CommandLineToolOptions(scriptPath)
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
                new CommandLineToolOptions(relativeToolPath),
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
                new CommandLineToolOptions("mp-runtime-path-test"),
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
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_HostDeath_KillsCommand()
    {
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsLinux())
        {
            return;
        }

        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "pipeline-host", TimeSpan.Zero);
        var host = await fixture.WaitForProcessAsync("host", TimeSpan.FromSeconds(15));
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(15));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(15));

        // Kill only the pipeline host: no cancellation callbacks or managed disposal can run.
        host.Kill();
        await host.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(await WaitForExitAsync(parent, TimeSpan.FromSeconds(5))).IsTrue();
        if (OperatingSystem.IsWindows())
        {
            await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(5))).IsTrue();
        }
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_BackgroundChild_LivesUntilPipelineDisposal()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "pipeline-background-host", TimeSpan.Zero);
        var host = await fixture.WaitForProcessAsync("host", TimeSpan.FromSeconds(15));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(15));
        await fixture.WaitForReadyAsync("command-completed", TimeSpan.FromSeconds(15));
        await Assert.That(child.HasExited).IsFalse();
        await fixture.TriggerAsync("dispose-pipeline");
        await fixture.WaitForReadyAsync("pipeline-disposed", TimeSpan.FromSeconds(5));
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(5))).IsTrue();
        await Assert.That(host.HasExited).IsFalse();
        await fixture.TriggerAsync("host-exit");
        await fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [Timeout(30_000)]
    public async Task Command_Completes_When_Descendant_Keeps_Output_Open(CancellationToken cancellationToken)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "parent-exit", TimeSpan.FromMilliseconds(50));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        await fixture.TriggerAsync("parent-exit");

        var result = await fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);

        await Assert.That(result.ExitCode).IsEqualTo(0);
        await Assert.That(child.HasExited).IsFalse();
        await Assert.That(result.OutputMayBeIncomplete).IsTrue();
        await Assert.That(result.StandardOutput).IsEqualTo("parent output");
        await Assert.That(result.StandardError).IsEqualTo("parent error");
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_BackgroundChild_DiesWithHostAfterCommandCompletes()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "pipeline-background-host", TimeSpan.Zero);
        var host = await fixture.WaitForProcessAsync("host", TimeSpan.FromSeconds(15));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(15));
        await fixture.WaitForReadyAsync("command-completed", TimeSpan.FromSeconds(15));
        await Assert.That(child.HasExited).IsFalse();
        host.Kill();
        await host.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(5))).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [Arguments(0, false, 0)]
    [Arguments(0, false, 250)]
    [Arguments(23, false, 250)]
    [Arguments(23, true, 250)]
    [Timeout(30_000)]
    public async Task Command_Drain_Timeout_Reports_Incomplete_Output_And_Preserves_Exit_Code(
        int exitCode, bool throwOnFailure, int drainMilliseconds, CancellationToken cancellationToken)
    {
        var commandLogger = new Mock<ICommandLogger>();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.Replace(ServiceDescriptor.Scoped(_ => commandLogger.Object)));
        var options = new CommandExecutionOptions
        {
            OutputDrainTimeout = TimeSpan.FromMilliseconds(drainMilliseconds),
            ThrowOnNonZeroExitCode = throwOnFailure,
        };
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(command, exitCode == 0 ? "parent-exit" : "parent-error-exit",
            TimeSpan.FromMilliseconds(50), executionOptions: options);
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        await fixture.TriggerAsync("parent-exit");

        CommandResult result;
        if (throwOnFailure)
        {
            var failure = await Assert.ThrowsAsync<CommandException>(
                () => fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken));
            result = failure!.Result;
        }
        else
        {
            result = await fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        }

        await Assert.That(result.ExitCode).IsEqualTo(exitCode);
        await Assert.That(result.OutputMayBeIncomplete).IsTrue();
        if (drainMilliseconds > 0)
        {
            await Assert.That(result.StandardOutput).IsEqualTo("parent output");
            await Assert.That(result.StandardError).IsEqualTo("parent error");
        }

        await Assert.That(child.HasExited).IsFalse();
        commandLogger.Verify(logger => logger.LogOutputDrainTimeout(
            It.IsAny<CommandLineToolOptions>(), It.Is<CommandExecutionOptions>(value => value.OutputDrainTimeout == options.OutputDrainTimeout)), Times.Once());
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task Command_Drains_Descendant_Output_Within_Grace_Period(bool infinite, CancellationToken cancellationToken)
    {
        var commandLogger = new Mock<ICommandLogger>();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.Replace(ServiceDescriptor.Scoped(_ => commandLogger.Object)));
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(command, "draining-parent", TimeSpan.FromMilliseconds(50), executionOptions: new CommandExecutionOptions
        {
            OutputDrainTimeout = infinite ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(10),
        });
        await fixture.WaitForReadyProcessAsync("draining-child", TimeSpan.FromSeconds(10));
        var parent = await fixture.WaitForProcessAsync("parent", TimeSpan.FromSeconds(5));
        await fixture.TriggerAsync("parent-exit");
        await parent.WaitForExitAsync(cancellationToken).WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        await Assert.That(fixture.Execution.IsCompleted).IsFalse();
        await fixture.TriggerAsync("child-exit");

        var result = await fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);

        await Assert.That(result.ExitCode).IsEqualTo(0);
        await Assert.That(result.OutputMayBeIncomplete).IsFalse();
        await Assert.That(result.StandardOutput).IsEqualTo("parent outputchild output");
        await Assert.That(result.StandardError).IsEqualTo("parent errorchild error");
        commandLogger.Verify(logger => logger.LogOutputDrainTimeout(
            It.IsAny<CommandLineToolOptions>(), It.IsAny<CommandExecutionOptions>()), Times.Never());
    }

    [Test]
    [Arguments(-2)]
    [Arguments(4294967295)]
    public async Task Invalid_Output_Drain_Timeout_Is_Rejected_Before_Starting_Process(long milliseconds)
    {
        var buffer = new BoundedCommandOutputBuffer(0);
        var target = new CommandOutputTarget(buffer, null, null);
        var started = false;

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ProcessRunner.RunAsync(
            new ProcessCommand("unused", [], string.Empty, Environment.CurrentDirectory, new Dictionary<string, string?>(), null),
            target, target, Encoding.UTF8, TimeSpan.FromMilliseconds(milliseconds), _ => started = true, CancellationToken.None));

        await Assert.That(started).IsFalse();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_KillsDescendantProcesses()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "parent", TimeSpan.FromMilliseconds(50));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(5));
        fixture.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await fixture.Execution);
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(2))).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_WindowsCancellation_DoesNotWaitForDefaultGracefulTimeout()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "parent", new CommandExecutionOptions().GracefulShutdownTimeout);
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(10));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        fixture.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5)));
        await Assert.That(await WaitForExitAsync(parent, TimeSpan.FromSeconds(2))).IsTrue();
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(2))).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_LogsEachTermination()
    {
        var commandLogger = new Mock<ICommandLogger>();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.Replace(ServiceDescriptor.Scoped(_ => commandLogger.Object)));
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(command, "parent", TimeSpan.FromMilliseconds(50));
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(5));
        await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(5));
        fixture.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await fixture.Execution);

        commandLogger.Verify(logger => logger.LogProcessTermination(
            It.Is<ProcessTerminationTarget>(target => target.ProcessId == parent.Id && target.ProcessName != null),
            ProcessTerminationReason.ForcefulCancellation,
            It.IsAny<ProcessTerminationMethod>()), Times.AtLeastOnce());
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_KillsWhenTerminationLoggingThrows()
    {
        var commandLogger = new Mock<ICommandLogger>();
        commandLogger
            .Setup(logger => logger.LogProcessTermination(
                It.IsAny<ProcessTerminationTarget>(),
                It.IsAny<ProcessTerminationReason>(),
                It.IsAny<ProcessTerminationMethod>()))
            .Throws(new InvalidOperationException("logger failed"));
        var (command, _) = await GetService<ICommandContext>(services =>
            services.Replace(ServiceDescriptor.Scoped(_ => commandLogger.Object)));
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(command, "parent", TimeSpan.FromMilliseconds(50));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(5));
        fixture.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await fixture.Execution);
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(2))).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [Arguments("parent")]
    [Arguments("threaded-parent")]
    public async Task ProcessTree_Kill_Kills_Process_And_Descendants(string role)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), role, TimeSpan.FromMilliseconds(50));
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(5));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(5));

        ModularPipelines.Helpers.Internal.ProcessTree.Kill(parent);

        using (Assert.Multiple())
        {
            await Assert.That(await WaitForExitAsync(parent, TimeSpan.FromSeconds(5))).IsTrue();
            await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(5))).IsTrue();
        }
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [LinuxOnlyTest]
    [RequiresTool("dotnet")]
    public async Task ProcessTree_Discovers_Children_Spawned_By_Worker_Threads()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "threaded-parent", TimeSpan.FromMilliseconds(50));
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(10));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));

        await Assert.That(ProcessTree.GetDescendantProcessIds(parent.Id)).Contains(child.Id);
        fixture.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await fixture.Execution);
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(2))).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [UnixOnlyTest]
    [Timeout(30_000)]
    public async Task ExecuteCommandLineToolAsync_GracefulCancellation_InterruptsDescendants(
        CancellationToken cancellationToken)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "interruptible-parent", TimeSpan.FromSeconds(30),
            gateForcefulCancellation: true);
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(10));
        var child = await fixture.WaitForReadyProcessAsync("interruptible-child", TimeSpan.FromSeconds(10));
        var grandchild = await fixture.WaitForReadyProcessAsync("interruptible-grandchild", TimeSpan.FromSeconds(10));

        fixture.Cancel();

        // The forceful gate stays closed: every generation must handle SIGINT and finish cleanup.
        await Assert.ThrowsAsync<OperationCanceledException>(() => fixture.Execution.WaitAsync(cancellationToken));
        await Assert.That(File.Exists(Path.Combine(fixture.DirectoryPath, "interruptible-child-interrupted.ready"))).IsTrue();
        await Assert.That(File.Exists(Path.Combine(fixture.DirectoryPath, "interruptible-grandchild-interrupted.ready"))).IsTrue();
        await Assert.That(await WaitForExitAsync(parent, TimeSpan.FromSeconds(2))).IsTrue();
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(2))).IsTrue();
        await Assert.That(await WaitForExitAsync(grandchild, TimeSpan.FromSeconds(2))).IsTrue();
    }

    [Test]
    [RequiresTool("dotnet")]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_KillsDescendantAfterParentExits()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "parent-exit", TimeSpan.FromMilliseconds(100));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(5));
        fixture.Cancel();
        await fixture.TriggerAsync("parent-exit");

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await fixture.Execution);
        await Assert.That(await WaitForExitAsync(child, TimeSpan.FromSeconds(2))).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [Timeout(30_000)]
    public async Task ExecuteCommandLineToolAsync_GracefulExit_DoesNotWaitForForcefulTimeout(
        CancellationToken cancellationToken)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "graceful-exit", TimeSpan.FromMilliseconds(50),
            gateForcefulCancellation: true);
        await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(10));
        fixture.Cancel();
        await fixture.TriggerAsync("parent-exit");

        // The forceful cancellation gate stays closed: completion must come from the process exit.
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => fixture.Execution.WaitAsync(cancellationToken));
        await Assert.That(fixture.Execution.IsCompleted).IsTrue();
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task Interceptor_Dropping_Its_Token_Still_Honours_ExecutionTimeout()
    {
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(new DropCancellationInterceptor()));

        var exception = await Assert.ThrowsAsync<TimeoutException>(() =>
            command.ExecuteCommandLineToolAsync(
                new CommandLineToolOptions("pwsh")
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
    [RequiresTool("pwsh")]
    public async Task ExecuteCommandLineToolAsync_ExecutionTimeout_ThrowsTimeoutException()
    {
        var command = await GetService<ICommandContext>();

        var exception = await Assert.ThrowsAsync<TimeoutException>(() =>
            command.ExecuteCommandLineToolAsync(
                new CommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", "Start-Sleep -Seconds 60"],
                },
                new CommandExecutionOptions
                {
                    ExecutionTimeout = TimeSpan.FromMilliseconds(100),
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(50),
                }));

        await Assert.That(exception!.Message).Contains("timed out after");
        await Assert.That(exception.Message).Contains("Raise CommandExecutionOptions.ExecutionTimeout");
    }

    [Test]
    public async Task TimeoutException_For_Default_ExecutionTimeout_Names_The_Option()
    {
        var innerException = new OperationCanceledException();

        var exception = Command.CreateTimeoutException(new CommandExecutionOptions(), innerException);

        await Assert.That(exception.Message).Contains("timed out after 00:30:00");
        await Assert.That(exception.Message).Contains("the default CommandExecutionOptions.ExecutionTimeout");
        await Assert.That(exception.Message).Contains("Raise CommandExecutionOptions.ExecutionTimeout");
        await Assert.That(exception.Message).Contains("set it to null");
        await Assert.That(exception.InnerException).IsSameReferenceAs(innerException);
    }

    [Test]
    public async Task TimeoutException_For_Custom_ExecutionTimeout_Names_The_Value_And_Option()
    {
        var exception = Command.CreateTimeoutException(
            new CommandExecutionOptions { ExecutionTimeout = TimeSpan.FromMinutes(5) },
            new OperationCanceledException());

        await Assert.That(exception.Message).Contains("timed out after 00:05:00 (CommandExecutionOptions.ExecutionTimeout)");
        await Assert.That(exception.Message).DoesNotContain("default");
        await Assert.That(exception.Message).Contains("Raise CommandExecutionOptions.ExecutionTimeout");
    }

    [Test]
    [Arguments(30_000)]
    [Arguments(-1)]
    public async Task ScheduleForcefulCancellationAsync_Windows_IgnoresGracefulTimeout(int timeoutMilliseconds)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var forcefulCancellationToken = new CancellationTokenSource();
        await Command.ScheduleForcefulCancellationAsync(
            forcefulCancellationToken,
            TimeSpan.FromMilliseconds(timeoutMilliseconds),
            null);

        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            await Task.Delay(TimeSpan.FromSeconds(1), forcefulCancellationToken.Token));
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
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ExecuteCommandLineToolAsync_ForcefulCancellation_CapturesDescendantSpawnedDuringGrace(bool waitForParentExit)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "grace-parent", TimeSpan.FromSeconds(1), gateForcefulCancellation: true);
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(10));
        // Default command startup attaches the Windows job before Start returns. Keep the
        // initial child out of that startup race so this test isolates capture during grace.
        await fixture.TriggerAsync("start-child");
        await fixture.WaitForReadyProcessAsync("intermediate", TimeSpan.FromSeconds(10));
        fixture.Cancel();
        await fixture.TriggerAsync("spawn");
        var grandchild = await fixture.WaitForReadyProcessAsync("grandchild", TimeSpan.FromSeconds(10));
        if (waitForParentExit)
        {
            await parent.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
        }

        fixture.AllowForcefulCancellation();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await fixture.Execution);
        await Assert.That(await WaitForExitAsync(grandchild, TimeSpan.FromSeconds(2))).IsTrue();
    }

    [Test]
    public async Task WaitForForcefulCancellationAsync_Waits_For_Pending_Termination_Callback()
    {
        using var jobs = new WindowsJobLifetime();
        using var terminator = new Command.ProcessTreeTerminator(Mock.Of<ICommandLogger>(), jobs);
        using var gracefulCancellation = new CancellationTokenSource();
        using var forcefulCancellation = new CancellationTokenSource();
        using var allowTermination = new ManualResetEventSlim();
        var cancellationStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var terminationRegistration = forcefulCancellation.Token.Register(terminator.Kill);
        // Cancellation callbacks run in reverse registration order. Hold dispatch after the
        // token changes state, before the termination callback has been selected to run.
        using var blockingRegistration = forcefulCancellation.Token.Register(() =>
        {
            cancellationStarted.TrySetResult();
            if (!allowTermination.Wait(TimeSpan.FromSeconds(5)))
            {
                throw new TimeoutException("Test did not release cancellation dispatch.");
            }
        });
        gracefulCancellation.Cancel();
        var cancellation = Task.Run(forcefulCancellation.Cancel);
        try
        {
            await cancellationStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
            var wait = Command.WaitForForcefulCancellationAsync(
                terminator, gracefulCancellation.Token, forcefulCancellation.Token);

            await Assert.That(wait.IsCompleted).IsFalse();
            allowTermination.Set();
            await wait.WaitAsync(TimeSpan.FromSeconds(5));
        }
        finally
        {
            allowTermination.Set();
            await cancellation.WaitAsync(TimeSpan.FromSeconds(5));
        }
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    [Arguments("startup-failure", "parent")]
    [Arguments("child-failure-parent", "failing-child")]
    public async Task ProcessTreeFixture_Reports_Startup_Failure_Before_Readiness_Timeout(string role, string processName)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), role, TimeSpan.FromMilliseconds(50));
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            fixture.WaitForReadyAsync(processName, TimeSpan.FromSeconds(30)));
        await Assert.That(exception!.Message).Contains("Requested process-fixture startup failure.");
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ProcessTreeFixture_Reads_Identity_While_Publication_Handle_Is_Open()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "graceful-exit", TimeSpan.FromMilliseconds(50));
        await fixture.WaitForReadyAsync("parent", TimeSpan.FromSeconds(5));
        using var publicationHandle = new FileStream(Path.Combine(fixture.DirectoryPath, "parent.pid"),
            FileMode.Open, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete);

        // Identity sharing needs a live process, not a concurrently starting descendant.
        var process = await fixture.WaitForProcessAsync("parent", TimeSpan.FromSeconds(5));
        await Assert.That(process.HasExited).IsFalse();

        await fixture.TriggerAsync("parent-exit");
        var result = await fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(result.ExitCode).IsEqualTo(0);
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ProcessTreeFixture_Cleans_Up_When_Readiness_Fails_Before_Cancellation()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "never-ready", TimeSpan.FromMilliseconds(50));
        var parent = await fixture.WaitForProcessAsync("parent", TimeSpan.FromSeconds(5));
        var child = await fixture.WaitForProcessAsync("child", TimeSpan.FromSeconds(5));
        using var observer = Process.GetProcessById(parent.Id);
        using var childObserver = Process.GetProcessById(child.Id);
        var exception = await Assert.ThrowsAsync<TimeoutException>(() =>
            fixture.WaitForReadyAsync("parent", TimeSpan.FromMilliseconds(100)));
        await Assert.That(exception!.Message).Contains("parent.pid:");
        await fixture.DisposeAsync();
        await Assert.That(observer.HasExited).IsTrue();
        await Assert.That(childObserver.HasExited).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ProcessTreeFixture_Waits_For_Retained_Process_When_Cleanup_Lookup_Fails()
    {
        await using var ownerFixture = new ProcessTreeFixture();
        ownerFixture.Start(await GetService<ICommandContext>(), "graceful-exit", TimeSpan.FromMilliseconds(50));
        var parent = await ownerFixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(5));
        var lookupObserved = false;
        await using var fixture = new ProcessTreeFixture(_ =>
        {
            lookupObserved = true;
            return null;
        });
        var identity = ModularPipelines.ProcessTestHost.ProcessIdentity.Capture(parent);
        await File.WriteAllTextAsync(Path.Combine(fixture.DirectoryPath, "parent.pid"),
            System.Text.Json.JsonSerializer.Serialize(identity));
        await fixture.WaitForProcessAsync("parent", TimeSpan.FromSeconds(5));

        // Only the owner can release the live process; the failed lookup must not end cleanup.
        var cleanup = fixture.DisposeAsync().AsTask();
        await Assert.That(lookupObserved).IsTrue();
        await Assert.That(cleanup.IsCompleted).IsFalse();

        await ownerFixture.TriggerAsync("parent-exit");
        await cleanup.WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(Directory.Exists(fixture.DirectoryPath)).IsFalse();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ProcessTreeFixture_Cleans_Up_Child_Whose_Parent_Never_Published_It()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "orphaning-parent", TimeSpan.FromMilliseconds(50));
        await fixture.WaitForReadyAsync("child", TimeSpan.FromSeconds(10));
        var child = await fixture.WaitForProcessAsync("child.self", TimeSpan.FromSeconds(5));
        using var childObserver = Process.GetProcessById(child.Id);

        // The orphan holds the command's output pipes, so cleanup only completes if it finds the child.
        await fixture.DisposeAsync();

        await Assert.That(childObserver.HasExited).IsTrue();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ProcessTreeFixture_Does_Not_Kill_A_Reused_Process_Id()
    {
        await using var otherFixture = new ProcessTreeFixture();
        otherFixture.Start(await GetService<ICommandContext>(), "parent", TimeSpan.FromMilliseconds(50));
        var parent = await otherFixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(5));
        var child = await otherFixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(5));
        await using var staleFixture = new ProcessTreeFixture();
        var identity = ModularPipelines.ProcessTestHost.ProcessIdentity.Capture(parent);
        var staleIdentity = identity with { StartTimeValue = identity.StartTimeValue - 1 };
        await File.WriteAllTextAsync(Path.Combine(staleFixture.DirectoryPath, "stale.pid"),
            System.Text.Json.JsonSerializer.Serialize(staleIdentity));

        await staleFixture.DisposeAsync();

        await Assert.That(parent.HasExited).IsFalse();
        await Assert.That(child.HasExited).IsFalse();
    }

    [Test]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    [RequiresTool("dotnet")]
    public async Task ProcessTreeFixture_Kills_Child_When_Identity_Publication_Fails()
    {
        await using var fixture = new ProcessTreeFixture();
        Directory.CreateDirectory(Path.Combine(fixture.DirectoryPath, "child.pid"));
        fixture.Start(await GetService<ICommandContext>(), "parent", TimeSpan.FromMilliseconds(50));
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            fixture.WaitForReadyAsync("unpublished-child", TimeSpan.FromSeconds(5)));
        var identityJson = await File.ReadAllTextAsync(Directory.GetFiles(fixture.DirectoryPath, "child.pid.*.tmp").Single());
        // Retain fallback ownership even when this regression fails before the fix.
        await File.WriteAllTextAsync(Path.Combine(fixture.DirectoryPath, "failed-child.pid"), identityJson);
        var identity = System.Text.Json.JsonSerializer.Deserialize<ModularPipelines.ProcessTestHost.ProcessIdentity>(identityJson);
        var childIsRunning = false;
        try
        {
            using var child = Process.GetProcessById(identity.Id);
            childIsRunning = !child.HasExited && ModularPipelines.ProcessTestHost.ProcessIdentity.Capture(child) == identity;
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
        {
            // The child exited before its process state could be inspected.
        }

        await Assert.That(childIsRunning).IsFalse();
    }

    [Test]
    [Arguments("dotnet")]
    [Arguments("worker ) with (spaces)")]
    public async Task ProcessIdentity_Uses_Linux_Kernel_Start_Time(string processName)
    {
        var stat = $"123 ({processName}) R {string.Join(' ', Enumerable.Range(4, 18))} 987654321 0";
        await Assert.That(ModularPipelines.ProcessTestHost.ProcessIdentity.ParseLinuxStartTime(stat)).IsEqualTo(987654321L);
    }

    private static string EscapePowerShellLiteral(string value) => value.Replace("'", "''");

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
