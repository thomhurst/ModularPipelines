using System.Collections.Concurrent;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;

namespace ModularPipelines.UnitTests.Engine;

public class RuntimeDependencyTrackerTests
{
    [Test]
    [Arguments("sequential-waiter")]
    [Arguments("sequential-target")]
    [Arguments("key")]
    [Arguments("limiter")]
    [Arguments("hint")]
    public async Task Rejects_Runtime_Wait_Blocked_By_Held_Constraints(string constraint)
    {
        var (tracker, states) = CreateTracker();
        var first = states[typeof(First)];
        first.State = ModuleExecutionState.Executing;
        ConfigureConflict(first, states[typeof(Second)], constraint);

        var exception = Assert.Throws<DependencyCollisionException>(() =>
            Register(tracker, states, typeof(First), typeof(Second)));

        await Assert.That(exception!.Message).Contains(nameof(First));
        await Assert.That(exception.Message).Contains(nameof(Second));
        await Assert.That(exception.Message).Contains("active but blocked on a runtime wait");
        await Assert.That(exception.Message).Contains("DependsOn");
    }

    [Test]
    public async Task Rejects_Constraint_Conflict_In_Transitive_Dependency()
    {
        var (tracker, states) = CreateTracker();
        states[typeof(First)].State = ModuleExecutionState.Executing;
        states[typeof(Second)].RecordDependency(typeof(Third), optional: false);
        ConfigureConflict(states[typeof(First)], states[typeof(Third)], "key");

        await Assert.That(() => Register(tracker, states, typeof(First), typeof(Second)))
            .Throws<DependencyCollisionException>();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Rejects_When_All_Limiter_Slots_Are_Held_By_Waiters(bool executionHint)
    {
        var (tracker, states) = CreateTracker();
        foreach (var state in states.Values)
        {
            if (executionHint)
            {
                state.ExecutionHint = ExecutionHint.CpuBound;
                state.ExecutionHintLimit = 2;
            }
            else
            {
                state.ParallelLimiter = new ParallelLimitConfiguration("shared", 2);
            }
        }

        states[typeof(First)].State = ModuleExecutionState.Executing;
        states[typeof(Second)].State = ModuleExecutionState.Executing;
        Register(tracker, states, typeof(First), typeof(Third));

        await Assert.That(() => Register(tracker, states, typeof(Second), typeof(Third)))
            .Throws<DependencyCollisionException>();
    }

    [Test]
    public async Task Allows_Limiter_Wait_When_Another_Holder_Can_Complete()
    {
        var (tracker, states) = CreateTracker();
        foreach (var state in states.Values)
        {
            state.ParallelLimiter = new ParallelLimitConfiguration("shared", 2);
        }

        states[typeof(First)].State = ModuleExecutionState.Executing;
        states[typeof(Second)].State = ModuleExecutionState.Executing;

        await Assert.That(() => Register(tracker, states, typeof(First), typeof(Third))).ThrowsNothing();
    }

    [Test]
    public async Task Allows_Wait_For_Already_Executing_Target()
    {
        var (tracker, states) = CreateTracker();
        ConfigureConflict(states[typeof(First)], states[typeof(Second)], "limiter");
        states[typeof(First)].State = ModuleExecutionState.Executing;
        states[typeof(Second)].State = ModuleExecutionState.Executing;

        await Assert.That(() => Register(tracker, states, typeof(First), typeof(Second))).ThrowsNothing();
    }

    [Test]
    public async Task Rejected_Wait_Is_Not_Retained()
    {
        var (tracker, states) = CreateTracker();
        ConfigureConflict(states[typeof(First)], states[typeof(Second)], "key");
        states[typeof(First)].State = ModuleExecutionState.Executing;
        Assert.Throws<DependencyCollisionException>(() => Register(tracker, states, typeof(First), typeof(Second)));
        states[typeof(First)].RequiredLockKeys = [];

        await Assert.That(() => Register(tracker, states, typeof(Second), typeof(First))).ThrowsNothing();
    }

    [Test]
    public async Task Rejects_Cycle_Combining_Runtime_Waits_And_Different_Constraint_Keys()
    {
        var (tracker, states) = CreateTracker();
        states[typeof(Fourth)] = new(new Fourth(), typeof(Fourth));
        states[typeof(First)].State = ModuleExecutionState.Executing;
        states[typeof(Second)].State = ModuleExecutionState.Executing;
        states[typeof(First)].RequiredLockKeys = states[typeof(Fourth)].RequiredLockKeys = ["red"];
        states[typeof(Second)].RequiredLockKeys = states[typeof(Third)].RequiredLockKeys = ["blue"];
        Register(tracker, states, typeof(First), typeof(Third));

        await Assert.That(() => Register(tracker, states, typeof(Second), typeof(Fourth)))
            .Throws<DependencyCollisionException>();
    }

    [Test]
    public async Task Rejects_Wait_When_All_Global_Execution_Slots_Are_Held_By_Waiters()
    {
        var (tracker, states) = CreateTracker(maxParallelism: 2);
        states[typeof(First)].State = ModuleExecutionState.Executing;
        states[typeof(Second)].State = ModuleExecutionState.Executing;
        Register(tracker, states, typeof(First), typeof(Third));

        await Assert.That(() => Register(tracker, states, typeof(Second), typeof(Third)))
            .Throws<DependencyCollisionException>();
    }

    private static void ConfigureConflict(ModuleState waiter, ModuleState target, string constraint)
    {
        switch (constraint)
        {
            case "sequential-waiter":
                waiter.RequiresSequentialExecution = true;
                break;
            case "sequential-target":
                target.RequiresSequentialExecution = true;
                break;
            case "key":
                waiter.RequiredLockKeys = ["shared"];
                target.RequiredLockKeys = ["shared"];
                break;
            case "limiter":
                waiter.ParallelLimiter = new ParallelLimitConfiguration("shared", 1);
                target.ParallelLimiter = new ParallelLimitConfiguration("shared", 1);
                break;
            case "hint":
                waiter.ExecutionHint = target.ExecutionHint = ExecutionHint.CpuBound;
                waiter.ExecutionHintLimit = target.ExecutionHintLimit = 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(constraint));
        }
    }

