using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Commands;

public class DeferredCommandOutputLoggerTests
{
    [Test]
    public async Task Completion_Before_Delay_Keeps_Output_For_Inline_Summary()
    {
        var outputLogger = new RecordingOutputLogger(expectedLineCount: 1);
        using var deferredLogger = new DeferredCommandOutputLogger(
            outputLogger,
            new TestCommandOptions(),
            new CommandExecutionOptions(),
            TimeSpan.FromDays(1));

        deferredLogger.LogStandardOutputLine("output");

        var completion = deferredLogger.Complete();
        await Assert.That(completion.HasStreamedOutput).IsFalse();
        await Assert.That(completion.PendingStandardOutput).IsEqualTo("output");
        await Assert.That(outputLogger.Lines).IsEmpty();
    }

    [Test]
    public async Task Output_Is_Streamed_When_Command_Remains_Active()
    {
        var outputLogger = new RecordingOutputLogger(expectedLineCount: 1);
        using var deferredLogger = new DeferredCommandOutputLogger(
            outputLogger,
            new TestCommandOptions(),
            new CommandExecutionOptions(),
            TimeSpan.FromMilliseconds(10));

        deferredLogger.LogStandardOutputLine("output");
        await outputLogger.ExpectedLinesReceived.WaitAsync(TimeSpan.FromSeconds(5));

        var completion = deferredLogger.Complete();
        await Assert.That(completion.HasStreamedOutput).IsTrue();
        await Assert.That(completion.PendingStandardOutput).IsEmpty();
        await Assert.That(outputLogger.Lines).IsEquivalentTo([(Text: "output", IsError: false)]);
    }

    [Test]
    public async Task Multiple_Lines_Are_Streamed_Immediately()
    {
        var outputLogger = new RecordingOutputLogger(expectedLineCount: 2);
        using var deferredLogger = new DeferredCommandOutputLogger(
            outputLogger,
            new TestCommandOptions(),
            new CommandExecutionOptions(),
            TimeSpan.FromDays(1));

        deferredLogger.LogStandardOutputLine("first");
        deferredLogger.LogStandardOutputLine("second");
        await outputLogger.ExpectedLinesReceived.WaitAsync(TimeSpan.FromSeconds(5));

        var completion = deferredLogger.Complete();
        await Assert.That(completion.HasStreamedOutput).IsTrue();
        await Assert.That(completion.PendingStandardOutput).IsEmpty();
        await Assert.That(outputLogger.Lines).IsEquivalentTo([
            (Text: "first", IsError: false),
            (Text: "second", IsError: false),
        ]);
    }

    [Test]
    public async Task Delayed_Logging_Failure_Stops_Writes_And_Is_Propagated_Once()
    {
        var outputLogger = new ThrowingOutputLogger();
        using var deferredLogger = new DeferredCommandOutputLogger(
            outputLogger,
            new TestCommandOptions(),
            new CommandExecutionOptions(),
            TimeSpan.FromMilliseconds(10));

        deferredLogger.LogStandardOutputLine("output");
        await outputLogger.LoggingAttempted.WaitAsync(TimeSpan.FromSeconds(5));

        deferredLogger.LogStandardOutputLine("later output");
        await Assert.That(outputLogger.AttemptCount).IsEqualTo(1);

        var exception = Assert.Throws<InvalidOperationException>(() => deferredLogger.Complete());
        await Assert.That(exception.Message).IsEqualTo("Logging failed.");

        var completion = deferredLogger.Complete();
        await Assert.That(completion.HasStreamedOutput).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Immediate_Logging_Failure_Stops_Writes_And_Is_Propagated_Once(
        bool useStandardError)
    {
        var outputLogger = new ThrowingOutputLogger();
        using var deferredLogger = new DeferredCommandOutputLogger(
            outputLogger,
            new TestCommandOptions(),
            new CommandExecutionOptions(),
            TimeSpan.FromMilliseconds(10));

        if (useStandardError)
        {
            deferredLogger.LogStandardErrorLine("error");
        }
        else
        {
            deferredLogger.LogStandardOutputLine("first");
            deferredLogger.LogStandardOutputLine("second");
        }

        deferredLogger.LogStandardOutputLine("later output");
        var exception = Assert.Throws<InvalidOperationException>(() => deferredLogger.Complete());
        await Assert.That(exception.Message).IsEqualTo("Logging failed.");
        await Assert.That(outputLogger.AttemptCount).IsEqualTo(1);

        var completion = deferredLogger.Complete();
        await Assert.That(completion.HasStreamedOutput).IsTrue();
    }

    private sealed class RecordingOutputLogger(int expectedLineCount) : ICommandOutputLogger
    {
        private readonly TaskCompletionSource _expectedLinesReceived =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public List<(string Text, bool IsError)> Lines { get; } = [];

        public Task ExpectedLinesReceived => _expectedLinesReceived.Task;

        public void LogStandardOutputLine(
            CommandLineToolOptions options,
            CommandExecutionOptions executionOptions,
            string line)
        {
            Record(line, isError: false);
        }

        public void LogStandardErrorLine(
            CommandLineToolOptions options,
            CommandExecutionOptions executionOptions,
            string line)
        {
            Record(line, isError: true);
        }

        private void Record(string line, bool isError)
        {
            Lines.Add((line, isError));
            if (Lines.Count == expectedLineCount)
            {
                _expectedLinesReceived.TrySetResult();
            }
        }
    }

    private sealed class ThrowingOutputLogger : ICommandOutputLogger
    {
        private readonly TaskCompletionSource _loggingAttempted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _attemptCount;

        public Task LoggingAttempted => _loggingAttempted.Task;

        public int AttemptCount => _attemptCount;

        public void LogStandardOutputLine(
            CommandLineToolOptions options,
            CommandExecutionOptions executionOptions,
            string line)
        {
            Interlocked.Increment(ref _attemptCount);
            _loggingAttempted.TrySetResult();
            throw new InvalidOperationException("Logging failed.");
        }

        public void LogStandardErrorLine(
            CommandLineToolOptions options,
            CommandExecutionOptions executionOptions,
            string line)
        {
            Interlocked.Increment(ref _attemptCount);
            throw new InvalidOperationException("Logging failed.");
        }
    }

    internal sealed record TestCommandOptions : CommandLineToolOptions;
}
