using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine.Scheduling;

public class SchedulerStatusReporterTests
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    [Test]
    public async Task Unchanged_Status_Is_Not_Logged_Again()
    {
        var state = CreateModuleState(typeof(ExecutingModule), ModuleExecutionState.Executing);
        var states = new ConcurrentDictionary<Type, ModuleState>([new(typeof(ExecutingModule), state)]);
        var logger = new RecordingLogger<SchedulerStatusReporter>();
        var timeProvider = new FakeTimeProvider(StartTime);
        var reporter = new SchedulerStatusReporter(logger, timeProvider);
        var stateQueries = new ModuleStateQueries(states);
        using var stateLock = new ReaderWriterLockSlim();

        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);
        timeProvider.Advance(TimeSpan.FromSeconds(15));
        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);

        await Assert.That(logger.Messages).Count().IsEqualTo(2);
    }

    [Test]
    public async Task Changed_Status_Is_Logged_After_Interval()
    {
        var state = CreateModuleState(typeof(ExecutingModule), ModuleExecutionState.Executing);
        var states = new ConcurrentDictionary<Type, ModuleState>([new(typeof(ExecutingModule), state)]);
        var logger = new RecordingLogger<SchedulerStatusReporter>();
        var timeProvider = new FakeTimeProvider(StartTime);
        var reporter = new SchedulerStatusReporter(logger, timeProvider);
        var stateQueries = new ModuleStateQueries(states);
        using var stateLock = new ReaderWriterLockSlim();

        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);
        state.State = ModuleExecutionState.Completed;
        timeProvider.Advance(TimeSpan.FromSeconds(15));
        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);

        await Assert.That(logger.Messages).Count().IsEqualTo(3);
        await Assert.That(logger.Messages[^1]).Contains("Completed=1");
    }

    [Test]
    public async Task Changed_Dependency_Count_Is_Logged_When_Statistics_Are_Unchanged()
    {
        var state = CreateModuleState(typeof(PendingModule), ModuleExecutionState.Pending);
        state.UnresolvedDependencies.Add(typeof(DependencyModule));
        var states = new ConcurrentDictionary<Type, ModuleState>([new(typeof(PendingModule), state)]);
        var logger = new RecordingLogger<SchedulerStatusReporter>();
        var timeProvider = new FakeTimeProvider(StartTime);
        var reporter = new SchedulerStatusReporter(logger, timeProvider);
        var stateQueries = new ModuleStateQueries(states);
        using var stateLock = new ReaderWriterLockSlim();

        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);
        state.UnresolvedDependencies.Clear();
        timeProvider.Advance(TimeSpan.FromSeconds(15));
        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);

        await Assert.That(logger.Messages).Count().IsEqualTo(4);
        await Assert.That(logger.Messages[^1]).Contains("deps: 0");
    }

    private static ModuleState CreateModuleState(Type moduleType, ModuleExecutionState executionState)
    {
        return new ModuleState(new Mock<IModule>().Object, moduleType)
        {
            State = executionState,
        };
    }

    private sealed class ExecutingModule;

    private sealed class PendingModule;

    private sealed class DependencyModule;

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Messages.Add(formatter(state, exception));
        }
    }
}
