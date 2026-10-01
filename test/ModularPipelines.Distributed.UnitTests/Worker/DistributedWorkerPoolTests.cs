using System.Collections.Concurrent;
using Microsoft.Extensions.Time.Testing;
using Moq;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Helpers;

namespace ModularPipelines.Distributed.UnitTests.Worker;

public class DistributedWorkerPoolTests
{
    [Test]
    [Timeout(5_000)]
    public async Task Claim_Time_Is_Recorded_When_The_Lease_Is_Dequeued(CancellationToken cancellationToken)
    {
        var clock = new FakeTimeProvider();
        var leases = new Queue<ModuleLease>([CreateLease("first"), CreateLease("second")]);
        var claimedTimes = new ConcurrentDictionary<string, DateTimeOffset>();
        var releaseFirst = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var runTask = DistributedWorkerPool.RunAsync(
            _ =>
            {
                var lease = leases.TryDequeue(out var next) ? next : null;
                return Task.FromResult(lease);
            },
            (lease, claimedAt, token) =>
            {
                claimedTimes[lease.Assignment.ModuleId.Value] = claimedAt;
                return lease.Assignment.ModuleId.Value == "first"
                    ? releaseFirst.Task.WaitAsync(token)
                    : Task.CompletedTask;
            },
            maxConcurrency: 1,
            exception => throw new InvalidOperationException("Unexpected worker error", exception),
            cancellationToken,
            clock);

        var firstClaimTime = clock.GetUtcNow();
        clock.Advance(TimeSpan.FromHours(1));
        releaseFirst.SetResult();
        await runTask.WaitAsync(cancellationToken);

        await Assert.That(claimedTimes["first"]).IsEqualTo(firstClaimTime);
        await Assert.That(claimedTimes["second"]).IsEqualTo(firstClaimTime + TimeSpan.FromHours(1));
    }

    [Test]
    [Timeout(5_000)]
    public async Task Executes_In_Parallel_And_Claims_Only_When_A_Slot_Is_Free(
        CancellationToken cancellationToken)
    {
        var leases = new ConcurrentQueue<ModuleLease>(
        [
            CreateLease("first"),
            CreateLease("second"),
            CreateLease("third"),
        ]);
        var twoStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var dequeueCount = 0;
        var active = 0;
        var peakActive = 0;
        var completed = 0;

        Task<ModuleLease?> Dequeue(CancellationToken _)
        {
            Interlocked.Increment(ref dequeueCount);
            return Task.FromResult(leases.TryDequeue(out var lease) ? lease : null);
        }

        async Task Execute(ModuleLease _, DateTimeOffset claimedAt, CancellationToken token)
        {
            var currentActive = Interlocked.Increment(ref active);
            UpdateMaximum(ref peakActive, currentActive);
            if (currentActive == 2)
            {
                twoStarted.TrySetResult();
            }

            await release.Task.WaitAsync(token);
            Interlocked.Decrement(ref active);
            Interlocked.Increment(ref completed);
        }

        var runTask = DistributedWorkerPool.RunAsync(
            Dequeue,
            Execute,
            maxConcurrency: 2,
            _ => { },
            cancellationToken);

        await twoStarted.Task.WaitAsync(cancellationToken);
        await Task.Delay(50, cancellationToken);

        // Both slots are busy, so the worker must not hold a third claimed lease.
        await Assert.That(Volatile.Read(ref dequeueCount)).IsEqualTo(2);
        await Assert.That(peakActive).IsEqualTo(2);

        release.TrySetResult();
        await runTask.WaitAsync(cancellationToken);

        await Assert.That(completed).IsEqualTo(3);
    }

    [Test]
    [Timeout(5_000)]
    public async Task In_Flight_Leases_Are_Tracked_Until_Execution_Finishes(CancellationToken cancellationToken)
    {
        var inFlight = new InFlightLeases();
        var leases = new Queue<ModuleLease>([CreateLease("tracked")]);
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var runTask = DistributedWorkerPool.RunAsync(
            _ => Task.FromResult(leases.TryDequeue(out var lease) ? lease : null),
            async (_, _, token) =>
            {
                started.TrySetResult();
                await release.Task.WaitAsync(token);
            },
            maxConcurrency: 1,
            _ => { },
            cancellationToken,
            inFlightLeases: inFlight);

        await started.Task.WaitAsync(cancellationToken);
        await Assert.That(inFlight.GetModuleIds()).Contains(new ModuleId("tracked"));

        release.SetResult();
        await runTask.WaitAsync(cancellationToken);
        await Assert.That(inFlight.GetModuleIds()).IsEmpty();
    }

