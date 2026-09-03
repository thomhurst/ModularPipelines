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
