using System.Collections.Frozen;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRMasterCoordinatorTests
{
    private static SignalRMasterCoordinator CreateCoordinator(SignalRMasterState? state = null)
    {
        state ??= new SignalRMasterState();
        var hubContext = new Mock<IHubContext<DistributedPipelineHub>>();
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<ISingleClientProxy>();
        mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);
        mockClients.Setup(c => c.Client(It.IsAny<string>())).Returns(mockClientProxy.Object);
        hubContext.Setup(h => h.Clients).Returns(mockClients.Object);

        return new SignalRMasterCoordinator(
            hubContext.Object,
            state,
            NullLogger<SignalRMasterCoordinator>.Instance);
    }

    [Test]
    public async Task EnqueueModule_Creates_Result_Waiter()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);

        var assignment = CreateAssignment("TestModule");
        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        await Assert.That(state.ResultWaiters.ContainsKey("TestModule")).IsTrue();
    }

    [Test]
    public async Task EnqueueModule_Queues_When_No_Idle_Workers()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);

        var assignment = CreateAssignment("TestModule");
        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        await Assert.That(state.PendingAssignments.Count).IsEqualTo(1);
    }

    [Test]
    public async Task WaitForResult_Returns_When_Result_Published()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);

        // Pre-create the waiter
        var assignment = CreateAssignment("TestModule");
        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Publish result in background
        var result = CreateResult("TestModule");
        _ = Task.Run(async () =>
        {
            await Task.Delay(50);
            await coordinator.PublishResultAsync(result, CancellationToken.None);
        });

        var collected = await coordinator.WaitForResultAsync("TestModule", CancellationToken.None);
        await Assert.That(collected.ModuleTypeName).IsEqualTo("TestModule");
    }

    [Test]
    public async Task PublishResult_Releases_Worker_Tracked_By_Pending_Reconnect()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);
        var assignment = CreateAssignment("TestModule");
        state.ResultWaiters[assignment.ModuleTypeName] =
            new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);

        var disconnectedWorker = new WorkerState
        {
            ConnectionId = "disconnected-worker",
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };
        var pending = state.TrackPendingReconnect(disconnectedWorker, assignment)!;
        pending.TryMakeAvailableForRedispatch();

        var worker = new WorkerState
        {
            ConnectionId = "reconnected-worker",
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };

        var restored = state.TryRestoreReconnect(
            worker,
            assignment.ModuleTypeName,
            out _);
        await coordinator.PublishResultAsync(CreateResult("TestModule"), CancellationToken.None);

        await Assert.That(restored).IsTrue();
        await Assert.That(worker.IsIdle).IsTrue();
        await Assert.That(state.GetPendingReconnect(1)).IsNull();
    }

    [Test]
    public async Task WaitForResult_Cancellable()
    {
        var coordinator = CreateCoordinator();

        using var cts = new CancellationTokenSource(100);
        var threw = false;
        try
        {
            await coordinator.WaitForResultAsync("NonExistent", cts.Token);
        }
        catch (OperationCanceledException)
        {
            threw = true;
        }

        await Assert.That(threw).IsTrue();
    }

    [Test]
    public async Task RegisterWorker_Adds_To_State()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);

        var registration = new WorkerRegistration(1, new HashSet<Capability> { "linux" }, DateTimeOffset.UtcNow);
        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);

        await Assert.That(state.Registrations.Count).IsEqualTo(1);
        await Assert.That(state.Registrations[1].WorkerIndex).IsEqualTo(1);
    }

    [Test]
    public async Task GetRegisteredWorkers_Returns_All_Registered()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);

        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(1, new HashSet<Capability>(), DateTimeOffset.UtcNow), CancellationToken.None);
        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(2, new HashSet<Capability>(), DateTimeOffset.UtcNow), CancellationToken.None);

        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);
        await Assert.That(workers.Count).IsEqualTo(2);
    }

    [Test]
    public async Task GetRegisteredWorkers_Uses_Heartbeats_For_Liveness_But_Retains_Completed()
    {
        var state = new SignalRMasterState
        {
            WorkerTimeout = TimeSpan.FromSeconds(1),
        };
        var coordinator = CreateCoordinator(state);
        var recent = new WorkerRegistration(1, new HashSet<Capability>(), DateTimeOffset.UtcNow);
        var stale = new WorkerRegistration(2, new HashSet<Capability>(), DateTimeOffset.UtcNow);
        var completed = new WorkerRegistration(3, new HashSet<Capability>(), DateTimeOffset.UtcNow)
        {
            UnattributedCommandCount = 1,
        };
        state.Registrations[recent.WorkerIndex] = recent;
        state.Registrations[stale.WorkerIndex] = stale;
        state.Registrations[completed.WorkerIndex] = completed;
        state.Heartbeats[recent.WorkerIndex] = DateTimeOffset.UtcNow;
        state.Heartbeats[stale.WorkerIndex] = DateTimeOffset.UtcNow - TimeSpan.FromSeconds(2);
        state.Heartbeats[completed.WorkerIndex] = DateTimeOffset.UtcNow - TimeSpan.FromSeconds(2);

        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers).IsEquivalentTo([recent, completed]);
    }

    [Test]
    public async Task SignalCompletion_Sets_Completed_Flag()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);

        await coordinator.SignalCompletionAsync(CancellationToken.None);

        await Assert.That(state.IsCompleted).IsTrue();
    }

    [Test]
    public async Task BroadcastCancellation_Unblocks_Worker_Observer()
    {
        var coordinator = CreateCoordinator();
        var observer = coordinator.WaitForCancellationAsync(CancellationToken.None);

        await coordinator.BroadcastCancellationAsync(CancellationToken.None);

        await observer.WaitAsync(TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Heartbeat_Refreshes_Registered_Worker()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);
        var registration = new WorkerRegistration(
            1,
            new HashSet<Capability>(),
            DateTimeOffset.UtcNow);
        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        var original = state.Heartbeats[registration.WorkerIndex];

        await Task.Delay(10);
        await coordinator.SendHeartbeatAsync(registration.WorkerIndex, CancellationToken.None);

        await Assert.That(state.Heartbeats[registration.WorkerIndex]).IsGreaterThan(original);
    }

    [Test]
    public async Task SignalCompletion_Cancels_Pending_Waiters()
    {
        var state = new SignalRMasterState();
        var coordinator = CreateCoordinator(state);

        // Create a pending waiter
        var waiterTask = coordinator.WaitForResultAsync("PendingModule", CancellationToken.None);

        // Signal completion
        await coordinator.SignalCompletionAsync(CancellationToken.None);

        // Waiter should be cancelled
        var threw = false;
        try
        {
            await waiterTask;
        }
        catch (TaskCanceledException)
        {
            threw = true;
        }

        await Assert.That(threw).IsTrue();
    }

    [Test]
    public async Task EnqueueModule_Pushes_To_Idle_Worker_With_Matching_Capabilities()
    {
        var state = new SignalRMasterState();

        // Add an idle worker with "linux" capability
        state.Workers["conn-1"] = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, new HashSet<Capability> { "linux" }, DateTimeOffset.UtcNow),
        };

        var hubContext = new Mock<IHubContext<DistributedPipelineHub>>();
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<ISingleClientProxy>();
        mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);
        mockClients.Setup(c => c.Client("conn-1")).Returns(mockClientProxy.Object);
        hubContext.Setup(h => h.Clients).Returns(mockClients.Object);

        var coordinator = new SignalRMasterCoordinator(
            hubContext.Object, state, NullLogger<SignalRMasterCoordinator>.Instance);

        // Enqueue a module requiring "linux"
        var assignment = new ModuleAssignment(
            "LinuxModule", "System.String",
            new HashSet<Capability> { "linux" },
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, 0, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Should have been pushed directly — not in pending queue
        await Assert.That(state.PendingAssignments.Count).IsEqualTo(0);

        // Client should have received the assignment
        mockClientProxy.Verify(
            c => c.SendCoreAsync(HubMethodNames.ReceiveAssignment, It.IsAny<object?[]>(), It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [Test]
    public async Task Result_Cannot_Release_Redispatch_Worker_Until_Delivery_Completes()
    {
        var state = new SignalRMasterState();
        var assignment = CreateAssignment("TestModule");
        state.ResultWaiters[assignment.ModuleTypeName] =
            new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);

        var disconnectedWorker = new WorkerState
        {
            ConnectionId = "disconnected-worker",
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };
        var pending = state.TrackPendingReconnect(disconnectedWorker, assignment)!;
        pending.TryMakeAvailableForRedispatch();

        var retryWorker = new WorkerState
        {
            ConnectionId = "retry-worker",
            Registration = new WorkerRegistration(2, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
        };
        state.Workers[retryWorker.ConnectionId] = retryWorker;

        var deliveryStarted = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var allowDelivery = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var clientProxy = new Mock<ISingleClientProxy>();
        clientProxy
            .Setup(client => client.SendCoreAsync(
                HubMethodNames.ReceiveAssignment,
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                deliveryStarted.SetResult();
                await allowDelivery.Task;
            });

        var clients = new Mock<IHubClients>();
        clients.Setup(client => client.Client(retryWorker.ConnectionId))
            .Returns(clientProxy.Object);
        var hubContext = new Mock<IHubContext<DistributedPipelineHub>>();
        hubContext.Setup(context => context.Clients).Returns(clients.Object);
        var coordinator = new SignalRMasterCoordinator(
            hubContext.Object,
            state,
            NullLogger<SignalRMasterCoordinator>.Instance);

        var enqueueTask = coordinator.EnqueueModuleAsync(
            assignment,
            CancellationToken.None);
        await deliveryStarted.Task;

        var publishTask = coordinator.PublishResultAsync(
            CreateResult(assignment.ModuleTypeName),
            CancellationToken.None);
        await Task.Yield();

        await Assert.That(publishTask.IsCompleted).IsFalse();
        await Assert.That(retryWorker.IsIdle).IsFalse();
        await Assert.That(retryWorker.TryAssign(CreateAssignment("NextModule"))).IsFalse();

        allowDelivery.SetResult();
        await enqueueTask;
        await publishTask;

        await Assert.That(retryWorker.IsIdle).IsTrue();
    }

    [Test]
    public async Task EnqueueModule_Queues_When_No_Capability_Match()
    {
        var state = new SignalRMasterState();

        // Add worker with "windows" capability
        state.Workers["conn-1"] = new WorkerState
        {
            ConnectionId = "conn-1",
            Registration = new WorkerRegistration(1, new HashSet<Capability> { "windows" }, DateTimeOffset.UtcNow),
        };

        var coordinator = CreateCoordinator(state);

        // Enqueue a module requiring "linux" — no match
        var assignment = new ModuleAssignment(
            "LinuxModule", "System.String",
            new HashSet<Capability> { "linux" },
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, 0, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Should be in pending queue
        await Assert.That(state.PendingAssignments.Count).IsEqualTo(1);
    }

    private static ModuleAssignment CreateAssignment(string moduleTypeName)
    {
        return new ModuleAssignment(
            moduleTypeName, "System.String",
            new HashSet<Capability>(),
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, 0, false));
    }

    private static SerializedModuleResult CreateResult(string moduleTypeName)
    {
        return new SerializedModuleResult(
            moduleTypeName, "System.String", 1,
            "{}", DateTimeOffset.UtcNow);
    }
}
