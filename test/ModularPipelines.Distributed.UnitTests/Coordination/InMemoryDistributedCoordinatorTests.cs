using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.TestHelpers.Distributed;
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
    public Task Cancelled_Dequeue_Throws() =>
        DistributedCoordinatorContract.CancelledDequeueThrowsAsync(Create());

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
        var cancelledWait = coordinator.WaitForResultAsync(moduleId, cancellation.Token);
        var survivingWait = coordinator.WaitForResultAsync(moduleId, CancellationToken.None);

        await cancellation.CancelAsync();
        await Assert.That(async () => await cancelledWait).Throws<OperationCanceledException>();
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
