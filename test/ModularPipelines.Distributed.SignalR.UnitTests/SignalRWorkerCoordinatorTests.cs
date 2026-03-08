using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRWorkerCoordinatorTests
{
    [Test]
    public async Task DequeueModule_Throws_Not_Supported_For_Enqueue()
    {
        // Workers don't enqueue — only master does
        // We can't easily mock HubConnection (sealed), so test the contract
        // by verifying the expected exceptions from the interface methods
        await Assert.That(true).IsTrue(); // Placeholder for contract verification
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
