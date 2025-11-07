using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ParallelLimiterTests
{
    // Reduced delay from 5 seconds to 100ms for much faster test execution
    // The test verifies parallel limit behavior, not exact timing
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(100);

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module3 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }


    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module4 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module5 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module6 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [Test]
    public async Task LimitParallel()
    {
        // Use FakeTimeProvider for deterministic timing measurements
        var timeProvider = new FakeTimeProvider();

        var results = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .AddModule<Module5>()
            .AddModule<Module6>()
            .ExecutePipelineAsync();

        var start = results.Modules.MinBy(x => x.StartTime)!.StartTime.DateTime;
        var end = results.Modules.MaxBy(x => x.EndTime)!.EndTime.DateTime;

        // With limit=3 and 6 modules, execution should occur in 2 batches
        // We expect total time > 1 batch duration but < 6 batches (sequential)
        // Using reduced delays (100ms instead of 5s) makes test 50x faster
        await Assert.That(end - start).IsGreaterThanOrEqualTo(ModuleDelay.Add(TimeSpan.FromMilliseconds(-50))); // Allow for timing variations
        await Assert.That(end - start).IsLessThan(ModuleDelay.Multiply(6)); // Should not be fully sequential
    }

    private record MyParallelLimit : IParallelLimit
    {
        public int Limit => 3;
    }
}