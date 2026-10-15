using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Testing.Distributed;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class InMemoryDistributedCoordinatorTests
{
    private static InMemoryDistributedCoordinator Create(TimeSpan? workerTimeout = null) =>
        new(MsOptions.Create(new DistributedOptions
        {
            WorkerTimeout = workerTimeout ?? DistributedCoordinatorContract.LeaseTimeout,
        }));

    [Test]
    public async Task Concurrent_Dequeue_Retries_Claim_Only_One_Module()
    {
        var coordinator = Create();
        var claimId = Guid.NewGuid();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var first = coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, timeout.Token);
        var retry = coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, timeout.Token);
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("First")), CancellationToken.None);
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("Second")), CancellationToken.None);

        var leases = await Task.WhenAll(first, retry);

        await Assert.That(leases[1]).IsSameReferenceAs(leases[0]);
        await Assert.That(await coordinator.GetActiveLeasesAsync(CancellationToken.None)).HasSingleItem();
    }

    [Test]
    public async Task Claim_Ids_Are_Scoped_To_Their_Worker()
    {
        var coordinator = Create();
        var claimId = Guid.NewGuid();
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("First")), CancellationToken.None);
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("Second")), CancellationToken.None);
        var first = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, CancellationToken.None);
        var secondWorker = new WorkerId("other");
        var second = await coordinator.DequeueModuleAsync(secondWorker, new HashSet<Capability>(), claimId, CancellationToken.None);

        await Assert.That(second!.WorkerId).IsEqualTo(secondWorker);
        await Assert.That(second.Assignment.ModuleId).IsNotEqualTo(first!.Assignment.ModuleId);
    }

    [Test]
    public async Task Canceling_A_Pending_Claim_Does_Not_Poison_Its_Retry()
    {
        var coordinator = Create();
        var claimId = Guid.NewGuid();
        using var cancellation = new CancellationTokenSource();
        var pending = coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, cancellation.Token);
        await cancellation.CancelAsync();
        await Assert.That(async () => await pending).Throws<OperationCanceledException>();
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("Module")), CancellationToken.None);

        var retry = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, CancellationToken.None);

        await Assert.That(retry!.Assignment.ModuleId).IsEqualTo(new ModuleId("Module"));
    }

    [Test]
    public async Task Publishing_A_Result_Clears_The_Claim()
    {
        var coordinator = Create();
        var claimId = Guid.NewGuid();
        var moduleId = new ModuleId("First");
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(moduleId), CancellationToken.None);
        var first = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, CancellationToken.None);
        await coordinator.PublishResultAsync(DistributedTestData.Result(moduleId), first, CancellationToken.None);
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("Second")), CancellationToken.None);

        var next = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, CancellationToken.None);

        await Assert.That(next!.Assignment.ModuleId).IsEqualTo(new ModuleId("Second"));
    }

    [Test]
    public async Task Expiring_A_Lease_Clears_The_Claim()
    {
        var clock = new FakeTimeProvider();
        var coordinator = new InMemoryDistributedCoordinator(
            MsOptions.Create(new DistributedOptions { WorkerTimeout = TimeSpan.FromSeconds(10) }), clock);
        var claimId = Guid.NewGuid();
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("Module")), CancellationToken.None);
        var first = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, CancellationToken.None);
        clock.Advance(TimeSpan.FromSeconds(11));
        await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);

        var retry = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), claimId, CancellationToken.None);

        await Assert.That(retry!.Assignment.ModuleId).IsEqualTo(first!.Assignment.ModuleId);
        await Assert.That(retry.LeaseId).IsNotEqualTo(first.LeaseId);
        await Assert.That(await coordinator.GetActiveLeasesAsync(CancellationToken.None)).HasSingleItem();
    }

    [Test]
    public Task Enqueue_And_Dequeue_RoundTrips() =>
        DistributedCoordinatorContract.EnqueueAndDequeueRoundTripsAsync(Create());

    [Test]
    public Task Publish_Unblocks_Wait_And_RoundTrips_Result() =>
        DistributedCoordinatorContract.ResultRoundTripsAfterWaitStartsAsync(Create());

    [Test]
    public Task First_Published_Result_Is_Final() =>
        DistributedCoordinatorContract.FirstPublishedResultIsFinalAsync(Create());

    [Test]
    public Task Completion_Signal_Unblocks_Pending_Dequeue() =>
        DistributedCoordinatorContract.CompletionUnblocksPendingDequeueAsync(Create());

    [Test]
    public Task Completion_Closes_Registration() =>
        DistributedCoordinatorContract.CompletionClosesRegistrationAsync(Create());

    [Test]
    public Task Canceled_Dequeue_Throws() =>
        DistributedCoordinatorContract.CanceledDequeueThrowsAsync(Create());

    [Test]
    public Task Cancellation_Signal_Unblocks_Worker_Observer() =>
        DistributedCoordinatorContract.CancellationUnblocksWorkerObserverAsync(Create());

    [Test]
    public Task Pipeline_Failure_Only_Releases_AlwaysRun_Work() =>
        DistributedCoordinatorContract.PipelineFailureOnlyReleasesAlwaysRunWorkAsync(Create());

    [Test]
    public Task Stop_Closes_Dequeue() =>
        DistributedCoordinatorContract.StopClosesDequeueAsync(Create());

    [Test]
    public Task Withdraw_Removes_Queued_Assignment() =>
        DistributedCoordinatorContract.WithdrawRemovesQueuedAssignmentAsync(Create());

    [Test]
    public Task Expired_Lease_Is_Requeued() =>
        DistributedCoordinatorContract.ExpiredLeaseIsRequeuedAsync(Create());

    [Test]
    public Task Heartbeat_Renews_Lease() =>
        DistributedCoordinatorContract.HeartbeatRenewsLeaseAsync(Create());

    [Test]
    public Task Publishing_Releases_Lease() =>
        DistributedCoordinatorContract.PublishingReleasesLeaseAsync(Create());

    [Test]
    public Task Heartbeat_Keeps_Worker_Registration_Live() =>
        DistributedCoordinatorContract.WorkerHeartbeatKeepsRegistrationLiveAsync(Create(TimeSpan.FromSeconds(30)));

    [Test]
    public Task Duplicate_Live_Worker_Is_Rejected() =>
        DistributedCoordinatorContract.DuplicateLiveWorkerIsRejectedAsync(Create(TimeSpan.FromSeconds(30)));

    [Test]
    public Task Claim_Prefers_Scarce_Capability_Work() =>
        DistributedCoordinatorContract.ClaimPrefersScarceCapabilityWorkAsync(Create(TimeSpan.FromSeconds(30)));

    [Test]
    public Task Claim_Prefers_Priority_Then_Critical_Path() =>
        DistributedCoordinatorContract.ClaimPrefersPriorityThenCriticalPathAsync(Create());

    [Test]
    public Task Claim_Matches_Alternative_Capabilities() =>
        DistributedCoordinatorContract.ClaimMatchesAlternativeCapabilitiesAsync(Create());

    [Test]
    public Task Final_Metrics_Keep_Worker_Registration_After_Heartbeat_Expires() =>
        DistributedCoordinatorContract.FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync(
            Create(TimeSpan.FromMilliseconds(100)),
            TimeSpan.FromMilliseconds(250));

    [Test]
    public Task Cancelling_One_Observer_Leaves_Concurrent_Observer_Subscribed() =>
        DistributedCoordinatorContract.CancellationKeepsConcurrentObserverSubscribedAsync(Create());

    [Test]
    public async Task Cancelling_One_Result_Waiter_Preserves_Other_And_Late_Waiters()
    {
        var coordinator = Create();
        var moduleId = new ModuleId("Module");
        using var cancellation = new CancellationTokenSource();
        var canceledWait = coordinator.WaitForResultAsync(moduleId, cancellation.Token);
        var survivingWait = coordinator.WaitForResultAsync(moduleId, CancellationToken.None);

        await cancellation.CancelAsync();
        await Assert.That(async () => await canceledWait).Throws<OperationCanceledException>();
        var published = DistributedTestData.Result(moduleId);
        await coordinator.PublishResultAsync(published, lease: null, CancellationToken.None);

        await Assert.That(await survivingWait.WaitAsync(TimeSpan.FromSeconds(10))).IsSameReferenceAs(published);
        var lateResult = await coordinator.WaitForResultAsync(moduleId, CancellationToken.None)
            .WaitAsync(TimeSpan.FromSeconds(10));
        await Assert.That(lateResult).IsSameReferenceAs(published);
    }

    [Test]
    public async Task Stale_Worker_Is_Excluded_From_Live_Registrations()
    {
        var clock = new FakeTimeProvider();
        var coordinator = new InMemoryDistributedCoordinator(
            MsOptions.Create(new DistributedOptions { WorkerTimeout = TimeSpan.FromSeconds(10) }),
            clock);
        await coordinator.RegisterWorkerAsync(DistributedTestData.Registration(DistributedTestData.Worker), CancellationToken.None);

        clock.Advance(TimeSpan.FromSeconds(11));
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers).IsEmpty();
    }

    [Test]
    public async Task Dead_Worker_Can_Be_Replaced_By_A_New_Process()
    {
        var clock = new FakeTimeProvider();
        var coordinator = new InMemoryDistributedCoordinator(
            MsOptions.Create(new DistributedOptions { WorkerTimeout = TimeSpan.FromSeconds(10) }),
            clock);
        var first = DistributedTestData.Registration(DistributedTestData.Worker);
        await coordinator.RegisterWorkerAsync(first, CancellationToken.None);

        clock.Advance(TimeSpan.FromSeconds(11));
        var replacement = first with { RegisteredAt = first.RegisteredAt.AddMinutes(1) };
        await coordinator.RegisterWorkerAsync(replacement, CancellationToken.None);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers.Single().RegisteredAt).IsEqualTo(replacement.RegisteredAt);
    }

    [Test]
    public async Task Unmatched_Work_Does_Not_Spin_Until_State_Changes()
    {
        var coordinator = Create(TimeSpan.FromSeconds(30));
        await coordinator.EnqueueModuleAsync(
            DistributedTestData.Assignment(new ModuleId("Gpu")) with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(Capability.Gpu),
            },
            CancellationToken.None);

        var dequeue = coordinator.DequeueModuleAsync(
            DistributedTestData.Worker,
            new HashSet<Capability> { Capability.Linux },
            CancellationToken.None);
        await Task.Delay(100);
        await Assert.That(dequeue.IsCompleted).IsFalse();

        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("Any")), CancellationToken.None);
        var lease = await dequeue.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(lease!.Assignment.ModuleId).IsEqualTo(new ModuleId("Any"));
    }

    [Test]
    public async Task Late_Result_For_Requeued_Lease_Drops_The_Requeued_Copy()
    {
        var clock = new FakeTimeProvider();
        var coordinator = new InMemoryDistributedCoordinator(
            MsOptions.Create(new DistributedOptions { WorkerTimeout = TimeSpan.FromSeconds(10) }),
            clock);
        var moduleId = new ModuleId("Late");
        await coordinator.EnqueueModuleAsync(DistributedTestData.Assignment(moduleId), CancellationToken.None);
        var lease = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), CancellationToken.None);

        clock.Advance(TimeSpan.FromSeconds(11));
        await Assert.That(await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None)).Contains(moduleId);
        await coordinator.PublishResultAsync(DistributedTestData.Result(moduleId), lease, CancellationToken.None);
        using var noMoreWork = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        await Assert.That(async () => await coordinator.DequeueModuleAsync(
                new WorkerId("other"),
                new HashSet<Capability>(),
                noMoreWork.Token))
            .Throws<OperationCanceledException>();
    }
}
