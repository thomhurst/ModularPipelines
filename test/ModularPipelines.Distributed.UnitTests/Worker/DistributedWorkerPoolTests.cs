using System.Collections.Concurrent;
using Moq;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Helpers;

namespace ModularPipelines.Distributed.UnitTests.Worker;

public class DistributedWorkerPoolTests
{
    [Test]
    [Timeout(5_000)]
    public async Task Executes_In_Parallel_And_Prefetches_One_Assignment(
        CancellationToken cancellationToken)
    {
        var assignments = new ConcurrentQueue<ModuleAssignment>(
        [
            CreateAssignment("first"),
            CreateAssignment("second"),
            CreateAssignment("third"),
        ]);
        var twoStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var thirdDequeued = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var dequeueCount = 0;
        var active = 0;
        var peakActive = 0;
        var completed = 0;

        Task<ModuleAssignment?> Dequeue(CancellationToken _)
        {
            var currentDequeue = Interlocked.Increment(ref dequeueCount);
            if (currentDequeue == 3)
            {
                thirdDequeued.TrySetResult();
            }

            return Task.FromResult(assignments.TryDequeue(out var assignment)
                ? assignment
                : null);
        }

        async Task Execute(ModuleAssignment _, CancellationToken token)
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
        await thirdDequeued.Task.WaitAsync(cancellationToken);

        await Assert.That(peakActive).IsEqualTo(2);

        release.TrySetResult();
        await runTask.WaitAsync(cancellationToken);

        await Assert.That(completed).IsEqualTo(3);
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

    private static ModuleAssignment CreateAssignment(string name) => new(
        name,
        typeof(int).FullName!,
        new HashSet<Capability>(),
        DateTimeOffset.UtcNow,
        new ModuleAssignmentConfiguration(null, false));

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
