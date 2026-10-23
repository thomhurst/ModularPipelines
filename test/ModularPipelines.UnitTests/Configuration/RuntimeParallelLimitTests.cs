using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;
using ModularPipelines.Validation;

namespace ModularPipelines.UnitTests.Configuration;

public class RuntimeParallelLimitTests
{
    [Test]
    public async Task Builder_LastLimiterWins_AndPreservesEarlierConfiguration()
    {
        var builder = new ModuleConfigurationBuilder().WithParallelLimiter<InvalidLimit>();
        var typed = builder.Build();
        await Assert.That(builder.WithParallelLimit("docker")).IsSameReferenceAs(builder);
        var keyed = builder.Build();
        var replaced = builder.WithParallelLimiter<InvalidLimit>().Build();

        await Assert.That(typed.ParallelLimiterType).IsEqualTo(typeof(InvalidLimit));
        await Assert.That(typed.ParallelLimitKey).IsNull();
        await Assert.That(keyed.ParallelLimiterType).IsNull();
        await Assert.That(keyed.ParallelLimitKey).IsEqualTo("docker");
        await Assert.That(replaced.ParallelLimitKey).IsNull();
        await Assert.That(replaced.ParallelLimiterType).IsEqualTo(typeof(InvalidLimit));
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public void Builder_RejectsBlankKeys(string? key)
    {
        if (key is null)
        {
            Assert.Throws<ArgumentNullException>(() => new ModuleConfigurationBuilder().WithParallelLimit(key!));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => new ModuleConfigurationBuilder().WithParallelLimit(key));
        }
    }

    [Test]
    public async Task Options_CopyCapacities_AndUseOrdinalKeys()
    {
        var limits = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["docker"] = 2 };
        var options = new ConcurrencyOptions { Limits = limits };
        limits["docker"] = 8;

