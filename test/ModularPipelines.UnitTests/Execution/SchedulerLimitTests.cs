using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Execution;

public class SchedulerLimitTests
{
    public class SingleModuleLimit : IParallelLimit
    {
        public static int Limit => 1;
    }

    public class TwoModuleLimit : IParallelLimit
    {
        public static int Limit => 2;
    }

    public class WaitingModule<T>(TaskCompletionSource release, ExecutionHint hint) : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithPriority(ModulePriority.Critical)
            .WithExecutionHint(hint);

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Only unrelated work can release these modules. Limiter waiters must not occupy every worker.
            await release.Task.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
            return typeof(T).Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<SingleModuleLimit>]
    public class LimitedModule<T>(TaskCompletionSource release, ExecutionHint hint) : WaitingModule<T>(release, hint);

    [ModularPipelines.Attributes.ParallelLimiter<TwoModuleLimit>]
    public class TwoLimitedModule<T>(TaskCompletionSource release) : WaitingModule<T>(release, ExecutionHint.Default);

    public class UnrelatedModule(TaskCompletionSource release) : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            release.TrySetResult();
            return Task.FromResult<string>("released");
        }
    }

    [Test]
    [Arguments(ExecutionHint.CpuBound, false)]
    [Arguments(ExecutionHint.IoBound, false)]
    [Arguments(ExecutionHint.Default, true)]
    [Arguments(ExecutionHint.CpuBound, true)]
    public async Task LimitedModules_DoNotStarveUnrelatedWork(ExecutionHint hint, bool useLimiter)
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var builder = TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                Concurrency = options.Concurrency with
                {
                    MaxParallelism = 2,
                    MaxCpuIntensiveModules = 1,
                    MaxIoIntensiveModules = 1,
                },
            })
            .AddModule(new UnrelatedModule(release));

        if (useLimiter)
        {
            builder.AddModule(new LimitedModule<int>(release, hint))
                .AddModule(new LimitedModule<string>(release, hint))
                .AddModule(new LimitedModule<bool>(release, hint));
        }
        else
        {
            builder.AddModule(new WaitingModule<int>(release, hint))
                .AddModule(new WaitingModule<string>(release, hint))
                .AddModule(new WaitingModule<bool>(release, hint));
        }

        var result = await builder.RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    [Arguments(ExecutionHint.CpuBound, false, true)]
    [Arguments(ExecutionHint.IoBound, false, true)]
    [Arguments(ExecutionHint.Default, true, true)]
    [Arguments(ExecutionHint.CpuBound, true, false)]
    public async Task Scheduler_ReservesQueuedCapacity_AndReleasesOnCompletion(
        ExecutionHint hint, bool useLimiter, bool success)
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        IModule[] modules = useLimiter
            ? [new LimitedModule<int>(release, hint), new LimitedModule<string>(release, hint)]
            : [new WaitingModule<int>(release, hint), new WaitingModule<string>(release, hint)];
        using var scheduler = CreateScheduler(1);
        scheduler.InitializeModules([.. modules, new UnrelatedModule(release)]);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var scheduling = scheduler.RunSchedulerAsync(cancellation.Token);
        try
        {
            var first = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            var unrelated = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            await Assert.That(unrelated.ModuleType).IsEqualTo(typeof(UnrelatedModule));
            await Assert.That(scheduler.ReadyModules.TryRead(out _)).IsFalse();
            var pending = modules.Single(module => module.GetType() != first.ModuleType);
            await Assert.That(scheduler.GetModuleState(pending.GetType())!.State).IsEqualTo(ModuleExecutionState.Pending);

            await Assert.That(scheduler.MarkModuleStarted(first.ModuleType)).IsTrue();
            scheduler.MarkModuleCompleted(first.ModuleType, success);
            var next = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            await Assert.That(next.ModuleType).IsEqualTo(pending.GetType());
            scheduler.MarkModuleCompleted(next.ModuleType, true);
            scheduler.MarkModuleCompleted(unrelated.ModuleType, true);
            await scheduling.WaitAsync(cancellation.Token);
        }
        finally
        {
            await cancellation.CancelAsync();
            try
            {
                await scheduling;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    [Test]
    [Arguments(null, false)]
    [Arguments(2, false)]
    [Arguments(1, true)]
    public async Task Scheduler_HonorsCapacityAboveOne_AndUnlimitedHints(int? limit, bool useLimiter)
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var scheduler = CreateScheduler(limit);
        scheduler.InitializeModules(useLimiter
            ? [new TwoLimitedModule<int>(release), new TwoLimitedModule<string>(release)]
            : [new WaitingModule<int>(release, ExecutionHint.CpuBound), new WaitingModule<string>(release, ExecutionHint.CpuBound)]);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var scheduling = scheduler.RunSchedulerAsync(cancellation.Token);
        try
        {
            var first = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            var second = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            scheduler.MarkModuleCompleted(first.ModuleType, true);
            scheduler.MarkModuleCompleted(second.ModuleType, true);
            await scheduling.WaitAsync(cancellation.Token);
        }
        finally
        {
            await cancellation.CancelAsync();
            try
            {
                await scheduling;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    [Test]
    public async Task Scheduler_AppliesBothLimitsAcrossDifferentGroups()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var scheduler = CreateScheduler(1);
        scheduler.InitializeModules([
            new LimitedModule<int>(release, ExecutionHint.CpuBound),
            new LimitedModule<string>(release, ExecutionHint.IoBound),
            new WaitingModule<bool>(release, ExecutionHint.CpuBound),
            new UnrelatedModule(release),
        ], new Dictionary<Type, TimeSpan>
        {
            [typeof(LimitedModule<int>)] = TimeSpan.FromHours(1),
        });
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var scheduling = scheduler.RunSchedulerAsync(cancellation.Token);
        try
        {
            var first = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            var unrelated = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            await Assert.That(first.ModuleType).IsEqualTo(typeof(LimitedModule<int>));
            await Assert.That(unrelated.ModuleType).IsEqualTo(typeof(UnrelatedModule));
            await Assert.That(scheduler.ReadyModules.TryRead(out _)).IsFalse();
            scheduler.MarkModuleCompleted(first.ModuleType, true);

            // These modules use different hints and only one has the shared limiter.
            var next = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            var last = await scheduler.ReadyModules.ReadAsync(cancellation.Token);
            await Assert.That(new[] { next.ModuleType, last.ModuleType })
                .IsEquivalentTo([typeof(LimitedModule<string>), typeof(WaitingModule<bool>)]);
            scheduler.MarkModuleCompleted(next.ModuleType, true);
            scheduler.MarkModuleCompleted(last.ModuleType, true);
            scheduler.MarkModuleCompleted(unrelated.ModuleType, true);
            await scheduling.WaitAsync(cancellation.Token);
        }
        finally
        {
            await cancellation.CancelAsync();
            try
            {
                await scheduling;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    private static ModuleScheduler CreateScheduler(int? hintLimit) => new(
        NullLogger.Instance,
        TimeProvider.System,
        Microsoft.Extensions.Options.Options.Create(new PipelineOptions
        {
            Concurrency = new ConcurrencyOptions
            {
                MaxCpuIntensiveModules = hintLimit,
                MaxIoIntensiveModules = hintLimit,
            },
        }),
        new ModuleDependencyRegistry(),
        new ModuleMetadataRegistry(new ModuleAttributeEventService()),
        Mock.Of<IMetricsCollector>(),
        new ModuleConstraintEvaluator(NullLogger<ModuleConstraintEvaluator>.Instance),
        Mock.Of<ISchedulerStatusReporter>());
}
