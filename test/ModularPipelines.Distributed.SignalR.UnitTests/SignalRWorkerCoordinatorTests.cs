using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRWorkerCoordinatorTests
{
    [Test]
    public async Task Worker_Coordinator_Exposes_Only_Worker_Contract()
    {
        await using var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost")
            .Build();
        var coordinator = new SignalRWorkerCoordinator(
            connection,
            NullLogger<SignalRWorkerCoordinator>.Instance);
        await Assert.That(coordinator is IDistributedWorkerCoordinator).IsTrue();
        await Assert.That(coordinator is IDistributedMasterCoordinator).IsFalse();
    }
}
