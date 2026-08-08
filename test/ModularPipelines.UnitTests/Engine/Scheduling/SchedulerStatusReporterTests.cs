using System.Collections.Concurrent;
using System.Text.RegularExpressions;
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

    [Test]
    public async Task Different_Module_With_Same_Simple_Name_Is_Logged()
    {
        var firstState = CreateModuleState(typeof(FirstContainer.SameNameModule), ModuleExecutionState.Executing);
        var states = new ConcurrentDictionary<Type, ModuleState>(
            [new(typeof(FirstContainer.SameNameModule), firstState)]);
        var logger = new RecordingLogger<SchedulerStatusReporter>();
        var timeProvider = new FakeTimeProvider(StartTime);
        var reporter = new SchedulerStatusReporter(logger, timeProvider);
        var stateQueries = new ModuleStateQueries(states);
        using var stateLock = new ReaderWriterLockSlim();

        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);
        states.TryRemove(typeof(FirstContainer.SameNameModule), out _);
        var secondState = CreateModuleState(typeof(SecondContainer.SameNameModule), ModuleExecutionState.Executing);
        states[typeof(SecondContainer.SameNameModule)] = secondState;
        timeProvider.Advance(TimeSpan.FromSeconds(15));
        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);

        await Assert.That(logger.Messages).Count().IsEqualTo(4);
        await Assert.That(logger.Messages[^1]).Contains(typeof(SecondContainer.SameNameModule).FullName!);
    }

    [Test]
    public async Task Debug_Level_Logs_Only_Compact_Summary()
    {
        var pendingState = CreateModuleState(typeof(PendingModule), ModuleExecutionState.Pending);
        pendingState.UnresolvedDependencies.Add(typeof(DependencyModule));
        var executingState = CreateModuleState(typeof(ExecutingModule), ModuleExecutionState.Executing);
        var states = new ConcurrentDictionary<Type, ModuleState>(
        [
            new(typeof(PendingModule), pendingState),
            new(typeof(ExecutingModule), executingState),
        ]);
        var logger = new RecordingLogger<SchedulerStatusReporter>(LogLevel.Debug);
        var reporter = new SchedulerStatusReporter(logger, new FakeTimeProvider(StartTime));
        var stateQueries = new ModuleStateQueries(states);
        using var stateLock = new ReaderWriterLockSlim();

        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);

        await Assert.That(logger.Messages).Count().IsEqualTo(1);
        await Assert.That(logger.Messages[0]).Contains("Scheduler waiting:");
        await Assert.That(logger.Messages[0]).DoesNotContain("PendingModule");
        await Assert.That(logger.Messages[0]).DoesNotContain("ExecutingModule");
    }

    [Test]
    public async Task Trace_Module_Details_Are_Truncated()
    {
        var states = new ConcurrentDictionary<Type, ModuleState>();
        var moduleType = typeof(PendingModule);
        for (var index = 0; index < 12; index++)
        {
            moduleType = typeof(ModuleSlot<>).MakeGenericType(moduleType);
            var state = CreateModuleState(moduleType, ModuleExecutionState.Pending);
            state.UnresolvedDependencies.Add(typeof(DependencyModule));
            states[moduleType] = state;
        }

        var logger = new RecordingLogger<SchedulerStatusReporter>();
        var reporter = new SchedulerStatusReporter(logger, new FakeTimeProvider(StartTime));
        var stateQueries = new ModuleStateQueries(states);
        using var stateLock = new ReaderWriterLockSlim();

        reporter.LogStatusIfIntervalElapsed(stateQueries, stateLock);

        var details = logger.Messages.Single(message => message.StartsWith("Pending modules:", StringComparison.Ordinal));
        await Assert.That(Regex.Matches(details, "deps:").Count).IsEqualTo(10);
        await Assert.That(details).Contains("... (+2 more)");
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

    private sealed class ModuleSlot<T>;

    private sealed class FirstContainer
    {
        public sealed class SameNameModule;
    }

    private sealed class SecondContainer
    {
        public sealed class SameNameModule;
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        private readonly LogLevel _minimumLevel;

        public RecordingLogger(LogLevel minimumLevel = LogLevel.Trace)
        {
            _minimumLevel = minimumLevel;
        }

        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel >= _minimumLevel && logLevel != LogLevel.None;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                Messages.Add(formatter(state, exception));
            }
        }
    }
}
