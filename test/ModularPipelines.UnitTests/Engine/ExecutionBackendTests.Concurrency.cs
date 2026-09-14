using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.ExecutionBackend.TestFixtures;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public partial class ExecutionBackendTests
{
    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Timeout(30_000)]
    public async Task CustomBackendHonorsGlobalParallelism(int limit, CancellationToken cancellationToken)
    {
        var probe = new ConcurrencyProbe(limit);
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<FirstConcurrentModule>()
            .AddModule<SecondConcurrentModule>()
            .AddModule<ThirdConcurrentModule>()
            .AddExecutionBackend<InProcessExecutionBackend>()
            .ConfigureServices(services => services.AddSingleton(probe))
            .ConfigureOptions(options => options with
            {
                Concurrency = options.Concurrency with { MaxParallelism = limit },
            })
            .BuildAsync();

        await pipeline.RunAsync(cancellationToken);

        await Assert.That(probe.Completed).IsEqualTo(3);
        await Assert.That(probe.MaximumActive).IsEqualTo(limit);
    }

    [Test]
    [Timeout(30_000)]
    public async Task CustomBackendCanSubmitDependentFirstWithOneSlot(CancellationToken cancellationToken)
    {
        var backend = new CallbackBackend(async (modules, context, token) =>
        {
            var dependent = context.ExecuteModuleAsync(modules.OfType<OrderingDependentModule>().Single(), token);
            var dependency = context.ExecuteModuleAsync(modules.OfType<BackendTestModule>().Single(), token);
            return await Task.WhenAll(dependent, dependency);
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .AddModule<OrderingDependentModule>()
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .ConfigureOptions(options => options with
            {
                Concurrency = options.Concurrency with { MaxParallelism = 1 },
            })
            .BuildAsync();

        await pipeline.RunAsync(cancellationToken);

        var dependent = pipeline.Services.GetServices<IModule>().OfType<OrderingDependentModule>().Single();
        await Assert.That(dependent.ExecutionCount).IsEqualTo(1);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task CustomBackendAlwaysRunSurvivesDependencyFailure(
        bool requestAfterFailure, CancellationToken cancellationToken)
    {
        var backend = new CallbackBackend(async (modules, context, token) =>
        {
            var dependency = modules.OfType<FailingBackendModule>().Single();
            var cleanup = modules.OfType<AlwaysRunBackendCleanup>().Single();
            if (requestAfterFailure)
            {
                try
                {
                    await context.ExecuteModuleAsync(dependency, token);
                }
                catch (ModuleFailedException)
                {
                    await context.ExecuteModuleAsync(cleanup, token);
                    throw;
                }

                throw new InvalidOperationException("Expected dependency failure.");
            }

            var cleanupRequest = context.ExecuteModuleAsync(cleanup, token);
            var duplicateCleanupRequest = context.ExecuteModuleAsync(cleanup, token);
            var dependencyRequest = context.ExecuteModuleAsync(dependency, token);
            return await Task.WhenAll(cleanupRequest, duplicateCleanupRequest, dependencyRequest);
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<FailingBackendModule>()
            .AddModule<AlwaysRunBackendCleanup>()
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .ConfigureOptions(options => options with
            {
                FailureMode = FailureMode.FailFast,
                Concurrency = options.Concurrency with { MaxParallelism = 1 },
            })
            .BuildAsync();

        await Assert.ThrowsAsync<ModuleFailedException>(() => pipeline.RunAsync(cancellationToken));

        var cleanup = pipeline.Services.GetServices<IModule>().OfType<AlwaysRunBackendCleanup>().Single();
        await Assert.That(cleanup.ExecutionCount).IsEqualTo(1);
        await Assert.That((await cleanup).Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    [Timeout(30_000)]
    public async Task CustomBackendAlwaysRunHonorsPipelineUserCancellation(CancellationToken cancellationToken)
    {
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var backend = new CallbackBackend(async (modules, context, token) =>
        {
            var dependent = modules.OfType<OrderingDependentModule>().Single();
            var request = context.ExecuteModuleAsync(dependent, token);
            var duplicate = context.ExecuteModuleAsync(dependent, token);
            await cancellation.CancelAsync();
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => duplicate);
            await Assert.That(exception!.CancellationToken).IsEqualTo(token);
            return [await request];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .AddModule(new OrderingDependentModule { AlwaysRun = true })
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .BuildAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(() => pipeline.RunAsync(cancellation.Token));

        var dependent = pipeline.Services.GetServices<IModule>().OfType<OrderingDependentModule>().Single();
        await Assert.That(dependent.ExecutionCount).IsEqualTo(0);
    }

    private sealed class ConcurrencyProbe(int expectedParallelism)
    {
        private readonly Lock _sync = new();
        private readonly TaskCompletionSource _concurrencyReached = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _active;
        public int MaximumActive { get; private set; }
        public int Completed { get; private set; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                MaximumActive = Math.Max(MaximumActive, ++_active);
                if (_active >= expectedParallelism)
                {
                    _concurrencyReached.TrySetResult();
                }
            }

            try
            {
                await _concurrencyReached.Task.WaitAsync(cancellationToken);
                await Task.Delay(100, cancellationToken);
            }
            finally
            {
                lock (_sync)
                {
                    _active--;
                    Completed++;
                }
            }
        }
    }

    private abstract class ConcurrentBackendModule : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(
            IModuleContext context, CancellationToken cancellationToken)
        {
            await context.Services.GetRequiredService<ConcurrencyProbe>().ExecuteAsync(cancellationToken);
            return 42;
        }
    }

    private sealed class FirstConcurrentModule : ConcurrentBackendModule;
    private sealed class SecondConcurrentModule : ConcurrentBackendModule;
    private sealed class ThirdConcurrentModule : ConcurrentBackendModule;

    private sealed class FailingBackendModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dependency failed.");
    }

    [ModularPipelines.DependsOn<FailingBackendModule>]
    private sealed class AlwaysRunBackendCleanup : Module<int>
    {
        public int ExecutionCount { get; private set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module.WithAlwaysRun();

        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return Task.FromResult(42);
        }
    }
}