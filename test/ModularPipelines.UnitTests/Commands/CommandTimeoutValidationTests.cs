using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Commands;

public class CommandTimeoutValidationTests : TestBase
{
    [Test]
    [Arguments(false, 0L)]
    [Arguments(false, -2L)]
    [Arguments(false, 4294967295L)]
    [Arguments(false, long.MaxValue / TimeSpan.TicksPerMillisecond)]
    [Arguments(true, -2L)]
    [Arguments(true, 4294967295L)]
    [Arguments(true, long.MaxValue / TimeSpan.TicksPerMillisecond)]
    public async Task Invalid_Timeout_Throws_Synchronously_Before_Process_Launch(bool gracefulShutdown, long milliseconds)
    {
        var (command, pipeline) = await GetService<ICommandContext>(null);
        await using var pipelineLifetime = pipeline;
        var options = CreateOptions(gracefulShutdown, TimeSpan.FromMilliseconds(milliseconds));

        // A missing executable would fail differently if validation reached process launch.
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("missing-timeout-test-executable"), options);
        });

        await Assert.That(exception!.ParamName).IsEqualTo(gracefulShutdown
            ? nameof(CommandExecutionOptions.GracefulShutdownTimeout)
            : nameof(CommandExecutionOptions.ExecutionTimeout));
    }

    [Test]
    [Arguments(false, -1L)]
    [Arguments(false, 10000L)]
    [Arguments(false, 4294967294L)]
    [Arguments(true, -1L)]
    [Arguments(true, 0L)]
    [Arguments(true, 4294967294L)]
    public async Task Supported_Timeout_Is_Accepted(bool gracefulShutdown, long milliseconds)
    {
        var (command, pipeline) = await GetService<ICommandContext>(null);
        await using var pipelineLifetime = pipeline;
        var options = CreateOptions(gracefulShutdown, TimeSpan.FromMilliseconds(milliseconds)) with { InternalDryRun = true };

        var result = await command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("unused"), options);

        await Assert.That(result.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task Null_ExecutionTimeout_Disables_Deadline()
    {
        var (command, pipeline) = await GetService<ICommandContext>(null);
        await using var pipelineLifetime = pipeline;

        var result = await command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("unused"),
            new CommandExecutionOptions { ExecutionTimeout = null, InternalDryRun = true });

        await Assert.That(result.ExitCode).IsEqualTo(0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Interceptor_Cannot_Introduce_Invalid_Timeout(bool gracefulShutdown)
    {
        var (command, pipeline) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(new InvalidTimeoutInterceptor(gracefulShutdown)));
        await using var pipelineLifetime = pipeline;

        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("missing-timeout-test-executable")));

        await Assert.That(exception!.ParamName).IsEqualTo(gracefulShutdown
            ? nameof(CommandExecutionOptions.GracefulShutdownTimeout)
            : nameof(CommandExecutionOptions.ExecutionTimeout));
    }

    private sealed class InvalidTimeoutInterceptor(bool gracefulShutdown) : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(CommandInvocation invocation, CommandDelegate next, CancellationToken cancellationToken) =>
            next(invocation with
            {
                ExecutionOptions = gracefulShutdown
                    ? invocation.ExecutionOptions with { GracefulShutdownTimeout = TimeSpan.MaxValue }
                    : invocation.ExecutionOptions with { ExecutionTimeout = TimeSpan.MaxValue },
            }, cancellationToken);
    }

    private static CommandExecutionOptions CreateOptions(bool gracefulShutdown, TimeSpan timeout) => gracefulShutdown
        ? new CommandExecutionOptions { GracefulShutdownTimeout = timeout }
        : new CommandExecutionOptions { ExecutionTimeout = timeout };
}
