using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine.Execution;

public class DependencyWaiterTests
{
    private sealed class DependencyModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class DependentModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [Test]
    public async Task NormalizedWorkerCancellation_Is_NotWrappedAsDependencyFailure()
    {
        using var workerCancellationTokenSource = new CancellationTokenSource();
        using var limiterCancellationTokenSource = new CancellationTokenSource();
        using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        limiterCancellationTokenSource.Cancel();

        var normalizedCancellation = ModuleRunner.NormalizeLimiterCancellation(
            new OperationCanceledException(limiterCancellationTokenSource.Token),
            workerCancellationTokenSource.Token,
            limiterCancellationTokenSource.Token);
        var dependency = new DependencyModule();
        var dependentState = new ModuleState(new DependentModule(), typeof(DependentModule));
        dependentState.RecordDependency(typeof(DependencyModule), optional: false);
        var scheduler = new Mock<IModuleScheduler>();
        scheduler
            .Setup(x => x.GetModuleCompletionTask(typeof(DependencyModule)))
            .Returns(Task.FromException<IModule>(normalizedCancellation));
        scheduler
            .Setup(x => x.GetModuleState(typeof(DependencyModule)))
            .Returns(new ModuleState(dependency, typeof(DependencyModule)));

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await new DependencyWaiter().WaitForDependenciesAsync(
                dependentState,
                scheduler.Object,
                serviceProvider,
                workerCancellationTokenSource.Token));

        await Assert.That(exception).IsSameReferenceAs(normalizedCancellation);
    }
}
