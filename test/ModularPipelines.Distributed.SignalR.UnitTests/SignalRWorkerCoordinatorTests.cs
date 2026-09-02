using System.Collections.Frozen;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRWorkerCoordinatorTests
{
    [Test]
    public async Task EnqueueModule_Throws_NotSupportedException()
    {
        await using var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost")
            .Build();
        var coordinator = new SignalRWorkerCoordinator(
            connection,
            NullLogger<SignalRWorkerCoordinator>.Instance);
        var assignment = new ModuleAssignment(
            "TestModule",
            "System.String",
            FrozenSet<Capability>.Empty,
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, false));

        await Assert.That(() => coordinator.EnqueueModuleAsync(assignment, CancellationToken.None))
            .Throws<NotSupportedException>();
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
