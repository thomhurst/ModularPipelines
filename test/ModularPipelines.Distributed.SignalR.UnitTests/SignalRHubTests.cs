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
    [Arguments(false)]
    [Arguments(true)]
    public async Task Reconnect_Reconciles_Leases_Before_Resuming_Invocations(bool reassign)
    {
        await using var test = await SignalRTestMaster.StartAsync(
            options => options with { KeepAliveInterval = TimeSpan.FromSeconds(1), PeerTimeout = TimeSpan.FromSeconds(2) },
            workerTimeout: TimeSpan.FromSeconds(5), heartbeatInterval: TimeSpan.FromMilliseconds(100));
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        await test.Master.EnqueueModuleAsync(Assignment("Reconnect.Module"), CancellationToken.None);
        var old = (await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None))!;
        ModuleLease? current = null;
        if (reassign)
        {
            await Task.Delay(TimeSpan.FromSeconds(5.1));
            await test.Master.RequeueExpiredLeasesAsync(CancellationToken.None);
            current = await test.Master.DequeueModuleAsync(new WorkerId("replacement"), new HashSet<Capability>(), CancellationToken.None);
        }

        await worker.OnReconnectingAsync(null);
        var pending = worker.ReconcileHeartbeatAsync(new WorkerStatus
        {
            WorkerId = registration.WorkerId,
            RunId = registration.RunId,
            InFlightModules = [old.Assignment.ModuleId],
        }, [old], CancellationToken.None);
        await Assert.That(pending.IsCompleted).IsFalse();
        await worker.OnReconnectedAsync("reconnected").WaitAsync(Timeout);
        await Assert.That(await pending.WaitAsync(Timeout)).IsEquivalentTo(reassign ? [old.LeaseId] : Array.Empty<string>());
        await worker.PublishResultAsync(Result("Reconnect.Module", registration.WorkerId), old, CancellationToken.None);
        if (reassign)
        {
            await Assert.That((await test.Master.GetActiveLeasesAsync(CancellationToken.None)).Single().LeaseId)
                .IsEqualTo(current!.LeaseId);
            await Assert.That(test.Master.WaitForResultAsync(old.Assignment.ModuleId, CancellationToken.None).IsCompleted).IsFalse();
        }
        else
        {
            await Assert.That((await test.Master.WaitForResultAsync(old.Assignment.ModuleId, CancellationToken.None)).WorkerId)
                .IsEqualTo(registration.WorkerId);
        }
    }

    [Test]
    public async Task Lease_Reconciliation_Before_Expiry()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        await ModularPipelines.TestHelpers.LeaseReconciliationChecks.ReconnectBeforeExpiryAsync(
            test.Master, worker, SignalRTestMaster.RunId);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Lease_Reconciliation_After_Reassignment(bool sameWorker)
    {
        await using var test = await SignalRTestMaster.StartAsync(workerTimeout: TimeSpan.FromMilliseconds(500));
        var worker = await test.ConnectWorkerAsync();
        var replacement = sameWorker ? worker : await test.ConnectWorkerAsync();
        await ModularPipelines.TestHelpers.LeaseReconciliationChecks.ReconnectAfterReassignmentAsync(
            test.Master, () => Task.Delay(1500), sameWorker, worker, replacement, SignalRTestMaster.RunId);
    }

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
    public async Task Retried_Dequeue_After_Reconnect_Returns_The_Original_Lease()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await test.Master.EnqueueModuleAsync(Assignment("Signal.First"), CancellationToken.None);
        await test.Master.EnqueueModuleAsync(Assignment("Signal.Second"), CancellationToken.None);
        var endpoint = await test.Discovery.Endpoint;
        await using var connection = new HubConnectionBuilder()
            .WithUrl(new Uri(endpoint.Url, "/pipeline-hub"), options =>
                options.AccessTokenProvider = () => Task.FromResult(endpoint.AccessToken))
            .Build();
        await connection.StartAsync();
        await connection.InvokeAsync<bool>(HubMethodNames.RegisterWorker, registration);
        var claimId = Guid.NewGuid();

        // Simulate a lost reply by replaying the same claim after reconnecting.
        var original = await connection.InvokeAsync<ModuleLease?>(HubMethodNames.DequeueModule, claimId).WaitAsync(Timeout);
        await connection.StopAsync();
        await connection.StartAsync();
        await connection.InvokeAsync<bool>(HubMethodNames.RegisterWorker, registration);
        var retried = await connection.InvokeAsync<ModuleLease?>(HubMethodNames.DequeueModule, claimId).WaitAsync(Timeout);

        await Assert.That(retried!.LeaseId).IsEqualTo(original!.LeaseId);
        await Assert.That(await test.Master.GetActiveLeasesAsync(CancellationToken.None)).HasSingleItem();
        var next = await connection.InvokeAsync<ModuleLease?>(HubMethodNames.DequeueModule, Guid.NewGuid()).WaitAsync(Timeout);
        await Assert.That(next!.Assignment.ModuleId).IsNotEqualTo(original.Assignment.ModuleId);
    }

    [Test]
    public async Task New_Worker_Is_Rejected_After_Completion()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        await test.Master.SignalCompletionAsync(CancellationToken.None);
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("late-worker");

        await Assert.That(async () => await worker.RegisterWorkerAsync(registration, CancellationToken.None))
            .Throws<DistributedRunCompletedException>();
        await Assert.That(await test.Master.GetRegisteredWorkersAsync(CancellationToken.None)).IsEmpty();
        await Assert.That(await test.Master.GetWorkerStatusesAsync(CancellationToken.None)).IsEmpty();
        await Assert.That(await worker.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Completed);
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
        await using var test = await SignalRTestMaster.StartAsync(options => options with { AccessToken = "correct-token" });
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
    public async Task Master_That_Exits_During_A_Pending_Dequeue_After_Completing_Is_Not_Lost()
    {
        var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var dequeue = worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None);

        // The master shuts down straight after completing, without waiting for the dequeue reply.
        await test.Master.SignalCompletionAsync(CancellationToken.None);
        await ((IAsyncDisposable) test.Master).DisposeAsync();
        var lease = await dequeue.WaitAsync(Timeout);

        await Assert.That(lease).IsNull();
        await Assert.That(await worker.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Completed);
        await test.DisposeAsync();
    }

    [Test]
    public async Task Busy_Worker_Learns_Of_Completion_Without_Dequeuing()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);

        await test.Master.SignalCompletionAsync(CancellationToken.None);

        await WaitForMasterStateAsync(worker, DistributedMasterState.Completed);
    }

    [Test]
    public async Task Reconnecting_Worker_Learns_Of_Completion_Without_Dequeuing()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        var first = await test.ConnectWorkerAsync();
        await first.RegisterWorkerAsync(registration, CancellationToken.None);
        await first.DisposeAsync();
        await test.Master.SignalCompletionAsync(CancellationToken.None);
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);

        await WaitForMasterStateAsync(worker, DistributedMasterState.Completed);
        await Assert.That(await test.Master.GetRegisteredWorkersAsync(CancellationToken.None)).HasSingleItem();
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

    private static async Task WaitForMasterStateAsync(IDistributedWorkerCoordinator worker, DistributedMasterState expected)
    {
        using var timeout = new CancellationTokenSource(Timeout);
        while (await worker.GetMasterStateAsync(CancellationToken.None) != expected)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(20), timeout.Token);
        }
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
