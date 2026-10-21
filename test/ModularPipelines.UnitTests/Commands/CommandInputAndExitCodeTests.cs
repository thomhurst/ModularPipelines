using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;
using ModularPipelines.ProcessTestHost;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using ModularPipelines.Tracing;
using NReco.Logging.File;

namespace ModularPipelines.UnitTests.Commands;

public class CommandInputAndExitCodeTests : TestBase
{
    [Test]
    public async Task Startup_Failure_Is_Not_Logged_As_An_Accepted_Negative_Exit_Code()
    {
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");
        var executable = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N"));
        var (command, pipeline) = await GetService<ICommandContext>(services =>
        {
            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            services.AddLogging(builder => builder.AddFile(file));
        });

        await Assert.ThrowsAsync<ToolNotFoundException>(() => command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions(executable),
            new CommandExecutionOptions { SuccessExitCodes = [-1] }));

        await pipeline.DisposeAsync();
        var log = await File.ReadAllTextAsync(file);
        await Assert.That(log).Contains($"✗ {executable}");
        await Assert.That(log).DoesNotContain($"✓ {executable}");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Input_Does_Not_Register_Secrets_By_Default(bool rewriteOptions)
    {
        var input = string.Join('\n', Enumerable.Range(0, 2048).Select(index => $"script line {index}: café 東京 \"value\""));
        var (command, pipeline) = await GetService<ICommandContext>(services =>
        {
            if (rewriteOptions)
            {
                services.AddSingleton<ICommandInterceptor>(new StandardInputInterceptor(input));
            }
        });
        var secrets = pipeline.Services.GetRequiredService<ISecretProvider>();
        var registeredSecrets = secrets.GetSnapshot().Secrets;

        var result = await command.ExecuteCommandLineToolAsync(
            CreateCommand("exit-code", "0"),
            new CommandExecutionOptions { StandardInput = rewriteOptions ? null : input });

        await Assert.That(result.ExitCode).IsEqualTo(0);
        await Assert.That(secrets.GetSnapshot().Secrets).IsEquivalentTo(registeredSecrets);
    }

    [Test]
    public async Task Large_Input_Masks_Registered_Values_Without_Registering_The_Document()
    {
        const string secret = "embedded-stdin-password";
        var input = new string('x', 64 * 1024) + "\n" + secret + "\n";
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");
        var (command, pipeline) = await GetService<ICommandContext>(services =>
        {
            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            services.AddLogging(builder => builder.AddFile(file));
        });
        pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(secret);
        var secrets = pipeline.Services.GetRequiredService<ISecretProvider>();
        // Deferred publication can change the cache version without changing the registered values.
        var registeredSecrets = secrets.GetSnapshot().Secrets;

        var result = await command.ExecuteCommandLineToolAsync(
            CreateCommand("stdin-echo", "0", "0"),
            new CommandExecutionOptions { StandardInput = input });

        await Assert.That(result.StandardOutput).IsEqualTo(input);
        await Assert.That(secrets.GetSnapshot().Secrets).IsEquivalentTo(registeredSecrets);
        await pipeline.DisposeAsync();
        var log = await File.ReadAllTextAsync(file);
        await Assert.That(log).DoesNotContain(secret);
        await Assert.That(log).Contains("********");
    }

