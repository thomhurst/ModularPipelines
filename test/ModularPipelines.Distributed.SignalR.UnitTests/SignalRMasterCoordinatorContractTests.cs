using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Testing.Distributed;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

/// <summary>
/// Runs the shared coordinator contract against the SignalR master coordinator.
/// </summary>
public class SignalRMasterCoordinatorContractTests
{
    private static SignalRMasterCoordinator Create(TimeSpan? workerTimeout = null) =>
        new(new InMemoryDistributedCoordinator(MsOptions.Create(new DistributedOptions
        {
            WorkerTimeout = workerTimeout ?? DistributedCoordinatorContract.LeaseTimeout,
        })));

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
}
