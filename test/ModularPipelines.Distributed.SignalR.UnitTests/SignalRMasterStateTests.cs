using System.Collections.Frozen;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRMasterStateTests
{
    [Test]
    public async Task WorkerState_TryAssign_Returns_True_When_Idle()
    {
        var worker = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };

        var result = worker.TryAssign(CreateAssignment());
        await Assert.That(result).IsTrue();
        await Assert.That(worker.IsIdle).IsFalse();
    }

    [Test]
    public async Task WorkerState_TryAssign_Returns_False_When_Already_Busy()
    {
        var worker = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };

        worker.TryAssign(CreateAssignment());
        var result = worker.TryAssign(CreateAssignment());

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task WorkerState_TryCompleteAssignment_Resets_BusyFlag()
    {
        var worker = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };

        worker.TryAssign(CreateAssignment());
        await Assert.That(worker.IsIdle).IsFalse();

        worker.TryCompleteAssignment("TestModule");
        await Assert.That(worker.IsIdle).IsTrue();
    }

    [Test]
    public async Task SignalRMasterState_Collections_Are_Thread_Safe()
    {
        var state = new SignalRMasterState();

        // Concurrent access should not throw
        var tasks = Enumerable.Range(0, 100).Select(i => Task.Run(() =>
        {
            state.Workers[$"conn-{i}"] = new WorkerState
            {
                ConnectionId = $"conn-{i}",
                Registration = new WorkerRegistration(i, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
            };
            state.Registrations[i] = new WorkerRegistration(i, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow);
            state.PendingAssignments.Enqueue(new ModuleAssignment(
                $"Module{i}", "System.String", FrozenSet<Capability>.Empty,
                DateTimeOffset.UtcNow, new ModuleAssignmentConfiguration(null, false)));
            state.ResultWaiters[$"Module{i}"] = new TaskCompletionSource<SerializedModuleResult>();
        }));

        await Task.WhenAll(tasks);

        await Assert.That(state.Workers.Count).IsEqualTo(100);
        await Assert.That(state.Registrations.Count).IsEqualTo(100);
        await Assert.That(state.PendingAssignments.Count).IsEqualTo(100);
        await Assert.That(state.ResultWaiters.Count).IsEqualTo(100);
    }

    [Test]
    public async Task Completion_Flag_Is_Volatile()
    {
        var state = new SignalRMasterState();
        await Assert.That(state.IsCompleted).IsFalse();

        state.IsCompleted = true;
        await Assert.That(state.IsCompleted).IsTrue();
    }

    [Test]
    public async Task PendingReconnect_Allows_Exactly_One_Resume_Or_Redispatch()
    {
        using var pending = new PendingReconnect(
            1,
            new ModuleAssignment(
                "TestModule",
                "System.String",
                FrozenSet<Capability>.Empty,
                DateTimeOffset.UtcNow,
                new ModuleAssignmentConfiguration(null, false)));

        await Assert.That(pending.TryMakeAvailableForRedispatch()).IsTrue();

        var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var resume = Task.Run(async () =>
        {
            await start.Task;
            return pending.TryResume();
        });
        var redispatch = Task.Run(async () =>
        {
            await start.Task;
            return pending.TryClaimRedispatch();
        });

        start.SetResult();
        var results = await Task.WhenAll(resume, redispatch);

        await Assert.That(results.Count(result => result)).IsEqualTo(1);
    }

    [Test]
    public async Task PendingReconnect_Allows_Only_One_Resume_Owner()
    {
        using var pending = new PendingReconnect(1, CreateAssignment());

        await Assert.That(pending.TryResume()).IsTrue();
        await Assert.That(pending.TryResume()).IsFalse();
    }

    [Test]
    public async Task Reconnect_Reclaims_Queued_Retry_Until_Dispatch_Claims_It()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;

        await Assert.That(pending.TryMakeAvailableForRedispatch()).IsTrue();
        await Assert.That(pending.TryResume()).IsTrue();
        await Assert.That(state.TryClaimRedispatch(assignment)).IsFalse();

        state.CompletePendingReconnect(assignment.ModuleTypeName);
    }

    [Test]
    public async Task ReplacementWorker_Cannot_Reclaim_Queued_Retry_By_Index()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
        var replacement = new WorkerState
        {
            ConnectionId = "replacement",
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };

        await Assert.That(pending.TryMakeAvailableForRedispatch()).IsTrue();

        var restored = state.TryRestoreReconnect(replacement, null, out _);

        await Assert.That(restored).IsFalse();
        await Assert.That(replacement.IsIdle).IsTrue();
        await Assert.That(state.TryClaimRedispatch(assignment)).IsTrue();

        state.CompletePendingReconnect(assignment.ModuleTypeName);
    }

    [Test]
    public async Task ResultCompletion_And_ReconnectRegistration_Cannot_Leave_Worker_Busy()
    {
        for (var i = 0; i < 100; i++)
        {
            var state = new SignalRMasterState();
            var assignment = CreateAssignment();
            state.ResultWaiters[assignment.ModuleTypeName] =
                new TaskCompletionSource<SerializedModuleResult>(
                    TaskCreationOptions.RunContinuationsAsynchronously);

            var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
            pending.TryMakeAvailableForRedispatch();
            pending.TryClaimRedispatch();

            var worker = new WorkerState
            {
                ConnectionId = $"connection-{i}",
                Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
            };

            var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var reconnect = Task.Run(async () =>
            {
                await start.Task;
                return state.TryRestoreReconnect(
                    worker,
                    assignment.ModuleTypeName,
                    out _);
            });
            var completion = Task.Run(async () =>
            {
                await start.Task;
                return await state.CompleteResultAsync(CreateResult());
            });

            start.SetResult();
            var reconnectRestored = await reconnect;
            var workersToRelease = await completion;

            foreach (var trackedWorker in workersToRelease)
            {
                trackedWorker.TryCompleteAssignment(assignment.ModuleTypeName);
            }

            await Assert.That(worker.IsIdle).IsTrue();
            await Assert.That(reconnectRestored).IsEqualTo(workersToRelease.Contains(worker));
        }
    }

    [Test]
    public async Task Redispatched_Assignment_Cannot_Be_Reclaimed_By_Late_Reconnect()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        state.ResultWaiters[assignment.ModuleTypeName] =
            new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
        var reconnect = CreateWorker(connectionId: "reconnect");

        pending.TryMakeAvailableForRedispatch();
        await Assert.That(state.TryClaimRedispatch(assignment)).IsTrue();

        var restored = state.TryRestoreReconnect(
            reconnect,
            assignment.ModuleTypeName,
            out var restoredAssignment);

        await Assert.That(restored).IsFalse();
        await Assert.That(restoredAssignment).IsNull();
        await Assert.That(reconnect.IsIdle).IsTrue();
    }

    [Test]
    public async Task Completed_Assignment_Cannot_Be_Claimed_After_Pending_Record_Is_Removed()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        state.ResultWaiters[assignment.ModuleTypeName] =
            new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
        pending.TryMakeAvailableForRedispatch();

        await state.CompleteResultAsync(CreateResult());

        await Assert.That(state.TryClaimRedispatch(assignment)).IsFalse();
    }

    [Test]
    public async Task Replacing_Pending_Reconnect_Preserves_Other_Tracked_Workers()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var firstOwner = CreateWorker(connectionId: "first-owner");
        var trackedWorker = CreateWorker(2, "tracked");
        var replacementOwner = CreateWorker(3, "replacement-owner");
        var first = state.TrackPendingReconnect(firstOwner, assignment)!;
        first.TrackWorker(trackedWorker);

        var replacement = state.TrackPendingReconnect(replacementOwner, assignment);
        var workersToRelease = await state.CompleteResultAsync(CreateResult());

        await Assert.That(replacement).IsNotNull();
        await Assert.That(workersToRelease).Contains(trackedWorker);
    }

    [Test]
    public async Task Tracked_Original_Disconnect_Does_Not_Replace_Active_Redispatch()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var original = CreateWorker(connectionId: "original");
        var pending = state.TrackPendingReconnect(original, assignment)!;
        pending.TrackWorker(original);
        pending.TryMakeAvailableForRedispatch();
        pending.TryClaimRedispatch();

        var replacement = state.TrackPendingReconnect(original, assignment);

        await Assert.That(replacement).IsNull();
        await Assert.That(state.GetPendingReconnect(original.Registration.WorkerIndex))
            .IsSameReferenceAs(pending);
        await Assert.That(state.TryReturnRedispatchToQueue(assignment)).IsTrue();
    }

    [Test]
    public async Task Failed_Redispatch_Is_Not_Requeued_After_Result_Completes()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        state.ResultWaiters[assignment.ModuleTypeName] =
            new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
        pending.TryMakeAvailableForRedispatch();
        state.TryClaimRedispatch(assignment);

        await state.CompleteResultAsync(CreateResult());

        await Assert.That(state.TryReturnRedispatchToQueue(assignment)).IsFalse();
    }

    [Test]
    public async Task First_Result_Releases_Remote_Redispatch_Worker()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var retryWorker = CreateWorker(2, "retry");
        state.ResultWaiters[assignment.ModuleTypeName] =
            new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
        pending.TryMakeAvailableForRedispatch();
        await Assert.That(retryWorker.TryAssign(assignment)).IsTrue();

        await Assert.That(state.TryClaimRedispatch(assignment, retryWorker)).IsTrue();

        var workersToRelease = await state.CompleteResultAsync(CreateResult());
        foreach (var worker in workersToRelease)
        {
            worker.TryCompleteAssignment(assignment.ModuleTypeName);
        }

        await Assert.That(workersToRelease).Contains(retryWorker);
        await Assert.That(retryWorker.IsIdle).IsTrue();
    }

    [Test]
    public async Task Redispatch_Claimant_Disconnect_Starts_New_Reconnect_Grace()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var retryWorker = CreateWorker(2, "retry");
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
        pending.TryMakeAvailableForRedispatch();
        await Assert.That(retryWorker.TryAssign(assignment)).IsTrue();
        await Assert.That(state.TryClaimRedispatch(assignment, retryWorker)).IsTrue();

        var replacement = state.TrackPendingReconnect(retryWorker, assignment);

        await Assert.That(replacement).IsNotNull();
        await Assert.That(state.GetPendingReconnect(retryWorker.Registration.WorkerIndex))
            .IsSameReferenceAs(replacement);
    }

    [Test]
    public async Task Failed_Redispatch_Untracks_Remote_Claimant()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var retryWorker = CreateWorker(2, "retry");
        var pending = state.TrackPendingReconnect(CreateWorker(), assignment)!;
        pending.TryMakeAvailableForRedispatch();
        await Assert.That(state.TryClaimRedispatch(assignment, retryWorker)).IsTrue();

        await Assert.That(pending.IsTracking(retryWorker)).IsTrue();
        await Assert.That(state.TryReturnRedispatchToQueue(assignment, retryWorker)).IsTrue();
        await Assert.That(pending.IsTracking(retryWorker)).IsFalse();
    }

    private static WorkerState CreateWorker(
        int workerIndex = 1,
        string connectionId = "connection")
    {
        return new WorkerState
        {
            ConnectionId = connectionId,
            Registration = new WorkerRegistration(workerIndex, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };
    }

    private static ModuleAssignment CreateAssignment()
    {
        return new ModuleAssignment(
            "TestModule",
            "System.String",
            FrozenSet<Capability>.Empty,
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, false));
    }

    private static SerializedModuleResult CreateResult()
    {
        return new SerializedModuleResult(
            "TestModule",
            "System.String",
            1,
            "{}",
            DateTimeOffset.UtcNow);
    }
}
