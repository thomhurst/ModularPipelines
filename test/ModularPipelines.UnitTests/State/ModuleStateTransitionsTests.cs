using System.Collections.Immutable;
using ModularPipelines.Engine.State;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.State;

/// <summary>
/// Tests for the pure state transition functions.
/// </summary>
public class ModuleStateTransitionsTests : TestBase
{
    #region Valid Transition Tests

    [Test]
    public async Task IsValidTransition_PendingToQueued_ReturnsTrue()
    {
        var from = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = ImmutableList<Type>.Empty,
        };
        var to = new ModuleExecutionPhase.Queued
        {
            DependentModules = ImmutableList<Type>.Empty,
            QueuedAt = DateTimeOffset.UtcNow,
            ReadyAt = DateTimeOffset.UtcNow,
        };

        var result = ModuleStateTransitions.IsValidTransition(from, to);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsValidTransition_QueuedToRunning_ReturnsTrue()
    {
        var from = new ModuleExecutionPhase.Queued
        {
            DependentModules = ImmutableList<Type>.Empty,
            QueuedAt = DateTimeOffset.UtcNow,
            ReadyAt = DateTimeOffset.UtcNow,
        };
        var to = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            QueuedAt = DateTimeOffset.UtcNow,
            CancellationSource = new CancellationTokenSource(),
        };

        var result = ModuleStateTransitions.IsValidTransition(from, to);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsValidTransition_QueuedToPending_ReturnsTrue()
    {
        // Constraint failure allows reverting Queued to Pending
        var from = new ModuleExecutionPhase.Queued
        {
            DependentModules = ImmutableList<Type>.Empty,
            QueuedAt = DateTimeOffset.UtcNow,
            ReadyAt = DateTimeOffset.UtcNow,
        };
        var to = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = ImmutableList<Type>.Empty,
        };

