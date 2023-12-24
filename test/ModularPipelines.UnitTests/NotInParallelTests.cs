using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class NotInParallelTests
{
    [NotInParallel]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            return GetType().Name;
        }
    }

    [NotInParallel]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            return GetType().Name;
        }
    }

    [NotInParallel]
    [DependsOn<ParallelDependency>]
    public class NotParallelModuleWithParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            return GetType().Name;
        }
    }

    public class ParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            return GetType().Name;
        }
    }

    [NotInParallel]
    [DependsOn<NotParallelModuleWithParallelDependency>]
    public class NotParallelModuleWithNonParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            return GetType().Name;
        }
    }

    [Test]
    public async Task NotInParallel()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();

        var firstModule = results.Modules.MinBy(x => x.EndTime)!;
        var nextModule = results.Modules.MaxBy(x => x.EndTime)!;

        Assert.That(
            nextModule.StartTime.ToUnixTimeMilliseconds(),
            Is.EqualTo((firstModule.StartTime + TimeSpan.FromSeconds(2)).ToUnixTimeMilliseconds())
                .Within(500)
        );
    }

    [Test]
    public async Task NotInParallel_With_ParallelDependency()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .ExecutePipelineAsync();

        var firstModule = results.Modules.MinBy(x => x.EndTime)!;
        var nextModule = results.Modules.MaxBy(x => x.EndTime)!;

        Assert.That(
            nextModule.StartTime.ToUnixTimeMilliseconds(),
            Is.EqualTo((firstModule.StartTime + TimeSpan.FromSeconds(2)).ToUnixTimeMilliseconds())
                .Within(500)
        );
    }

    [Test]
    public async Task NotInParallel_With_NonParallelDependency()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .AddModule<NotParallelModuleWithNonParallelDependency>()
            .ExecutePipelineAsync();

        var firstModule = results.Modules.MinBy(x => x.EndTime)!;
        var nextModule = results.Modules.MaxBy(x => x.EndTime)!;

        Assert.That(
            nextModule.StartTime.ToUnixTimeMilliseconds(),
            Is.EqualTo((firstModule.StartTime + TimeSpan.FromSeconds(4)).ToUnixTimeMilliseconds())
                .Within(500)
        );
    }
}