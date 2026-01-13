using System.Collections.Immutable;
using ModularPipelines.Engine.State;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.State;

/// <summary>
/// Tests for the immutable ModuleExecutionPhase discriminated union.
/// </summary>
public class ModuleExecutionPhaseTests : TestBase
{
    #region Pending Phase Tests

    [Test]
    public async Task Pending_WithNoDependencies_IsReadyToQueue()
    {
        var pending = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = ImmutableList<Type>.Empty,
        };

        await Assert.That(pending.IsReadyToQueue).IsTrue();
    }

    [Test]
    public async Task Pending_WithDependencies_IsNotReadyToQueue()
    {
        var pending = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet.Create(typeof(string)),
            DependentModules = ImmutableList<Type>.Empty,
        };

        await Assert.That(pending.IsReadyToQueue).IsFalse();
    }

    [Test]
    public async Task Pending_RemovingDependency_CreatesNewInstance()
    {
        var original = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet.Create(typeof(string), typeof(int)),
            DependentModules = ImmutableList<Type>.Empty,
        };

        var updated = original with
        {
            UnresolvedDependencies = original.UnresolvedDependencies.Remove(typeof(string)),
        };

        await Assert.That(updated.UnresolvedDependencies.Count).IsEqualTo(1);
        await Assert.That(original.UnresolvedDependencies.Count).IsEqualTo(2);
    }

    #endregion

    #region Queued Phase Tests

    [Test]
    public async Task Queued_HasCorrectTimestamps()
    {
        var now = DateTimeOffset.UtcNow;
        var queued = new ModuleExecutionPhase.Queued
        {
            DependentModules = ImmutableList<Type>.Empty,
            QueuedAt = now,
            ReadyAt = now,
        };

        await Assert.That(queued.QueuedAt).IsEqualTo(now);
        await Assert.That(queued.ReadyAt).IsEqualTo(now);
    }

    #endregion

    #region Running Phase Tests

    [Test]
    public async Task Running_HasStartedAt()
    {
        var now = DateTimeOffset.UtcNow;
        using var cts = new CancellationTokenSource();

        var running = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = now,
            QueuedAt = now.AddSeconds(-1),
            CancellationSource = cts,
        };

        await Assert.That(running.StartedAt).IsEqualTo(now);
        await Assert.That(running.CancellationSource).IsEqualTo(cts);
    }

    #endregion

    #region Completed Phase Tests

    [Test]
    public async Task Completed_CalculatesDuration()
    {
        var startTime = DateTimeOffset.UtcNow;
        var endTime = startTime.AddSeconds(5);

        var completed = new ModuleExecutionPhase.Completed
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = startTime,
            CompletedAt = endTime,
            Result = new TestModuleResult(),
        };

        await Assert.That(completed.Duration).IsEqualTo(TimeSpan.FromSeconds(5));
    }

    #endregion

    #region Failed Phase Tests

    [Test]
    public async Task Failed_CalculatesDuration()
    {
        var startTime = DateTimeOffset.UtcNow;
        var failTime = startTime.AddSeconds(3);
        var exception = new InvalidOperationException("Test failure");

        var failed = new ModuleExecutionPhase.Failed
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = startTime,
            FailedAt = failTime,
            Exception = exception,
            Result = new TestModuleResult(),
        };

        await Assert.That(failed.Duration).IsEqualTo(TimeSpan.FromSeconds(3));
        await Assert.That(failed.Exception.Message).IsEqualTo("Test failure");
    }

    #endregion

    #region Skipped Phase Tests

    [Test]
    public async Task Skipped_HasSkipDecision()
    {
        var skipDecision = SkipDecision.Skip("Test skip reason");

        var skipped = new ModuleExecutionPhase.Skipped
        {
            DependentModules = ImmutableList<Type>.Empty,
            SkippedAt = DateTimeOffset.UtcNow,
            SkipDecision = skipDecision,
            Result = new TestModuleResult(),
        };

        await Assert.That(skipped.SkipDecision.ShouldSkip).IsTrue();
        await Assert.That(skipped.SkipDecision.Reason).IsEqualTo("Test skip reason");
    }

    #endregion

    #region Cancelled Phase Tests

    [Test]
    public async Task Cancelled_PreservesPreviousPhase()
    {
        var previousPhase = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            QueuedAt = DateTimeOffset.UtcNow.AddSeconds(-1),
            CancellationSource = new CancellationTokenSource(),
        };

        var cancelled = new ModuleExecutionPhase.Cancelled
        {
            DependentModules = ImmutableList<Type>.Empty,
            CancelledAt = DateTimeOffset.UtcNow,
            PreviousPhase = previousPhase,
        };

        await Assert.That(cancelled.PreviousPhase).IsEqualTo(previousPhase);
    }

    #endregion

    #region Helper Classes

    private class TestModuleResult : IModuleResult
    {
        public string ModuleName => "TestModule";

        public TimeSpan ModuleDuration => TimeSpan.Zero;

        public DateTimeOffset ModuleStart => DateTimeOffset.UtcNow;

        public DateTimeOffset ModuleEnd => DateTimeOffset.UtcNow;

        public Enums.Status ModuleStatus => Enums.Status.Successful;

        public ModuleResultType ModuleResultType => ModuleResultType.Success;

        public bool IsSuccess => true;

        public bool IsFailure => false;

        public bool IsSkipped => false;

        public object? ValueOrDefault => null;

        public Exception? ExceptionOrDefault => null;

        public SkipDecision? SkipDecisionOrDefault => null;
    }

    #endregion
}
