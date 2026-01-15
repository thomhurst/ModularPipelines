using System.Collections.Immutable;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Engine.State;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.State;

/// <summary>
/// Tests for the lock-free ModuleStateStore.
/// </summary>
public class ModuleStateStoreTests : TestBase
{
    private FakeTimeProvider _timeProvider = null!;
    private ModuleStateStore _store = null!;

    [Before(Test)]
    public void Setup()
    {
        _timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
        _store = new ModuleStateStore(_timeProvider);
    }

    #region Registration Tests

    [Test]
    public async Task RegisterModule_CreatesInitialPendingState()
    {
        var module = new TestModule();
        var moduleType = typeof(TestModule);
        var dependencies = ImmutableHashSet.Create(typeof(string));
        var dependents = ImmutableList.Create(typeof(int));

        var state = _store.RegisterModule(
            module,
            moduleType,
            dependencies,
            dependents,
            requiresSequentialExecution: false,
            requiredLockKeys: Array.Empty<string>(),
            priority: ModulePriority.Normal,
            executionType: ExecutionType.Default);

        await Assert.That(state.Module).IsEqualTo(module);
        await Assert.That(state.ModuleType).IsEqualTo(moduleType);
        await Assert.That(state.Phase).IsTypeOf<ModuleExecutionPhase.Pending>();

        var pending = (ModuleExecutionPhase.Pending)state.Phase;
        await Assert.That(pending.UnresolvedDependencies.Count).IsEqualTo(1);
        await Assert.That(pending.DependentModules.Count).IsEqualTo(1);
    }

    [Test]
    public async Task RegisterModule_DuplicateRegistration_ThrowsException()
    {
        var module = new TestModule();
        var moduleType = typeof(TestModule);

        _store.RegisterModule(
            module, moduleType,
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        await Assert.That(() => _store.RegisterModule(
            module, moduleType,
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default))
            .ThrowsExactly<InvalidOperationException>();
    }

    #endregion

    #region Get State Tests

    [Test]
    public async Task GetState_ReturnsRegisteredState()
    {
        var module = new TestModule();
        _store.RegisterModule(
            module, typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var state = _store.GetState(typeof(TestModule));

        await Assert.That(state).IsNotNull();
        await Assert.That(state!.Module).IsEqualTo(module);
    }

    [Test]
    public async Task GetState_UnknownModule_ReturnsNull()
    {
        var state = _store.GetState(typeof(string));

        await Assert.That(state).IsNull();
    }

    #endregion

    #region Transition Tests

    [Test]
    public async Task TransitionToQueued_FromPendingWithNoDependencies_Succeeds()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var result = _store.TransitionToQueued(typeof(TestModule));

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Phase).IsTypeOf<ModuleExecutionPhase.Queued>();
    }

    [Test]
    public async Task TransitionToQueued_FromPendingWithDependencies_ReturnsNull()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet.Create(typeof(string)), ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var result = _store.TransitionToQueued(typeof(TestModule));

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task TransitionToRunning_FromQueued_Succeeds()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        _store.TransitionToQueued(typeof(TestModule));
        using var cts = new CancellationTokenSource();