    [Test]
    public async Task Detects_Transitive_Runtime_Cycle()
    {
        var (tracker, states) = CreateTracker();
        Register(tracker, states, typeof(First), typeof(Second));
        Register(tracker, states, typeof(Second), typeof(Third));

        var exception = Assert.Throws<CircularDependencyException>(() =>
            Register(tracker, states, typeof(Third), typeof(First)));

        await Assert.That(exception!.CycleTypes.SequenceEqual(new[]
        {
            typeof(Third), typeof(First), typeof(Second), typeof(Third),
        })).IsTrue();
    }

    [Test]
    public async Task Detects_Transitive_Declared_Cycle()
    {
        var (tracker, states) = CreateTracker();
        states[typeof(Second)].RecordDependency(typeof(Third), optional: false);
        states[typeof(Third)].RecordDependency(typeof(First), optional: true);

        await Assert.That(() => Register(tracker, states, typeof(First), typeof(Second)))
            .Throws<CircularDependencyException>();
    }

    [Test]
    public async Task Allows_Acyclic_Shared_Dependencies_And_Repeated_Waits()
    {
        var (tracker, states) = CreateTracker();
        states[typeof(Second)].RecordDependency(typeof(Third), optional: false);

        await Assert.That(() =>
        {
            Register(tracker, states, typeof(First), typeof(Second));
            Register(tracker, states, typeof(First), typeof(Third));
            Register(tracker, states, typeof(First), typeof(Second));
        }).ThrowsNothing();
    }

    [Test]
    public async Task Completed_Runtime_Wait_Does_Not_Create_A_Cycle()
    {
        var (tracker, states) = CreateTracker();
        var completedResult = new TaskCompletionSource();
        tracker.RegisterWait(states[typeof(First)].Module, states[typeof(Second)].Module, completedResult.Task);
        completedResult.SetResult();

        await Assert.That(() => Register(tracker, states, typeof(Second), typeof(First))).ThrowsNothing();
    }

    [Test]
    public async Task Completed_Declared_Dependency_Does_Not_Create_A_Cycle()
    {
        var (tracker, states) = CreateTracker();
        states[typeof(Second)].RecordDependency(typeof(Third), optional: false);
        states[typeof(Third)].RecordDependency(typeof(First), optional: false);
        states[typeof(Third)].CompletionSource.SetResult(states[typeof(Third)].Module);

        await Assert.That(() => Register(tracker, states, typeof(First), typeof(Second))).ThrowsNothing();
    }

    [Test]
    public async Task Concurrent_Opposing_Waits_Reject_One_Edge()
    {
        var (tracker, states) = CreateTracker();
        var errors = new ConcurrentBag<CircularDependencyException>();

        Parallel.Invoke(
            () => TryRegister(typeof(First), typeof(Second)),
            () => TryRegister(typeof(Second), typeof(First)));

        await Assert.That(errors.Count).IsEqualTo(1);

        void TryRegister(Type module, Type dependency)
        {
            try
            {
                Register(tracker, states, module, dependency);
            }
            catch (CircularDependencyException exception)
            {
                errors.Add(exception);
            }
        }
    }

    [Test]
    public async Task Does_Not_Track_Modules_From_Another_Pipeline()
    {
        var (tracker, states) = CreateTracker();
        Register(tracker, states, typeof(First), typeof(Second));

        await Assert.That(() => tracker.RegisterWait(
            states[typeof(Second)].Module, new First(), new TaskCompletionSource().Task)).ThrowsNothing();
    }

    private static (RuntimeDependencyTracker Tracker, Dictionary<Type, ModuleState> States) CreateTracker(int maxParallelism = int.MaxValue)
    {
        var states = new Dictionary<Type, ModuleState>
        {
            [typeof(First)] = new(new First(), typeof(First)),
            [typeof(Second)] = new(new Second(), typeof(Second)),
            [typeof(Third)] = new(new Third(), typeof(Third)),
        };

        return (new RuntimeDependencyTracker(states, new ModuleConstraintEvaluator(NullLogger<ModuleConstraintEvaluator>.Instance), maxParallelism), states);
    }

    private static void Register(RuntimeDependencyTracker tracker, Dictionary<Type, ModuleState> states, Type module, Type dependency) =>
        tracker.RegisterWait(states[module].Module, states[dependency].Module, states[dependency].CompletionSource.Task);

    private abstract class TestModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

    private sealed class First : TestModule;

    private sealed class Second : TestModule;

    private sealed class Third : TestModule;

    private sealed class Fourth : TestModule;
}
