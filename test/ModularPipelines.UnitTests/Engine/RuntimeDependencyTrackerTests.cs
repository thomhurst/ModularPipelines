using System.Collections.Concurrent;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;

namespace ModularPipelines.UnitTests.Engine;

public class RuntimeDependencyTrackerTests
{
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

    private static (RuntimeDependencyTracker Tracker, Dictionary<Type, ModuleState> States) CreateTracker()
    {
        var states = new Dictionary<Type, ModuleState>
        {
            [typeof(First)] = new(new First(), typeof(First)),
            [typeof(Second)] = new(new Second(), typeof(Second)),
            [typeof(Third)] = new(new Third(), typeof(Third)),
        };

        return (new RuntimeDependencyTracker(states), states);
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
}