        var result = _store.TransitionToRunning(typeof(TestModule), cts);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Phase).IsTypeOf<ModuleExecutionPhase.Running>();
    }

    [Test]
    public async Task TransitionToCompleted_FromRunning_Succeeds()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        _store.TransitionToQueued(typeof(TestModule));
        _store.TransitionToRunning(typeof(TestModule), new CancellationTokenSource());

        var moduleResult = new TestModuleResult();
        var result = _store.TransitionToCompleted(typeof(TestModule), moduleResult);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Phase).IsTypeOf<ModuleExecutionPhase.Completed>();
        await Assert.That(result.IsSuccessful).IsTrue();
    }

    [Test]
    public async Task TransitionToFailed_FromRunning_Succeeds()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        _store.TransitionToQueued(typeof(TestModule));
        _store.TransitionToRunning(typeof(TestModule), new CancellationTokenSource());

        var exception = new Exception("Test failure");
        var moduleResult = new TestModuleResult();
        var result = _store.TransitionToFailed(typeof(TestModule), exception, moduleResult);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Phase).IsTypeOf<ModuleExecutionPhase.Failed>();

        var failed = (ModuleExecutionPhase.Failed)result.Phase;
        await Assert.That(failed.Exception.Message).IsEqualTo("Test failure");
    }

    [Test]
    public async Task TransitionToSkipped_FromPending_Succeeds()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet.Create(typeof(string)), ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var skipDecision = SkipDecision.Skip("Test skip");
        var moduleResult = new TestModuleResult();
        var result = _store.TransitionToSkipped(typeof(TestModule), skipDecision, moduleResult);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Phase).IsTypeOf<ModuleExecutionPhase.Skipped>();
    }

    [Test]
    public async Task RevertToPending_FromQueued_Succeeds()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        _store.TransitionToQueued(typeof(TestModule));
        var result = _store.RevertToPending(typeof(TestModule));

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Phase).IsTypeOf<ModuleExecutionPhase.Pending>();
    }

    #endregion

    #region Dependency Resolution Tests

    [Test]
    public async Task ResolveDependency_RemovesDependencyFromPending()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet.Create(typeof(string), typeof(int)), ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var result = _store.ResolveDependency(typeof(TestModule), typeof(string));

        await Assert.That(result).IsNotNull();
        var pending = (ModuleExecutionPhase.Pending)result!.Phase;
        await Assert.That(pending.UnresolvedDependencies.Count).IsEqualTo(1);
        await Assert.That(pending.UnresolvedDependencies.Contains(typeof(int))).IsTrue();
    }

    [Test]
    public async Task ResolveDependency_AllResolved_BecomesReadyToQueue()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet.Create(typeof(string)), ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var result = _store.ResolveDependency(typeof(TestModule), typeof(string));

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.IsReadyToQueue).IsTrue();
    }

    #endregion

    #region Query Tests

    [Test]
    public async Task GetReadyModules_ReturnsOnlyReadyPendingModules()
    {
        // Module with no dependencies (ready)
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        // Module with dependencies (not ready)
        _store.RegisterModule(
            new TestModule2(), typeof(TestModule2),
            ImmutableHashSet.Create(typeof(string)), ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var ready = _store.GetReadyModules().ToList();

        await Assert.That(ready.Count).IsEqualTo(1);
        await Assert.That(ready[0].ModuleType).IsEqualTo(typeof(TestModule));
    }

    [Test]
    public async Task GetStateCounts_ReturnsCorrectCounts()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        _store.RegisterModule(
            new TestModule2(), typeof(TestModule2),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        _store.TransitionToQueued(typeof(TestModule));

        var (pending, queued, running, completed) = _store.GetStateCounts();

        await Assert.That(pending).IsEqualTo(1);
        await Assert.That(queued).IsEqualTo(1);
        await Assert.That(running).IsEqualTo(0);
        await Assert.That(completed).IsEqualTo(0);
    }

    #endregion

    #region State Changed Event Tests

    [Test]
    public async Task StateChanged_FiresOnTransition()
    {
        var eventFired = false;
        ModuleStateSnapshot? oldState = null;
        ModuleStateSnapshot? newState = null;

        _store.StateChanged += (old, @new) =>
        {
            eventFired = true;
            oldState = old;
            newState = @new;
        };

        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        _store.TransitionToQueued(typeof(TestModule));

        await Assert.That(eventFired).IsTrue();
        await Assert.That(oldState!.Phase).IsTypeOf<ModuleExecutionPhase.Pending>();
        await Assert.That(newState!.Phase).IsTypeOf<ModuleExecutionPhase.Queued>();
    }

    #endregion

    #region Status Compatibility Tests

    [Test]
    public async Task Status_ReturnsCorrectEnumForEachPhase()
    {
        _store.RegisterModule(
            new TestModule(), typeof(TestModule),
            ImmutableHashSet<Type>.Empty, ImmutableList<Type>.Empty,
            false, Array.Empty<string>(), ModulePriority.Normal, ExecutionType.Default);

        var pendingState = _store.GetState(typeof(TestModule));
        await Assert.That(pendingState!.Status).IsEqualTo(Status.NotYetStarted);

        _store.TransitionToQueued(typeof(TestModule));
        var queuedState = _store.GetState(typeof(TestModule));
        await Assert.That(queuedState!.Status).IsEqualTo(Status.NotYetStarted);

        _store.TransitionToRunning(typeof(TestModule), new CancellationTokenSource());
        var runningState = _store.GetState(typeof(TestModule));
        await Assert.That(runningState!.Status).IsEqualTo(Status.Processing);

        _store.TransitionToCompleted(typeof(TestModule), new TestModuleResult());
        var completedState = _store.GetState(typeof(TestModule));
        await Assert.That(completedState!.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region Helper Classes

    private class TestModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    private class TestModule2 : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test2");
    }

    private class TestModuleResult : IModuleResult
    {
        public string ModuleName => "TestModule";

        public TimeSpan ModuleDuration => TimeSpan.Zero;

        public DateTimeOffset ModuleStart => DateTimeOffset.UtcNow;

        public DateTimeOffset ModuleEnd => DateTimeOffset.UtcNow;

        public Status ModuleStatus => Status.Successful;

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