        var result = ModuleStateTransitions.IsValidTransition(from, to);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsValidTransition_RunningToCompleted_ReturnsTrue()
    {
        var from = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            QueuedAt = DateTimeOffset.UtcNow,
            CancellationSource = new CancellationTokenSource(),
        };
        var to = new ModuleExecutionPhase.Completed
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            CompletedAt = DateTimeOffset.UtcNow,
            Result = new TestModuleResult(),
        };

        var result = ModuleStateTransitions.IsValidTransition(from, to);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsValidTransition_RunningToFailed_ReturnsTrue()
    {
        var from = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            QueuedAt = DateTimeOffset.UtcNow,
            CancellationSource = new CancellationTokenSource(),
        };
        var to = new ModuleExecutionPhase.Failed
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            FailedAt = DateTimeOffset.UtcNow,
            Exception = new Exception(),
            Result = new TestModuleResult(),
        };

        var result = ModuleStateTransitions.IsValidTransition(from, to);
        await Assert.That(result).IsTrue();
    }

    #endregion

    #region Invalid Transition Tests

    [Test]
    public async Task IsValidTransition_CompletedToAny_ReturnsFalse()
    {
        var from = new ModuleExecutionPhase.Completed
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            CompletedAt = DateTimeOffset.UtcNow,
            Result = new TestModuleResult(),
        };
        var to = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            QueuedAt = DateTimeOffset.UtcNow,
            CancellationSource = new CancellationTokenSource(),
        };

        var result = ModuleStateTransitions.IsValidTransition(from, to);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsValidTransition_PendingToRunning_ReturnsFalse()
    {
        // Cannot skip Queued phase
        var from = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = ImmutableList<Type>.Empty,
        };
        var to = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            QueuedAt = DateTimeOffset.UtcNow,
            CancellationSource = new CancellationTokenSource(),
        };

        var result = ModuleStateTransitions.IsValidTransition(from, to);
        await Assert.That(result).IsFalse();
    }

    #endregion

    #region Pure Transition Function Tests

    [Test]
    public async Task RemoveDependency_CreatesPendingWithDependencyRemoved()
    {
        var pending = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet.Create(typeof(string), typeof(int)),
            DependentModules = ImmutableList<Type>.Empty,
        };

        var result = ModuleStateTransitions.RemoveDependency(pending, typeof(string));

        await Assert.That(result.UnresolvedDependencies.Count).IsEqualTo(1);
        await Assert.That(result.UnresolvedDependencies.Contains(typeof(int))).IsTrue();
        await Assert.That(result.UnresolvedDependencies.Contains(typeof(string))).IsFalse();
    }

    [Test]
    public async Task ToQueued_CreateQueuedFromPending()
    {
        var pending = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = ImmutableList.Create(typeof(string)),
        };
        var now = DateTimeOffset.UtcNow;

        var result = ModuleStateTransitions.ToQueued(pending, now);

        await Assert.That(result.QueuedAt).IsEqualTo(now);
        await Assert.That(result.ReadyAt).IsEqualTo(now);
        await Assert.That(result.DependentModules.Count).IsEqualTo(1);
    }

    [Test]
    public async Task ToQueued_WithUnresolvedDependencies_ThrowsException()
    {
        var pending = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet.Create(typeof(string)),
            DependentModules = ImmutableList<Type>.Empty,
        };

        await Assert.That(() => ModuleStateTransitions.ToQueued(pending, DateTimeOffset.UtcNow))
            .ThrowsExactly<InvalidOperationException>();
    }

    [Test]
    public async Task ToRunning_CreatesRunningFromQueued()
    {
        var queuedAt = DateTimeOffset.UtcNow.AddSeconds(-1);
        var queued = new ModuleExecutionPhase.Queued
        {
            DependentModules = ImmutableList.Create(typeof(string)),
            QueuedAt = queuedAt,
            ReadyAt = queuedAt,
        };
        var now = DateTimeOffset.UtcNow;
        using var cts = new CancellationTokenSource();

        var result = ModuleStateTransitions.ToRunning(queued, now, cts);

        await Assert.That(result.StartedAt).IsEqualTo(now);
        await Assert.That(result.QueuedAt).IsEqualTo(queuedAt);
        await Assert.That(result.CancellationSource).IsEqualTo(cts);
        await Assert.That(result.DependentModules.Count).IsEqualTo(1);
    }

    [Test]
    public async Task ToPending_RevertQueuedToPending()
    {
        var queued = new ModuleExecutionPhase.Queued
        {
            DependentModules = ImmutableList.Create(typeof(string)),
            QueuedAt = DateTimeOffset.UtcNow,
            ReadyAt = DateTimeOffset.UtcNow,
        };

        var result = ModuleStateTransitions.ToPending(queued);

        await Assert.That(result.UnresolvedDependencies.IsEmpty).IsTrue();
        await Assert.That(result.DependentModules.Count).IsEqualTo(1);
    }

    [Test]
    public async Task ToCompleted_CreatesCompletedFromRunning()
    {
        var startedAt = DateTimeOffset.UtcNow.AddSeconds(-5);
        var running = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = startedAt,
            QueuedAt = startedAt.AddSeconds(-1),
            CancellationSource = new CancellationTokenSource(),
        };
        var completedAt = DateTimeOffset.UtcNow;
        var moduleResult = new TestModuleResult();

        var result = ModuleStateTransitions.ToCompleted(running, completedAt, moduleResult);

        await Assert.That(result.StartedAt).IsEqualTo(startedAt);
        await Assert.That(result.CompletedAt).IsEqualTo(completedAt);
        await Assert.That(result.Result).IsEqualTo(moduleResult);
        await Assert.That(result.Duration).IsEqualTo(completedAt - startedAt);
    }

    [Test]
    public async Task ToFailed_CreatesFailedFromRunning()
    {
        var startedAt = DateTimeOffset.UtcNow.AddSeconds(-3);
        var running = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = startedAt,
            QueuedAt = startedAt.AddSeconds(-1),
            CancellationSource = new CancellationTokenSource(),
        };
        var failedAt = DateTimeOffset.UtcNow;
        var exception = new InvalidOperationException("Test");
        var moduleResult = new TestModuleResult();

        var result = ModuleStateTransitions.ToFailed(running, failedAt, exception, moduleResult);

        await Assert.That(result.StartedAt).IsEqualTo(startedAt);
        await Assert.That(result.FailedAt).IsEqualTo(failedAt);
        await Assert.That(result.Exception).IsEqualTo(exception);
        await Assert.That(result.Result).IsEqualTo(moduleResult);
    }

    #endregion

    #region Helper Methods Tests

    [Test]
    public async Task IsTerminal_ReturnsTrueForTerminalStates()
    {
        var completed = new ModuleExecutionPhase.Completed
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            CompletedAt = DateTimeOffset.UtcNow,
            Result = new TestModuleResult(),
        };
        var failed = new ModuleExecutionPhase.Failed
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            FailedAt = DateTimeOffset.UtcNow,
            Exception = new Exception(),
            Result = new TestModuleResult(),
        };

        await Assert.That(ModuleStateTransitions.IsTerminal(completed)).IsTrue();
        await Assert.That(ModuleStateTransitions.IsTerminal(failed)).IsTrue();
    }

    [Test]
    public async Task IsTerminal_ReturnsFalseForNonTerminalStates()
    {
        var pending = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = ImmutableList<Type>.Empty,
        };
        var running = new ModuleExecutionPhase.Running
        {
            DependentModules = ImmutableList<Type>.Empty,
            StartedAt = DateTimeOffset.UtcNow,
            QueuedAt = DateTimeOffset.UtcNow,
            CancellationSource = new CancellationTokenSource(),
        };

        await Assert.That(ModuleStateTransitions.IsTerminal(pending)).IsFalse();
        await Assert.That(ModuleStateTransitions.IsTerminal(running)).IsFalse();
    }

    [Test]
    public async Task GetDependentModules_ReturnsCorrectListForAllPhases()
    {
        var dependents = ImmutableList.Create(typeof(string), typeof(int));

        var pending = new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = dependents,
        };

        var result = ModuleStateTransitions.GetDependentModules(pending);
        await Assert.That(result.Count).IsEqualTo(2);
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
