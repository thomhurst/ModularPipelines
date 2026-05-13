namespace ModularPipelines.UnitTests.FSharp.State

open System
open System.Collections.Immutable
open System.Threading
open ModularPipelines.Engine.State
open ModularPipelines.Enums
open ModularPipelines.Models
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private TransitionTestModuleResult() =
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

type ModuleStateTransitionsTests() =
    inherit TestBase()

    [<Test>]
    member _.IsValidTransition_PendingToQueued_ReturnsTrue() = async {
        let fromPhase =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                DependentModules = ImmutableList<Type>.Empty
            )

        let toPhase =
            ModuleExecutionPhase.Queued(
                DependentModules = ImmutableList<Type>.Empty,
                QueuedAt = DateTimeOffset.UtcNow,
                ReadyAt = DateTimeOffset.UtcNow
            )

        let result = ModuleStateTransitions.IsValidTransition(fromPhase, toPhase)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.IsValidTransition_QueuedToRunning_ReturnsTrue() = async {
        let fromPhase =
            ModuleExecutionPhase.Queued(
                DependentModules = ImmutableList<Type>.Empty,
                QueuedAt = DateTimeOffset.UtcNow,
                ReadyAt = DateTimeOffset.UtcNow
            )

        let toPhase =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                QueuedAt = DateTimeOffset.UtcNow,
                CancellationSource = CancellationTokenSource()
            )

        let result = ModuleStateTransitions.IsValidTransition(fromPhase, toPhase)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.IsValidTransition_QueuedToPending_ReturnsTrue() = async {
        let fromPhase =
            ModuleExecutionPhase.Queued(
                DependentModules = ImmutableList<Type>.Empty,
                QueuedAt = DateTimeOffset.UtcNow,
                ReadyAt = DateTimeOffset.UtcNow
            )

        let toPhase =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                DependentModules = ImmutableList<Type>.Empty
            )

        let result = ModuleStateTransitions.IsValidTransition(fromPhase, toPhase)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.IsValidTransition_RunningToCompleted_ReturnsTrue() = async {
        let fromPhase =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                QueuedAt = DateTimeOffset.UtcNow,
                CancellationSource = CancellationTokenSource()
            )

        let toPhase =
            ModuleExecutionPhase.Completed(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                CompletedAt = DateTimeOffset.UtcNow,
                Result = TransitionTestModuleResult()
            )

        let result = ModuleStateTransitions.IsValidTransition(fromPhase, toPhase)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.IsValidTransition_RunningToFailed_ReturnsTrue() = async {
        let fromPhase =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                QueuedAt = DateTimeOffset.UtcNow,
                CancellationSource = CancellationTokenSource()
            )

        let toPhase =
            ModuleExecutionPhase.Failed(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                FailedAt = DateTimeOffset.UtcNow,
                Exception = Exception(),
                Result = TransitionTestModuleResult()
            )

        let result = ModuleStateTransitions.IsValidTransition(fromPhase, toPhase)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.IsValidTransition_CompletedToAny_ReturnsFalse() = async {
        let fromPhase =
            ModuleExecutionPhase.Completed(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                CompletedAt = DateTimeOffset.UtcNow,
                Result = TransitionTestModuleResult()
            )

        let toPhase =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                QueuedAt = DateTimeOffset.UtcNow,
                CancellationSource = CancellationTokenSource()
            )

        let result = ModuleStateTransitions.IsValidTransition(fromPhase, toPhase)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.IsValidTransition_PendingToRunning_ReturnsFalse() = async {
        let fromPhase =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                DependentModules = ImmutableList<Type>.Empty
            )

        let toPhase =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                QueuedAt = DateTimeOffset.UtcNow,
                CancellationSource = CancellationTokenSource()
            )

        let result = ModuleStateTransitions.IsValidTransition(fromPhase, toPhase)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.RemoveDependency_CreatesPendingWithDependencyRemoved() = async {
        let pending =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet.Create(typeof<string>, typeof<int>),
                DependentModules = ImmutableList<Type>.Empty
            )

        let result = ModuleStateTransitions.RemoveDependency(pending, typeof<string>)

        do! check(Assert.That(result.UnresolvedDependencies.Count = 1).IsTrue())
        do! check(Assert.That(result.UnresolvedDependencies.Contains(typeof<int>)).IsTrue())
        do! check(Assert.That(result.UnresolvedDependencies.Contains(typeof<string>)).IsFalse())
    }

    [<Test>]
    member _.ToQueued_CreateQueuedFromPending() = async {
        let pending =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                DependentModules = ImmutableList.Create(typeof<string>)
            )

        let now = DateTimeOffset.UtcNow
        let result = ModuleStateTransitions.ToQueued(pending, now)

        do! check(Assert.That(result.QueuedAt = now).IsTrue())
        do! check(Assert.That(result.ReadyAt = now).IsTrue())
        do! check(Assert.That(result.DependentModules.Count = 1).IsTrue())
    }

    [<Test>]
    member _.ToQueued_WithUnresolvedDependencies_ThrowsException() = async {
        let pending =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet.Create(typeof<string>),
                DependentModules = ImmutableList<Type>.Empty
            )

        let mutable threw = false

        try
            ModuleStateTransitions.ToQueued(pending, DateTimeOffset.UtcNow) |> ignore
        with :? InvalidOperationException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.ToRunning_CreatesRunningFromQueued() = async {
        let queuedAt = DateTimeOffset.UtcNow.AddSeconds(-1.0)

        let queued =
            ModuleExecutionPhase.Queued(
                DependentModules = ImmutableList.Create(typeof<string>),
                QueuedAt = queuedAt,
                ReadyAt = queuedAt
            )

        let now = DateTimeOffset.UtcNow
        use cts = new CancellationTokenSource()

        let result = ModuleStateTransitions.ToRunning(queued, now, cts)

        do! check(Assert.That(result.StartedAt = now).IsTrue())
        do! check(Assert.That(result.QueuedAt = queuedAt).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result.CancellationSource, cts)).IsTrue())
        do! check(Assert.That(result.DependentModules.Count = 1).IsTrue())
    }

    [<Test>]
    member _.ToPending_RevertQueuedToPending() = async {
        let queued =
            ModuleExecutionPhase.Queued(
                DependentModules = ImmutableList.Create(typeof<string>),
                QueuedAt = DateTimeOffset.UtcNow,
                ReadyAt = DateTimeOffset.UtcNow
            )

        let result = ModuleStateTransitions.ToPending(queued)

        do! check(Assert.That(result.UnresolvedDependencies.IsEmpty).IsTrue())
        do! check(Assert.That(result.DependentModules.Count = 1).IsTrue())
    }

    [<Test>]
    member _.ToCompleted_CreatesCompletedFromRunning() = async {
        let startedAt = DateTimeOffset.UtcNow.AddSeconds(-5.0)

        let running =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = startedAt,
                QueuedAt = startedAt.AddSeconds(-1.0),
                CancellationSource = CancellationTokenSource()
            )

        let completedAt = DateTimeOffset.UtcNow
        let moduleResult = TransitionTestModuleResult()

        let result = ModuleStateTransitions.ToCompleted(running, completedAt, moduleResult)

        do! check(Assert.That(result.StartedAt = startedAt).IsTrue())
        do! check(Assert.That(result.CompletedAt = completedAt).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result.Result, moduleResult)).IsTrue())
        do! check(Assert.That(result.Duration = (completedAt - startedAt)).IsTrue())
    }

    [<Test>]
    member _.ToFailed_CreatesFailedFromRunning() = async {
        let startedAt = DateTimeOffset.UtcNow.AddSeconds(-3.0)

        let running =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = startedAt,
                QueuedAt = startedAt.AddSeconds(-1.0),
                CancellationSource = CancellationTokenSource()
            )

        let failedAt = DateTimeOffset.UtcNow
        let exnValue = InvalidOperationException("Test")
        let moduleResult = TransitionTestModuleResult()

        let result = ModuleStateTransitions.ToFailed(running, failedAt, exnValue, moduleResult)

        do! check(Assert.That(result.StartedAt = startedAt).IsTrue())
        do! check(Assert.That(result.FailedAt = failedAt).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result.Exception, exnValue)).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result.Result, moduleResult)).IsTrue())
    }

    [<Test>]
    member _.IsTerminal_ReturnsTrueForTerminalStates() = async {
        let completed =
            ModuleExecutionPhase.Completed(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                CompletedAt = DateTimeOffset.UtcNow,
                Result = TransitionTestModuleResult()
            )

        let failed =
            ModuleExecutionPhase.Failed(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                FailedAt = DateTimeOffset.UtcNow,
                Exception = Exception(),
                Result = TransitionTestModuleResult()
            )

        do! check(Assert.That(ModuleStateTransitions.IsTerminal(completed)).IsTrue())
        do! check(Assert.That(ModuleStateTransitions.IsTerminal(failed)).IsTrue())
    }

    [<Test>]
    member _.IsTerminal_ReturnsFalseForNonTerminalStates() = async {
        let pending =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                DependentModules = ImmutableList<Type>.Empty
            )

        let running =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                QueuedAt = DateTimeOffset.UtcNow,
                CancellationSource = CancellationTokenSource()
            )

        do! check(Assert.That(ModuleStateTransitions.IsTerminal(pending)).IsFalse())
        do! check(Assert.That(ModuleStateTransitions.IsTerminal(running)).IsFalse())
    }

    [<Test>]
    member _.GetDependentModules_ReturnsCorrectListForAllPhases() = async {
        let dependents = ImmutableList.Create(typeof<string>, typeof<int>)

        let pending =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                DependentModules = dependents
            )

        let result = ModuleStateTransitions.GetDependentModules(pending)
        do! check(Assert.That(result.Count = 2).IsTrue())
    }