    [Test]
    public async Task Rejects_Null_Success_Exit_Codes_At_Initialization()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CommandExecutionOptions { SuccessExitCodes = null! });

        await Assert.That(exception!.ParamName).IsEqualTo(nameof(CommandExecutionOptions.SuccessExitCodes));
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("café 東京 🚀\nsecond line\n")]
    public async Task Writes_Exact_Utf8_Input_And_Closes_Stdin(string? input)
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            CreateCommand("stdin-echo", "0", "0"),
            new CommandExecutionOptions { StandardInput = input });

        await Assert.That(result.StandardOutput).IsEqualTo(input ?? string.Empty);
        await Assert.That(result.StandardError).IsEqualTo(input ?? string.Empty);
    }

    [Test]
    public async Task Writes_Input_While_Draining_Both_Output_Streams()
    {
        var input = new string('y', 256 * 1024);
        var prefix = new string('x', 128 * 1024);
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            CreateCommand("stdin-echo", prefix.Length.ToString(), "0"),
            new CommandExecutionOptions { StandardInput = input, ExecutionTimeout = TimeSpan.FromSeconds(15) });

        await Assert.That(result.StandardOutput).IsEqualTo(prefix + input);
        await Assert.That(result.StandardError).IsEqualTo(prefix + input);
    }

    [Test]
    [Arguments(0)]
    [Arguments(7)]
    public async Task Early_Exit_While_Writing_Input_Preserves_Exit_Code(int exitCode)
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            CreateCommand("exit-code", exitCode.ToString()),
            new CommandExecutionOptions { StandardInput = new string('y', 256 * 1024), SuccessExitCodes = [0, 7] });

        await Assert.That(result.ExitCode).IsEqualTo(exitCode);
    }

    [Test]
    public async Task Timeout_Cancels_A_Blocked_Input_Write()
    {
        var command = await GetService<ICommandContext>();
        await Assert.ThrowsAsync<TimeoutException>(() => command.ExecuteCommandLineToolAsync(
            CreateCommand("stdin-block"),
            new CommandExecutionOptions
            {
                StandardInput = new string('y', 256 * 1024),
                ExecutionTimeout = TimeSpan.FromMilliseconds(500),
                GracefulShutdownTimeout = TimeSpan.Zero,
            }));
    }

    [Test]
    public async Task Caller_Cancellation_Cancels_A_Blocked_Input_Write()
    {
        var command = await GetService<ICommandContext>();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
        await Assert.ThrowsAsync<OperationCanceledException>(() => command.ExecuteCommandLineToolAsync(
            CreateCommand("stdin-block"),
            new CommandExecutionOptions
            {
                StandardInput = new string('y', 256 * 1024),
                GracefulShutdownTimeout = TimeSpan.Zero,
            },
            cancellation.Token));
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(7, false)]
    [Arguments(0, true)]
    [Arguments(7, true)]
    public async Task Secret_Input_Is_Masked_In_Logs_And_Failure_Diagnostics(int exitCode, bool rewriteOptions)
    {
        const string secret = "stdin-password-secret";
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");
        var (command, pipeline) = await GetService<ICommandContext>(services =>
        {
            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            services.AddLogging(builder => builder.AddFile(file));
            if (rewriteOptions)
            {
                services.AddSingleton<ICommandInterceptor>(new StandardInputInterceptor(secret + "\n", isSecret: true));
            }
        });
        var options = new CommandExecutionOptions
        {
            StandardInput = rewriteOptions ? null : secret + "\n",
            StandardInputIsSecret = !rewriteOptions,
        };
        var tool = CreateCommand("stdin-echo", "0", exitCode.ToString());
        if (exitCode == 0)
        {
            var result = await command.ExecuteCommandLineToolAsync(tool, options);
            await Assert.That(result.StandardOutput).IsEqualTo(secret + "\n");
            await Assert.That(result.CommandInput).DoesNotContain(secret);
        }
        else
        {
            var exception = await Assert.ThrowsAsync<CommandException>(() => command.ExecuteCommandLineToolAsync(tool, options));
            await Assert.That(exception!.ToString()).DoesNotContain(secret);
            await Assert.That(exception.Result.StandardOutput).DoesNotContain(secret);
            await Assert.That(exception.Result.StandardError).DoesNotContain(secret);
        }

        await pipeline.DisposeAsync();
        var log = await File.ReadAllTextAsync(file);
        await Assert.That(log).DoesNotContain(secret);
        await Assert.That(log).Contains("********");
    }

    [Test]
    [Arguments(0)]
    [Arguments(2)]
    [Arguments(7)]
    public async Task Accepts_Configured_Exit_Codes(int exitCode)
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            CreateCommand("exit-code", exitCode.ToString()),
            new CommandExecutionOptions { SuccessExitCodes = [0, 2, 7] });

        await Assert.That(result.ExitCode).IsEqualTo(exitCode);
    }

    [Test]
    [Arguments(0)]
    [Arguments(3)]
    public async Task Rejects_Exit_Codes_Outside_Configured_Collection(int exitCode)
    {
        var command = await GetService<ICommandContext>();
        var exception = await Assert.ThrowsAsync<CommandException>(() => command.ExecuteCommandLineToolAsync(
            CreateCommand("exit-code", exitCode.ToString()),
            new CommandExecutionOptions { SuccessExitCodes = [2] }));

        await Assert.That(exception!.Result.ExitCode).IsEqualTo(exitCode);
    }

    [Test]
    [Arguments(0, true)]
    [Arguments(3, true)]
    [Arguments(3, false)]
    public async Task Empty_Collection_And_Disabled_Throwing_Are_Respected(int exitCode, bool throwOnFailure)
    {
        var command = await GetService<ICommandContext>();
        var execution = command.ExecuteCommandLineToolAsync(
            CreateCommand("exit-code", exitCode.ToString()),
            new CommandExecutionOptions { SuccessExitCodes = [], ThrowOnNonZeroExitCode = throwOnFailure });
        if (throwOnFailure)
        {
            var exception = await Assert.ThrowsAsync<CommandException>(async () => await execution);
            await Assert.That(exception!.Result.ExitCode).IsEqualTo(exitCode);
        }
        else
        {
            await Assert.That((await execution).ExitCode).IsEqualTo(exitCode);
        }
    }

    [Test]
    [Arguments(0, true)]
    [Arguments(3, false)]
    public async Task Default_Accepts_Only_Zero(int exitCode, bool succeeds)
    {
        var command = await GetService<ICommandContext>();
        var execution = command.ExecuteCommandLineToolAsync(CreateCommand("exit-code", exitCode.ToString()));
        if (succeeds)
        {
            await Assert.That((await execution).ExitCode).IsEqualTo(exitCode);
        }
        else
        {
            var exception = await Assert.ThrowsAsync<CommandException>(async () => await execution);
            await Assert.That(exception!.Result.ExitCode).IsEqualTo(exitCode);
        }
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    public async Task Interceptor_Results_Use_Configured_Exit_Codes(int exitCode, bool succeeds)
    {
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(new ExitCodeInterceptor(exitCode)));
        var execution = command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("unused"),
            new CommandExecutionOptions { SuccessExitCodes = [2] });
        if (succeeds)
        {
            await Assert.That((await execution).ExitCode).IsEqualTo(exitCode);
        }
        else
        {
            var exception = await Assert.ThrowsAsync<CommandException>(async () => await execution);
            await Assert.That(exception!.Result.ExitCode).IsEqualTo(exitCode);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Accepted_Nonzero_Exit_Code_Has_Successful_Telemetry(bool rewriteOptions)
    {
        var activities = new ConcurrentBag<Activity>();
        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == ModuleActivityTracing.CommandSourceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
            ActivityStopped = activities.Add,
        };
        ActivitySource.AddActivityListener(listener);
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            if (rewriteOptions)
            {
                services.AddSingleton<ICommandInterceptor>(new SuccessCodesInterceptor());
            }
        });
        var result = await command.ExecuteCommandLineToolAsync(
            CreateCommand("exit-code", "2"),
            new CommandExecutionOptions { SuccessExitCodes = rewriteOptions ? [0] : [2] });

        var activity = activities.Single(activity => Equals(activity.GetTagItem(ModuleActivityTracing.CommandExitCodeTag), result.ExitCode));
        await Assert.That(activity.Status).IsEqualTo(ActivityStatusCode.Unset);
    }

    private sealed class SuccessCodesInterceptor : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(CommandInvocation invocation, CommandDelegate next, CancellationToken cancellationToken) =>
            next(invocation with { ExecutionOptions = invocation.ExecutionOptions with { SuccessExitCodes = [2] } }, cancellationToken);
    }

    private sealed class StandardInputInterceptor(string input, bool isSecret = false) : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(CommandInvocation invocation, CommandDelegate next, CancellationToken cancellationToken) =>
            next(invocation with { ExecutionOptions = invocation.ExecutionOptions with { StandardInput = input, StandardInputIsSecret = isSecret } }, cancellationToken);
    }

    private sealed class ExitCodeInterceptor(int exitCode) : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(CommandInvocation invocation, CommandDelegate next, CancellationToken cancellationToken) =>
            ValueTask.FromResult(CommandResult.Ok() with { ExitCode = exitCode });
    }

    private static CommandLineToolOptions CreateCommand(params string[] arguments) => new("dotnet")
    {
        Arguments = ["exec", "--runtimeconfig", Path.ChangeExtension(typeof(CommandInputAndExitCodeTests).Assembly.Location, ".runtimeconfig.json"), typeof(Host).Assembly.Location, .. arguments],
    };
}
