using Microsoft.Extensions.Logging;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Exceptions;

namespace ModularPipelines.UnitTests.Engine.Execution;

public class ModuleRunnerLoggingTests
{
    [Test]
    public async Task ArtifactUploadFailureWrapper_IsLoggedAsError()
    {
        var logger = new RecordingLogger();
        var exception = new ModuleFailedException(
            typeof(ExampleModule),
            new InvalidOperationException("Artifact upload failed"));

        ModuleRunner.LogModuleFailure(logger, nameof(ExampleModule), exception);

        await Assert.That(logger.Entries).HasSingleItem();
        var entry = logger.Entries.Single();
        await Assert.That(entry.Level).IsEqualTo(LogLevel.Error);
        await Assert.That(entry.Exception).IsSameReferenceAs(exception);
    }

    [Test]
    public async Task PreviouslyLoggedModuleFailure_IsLoggedAsDebugWithoutException()
    {
        var logger = new RecordingLogger();
        var exception = new ModuleFailedException(
            typeof(ExampleModule),
            new InvalidOperationException("Module failed"),
            wasLogged: true);

        ModuleRunner.LogModuleFailure(logger, nameof(ExampleModule), exception);

        await Assert.That(logger.Entries).HasSingleItem();
        var entry = logger.Entries.Single();
        await Assert.That(entry.Level).IsEqualTo(LogLevel.Debug);
        await Assert.That(entry.Exception).IsNull();
    }

    private sealed class ExampleModule;

    private sealed class RecordingLogger : ILogger
    {
        public List<LogEntry> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add(new LogEntry(logLevel, formatter(state, exception), exception));
        }
    }

    private sealed record LogEntry(
        LogLevel Level,
        string Message,
        Exception? Exception);
}
