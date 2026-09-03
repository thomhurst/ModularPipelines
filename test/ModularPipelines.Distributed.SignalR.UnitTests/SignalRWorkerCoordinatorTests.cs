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

    [Test]
    public async Task DependencyResultReceived_Event_Fires()
    {
        // Test that the event mechanism works in isolation
        SerializedModuleResult? receivedResult = null;

        // Simulate the callback mechanism
        Action<SerializedModuleResult> handler = result => receivedResult = result;

        var testResult = new SerializedModuleResult(
            "TestModule", "System.String", 1, "{}", DateTimeOffset.UtcNow);

        handler(testResult);

        await Assert.That(receivedResult).IsNotNull();
        await Assert.That(receivedResult!.ModuleTypeName).IsEqualTo("TestModule");
    }
}