        await Assert.That(options.Limits["docker"]).IsEqualTo(2);
        await Assert.That(options.Limits.ContainsKey("DOCKER")).IsFalse();
        await Assert.That(new ConcurrencyOptions().Limits.Count).IsEqualTo(0);
    }

    [Test]
    [Arguments("docker", 0)]
    [Arguments("docker", -1)]
    [Arguments(" ", 2)]
    public async Task Options_RejectInvalidLimits(string key, int limit)
    {
        var result = new OptionsValidator().ValidateOptions(new PipelineOptions
        {
            Concurrency = new ConcurrencyOptions { Limits = new Dictionary<string, int> { [key] = limit } },
        });

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors.Any(error => error.Message.Contains("Concurrency.Limits"))).IsTrue();
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Execution_SharesConfiguredCapacity_AndReleasesCancelledWaiters(int limit)
    {
        var options = CreateOptions(limit);
        using var provider = new ParallelLimitProvider(Microsoft.Extensions.Options.Options.Create(options));
        using var otherProvider = new ParallelLimitProvider(Microsoft.Extensions.Options.Options.Create(options));
        var handler = new ParallelLimitHandler(provider, NullLogger<ParallelLimitHandler>.Instance);
        using var timeout = new CancellationTokenSource(TestHostSettings.DefaultTestTimeout);
        var slots = new List<IDisposable>();
        try
        {
            for (var i = 0; i < limit; i++)
            {
                slots.Add(await handler.AcquireParallelLimitAsync(new KeyedModule<int>(), timeout.Token));
            }

            using var cancelled = new CancellationTokenSource();
            var cancelledWait = handler.AcquireParallelLimitAsync(new KeyedModule<string>(), cancelled.Token);
            await Assert.That(cancelledWait.IsCompleted).IsFalse();
            await cancelled.CancelAsync();
            await Assert.ThrowsAsync<OperationCanceledException>(() => cancelledWait);

            // Different keys and different pipelines must not share slots.
            await Assert.That(provider.GetLock("DOCKER").Wait(0)).IsTrue();
            provider.GetLock("DOCKER").Release();
            await Assert.That(otherProvider.GetLock("docker").Wait(0)).IsTrue();
            otherProvider.GetLock("docker").Release();

            var waiting = handler.AcquireParallelLimitAsync(new KeyedModule<string>(), timeout.Token);
            await Assert.That(waiting.IsCompleted).IsFalse();
            slots[0].Dispose();
            slots[0].Dispose(); // Releasing a slot is idempotent.
            using var next = await waiting;
            await Assert.That(provider.GetLock("docker").CurrentCount).IsEqualTo(0);
        }
        finally
        {
            foreach (var slot in slots)
            {
                slot.Dispose();
            }
        }
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 2)]
    public async Task Plan_UsesRuntimeCapacity_AndOverridesInvalidAttribute(int limit, int minutes)
    {
        var builder = Pipeline.CreateBuilder()
            .ConfigureConcurrency(options => options with { Limits = CreateOptions(limit).Concurrency.Limits })
            .AddModule<KeyedModule<int>>()
            .AddModule<KeyedModule<string>>()
            .AddModule<KeyedModule<bool>>()
            .AddModuleEstimatedTimeProvider<EstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(minutes));
        IModule module = new KeyedModule<int>();
        await Assert.That(module.Configuration.ParallelLimiterType).IsNull();
        await Assert.That(module.Configuration.ParallelLimitKey).IsEqualTo("docker");
    }

    [Test]
    public async Task MissingCapacity_IsRejectedByPlanningAndExecution()
    {
        await using var pipeline = await Pipeline.CreateBuilder().AddModule<KeyedModule<int>>().BuildAsync();
        await Assert.ThrowsAsync<ArgumentException>(() => pipeline.PlanAsync());
        using var provider = new ParallelLimitProvider(Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        var handler = new ParallelLimitHandler(provider, NullLogger<ParallelLimitHandler>.Instance);
        await Assert.ThrowsAsync<ArgumentException>(() => handler.AcquireParallelLimitAsync(new KeyedModule<int>(), CancellationToken.None));
    }

    [Test]
    public async Task Scheduler_ReservesKeyedCapacity_AndKeepsKeysIndependent()
    {
        var evaluator = new ModuleConstraintEvaluator(NullLogger<ModuleConstraintEvaluator>.Instance);
        var options = CreateOptions(2).Concurrency;
        var configuration = ((IModule) new KeyedModule<int>()).Configuration;
        var first = new ModuleState(new KeyedModule<int>(), typeof(KeyedModule<int>)) { ParallelLimiter = ParallelLimitConfiguration.Resolve(configuration, options) };
        var second = new ModuleState(new KeyedModule<string>(), typeof(KeyedModule<string>)) { ParallelLimiter = ParallelLimitConfiguration.Resolve(configuration, options) };
        var candidate = new ModuleState(new KeyedModule<bool>(), typeof(KeyedModule<bool>)) { ParallelLimiter = ParallelLimitConfiguration.Resolve(configuration, options) };
        await Assert.That(evaluator.CanQueue(candidate, [first])).IsTrue();
        await Assert.That(evaluator.CanQueue(candidate, [first, second])).IsFalse();
        candidate.ParallelLimiter = ParallelLimitConfiguration.Resolve(new ModuleConfigurationBuilder().WithParallelLimit("DOCKER").Build(), options);
        await Assert.That(evaluator.CanQueue(candidate, [first, second])).IsTrue();
    }

    [Test]
    public async Task Pipeline_KeyedWaitersDoNotStarveUnrelatedWork()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var result = await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with { Concurrency = CreateOptions(1).Concurrency with { MaxParallelism = 2 } })
            .AddModule(new WaitingModule<int>(release))
            .AddModule(new WaitingModule<string>(release))
            .AddModule(new ReleaseModule(release))
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    private static PipelineOptions CreateOptions(int limit) => new()
    {
        Concurrency = new ConcurrencyOptions { Limits = new Dictionary<string, int> { ["docker"] = limit, ["DOCKER"] = 1 } },
    };

    private sealed class InvalidLimit : IParallelLimit
    {
        public static int Limit => 0;
    }

    [ModularPipelines.ParallelLimiter<InvalidLimit>]
    private class KeyedModule<T> : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithParallelLimit("docker");

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(typeof(T).Name);
    }

    private sealed class WaitingModule<T>(TaskCompletionSource release) : KeyedModule<T>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithParallelLimit("docker").WithPriority(ModulePriority.Critical);

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await release.Task.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
            return typeof(T).Name;
        }
    }

    private sealed class ReleaseModule(TaskCompletionSource release) : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            release.TrySetResult();
            return Task.FromResult("released");
        }
    }

    private sealed class EstimatedTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(ModuleId moduleId, CancellationToken cancellationToken) => Task.FromResult(TimeSpan.FromMinutes(1));

        public Task SaveModuleTimeAsync(ModuleId moduleId, TimeSpan duration, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