    [Test]
    public async Task Per_Node_Limit_Can_Lower_But_Not_Raise_Pipeline_Limit()
    {
        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(instance => instance.GetMaxDegreeOfParallelism()).Returns(8);

        var lowered = DistributedWorkerPool.GetMaxConcurrency(
            parallelLimitProvider.Object,
            new DistributedOptions { MaxParallelism = 3 });
        var capped = DistributedWorkerPool.GetMaxConcurrency(
            parallelLimitProvider.Object,
            new DistributedOptions { MaxParallelism = 12 });
        var inherited = DistributedWorkerPool.GetMaxConcurrency(
            parallelLimitProvider.Object,
            new DistributedOptions());

        using (Assert.Multiple())
        {
            await Assert.That(lowered).IsEqualTo(3);
            await Assert.That(capped).IsEqualTo(8);
            await Assert.That(inherited).IsEqualTo(8);
        }
    }

    [Test]
    [Timeout(30_000)]
    public async Task Dequeue_Errors_Are_Throttled(CancellationToken cancellationToken)
    {
        using var stop = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        stop.CancelAfter(TimeSpan.FromMilliseconds(300));
        var errorCount = 0;

        await DistributedWorkerPool.RunAsync(
            _ => throw new InvalidOperationException("Coordinator unavailable"),
            (_, _, _) => Task.CompletedTask,
            maxConcurrency: 1,
            _ => Interlocked.Increment(ref errorCount),
            stop.Token);

        await Assert.That(errorCount).IsLessThan(10);
    }

    [Test]
    [Timeout(5_000)]
    public async Task Cancellation_Stops_A_Pending_Dequeue(CancellationToken cancellationToken)
    {
        using var stop = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var dequeueStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var pendingDequeueObservedCancellation = false;

        var runTask = DistributedWorkerPool.RunAsync(
            async token =>
            {
                dequeueStarted.TrySetResult();
                try
                {
                    await Task.Delay(Timeout.InfiniteTimeSpan, token);
                    return null;
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    pendingDequeueObservedCancellation = true;
                    throw;
                }
            },
            (_, _, _) => Task.CompletedTask,
            maxConcurrency: 1,
            _ => { },
            stop.Token);

        await dequeueStarted.Task.WaitAsync(cancellationToken);
        await stop.CancelAsync();
        await runTask.WaitAsync(cancellationToken);

        await Assert.That(pendingDequeueObservedCancellation).IsTrue();
    }

    [Test]
    [Timeout(5_000)]
    public async Task Claimed_Lease_Runs_After_Cancellation(CancellationToken cancellationToken)
    {
        using var stop = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var executions = 0;

        await DistributedWorkerPool.RunAsync(
            async token =>
            {
                // The coordinator hands over a lease as the worker is being canceled.
                await stop.CancelAsync();
                return CreateLease("claimed");
            },
            (_, _, _) =>
            {
                Interlocked.Increment(ref executions);
                return Task.CompletedTask;
            },
            maxConcurrency: 1,
            _ => { },
            stop.Token);

        await Assert.That(executions).IsEqualTo(1);
    }

    [Test]
    public async Task Execution_Cancellation_Uses_The_Exception_Token()
    {
        using var executionCancellation = new CancellationTokenSource();
        await executionCancellation.CancelAsync();
        var leases = new ConcurrentQueue<ModuleLease>([CreateLease("canceled")]);
        var errorCount = 0;

        await DistributedWorkerPool.RunAsync(
            _ => Task.FromResult(leases.TryDequeue(out var lease) ? lease : null),
            (_, _, _) => Task.FromException(
                new OperationCanceledException(executionCancellation.Token)),
            maxConcurrency: 1,
            _ => Interlocked.Increment(ref errorCount),
            CancellationToken.None);

        await Assert.That(errorCount).IsEqualTo(0);
    }

    private static ModuleLease CreateLease(string name) =>
        DistributedTestData.Lease(DistributedTestData.Assignment(new ModuleId(name)));

    private static void UpdateMaximum(ref int maximum, int candidate)
    {
        var current = Volatile.Read(ref maximum);
        while (candidate > current)
        {
            var observed = Interlocked.CompareExchange(ref maximum, candidate, current);
            if (observed == current)
            {
                return;
            }

            current = observed;
        }
    }
}
