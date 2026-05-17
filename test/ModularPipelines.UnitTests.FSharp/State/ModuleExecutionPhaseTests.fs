namespace ModularPipelines.UnitTests.FSharp.State

open System
open System.Collections.Immutable
open ModularPipelines.Engine.State
open ModularPipelines.Enums
open ModularPipelines.Models
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private TestModuleResult() =
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

type ModuleExecutionPhaseTests() =
    inherit TestBase()

    [<Test>]
    member _.Pending_WithNoDependencies_IsReadyToQueue() = async {
        let pending =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                DependentModules = ImmutableList<Type>.Empty
            )

        do! check(Assert.That(pending.IsReadyToQueue).IsTrue())
    }

    [<Test>]
    member _.Pending_WithDependencies_IsNotReadyToQueue() = async {
        let pending =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet.Create(typeof<string>),
                DependentModules = ImmutableList<Type>.Empty
            )

        do! check(Assert.That(pending.IsReadyToQueue).IsFalse())
    }

    [<Test>]
    member _.Pending_RemovingDependency_CreatesNewInstance() = async {
        let original =
            ModuleExecutionPhase.Pending(
                UnresolvedDependencies = ImmutableHashSet.Create(typeof<string>, typeof<int>),
                DependentModules = ImmutableList<Type>.Empty
            )

        let updated = ModuleStateTransitions.RemoveDependency(original, typeof<string>)

        do! check(Assert.That(updated.UnresolvedDependencies.Count = 1).IsTrue())
        do! check(Assert.That(original.UnresolvedDependencies.Count = 2).IsTrue())
    }

    [<Test>]
    member _.Queued_HasCorrectTimestamps() = async {
        let now = DateTimeOffset.UtcNow

        let queued =
            ModuleExecutionPhase.Queued(
                DependentModules = ImmutableList<Type>.Empty,
                QueuedAt = now,
                ReadyAt = now
            )

        do! check(Assert.That(queued.QueuedAt = now).IsTrue())
        do! check(Assert.That(queued.ReadyAt = now).IsTrue())
    }

    [<Test>]
    member _.Running_HasStartedAt() = async {
        let now = DateTimeOffset.UtcNow
        use cts = new System.Threading.CancellationTokenSource()

        let running =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = now,
                QueuedAt = now.AddSeconds(-1),
                CancellationSource = cts
            )

        do! check(Assert.That(running.StartedAt = now).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(running.CancellationSource, cts)).IsTrue())
    }

    [<Test>]
    member _.Completed_CalculatesDuration() = async {
        let startTime = DateTimeOffset.UtcNow
        let endTime = startTime.AddSeconds(5)

        let completed =
            ModuleExecutionPhase.Completed(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = startTime,
                CompletedAt = endTime,
                Result = TestModuleResult()
            )

        do! check(Assert.That(completed.Duration = TimeSpan.FromSeconds(5.0)).IsTrue())
    }

    [<Test>]
    member _.Failed_CalculatesDuration() = async {
        let startTime = DateTimeOffset.UtcNow
        let failTime = startTime.AddSeconds(3)
        let ex = InvalidOperationException("Test failure")

        let failed =
            ModuleExecutionPhase.Failed(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = startTime,
                FailedAt = failTime,
                Exception = ex,
                Result = TestModuleResult()
            )

        do! check(Assert.That(failed.Duration = TimeSpan.FromSeconds(3.0)).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(failed.Exception.Message), "Test failure"))
    }

    [<Test>]
    member _.Skipped_HasSkipDecision() = async {
        let skipDecision = SkipDecision.Skip("Test skip reason")

        let skipped =
            ModuleExecutionPhase.Skipped(
                DependentModules = ImmutableList<Type>.Empty,
                SkippedAt = DateTimeOffset.UtcNow,
                SkipDecision = skipDecision,
                Result = TestModuleResult()
            )

        do! check(Assert.That(skipped.SkipDecision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(skipped.SkipDecision.Reason), "Test skip reason"))
    }

    [<Test>]
    member _.Cancelled_PreservesPreviousPhase() = async {
        let previousPhase =
            ModuleExecutionPhase.Running(
                DependentModules = ImmutableList<Type>.Empty,
                StartedAt = DateTimeOffset.UtcNow,
                QueuedAt = DateTimeOffset.UtcNow.AddSeconds(-1),
                CancellationSource = new System.Threading.CancellationTokenSource()
            )

        let cancelled =
            ModuleExecutionPhase.Cancelled(
                DependentModules = ImmutableList<Type>.Empty,
                CancelledAt = DateTimeOffset.UtcNow,
                PreviousPhase = previousPhase
            )

        do! check(Assert.That(obj.ReferenceEquals(cancelled.PreviousPhase, previousPhase)).IsTrue())
    }
