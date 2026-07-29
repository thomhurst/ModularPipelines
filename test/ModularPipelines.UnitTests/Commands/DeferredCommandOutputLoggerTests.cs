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

        await Assert.That(deferredLogger.Complete()).IsFalse();
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

        await Assert.That(deferredLogger.Complete()).IsTrue();
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

        await Assert.That(deferredLogger.Complete()).IsTrue();
        await Assert.That(outputLogger.Lines).IsEquivalentTo([
            (Text: "first", IsError: false),
            (Text: "second", IsError: false),
        ]);
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

    private sealed record TestCommandOptions : CommandLineToolOptions;
}
