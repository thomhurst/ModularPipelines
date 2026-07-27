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
            Registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
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
            Registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
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
            Registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
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
                Registration = new WorkerRegistration(i, [], DateTimeOffset.UtcNow),
            };
            state.Registrations[i] = new WorkerRegistration(i, [], DateTimeOffset.UtcNow);
            state.PendingAssignments.Enqueue(new ModuleAssignment(
                $"Module{i}", "System.String", [],
                null, DateTimeOffset.UtcNow, new ModuleAssignmentConfig(null, 0, false)));
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
                [],
                null,
                DateTimeOffset.UtcNow,
                new ModuleAssignmentConfig(null, 0, false)));

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
    public async Task Reconnect_Reclaims_Queued_Retry_Until_Dispatch_Claims_It()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment();
        var pending = state.TrackPendingReconnect(1, assignment);

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
        var pending = state.TrackPendingReconnect(1, assignment);
        var replacement = new WorkerState
        {
            ConnectionId = "replacement",
            Registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
        };

        await Assert.That(pending.TryMakeAvailableForRedispatch()).IsTrue();

        var restored = state.TryRestoreReconnect(replacement, null, out _, out _);

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

            var pending = state.TrackPendingReconnect(1, assignment);
            pending.TryMakeAvailableForRedispatch();
            pending.TryClaimRedispatch();

            var worker = new WorkerState
            {
                ConnectionId = $"connection-{i}",
                Registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
            };

            var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var reconnect = Task.Run(async () =>
            {
                await start.Task;
                return state.TryRestoreReconnect(
                    worker,
                    assignment.ModuleTypeName,
                    out _,
                    out _);
            });
            var completion = Task.Run(async () =>
            {
                await start.Task;
                return state.CompleteResult(CreateResult());
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

    private static ModuleAssignment CreateAssignment()
    {
        return new ModuleAssignment(
            "TestModule",
            "System.String",
            [],
            null,
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfig(null, 0, false));
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
