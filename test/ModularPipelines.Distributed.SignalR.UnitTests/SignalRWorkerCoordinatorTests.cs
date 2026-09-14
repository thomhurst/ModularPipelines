using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRWorkerCoordinatorTests
{
    [Test]
    public async Task Permanent_Close_Invalidates_Earlier_Successful_Reconnect()
    {
        await using var connection = new HubConnectionBuilder().WithUrl("http://localhost").Build();
        var coordinator = new SignalRWorkerCoordinator(connection, NullLogger<SignalRWorkerCoordinator>.Instance);
        const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;

        // Drive the connection callbacks without network timing so an older failed invocation
        // observes a successful reconnect followed by a permanent close.
        await (Task) typeof(SignalRWorkerCoordinator).GetMethod("OnReconnectingAsync", flags)!
            .Invoke(coordinator, [null])!;
        await (Task) typeof(SignalRWorkerCoordinator).GetMethod("OnReconnectedAsync", flags)!
            .Invoke(coordinator, [null])!;
        await (Task) typeof(SignalRWorkerCoordinator).GetMethod("OnClosedAsync", flags)!
            .Invoke(coordinator, [null])!;
        var retry = await (Task<bool>) typeof(SignalRWorkerCoordinator).GetMethod("WaitForReconnectAsync", flags)!
            .Invoke(coordinator, [0L, CancellationToken.None])!;

        await Assert.That(retry).IsFalse();
    }

    [Test]
    [Arguments(false, false, false)]
    [Arguments(true, false, true)]
    [Arguments(false, true, false)]
    [Arguments(true, true, false)]
    public async Task Hub_Rejection_Retries_Only_During_Reconnect_Without_Caller_Cancellation(
        bool reconnecting, bool cancelled, bool expectedRetry)
    {
        await using var connection = new HubConnectionBuilder().WithUrl("http://localhost").Build();
        var coordinator = new SignalRWorkerCoordinator(connection, NullLogger<SignalRWorkerCoordinator>.Instance);
        const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
        if (reconnecting)
        {
            await (Task) typeof(SignalRWorkerCoordinator).GetMethod("OnReconnectingAsync", flags)!
                .Invoke(coordinator, [null])!;
        }

        var retry = (bool) typeof(SignalRWorkerCoordinator).GetMethod("CanRetryHubInvocation", flags)!
            .Invoke(coordinator, [new Microsoft.AspNetCore.SignalR.HubException("Rejected"), 0L, new CancellationToken(cancelled)])!;

        await Assert.That(retry).IsEqualTo(expectedRetry);
    }

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
