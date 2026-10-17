using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed;
using Moq;

namespace ModularPipelines.Distributed.UnitTests;

public class DependencyResultCacheTests
{
    [Test]
    [Timeout(5_000)]
    public async Task Canceling_One_Waiter_Does_Not_Cancel_Or_Evict_Shared_Fetch(CancellationToken cancellationToken)
    {
        using var canceledWaiter = new CancellationTokenSource();
        var result = new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        CancellationToken fetchToken = default;
        coordinator.Setup(value => value.WaitForResultAsync(new ModuleId("dependency"), It.IsAny<CancellationToken>()))
            .Returns<ModuleId, CancellationToken>((_, token) =>
            {
                fetchToken = token;
                return result.Task.WaitAsync(token);
            });
        var cache = new DependencyResultCache(coordinator.Object, cancellationToken);
        var first = cache.GetAsync(new ModuleId("dependency"), canceledWaiter.Token);
        var second = cache.GetAsync(new ModuleId("dependency"), cancellationToken);

        await canceledWaiter.CancelAsync();

        await Assert.That(async () => await first).Throws<OperationCanceledException>();
        await Assert.That(fetchToken.IsCancellationRequested).IsFalse();
        await Assert.That(second.IsCompleted).IsFalse();
        var third = cache.GetAsync(new ModuleId("dependency"), cancellationToken);
        var expected = CreateResult();
        result.SetResult(expected);
        await Assert.That(await second).IsSameReferenceAs(expected);
        await Assert.That(await third).IsSameReferenceAs(expected);
        coordinator.Verify(value => value.WaitForResultAsync(new ModuleId("dependency"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(5_000)]
    public async Task Missing_Result_Times_Out_And_Can_Be_Retried(bool ignoresCancellation, CancellationToken cancellationToken)
    {
        var clock = new FakeTimeProvider();
        var pending = new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        CancellationToken fetchToken = default;
        coordinator.Setup(value => value.WaitForResultAsync(new ModuleId("dependency"), It.IsAny<CancellationToken>()))
            .Returns<ModuleId, CancellationToken>((_, token) =>
            {
                fetchToken = token;
                return ignoresCancellation ? pending.Task : pending.Task.WaitAsync(token);
            });
        var cache = new DependencyResultCache(coordinator.Object, CancellationToken.None, clock);
        var fetch = cache.GetAsync(new ModuleId("dependency"), cancellationToken);

        clock.Advance(TimeSpan.FromSeconds(30));

        var exception = await Assert.That(async () => await fetch.WaitAsync(cancellationToken)).Throws<TimeoutException>();
        await Assert.That(exception!.Message).Contains("Dependency result unavailable");
        await Assert.That(exception.Message).Contains("dependency");
        await Assert.That(fetchToken.IsCancellationRequested).IsTrue();
        var expected = CreateResult();
        coordinator.Setup(value => value.WaitForResultAsync(new ModuleId("dependency"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        await Assert.That(await cache.GetAsync(new ModuleId("dependency"), cancellationToken)).IsSameReferenceAs(expected);
        pending.TrySetResult(expected);
        coordinator.Verify(value => value.WaitForResultAsync(new ModuleId("dependency"), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Test]
    [Timeout(5_000)]
    public async Task Worker_Cancellation_Stops_Shared_Fetch_Even_When_Coordinator_Ignores_Token(CancellationToken cancellationToken)
    {
        using var worker = new CancellationTokenSource();
        var pending = new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        coordinator.Setup(value => value.WaitForResultAsync(new ModuleId("dependency"), It.IsAny<CancellationToken>()))
            .Returns(pending.Task);
        var cache = new DependencyResultCache(coordinator.Object, worker.Token);
        var fetch = cache.GetAsync(new ModuleId("dependency"), cancellationToken);

        await worker.CancelAsync();

        await Assert.That(async () => await fetch.WaitAsync(cancellationToken)).Throws<OperationCanceledException>();
        pending.SetResult(CreateResult());
    }

    private static SerializedModuleResult CreateResult() => new()
    {
        ModuleId = new ModuleId("dependency"),
        WorkerId = WorkerId.FromInstanceIndex(0),
        Payload = "{}",
        CompletedAt = DateTimeOffset.UtcNow,
    };
}
