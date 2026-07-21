using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRMasterStateTests
{
    [Test]
    public async Task WorkerState_TryMarkBusy_Returns_True_When_Idle()
    {
        var worker = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, new HashSet<string>(), DateTimeOffset.UtcNow),
        };

        var result = worker.TryMarkBusy();
        await Assert.That(result).IsTrue();
        await Assert.That(worker.IsIdle).IsFalse();
    }

    [Test]
    public async Task WorkerState_TryMarkBusy_Returns_False_When_Already_Busy()
    {
        var worker = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, new HashSet<string>(), DateTimeOffset.UtcNow),
        };

        worker.TryMarkBusy(); // First call succeeds
        var result = worker.TryMarkBusy(); // Second call should fail

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task WorkerState_MarkIdle_Resets_BusyFlag()
    {
        var worker = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, new HashSet<string>(), DateTimeOffset.UtcNow),
        };

        worker.TryMarkBusy();
        await Assert.That(worker.IsIdle).IsFalse();

        worker.MarkIdle();
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
                Registration = new WorkerRegistration(i, new HashSet<string>(), DateTimeOffset.UtcNow),
            };
            state.Registrations[i] = new WorkerRegistration(i, new HashSet<string>(), DateTimeOffset.UtcNow);
            state.PendingAssignments.Enqueue(new ModuleAssignment(
                $"Module{i}", "System.String", new HashSet<string>(),
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
}
