using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Engine.BuildSystemFormatters;

namespace ModularPipelines.UnitTests.Console;

public class ModuleOutputBufferTests
{
    [Test]
    public async Task Flush_Waits_For_Queued_Logs_Before_Group_End()
    {
        var writer = new StringWriter();
        var loggerControl = new QueuedLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();

        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);

        var output = writer.ToString();
        var structuredLog = output.IndexOf("structured log", StringComparison.Ordinal);
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);

        await Assert.That(structuredLog).IsGreaterThanOrEqualTo(0);
        await Assert.That(groupEnd).IsGreaterThan(structuredLog);
    }

    [Test]
    public async Task Flush_Waits_For_Queued_Logs_Before_Direct_Output_And_Group_End()
    {
        var writer = new StringWriter();
        var loggerControl = new QueuedLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("direct output");

        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);

        var output = writer.ToString();
        var groupStart = output.IndexOf("::group::", StringComparison.Ordinal);
        var structuredLog = output.IndexOf("structured log", StringComparison.Ordinal);
        var directOutput = output.IndexOf("direct output", StringComparison.Ordinal);
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);

        await Assert.That(groupStart).IsGreaterThanOrEqualTo(0);
        await Assert.That(structuredLog).IsGreaterThan(groupStart);
        await Assert.That(directOutput).IsGreaterThan(structuredLog);
        await Assert.That(groupEnd).IsGreaterThan(directOutput);
    }

    private static ModuleOutputBuffer CreateBufferWithStructuredLog()
    {
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.AddLogEvent(
            LogLevel.Information,
            default,
            "structured log",
            null,
            static (state, _) => state.ToString()!);
        return buffer;
    }

    private sealed class QueuedLoggerControl(TextWriter writer) : ILogger, ISpectreConsoleLoggerControl
    {
        private readonly Queue<string> _pendingMessages = new();

        public object SynchronizationLock { get; } = new();

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _pendingMessages.Enqueue(formatter(state, exception));
        }

        public Task FlushAsync(CancellationToken cancellationToken = default)
        {
            lock (SynchronizationLock)
            {
                while (_pendingMessages.TryDequeue(out var message))
                {
                    writer.WriteLine(message);
                }
            }

            return Task.CompletedTask;
        }
    }
}
