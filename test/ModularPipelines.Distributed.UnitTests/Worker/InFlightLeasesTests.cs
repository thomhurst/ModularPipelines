using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.Worker;

namespace ModularPipelines.Distributed.UnitTests.Worker;

public class InFlightLeasesTests
{
    [Test]
    public async Task Lost_Lease_Cancels_Only_Its_Assignment()
    {
        var leases = new InFlightLeases();
        var first = CreateLease("first");
        var second = CreateLease("second");
        var firstToken = leases.Add(first);
        var secondToken = leases.Add(second);
        await leases.ReconcileAsync([first, second], [first.LeaseId], leases.GetTimestamp());
        await Assert.That(firstToken.IsCancellationRequested).IsTrue();
        await Assert.That(secondToken.IsCancellationRequested).IsFalse();
        await Assert.That(leases.GetLeases().Select(lease => lease.LeaseId)).IsEquivalentTo([second.LeaseId]);
        leases.Remove(first);
        leases.Remove(second);
    }

    [Test]
    public async Task Missing_Heartbeat_Cancels_Lease_At_Its_Local_Deadline()
    {
        var clock = new FakeTimeProvider();
        var leases = new InFlightLeases(TimeSpan.FromSeconds(10), clock);
        var lease = CreateLease("timeout");
        var token = leases.Add(lease);
        clock.Advance(TimeSpan.FromSeconds(10));
        await Assert.That(token.IsCancellationRequested).IsTrue();
        await Assert.That(leases.GetLeases()).IsEmpty();
        leases.Remove(lease);
    }

    [Test]
    public async Task Renewal_Counts_Reply_Time_And_Does_Not_Revive_Expired_Execution()
    {
        var clock = new FakeTimeProvider();
        var leases = new InFlightLeases(TimeSpan.FromSeconds(10), clock);
        var lease = CreateLease("renewal");
        var token = leases.Add(lease);
        clock.Advance(TimeSpan.FromSeconds(3));
        var sentAt = leases.GetTimestamp();
        clock.Advance(TimeSpan.FromSeconds(4));
        await leases.ReconcileAsync([lease], [], sentAt);
        clock.Advance(TimeSpan.FromSeconds(5));
        await Assert.That(token.IsCancellationRequested).IsFalse();
        clock.Advance(TimeSpan.FromSeconds(1));
        await Assert.That(token.IsCancellationRequested).IsTrue();
        await leases.ReconcileAsync([lease], [], leases.GetTimestamp());
        await Assert.That(token.IsCancellationRequested).IsTrue();
        leases.Remove(lease);
    }

    [Test]
    public async Task Heartbeat_Snapshot_Does_Not_Renew_A_Newer_Claim()
    {
        var clock = new FakeTimeProvider();
        var leases = new InFlightLeases(TimeSpan.FromSeconds(10), clock);
        var old = CreateLease("old");
        leases.Add(old);
        var snapshot = leases.GetLeases();
        var sentAt = leases.GetTimestamp();
        leases.Remove(old);
        var current = CreateLease("new");
        var token = leases.Add(current);
        clock.Advance(TimeSpan.FromSeconds(5));
        await leases.ReconcileAsync(snapshot, [old.LeaseId], sentAt);
        clock.Advance(TimeSpan.FromSeconds(5));
        await Assert.That(token.IsCancellationRequested).IsTrue();
        leases.Remove(current);
    }

    [Test]
    [Timeout(5000)]
    public async Task Pool_Passes_Lease_Loss_To_Running_Module(CancellationToken cancellationToken)
    {
        var leases = new InFlightLeases();
        var lease = CreateLease("pool");
        var queue = new Queue<ModuleLease>([lease]);
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var canceled = false;
        var run = DistributedWorkerPool.RunAsync(
            _ => Task.FromResult(queue.TryDequeue(out var next) ? next : null),
            async (_, _, token) =>
            {
                started.SetResult();
                try
                {
                    await Task.Delay(Timeout.InfiniteTimeSpan, token);
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    canceled = true;
                }
            }, 1, exception => throw exception, cancellationToken, inFlightLeases: leases);
        await started.Task.WaitAsync(cancellationToken);
        await leases.ReconcileAsync([lease], [lease.LeaseId], leases.GetTimestamp());
        await run.WaitAsync(cancellationToken);
        await Assert.That(canceled).IsTrue();
        await Assert.That(leases.GetLeases()).IsEmpty();
    }

    private static ModuleLease CreateLease(string id) => new()
    {
        LeaseId = id,
        WorkerId = new WorkerId("worker"),
        Assignment = DistributedTestData.Assignment(new ModuleId(id)),
    };
}
