using ModularPipelines.Context;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Context;

public class LoggingCommandLineExecutorTests
{
    [Test]
    public async Task InnerFailureStillLogsCompletion()
    {
        var expectedException = new InvalidOperationException("Execution failed.");
        var logger = new RecordingCommandLogger();
        var executor = new LoggingCommandLineExecutor(
            new StubCommandLineExecutor(
                (_, _, _) => Task.FromException<CommandResult>(expectedException)),
            logger);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => executor.ExecuteAsync(new CommandLine("tool", [])));

        using (Assert.Multiple())
        {
            await Assert.That(exception).IsSameReferenceAs(expectedException);
            await Assert.That(logger.Events).Count().IsEqualTo(2);
            await Assert.That(logger.Events[0]).IsEqualTo("start");
            await Assert.That(logger.Events[1]).IsEqualTo("completion");
            await Assert.That(logger.CompletionExitCode).IsEqualTo(-1);
            await Assert.That(logger.CompletionError).IsEqualTo(expectedException.Message);
        }
    }

    [Test]
    public async Task StartLoggingFailureDoesNotPreventExecution()
    {
        var expectedException = new InvalidOperationException("Logging failed.");
        var executed = false;
        var logger = new RecordingCommandLogger
        {
            StartFailure = expectedException,
        };
        var result = CreateResult();
        var executor = new LoggingCommandLineExecutor(
            new StubCommandLineExecutor((_, _, _) =>
            {
                executed = true;
                return Task.FromResult(result);
            }),
            logger);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => executor.ExecuteAsync(new CommandLine("tool", [])));

        using (Assert.Multiple())
        {
            await Assert.That(executed).IsTrue();
            await Assert.That(exception).IsSameReferenceAs(expectedException);
            await Assert.That(logger.Events).Count().IsEqualTo(2);
            await Assert.That(logger.Events[1]).IsEqualTo("completion");
        }
    }

    private static CommandResult CreateResult()
    {
        var timestamp = DateTimeOffset.UtcNow;
        return new CommandResult(
            "tool",
            Environment.CurrentDirectory,
            string.Empty,
            string.Empty,
            new Dictionary<string, string?>(),
            timestamp,
            timestamp,
            TimeSpan.Zero,
            0);
    }

    private sealed class StubCommandLineExecutor(
        Func<CommandLine, CommandExecutionOptions?, CancellationToken, Task<CommandResult>> execute)
        : ICommandLineExecutor
    {
        public Task<CommandResult> ExecuteAsync(
            CommandLine commandLine,
            CommandExecutionOptions? options = null,
            CancellationToken cancellationToken = default) =>
            execute(commandLine, options, cancellationToken);
    }

    private sealed class RecordingCommandLogger : ICommandLogger
    {
        public List<string> Events { get; } = [];

        public Exception? StartFailure { get; init; }

        public int? CompletionExitCode { get; private set; }

        public string? CompletionError { get; private set; }

        public void LogCommandStart(
            CommandLineToolOptions? options,
            CommandExecutionOptions? execOpts,
            string? inputToLog,
            string commandWorkingDirPath)
        {
            Events.Add("start");
            if (StartFailure is not null)
            {
                throw StartFailure;
            }
        }

        public void LogCommandCompletion(
            CommandLineToolOptions? options,
            CommandExecutionOptions? execOpts,
            string? inputToLog,
            int? exitCode,
            TimeSpan? runTime,
            string standardOutput,
            string standardError,
            string commandWorkingDirPath)
        {
            Events.Add("completion");
            CompletionExitCode = exitCode;
            CompletionError = standardError;
        }

        public void Log(
            CommandLineToolOptions? options,
            CommandExecutionOptions? execOpts,
            string? inputToLog,
            int? exitCode,
            TimeSpan? runTime,
            string standardOutput,
            string standardError,
            string commandWorkingDirPath)
        {
        }
    }
}
