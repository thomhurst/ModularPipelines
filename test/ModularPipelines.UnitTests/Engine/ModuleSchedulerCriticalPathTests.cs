using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleSchedulerCriticalPathTests
{
    [Test]
    public async Task Ready_Modules_Are_Ordered_By_Longest_Estimated_Downstream_Path()
    {
        var scheduler = CreateScheduler(new FixedEstimatedTimeProvider(new Dictionary<Type, TimeSpan>
        {
            [typeof(CriticalPathRootModule)] = TimeSpan.FromMinutes(1),
            [typeof(CriticalPathMiddleModule)] = TimeSpan.FromMinutes(10),
            [typeof(CriticalPathSinkModule)] = TimeSpan.FromMinutes(10),
            [typeof(IndependentModule)] = TimeSpan.FromMinutes(15),
        }));
        scheduler.InitializeModules(
        [
            new IndependentModule(),
            new CriticalPathSinkModule(),
            new CriticalPathMiddleModule(),
            new CriticalPathRootModule(),
        ]);

        using var cancellation = new CancellationTokenSource();
        var schedulerTask = scheduler.RunSchedulerAsync(cancellation.Token);
        var firstReady = await scheduler.ReadyModules.ReadAsync(cancellation.Token);

        await Assert.That(firstReady.ModuleType).IsEqualTo(typeof(CriticalPathRootModule));
        await Assert.That(scheduler.GetModuleState(typeof(CriticalPathRootModule))!.CriticalPathWeight)
            .IsEqualTo(TimeSpan.FromMinutes(21));

        cancellation.Cancel();
        scheduler.Dispose();
        try
        {
            await schedulerTask;
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            // Disposal and cancellation can race after the assertions complete.
        }
    }

    [Test]
    public async Task Cancellation_Interrupts_Critical_Path_Estimation()
    {
        var provider = new NeverCompletingEstimatedTimeProvider();
        var scheduler = CreateScheduler(provider);
        scheduler.InitializeModules([new IndependentModule()]);
        using var cancellation = new CancellationTokenSource();

        var schedulerTask = scheduler.RunSchedulerAsync(cancellation.Token);
        await provider.Started.Task.WaitAsync(TimeSpan.FromSeconds(5));
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await schedulerTask.WaitAsync(TimeSpan.FromSeconds(5)));
        scheduler.Dispose();
    }

    [Test]
    public async Task Planning_Estimates_Are_Reused_By_Critical_Path_Calculation()
    {
        var provider = new Mock<ISafeModuleEstimatedTimeProvider>(MockBehavior.Strict);
        var scheduler = CreateScheduler(provider.Object);
        scheduler.InitializeModules(
            [new IndependentModule()],
            new Dictionary<Type, TimeSpan>
            {
                [typeof(IndependentModule)] = TimeSpan.FromMinutes(7),
            });
        using var cancellation = new CancellationTokenSource();

        var schedulerTask = scheduler.RunSchedulerAsync(cancellation.Token);
        var ready = await scheduler.ReadyModules.ReadAsync(cancellation.Token);

        await Assert.That(ready.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(7));
        provider.Verify(
            instance => instance.GetModuleEstimatedTimeAsync(It.IsAny<Type>()),
            Times.Never());

        cancellation.Cancel();
        scheduler.Dispose();
        try
        {
            await schedulerTask;
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            // Disposal and cancellation can race after the assertions complete.
        }
    }

    [Test]
    [Timeout(10_000)]
    public async Task Critical_Path_Calculation_Handles_Deep_Graphs_Iteratively(
        CancellationToken cancellationToken)
    {
        const int moduleCount = 20_000;
        var states = Enumerable.Range(0, moduleCount)
            .Select(_ => new ModuleState(new IndependentModule(), typeof(IndependentModule))
            {
                EstimatedDuration = TimeSpan.FromTicks(1),
            })
            .ToArray();
        for (var index = 0; index < states.Length - 1; index++)
        {
            states[index].DependentModules.Add(states[index + 1]);
        }

        ModuleScheduler.CalculateCriticalPathWeights(states, cancellationToken);

        await Assert.That(states[0].CriticalPathWeight).IsEqualTo(TimeSpan.FromTicks(moduleCount));
        await Assert.That(states[^1].CriticalPathWeight).IsEqualTo(TimeSpan.FromTicks(1));
    }

    private static ModuleScheduler CreateScheduler(ISafeModuleEstimatedTimeProvider estimatedTimeProvider)
    {
        return new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Mock.Of<IMetricsCollector>(),
            Mock.Of<IModuleConstraintEvaluator>(),
            Mock.Of<ISchedulerStatusReporter>(),
            estimatedTimeProvider);
    }

    private sealed class FixedEstimatedTimeProvider(IReadOnlyDictionary<Type, TimeSpan> estimates)
        : ISafeModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType) =>
            Task.FromResult(estimates[moduleType]);

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration) => Task.CompletedTask;

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType) =>
            Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation) =>
            Task.CompletedTask;
    }

    private sealed class NeverCompletingEstimatedTimeProvider : ISafeModuleEstimatedTimeProvider
    {
        public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            Started.TrySetResult();
            return new TaskCompletionSource<TimeSpan>(TaskCreationOptions.RunContinuationsAsynchronously).Task;
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration) => Task.CompletedTask;

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType) =>
            Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation) =>
            Task.CompletedTask;
    }

    private sealed class CriticalPathRootModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [DependsOn<CriticalPathRootModule>]
    private sealed class CriticalPathMiddleModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [DependsOn<CriticalPathMiddleModule>]
    private sealed class CriticalPathSinkModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    private sealed class IndependentModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }
}
