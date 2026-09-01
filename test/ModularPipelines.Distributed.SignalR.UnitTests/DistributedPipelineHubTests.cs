using System.Collections.Frozen;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.SignalR.Hub;
using Moq;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class DistributedPipelineHubTests
{
    [Test]
    public async Task Disconnect_Retains_Final_Registration_For_Report_Collection()
    {
        var state = new SignalRMasterState();
        var registration = new WorkerRegistration(
            1,
            FrozenSet<string>.Empty,
            DateTimeOffset.UtcNow)
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
            Registration = new WorkerRegistration(1, FrozenSet<Capability>.Empty, DateTimeOffset.UtcNow),
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
            FrozenSet<Capability>.Empty,
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, 0, false));
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
}
