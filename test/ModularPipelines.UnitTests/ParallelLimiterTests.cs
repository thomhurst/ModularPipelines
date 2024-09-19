using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

[Retry(3)]
public class ParallelLimiterTests
{
    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module3 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }


    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module4 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }
    
    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module5 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }
    
    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module6 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }

    [Test]
    public async Task LimitParallel()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .AddModule<Module5>()
            .AddModule<Module6>()
            .ExecutePipelineAsync();

        var start = results.Modules.MinBy(x => x.StartTime)!.StartTime.DateTime;
        var end = results.Modules.MaxBy(x => x.EndTime)!.EndTime.DateTime;

        await Assert.That(end - start).IsGreaterThan(TimeSpan.FromSeconds(10));
        await Assert.That(end - start).IsLessThan(TimeSpan.FromSeconds(30));
    }

    private record MyParallelLimit : IParallelLimit
    {
        public int Limit => 3;
    }
}