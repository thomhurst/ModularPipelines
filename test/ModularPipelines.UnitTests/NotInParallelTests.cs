using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class NotInParallelTests
{
    // Reduced delay from 5 seconds to 100ms for much faster test execution
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(100);

    [ModularPipelines.Attributes.NotInParallel]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel]
    [ModularPipelines.Attributes.DependsOn<ParallelDependency>]
    public class NotParallelModuleWithParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    public class ParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel]
    [ModularPipelines.Attributes.DependsOn<NotParallelModuleWithParallelDependency>]
    public class NotParallelModuleWithNonParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [Test]
    public async Task NotInParallel()
    {
        var timeProvider = new FakeTimeProvider();

        var results = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();

        var firstModule = results.Modules.MinBy(x => x.EndTime)!;
        var nextModule = results.Modules.MaxBy(x => x.EndTime)!;
        await Assert.That(nextModule.StartTime)
            .IsGreaterThanOrEqualTo(firstModule.StartTime + ModuleDelay.Add(TimeSpan.FromMilliseconds(-50)));
    }

    [Test]
    public async Task NotInParallel_With_ParallelDependency()
    {
        var timeProvider = new FakeTimeProvider();

        var results = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .ExecutePipelineAsync();

        var firstModule = results.Modules.MinBy(x => x.EndTime)!;
        var nextModule = results.Modules.MaxBy(x => x.EndTime)!;
        await Assert.That(nextModule.StartTime)
            .IsGreaterThanOrEqualTo(firstModule.StartTime + ModuleDelay.Add(TimeSpan.FromMilliseconds(-50)));
    }

    [Test]
    public async Task NotInParallel_With_NonParallelDependency()
    {
        var timeProvider = new FakeTimeProvider();

        var results = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .AddModule<NotParallelModuleWithNonParallelDependency>()
            .ExecutePipelineAsync();

        var firstModule = results.Modules.MinBy(x => x.EndTime)!;
        var nextModule = results.Modules.MaxBy(x => x.EndTime)!;

        var expectedStartTime = firstModule.StartTime + ModuleDelay.Multiply(2).Add(TimeSpan.FromMilliseconds(-50));

        await Assert.That(nextModule.StartTime)
            .IsGreaterThanOrEqualTo(expectedStartTime);
    }
}