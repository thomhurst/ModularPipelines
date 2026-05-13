namespace ModularPipelines.UnitTests.FSharp.State

open System
open System.Collections.Immutable
open System.Threading
open Microsoft.Extensions.Time.Testing
open ModularPipelines.Context
open ModularPipelines.Engine.State
open ModularPipelines.Enums
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private StoreTestModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        System.Threading.Tasks.Task.FromResult<string>(null)

type private StoreTestModule2() =
    inherit Module<string>()
    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        System.Threading.Tasks.Task.FromResult<string>(null)

type private StoreTestModuleResult() =
    interface IModuleResult with
        member _.ModuleName = "TestModule"
        member _.ModuleDuration = TimeSpan.Zero
        member _.ModuleStart = DateTimeOffset.UtcNow
        member _.ModuleEnd = DateTimeOffset.UtcNow
        member _.ModuleStatus = Status.Successful
        member _.ModuleResultType = ModuleResultType.Success
        member _.IsSuccess = true
        member _.IsFailure = false
        member _.IsSkipped = false
        member _.ValueOrDefault = null
        member _.ExceptionOrDefault = null
        member _.SkipDecisionOrDefault = Unchecked.defaultof<_>

type ModuleStateStoreTests() =
    inherit TestBase()

    let mutable _timeProvider: FakeTimeProvider = Unchecked.defaultof<_>
    let mutable _store: ModuleStateStore = Unchecked.defaultof<_>

    [<Before(HookType.Test)>]
    member _.Setup() =
        _timeProvider <- FakeTimeProvider(DateTimeOffset.UtcNow)
        _store <- ModuleStateStore(_timeProvider)

    [<Test>]
    member _.RegisterModule_CreatesInitialPendingState() = async {
        let m = StoreTestModule()
        let moduleType = typeof<StoreTestModule>
        let dependencies = ImmutableHashSet.Create(typeof<string>)
        let dependents = ImmutableList.Create(typeof<int>)

        let state =
            _store.RegisterModule(
                m,
                moduleType,
                dependencies,
                dependents,
                requiresSequentialExecution = false,
                requiredLockKeys = Array.empty,
                priority = ModulePriority.Normal,
                executionType = ExecutionType.Default
            )

        do! check(Assert.That(LanguagePrimitives.PhysicalEquality state.Module m).IsTrue())
        do! check(Assert.That(state.ModuleType = moduleType).IsTrue())
        do! check(Assert.That(state.Phase).IsTypeOf<ModuleExecutionPhase.Pending>())

        let pending = state.Phase :?> ModuleExecutionPhase.Pending
        do! check(Assert.That(pending.UnresolvedDependencies.Count = 1).IsTrue())
        do! check(Assert.That(pending.DependentModules.Count = 1).IsTrue())
    }

    [<Test>]
    member _.RegisterModule_DuplicateRegistration_ThrowsException() = async {
        let m = StoreTestModule()
        let moduleType = typeof<StoreTestModule>

        _store.RegisterModule(
            m,
            moduleType,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let mutable threw = false

        try
            _store.RegisterModule(
                m,
                moduleType,
                ImmutableHashSet<Type>.Empty,
                ImmutableList<Type>.Empty,
                false,
                Array.empty,
                ModulePriority.Normal,
                ExecutionType.Default
            )
            |> ignore
        with :? InvalidOperationException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.GetState_ReturnsRegisteredState() = async {
        let m = StoreTestModule()

        _store.RegisterModule(
            m,
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let state = _store.GetState(typeof<StoreTestModule>)

        do! check(Assert.That(state).IsNotNull())
        do! check(Assert.That(LanguagePrimitives.PhysicalEquality state.Module m).IsTrue())
    }

    [<Test>]
    member _.GetState_UnknownModule_ReturnsNull() = async {
        let state = _store.GetState(typeof<string>)
        do! check(Assert.That(state).IsNull())
    }

    [<Test>]
    member _.TransitionToQueued_FromPendingWithNoDependencies_Succeeds() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let result = _store.TransitionToQueued(typeof<StoreTestModule>)

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(result.Phase).IsTypeOf<ModuleExecutionPhase.Queued>())
    }

    [<Test>]
    member _.TransitionToQueued_FromPendingWithDependencies_ReturnsNull() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet.Create(typeof<string>),
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let result = _store.TransitionToQueued(typeof<StoreTestModule>)

        do! check(Assert.That(result).IsNull())
    }

    [<Test>]
    member _.TransitionToRunning_FromQueued_Succeeds() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        _store.TransitionToQueued(typeof<StoreTestModule>) |> ignore
        use cts = new CancellationTokenSource()

        let result = _store.TransitionToRunning(typeof<StoreTestModule>, cts)

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(result.Phase).IsTypeOf<ModuleExecutionPhase.Running>())
    }

    [<Test>]
    member _.TransitionToCompleted_FromRunning_Succeeds() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        _store.TransitionToQueued(typeof<StoreTestModule>) |> ignore
        _store.TransitionToRunning(typeof<StoreTestModule>, new CancellationTokenSource()) |> ignore

        let moduleResult = StoreTestModuleResult()
        let result = _store.TransitionToCompleted(typeof<StoreTestModule>, moduleResult)

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(result.Phase).IsTypeOf<ModuleExecutionPhase.Completed>())
        do! check(Assert.That(result.IsSuccessful).IsTrue())
    }

    [<Test>]
    member _.TransitionToFailed_FromRunning_Succeeds() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        _store.TransitionToQueued(typeof<StoreTestModule>) |> ignore
        _store.TransitionToRunning(typeof<StoreTestModule>, new CancellationTokenSource()) |> ignore

        let ex = Exception("Test failure")
        let moduleResult = StoreTestModuleResult()
        let result = _store.TransitionToFailed(typeof<StoreTestModule>, ex, moduleResult)

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(result.Phase).IsTypeOf<ModuleExecutionPhase.Failed>())

        let failed = result.Phase :?> ModuleExecutionPhase.Failed
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(failed.Exception.Message), "Test failure"))
    }

    [<Test>]
    member _.TransitionToSkipped_FromPending_Succeeds() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet.Create(typeof<string>),
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let skipDecision = SkipDecision.Skip("Test skip")
        let moduleResult = StoreTestModuleResult()
        let result = _store.TransitionToSkipped(typeof<StoreTestModule>, skipDecision, moduleResult)

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(result.Phase).IsTypeOf<ModuleExecutionPhase.Skipped>())
    }

    [<Test>]
    member _.RevertToPending_FromQueued_Succeeds() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        _store.TransitionToQueued(typeof<StoreTestModule>) |> ignore
        let result = _store.RevertToPending(typeof<StoreTestModule>)

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(result.Phase).IsTypeOf<ModuleExecutionPhase.Pending>())
    }

    [<Test>]
    member _.ResolveDependency_RemovesDependencyFromPending() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet.Create(typeof<string>, typeof<int>),
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let result = _store.ResolveDependency(typeof<StoreTestModule>, typeof<string>)

        do! check(Assert.That(result).IsNotNull())
        let pending = result.Phase :?> ModuleExecutionPhase.Pending
        do! check(Assert.That(pending.UnresolvedDependencies.Count = 1).IsTrue())
        do! check(Assert.That(pending.UnresolvedDependencies.Contains(typeof<int>)).IsTrue())
    }

    [<Test>]
    member _.ResolveDependency_AllResolved_BecomesReadyToQueue() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet.Create(typeof<string>),
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let result = _store.ResolveDependency(typeof<StoreTestModule>, typeof<string>)

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(result.IsReadyToQueue).IsTrue())
    }

    [<Test>]
    member _.GetReadyModules_ReturnsOnlyReadyPendingModules() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        _store.RegisterModule(
            StoreTestModule2(),
            typeof<StoreTestModule2>,
            ImmutableHashSet.Create(typeof<string>),
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let ready = _store.GetReadyModules() |> Seq.toList

        do! check(Assert.That(ready.Length = 1).IsTrue())
        do! check(Assert.That(ready.[0].ModuleType = typeof<StoreTestModule>).IsTrue())
    }

    [<Test>]
    member _.GetStateCounts_ReturnsCorrectCounts() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        _store.RegisterModule(
            StoreTestModule2(),
            typeof<StoreTestModule2>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        _store.TransitionToQueued(typeof<StoreTestModule>) |> ignore

        let struct (pending, queued, running, completed) = _store.GetStateCounts()

        do! check(Assert.That((pending = 1)).IsTrue())
        do! check(Assert.That((queued = 1)).IsTrue())
        do! check(Assert.That((running = 0)).IsTrue())
        do! check(Assert.That((completed = 0)).IsTrue())
    }

    [<Test>]
    member _.StateChanged_FiresOnTransition() = async {
        let mutable eventFired = false
        let mutable oldState: ModuleStateSnapshot = Unchecked.defaultof<_>
        let mutable newState: ModuleStateSnapshot = Unchecked.defaultof<_>

        let handler =
            Action<ModuleStateSnapshot, ModuleStateSnapshot>(fun old n ->
                eventFired <- true
                oldState <- old
                newState <- n)

        try
            _store.add_StateChanged(handler)

            _store.RegisterModule(
                StoreTestModule(),
                typeof<StoreTestModule>,
                ImmutableHashSet<Type>.Empty,
                ImmutableList<Type>.Empty,
                false,
                Array.empty,
                ModulePriority.Normal,
                ExecutionType.Default
            )
            |> ignore

            _store.TransitionToQueued(typeof<StoreTestModule>) |> ignore
        finally
            _store.remove_StateChanged(handler)

        do! check(Assert.That(eventFired).IsTrue())
        do! check(Assert.That(oldState.Phase).IsTypeOf<ModuleExecutionPhase.Pending>())
        do! check(Assert.That(newState.Phase).IsTypeOf<ModuleExecutionPhase.Queued>())
    }

    [<Test>]
    member _.Status_ReturnsCorrectEnumForEachPhase() = async {
        _store.RegisterModule(
            StoreTestModule(),
            typeof<StoreTestModule>,
            ImmutableHashSet<Type>.Empty,
            ImmutableList<Type>.Empty,
            false,
            Array.empty,
            ModulePriority.Normal,
            ExecutionType.Default
        )
        |> ignore

        let pendingState = _store.GetState(typeof<StoreTestModule>)
        do! check(Assert.That(pendingState.Status = Status.NotYetStarted).IsTrue())

        _store.TransitionToQueued(typeof<StoreTestModule>) |> ignore
        let queuedState = _store.GetState(typeof<StoreTestModule>)
        do! check(Assert.That(queuedState.Status = Status.NotYetStarted).IsTrue())

        _store.TransitionToRunning(typeof<StoreTestModule>, new CancellationTokenSource()) |> ignore
        let runningState = _store.GetState(typeof<StoreTestModule>)
        do! check(Assert.That(runningState.Status = Status.Processing).IsTrue())

        _store.TransitionToCompleted(typeof<StoreTestModule>, StoreTestModuleResult()) |> ignore
        let completedState = _store.GetState(typeof<StoreTestModule>)
        do! check(Assert.That(completedState.Status = Status.Successful).IsTrue())
    }
