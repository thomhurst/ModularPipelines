using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.SignalR.Hub;
using Moq;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class DistributedPipelineHubTests
{
    [Test]
    public async Task Heartbeat_From_Connected_Worker_Cannot_Update_Another_Worker()
    {
        var state = new SignalRMasterState();
        const string connectionId = "connected-worker";
        state.Workers[connectionId] = new WorkerState
        {
            ConnectionId = connectionId,
            Registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
        };
        var context = new Mock<HubCallerContext>();
        context.SetupGet(x => x.ConnectionId).Returns(connectionId);
        var hub = new DistributedPipelineHub(
            state,
            NullLogger<DistributedPipelineHub>.Instance)
        {
            Context = context.Object,
        };

        await hub.Heartbeat(new WorkerStatus(2));

        await Assert.That(state.WorkerStatuses.ContainsKey(2)).IsFalse();
        await Assert.That(state.Heartbeats.ContainsKey(2)).IsFalse();
    }

    [Test]
    public async Task Heartbeat_Before_Reconnection_Registration_Preserves_Final_Status()
    {
        var state = new SignalRMasterState();
        var context = new Mock<HubCallerContext>();
        context.SetupGet(x => x.ConnectionId).Returns("reconnected-worker");
        var hub = new DistributedPipelineHub(
            state,
            NullLogger<DistributedPipelineHub>.Instance)
        {
            Context = context.Object,
        };
        var status = new WorkerStatus(1)
        {
            RunIdentifier = "run-1",
            UnattributedCommandCount = 3,
        };

        await hub.Heartbeat(status);

        await Assert.That(state.WorkerStatuses.ContainsKey(1)).IsFalse();
        await Assert.That(state.Heartbeats.ContainsKey(1)).IsFalse();

        await hub.RegisterWorker(
            new WorkerRegistration(1, [], DateTimeOffset.UtcNow)
            {
                RunIdentifier = "run-1",
            },
            resumingModuleTypeName: null);

        await Assert.That(state.WorkerStatuses[1]).IsSameReferenceAs(status);
        await Assert.That(state.Heartbeats.ContainsKey(1)).IsTrue();
    }

    [Test]
    public async Task Superseded_Connection_Cannot_Overwrite_Current_Worker_Status()
    {
        var state = new SignalRMasterState();
        var oldHub = CreateHub(state, "old-connection");
        var currentHub = CreateHub(state, "current-connection");
        var oldRegistration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow)
        {
            RunIdentifier = "old-run",
        };
        var currentRegistration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow)
        {
            RunIdentifier = "current-run",
        };

        await oldHub.RegisterWorker(oldRegistration, resumingModuleTypeName: null);
        await currentHub.RegisterWorker(currentRegistration, resumingModuleTypeName: null);
        var currentStatus = new WorkerStatus(1)
        {
            RunIdentifier = "current-run",
            UnattributedCommandCount = 2,
        };
        await currentHub.Heartbeat(currentStatus);
        var currentHeartbeat = state.Heartbeats[1];

        await oldHub.Heartbeat(new WorkerStatus(1)
        {
            RunIdentifier = "old-run",
            UnattributedCommandCount = 99,
        });

        await Assert.That(state.WorkerStatuses[1]).IsSameReferenceAs(currentStatus);
        await Assert.That(state.Heartbeats[1]).IsEqualTo(currentHeartbeat);
    }

    [Test]
    public async Task Disconnect_Retains_Final_Status_For_Report_Collection()
    {
        var state = new SignalRMasterState();
        var registration = new WorkerRegistration(
            1,
            [],
            DateTimeOffset.UtcNow);
        var status = new WorkerStatus(1)
        {
            UnattributedCommandCount = 3,
        };
        var worker = new WorkerState
        {
            ConnectionId = "connection-1",
            Registration = registration,
        };
        state.Workers[worker.ConnectionId] = worker;
        state.Registrations[registration.WorkerIndex] = registration;
        state.WorkerStatuses[registration.WorkerIndex] = status;
        state.Heartbeats[registration.WorkerIndex] = DateTimeOffset.UtcNow;

        var context = new Mock<HubCallerContext>();
        context.SetupGet(x => x.ConnectionId).Returns(worker.ConnectionId);
        var hub = new DistributedPipelineHub(
            state,
            NullLogger<DistributedPipelineHub>.Instance)
        {
            Context = context.Object,
        };

        await hub.OnDisconnectedAsync(null);

        using (Assert.Multiple())
        {
            await Assert.That(state.Workers).IsEmpty();
            await Assert.That(state.Registrations[registration.WorkerIndex])
                .IsSameReferenceAs(registration);
            await Assert.That(state.WorkerStatuses[registration.WorkerIndex])
                .IsSameReferenceAs(status);
            await Assert.That(state.Heartbeats.ContainsKey(registration.WorkerIndex)).IsTrue();
        }
    }

    [Test]
    public async Task PublishResult_Does_Not_Clear_A_Different_Current_Assignment()
    {
        var state = new SignalRMasterState();
        var worker = new WorkerState
        {
            ConnectionId = "connection-1",
            Registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
        };
        worker.TryAssign(CreateAssignment("CurrentModule"));
        state.Workers[worker.ConnectionId] = worker;

        var context = new Mock<HubCallerContext>();
        context.SetupGet(x => x.ConnectionId).Returns(worker.ConnectionId);

        var otherClients = new Mock<IClientProxy>();
        otherClients
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var clients = new Mock<IHubCallerClients>();
        clients.SetupGet(x => x.Others).Returns(otherClients.Object);

        var hub = new DistributedPipelineHub(
            state,
            NullLogger<DistributedPipelineHub>.Instance)
        {
            Context = context.Object,
            Clients = clients.Object,
        };

        await hub.PublishResult(CreateResult("PreviousModule"));

        await Assert.That(worker.CurrentAssignment?.ModuleTypeName)
            .IsEqualTo("CurrentModule");
        await Assert.That(worker.IsIdle).IsFalse();
    }

    private static ModuleAssignment CreateAssignment(string moduleTypeName)
    {
        return new ModuleAssignment(
            moduleTypeName,
            "System.String",
            [],
            DateTimeOffset.UtcNow,
            new ModuleAssignmentOptions(null, false));
    }

    private static SerializedModuleResult CreateResult(string moduleTypeName)
    {
        return new SerializedModuleResult(
            moduleTypeName,
            "System.String",
            1,
            "{}",
            DateTimeOffset.UtcNow);
    }

    private static DistributedPipelineHub CreateHub(SignalRMasterState state, string connectionId)
    {
        var context = new Mock<HubCallerContext>();
        context.SetupGet(instance => instance.ConnectionId).Returns(connectionId);
        return new DistributedPipelineHub(
            state,
            NullLogger<DistributedPipelineHub>.Instance)
        {
            Context = context.Object,
        };
    }
}
