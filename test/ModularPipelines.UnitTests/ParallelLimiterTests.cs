using System.Collections.Concurrent;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ParallelLimiterTests
{
    private static readonly ConcurrentBag<DateTimeRange> TestDateTimeRanges = [];

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
    [DependsOn<ParallelDependency>]
    public class ParallelLimitModuleWithParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }

    public class ParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    [DependsOn<ParallelLimitModuleWithParallelDependency>]
    public class ParallelLimitModuleWithNonParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return GetType().Name;
        }
    }

    [Test, Retry(3)]
    public async Task NotInParallel()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();

        var start = results.Modules.MinBy(x => x.StartTime)!.StartTime.DateTime;
        var end = results.Modules.MaxBy(x => x.EndTime)!.EndTime.DateTime;
        
        TestDateTimeRanges.Add(new DateTimeRange(start, end));
    }

    [Test, Retry(3)]
    public async Task NotInParallel_With_ParallelDependency()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<ParallelLimitModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .ExecutePipelineAsync();

        var start = results.Modules.MinBy(x => x.StartTime)!.StartTime.DateTime;
        var end = results.Modules.MaxBy(x => x.EndTime)!.EndTime.DateTime;
        
        TestDateTimeRanges.Add(new DateTimeRange(start, end));
    }

    [Test, Retry(3)]
    public async Task NotInParallel_With_NonParallelDependency()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<ParallelLimitModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .AddModule<ParallelLimitModuleWithNonParallelDependency>()
            .ExecutePipelineAsync();

        var start = results.Modules.MinBy(x => x.StartTime)!.StartTime.DateTime;
        var end = results.Modules.MaxBy(x => x.EndTime)!.EndTime.DateTime;
        
        TestDateTimeRanges.Add(new DateTimeRange(start, end));
    }
    
    [After(Class)]
    public static async Task AssertOverlaps()
    {
        var start = TestDateTimeRanges.MinBy(x => x.Start)!.Start;
        var end = TestDateTimeRanges.MaxBy(x => x.End)!.End;

        await Assert.That(end - start).Is.GreaterThan(TimeSpan.FromSeconds(10));
        await Assert.That(end - start).Is.LessThan(TimeSpan.FromSeconds(25));
    }

    private record MyParallelLimit : IParallelLimit
    {
        public int Limit => 2;
    }
    
    private class DateTimeRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        
        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}