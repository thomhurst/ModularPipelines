using System.Text.Json;
using ModularPipelines.Distributed.SignalR.Coordination;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

/// <summary>
/// Covers how the worker classifies an invocation failure that the connection callbacks have not explained yet.
/// </summary>
public class SignalRWorkerCoordinatorTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    [Test]
    public async Task Transport_Failure_Before_The_Close_Callback_Stops_The_Worker()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);
        worker.ConnectionChangeGracePeriod = TimeSpan.FromMinutes(1);

        // The invocation fails while the connection still reports Connected, as when the transport
        // errors before HubConnection raises Closed.
        var invocation = worker.InvokeAsync<bool>(
            _ => Task.FromException<bool>(new IOException("Connection reset by peer")),
            CancellationToken.None);
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        await Assert.That(invocation.IsCompleted).IsFalse();

        await ((IAsyncDisposable) test.Master).DisposeAsync();

        await Assert.That(async () => await invocation.WaitAsync(Timeout))
            .Throws<SignalRWorkerCoordinator.MasterConnectionClosedException>();
    }

    [Test]
    public async Task Transport_Failure_On_A_Healthy_Connection_Surfaces_After_The_Grace_Period()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);
        worker.ConnectionChangeGracePeriod = TimeSpan.FromMilliseconds(200);

        var invocation = worker.InvokeAsync<bool>(
            _ => Task.FromException<bool>(new IOException("transient")),
            CancellationToken.None);

        await Assert.That(async () => await invocation.WaitAsync(Timeout))
            .Throws<IOException>()
            .WithMessage("transient");
    }

    [Test]
    public async Task Unrelated_Failure_On_A_Healthy_Connection_Surfaces_Without_Waiting()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);

        // A grace period longer than the timeout proves the failure did not wait for a connection change.
        worker.ConnectionChangeGracePeriod = TimeSpan.FromMinutes(1);

        var invocation = worker.InvokeAsync<bool>(
            _ => Task.FromException<bool>(new JsonException("bad payload")),
            CancellationToken.None);

        await Assert.That(async () => await invocation.WaitAsync(Timeout))
            .Throws<JsonException>()
            .WithMessage("bad payload");
    }
}
