using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

/// <summary>
/// Runs a real SignalR master and workers over loopback.
/// </summary>
public class SignalRHubTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    [Test]
    public async Task Worker_Claims_Lease_And_Master_Receives_Result()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);

        var dequeue = worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None);
        await test.Master.EnqueueModuleAsync(Assignment("Signal.Module"), CancellationToken.None);
        var lease = await dequeue.WaitAsync(Timeout);
        var result = Result("Signal.Module", registration.WorkerId);
        await worker.PublishResultAsync(result, lease, CancellationToken.None);
        var received = await test.Master.WaitForResultAsync(new ModuleId("Signal.Module"), CancellationToken.None)
            .WaitAsync(Timeout);

        await Assert.That(lease!.WorkerId).IsEqualTo(registration.WorkerId);
        await Assert.That(received.Payload).IsEqualTo(result.Payload);
        await Assert.That(await test.Master.GetActiveLeasesAsync(CancellationToken.None)).IsEmpty();
    }

    [Test]
    public async Task Worker_Holds_Several_Leases_At_Once()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        await test.Master.EnqueueModuleAsync(Assignment("Signal.First"), CancellationToken.None);
        await test.Master.EnqueueModuleAsync(Assignment("Signal.Second"), CancellationToken.None);

        var first = worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None);
        var second = worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None);
        var leases = await Task.WhenAll(first, second).WaitAsync(Timeout);

        await Assert.That(leases.Select(lease => lease!.Assignment.ModuleId.Value).Order())
            .IsEquivalentTo(["Signal.First", "Signal.Second"]);
        await Assert.That(await test.Master.GetActiveLeasesAsync(CancellationToken.None)).Count().IsEqualTo(2);
    }

    [Test]
    public async Task Worker_That_Missed_Completion_Still_Stops()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        await test.Master.SignalCompletionAsync(CancellationToken.None);

        // The worker connects after the master finished, so it never saw a completion broadcast.
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("late-worker");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var lease = await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);

        await Assert.That(lease).IsNull();
    }

    [Test]
    public async Task Worker_Observes_Cancellation_Reason_Broadcast_Before_It_Connected()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        await test.Master.BroadcastCancellationAsync(DistributedCancellationReason.PipelineFailed, CancellationToken.None);
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);

        var reason = await worker.WaitForCancellationAsync(CancellationToken.None).WaitAsync(Timeout);

        await Assert.That(reason).IsEqualTo(DistributedCancellationReason.PipelineFailed);
    }

    [Test]
    public async Task Heartbeat_From_Worker_Renews_Its_Lease()
    {
        await using var test = await SignalRTestMaster.StartAsync(workerTimeout: TimeSpan.FromMilliseconds(500));
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        await test.Master.EnqueueModuleAsync(Assignment("Signal.Renewed"), CancellationToken.None);
        var lease = await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);

        for (var i = 0; i < 4; i++)
        {
            await Task.Delay(250);
            await worker.SendHeartbeatAsync(
                new WorkerStatus
                {
                    WorkerId = registration.WorkerId,
                    RunId = SignalRTestMaster.RunId,
                    InFlightModules = [lease!.Assignment.ModuleId],
                },
                CancellationToken.None);
        }

        await Assert.That(await test.Master.RequeueExpiredLeasesAsync(CancellationToken.None)).IsEmpty();
    }

    [Test]
    public async Task Registration_For_Another_Run_Is_Rejected()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();

        await Assert.That(async () => await worker.RegisterWorkerAsync(
                SignalRTestMaster.Registration("worker-a", runId: "another-run"),
                CancellationToken.None))
            .Throws<HubException>();
        await Assert.That(await test.Master.GetRegisteredWorkersAsync(CancellationToken.None)).IsEmpty();
    }

    [Test]
    public async Task Duplicate_Live_Worker_Is_Rejected()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var first = await test.ConnectWorkerAsync();
        var second = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await first.RegisterWorkerAsync(registration, CancellationToken.None);

        await Assert.That(async () => await second.RegisterWorkerAsync(
                registration with { RegisteredAt = registration.RegisteredAt.AddSeconds(1) },
                CancellationToken.None))
            .Throws<HubException>();
    }

    [Test]
    public async Task Unregistered_Connection_Cannot_Read_Results_Or_Claim_Work()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();

        await Assert.That(async () => await worker.WaitForResultAsync(new ModuleId("Secret"), CancellationToken.None))
            .Throws<HubException>();
        await Assert.That(async () => await worker.DequeueModuleAsync(
                new WorkerId("anyone"),
                new HashSet<Capability>(),
                CancellationToken.None))
            .Throws<HubException>();
    }

    [Test]
    public async Task Result_For_Another_Worker_Is_Rejected()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);

        await Assert.That(async () => await worker.PublishResultAsync(
                Result("Signal.Forged", new WorkerId("worker-b")),
                lease: null,
                CancellationToken.None))
            .Throws<HubException>();
    }

    [Test]
    public async Task Master_With_Access_Token_Rejects_Workers_Without_It()
    {
        await using var test = await SignalRTestMaster.StartAsync(options => options.AccessToken = "correct-token");
        var endpoint = await test.Discovery.Endpoint;

        test.Discovery.Override = endpoint with { AccessToken = "wrong-token" };
        await Assert.That(async () => await test.Factory.CreateWorkerAsync(
                new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token))
            .Throws<Exception>();

        test.Discovery.Override = null;
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);
        await Assert.That(endpoint.AccessToken).IsEqualTo("correct-token");
        await Assert.That(endpoint.ToString()).DoesNotContain("correct-token");
    }

    [Test]
    public async Task Worker_Stops_Claiming_When_The_Master_Goes_Away()
    {
        var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var dequeue = worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None);

        await ((IAsyncDisposable) test.Master).DisposeAsync();
        var lease = await dequeue.WaitAsync(Timeout);

        await Assert.That(lease).IsNull();
        await Assert.That(await worker.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Lost);
        await test.DisposeAsync();
    }

    [Test]
    public async Task Master_That_Signalled_Completion_Is_Not_Lost_When_It_Exits()
    {
        var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);

        await test.Master.SignalCompletionAsync(CancellationToken.None);
        var lease = await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);
        await Assert.That(lease).IsNull();

        await ((IAsyncDisposable) test.Master).DisposeAsync();
        await Assert.That(async () => await worker.SendHeartbeatAsync(
                new WorkerStatus { WorkerId = registration.WorkerId },
                CancellationToken.None).WaitAsync(Timeout))
            .Throws<Exception>();

        await Assert.That(await worker.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Completed);
        await test.DisposeAsync();
    }

    [Test]
    public async Task Unauthorized_Requests_Get_401()
    {
        var request = new Microsoft.AspNetCore.Http.DefaultHttpContext().Request;
        request.QueryString = new Microsoft.AspNetCore.Http.QueryString("?access_token=token");
        var expected = System.Text.Encoding.UTF8.GetBytes("token");

        await Assert.That(Server.MasterServerHost.IsAuthorized(request, expected)).IsTrue();
        request.QueryString = Microsoft.AspNetCore.Http.QueryString.Empty;
        request.Headers.Authorization = "Bearer other";
        await Assert.That(Server.MasterServerHost.IsAuthorized(request, expected)).IsFalse();
        request.Headers.Authorization = "Bearer token";
        await Assert.That(Server.MasterServerHost.IsAuthorized(request, expected)).IsTrue();
    }

    private static ModuleAssignment Assignment(string moduleId) => new()
    {
        ModuleId = new ModuleId(moduleId),
        RequiredCapabilities = CapabilityRequirement.None,
        PipelineSchemaVersion = "schema",
    };

    private static SerializedModuleResult Result(string moduleId, WorkerId workerId) => new()
    {
        ModuleId = new ModuleId(moduleId),
        WorkerId = workerId,
        Payload = "{\"value\":1}",
        CompletedAt = DateTimeOffset.UtcNow,
    };
}
