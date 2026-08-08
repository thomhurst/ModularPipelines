using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel(nameof(MetricsCollectorTests))]
public class MetricsCollectorTests : TestBase
{
    private class MetricsModule<T>
    {
    }

    public class QuickModule1 : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "Done";
        }
    }

    public class QuickModule2 : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "Done";
        }
    }

    public class QuickModule3 : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "Done";
        }
    }

    [Test]
    public async Task PipelineSummary_ContainsMetrics()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .AddModule<QuickModule3>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Metrics).IsNotNull();
    }

    [Test]
    public async Task PipelineMetrics_HasParallelismFactor()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.ParallelismFactor).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    public async Task PipelineMetrics_HasPeakConcurrency()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .AddModule<QuickModule3>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.PeakConcurrency).IsGreaterThanOrEqualTo(1);
    }

    [Test]
    public async Task PipelineMetrics_HasAverageConcurrency()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.AverageConcurrency).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    public async Task PipelineMetrics_HasEfficiency()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.Efficiency).IsGreaterThanOrEqualTo(0);
        await Assert.That(result.Metrics!.Efficiency).IsLessThanOrEqualTo(1);
    }

    [Test]
    public async Task PipelineMetrics_HasModuleCounts()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .AddModule<QuickModule3>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.TotalModules).IsEqualTo(3);
        await Assert.That(result.Metrics!.SuccessfulModules).IsEqualTo(3);
        await Assert.That(result.Metrics!.FailedModules).IsEqualTo(0);
    }

    [Test]
    public async Task PipelineMetrics_PreservesStatusCounts()
    {
        var collector = new MetricsCollector();
        var moduleTypes = CreateModuleTypes(68).GetEnumerator();
        var now = DateTimeOffset.UtcNow;

        RecordCompletedModules(collector, moduleTypes, now, 34, Status.Successful);
        RecordCompletedModules(collector, moduleTypes, now, 6, Status.Failed);
        RecordCompletedModules(collector, moduleTypes, now, 5, Status.Skipped);
        RecordPendingModules(collector, moduleTypes, now, 23);

        var metrics = collector.ComputeMetrics(now, now.AddMinutes(1), maxParallelism: 4);

        using (Assert.Multiple())
        {
            await Assert.That(metrics.TotalModules).IsEqualTo(68);
            await Assert.That(metrics.SuccessfulModules).IsEqualTo(34);
            await Assert.That(metrics.FailedModules).IsEqualTo(6);
            await Assert.That(metrics.SkippedModules).IsEqualTo(5);
            await Assert.That(metrics.PendingModules).IsEqualTo(23);
        }
    }

    [Test]
    public async Task PipelineMetrics_DoesNotTreatNonFailureStatusesAsFailed()
    {
        var collector = new MetricsCollector();
        var moduleTypes = CreateModuleTypes(5).GetEnumerator();
        var now = DateTimeOffset.UtcNow;

        RecordCompletedModules(collector, moduleTypes, now, 1, Status.UsedHistory);
        RecordCompletedModules(collector, moduleTypes, now, 1, Status.CachedResult);
        RecordCompletedModules(collector, moduleTypes, now, 1, Status.IgnoredFailure);
        RecordCompletedModules(collector, moduleTypes, now, 1, Status.Processing);
        RecordCompletedModules(collector, moduleTypes, now, 1, Status.Unknown);

        var metrics = collector.ComputeMetrics(now, now.AddMinutes(1), maxParallelism: 4);

        using (Assert.Multiple())
        {
            await Assert.That(metrics.SuccessfulModules).IsEqualTo(2);
            await Assert.That(metrics.FailedModules).IsEqualTo(0);
            await Assert.That(metrics.IgnoredFailureModules).IsEqualTo(1);
            await Assert.That(metrics.ProcessingModules).IsEqualTo(1);
            await Assert.That(metrics.UnknownModules).IsEqualTo(1);
        }
    }

    [Test]
    public async Task PipelineMetrics_HasTimingData()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.WallClockDuration).IsGreaterThan(TimeSpan.Zero);
        await Assert.That(result.Metrics!.TotalModuleExecutionTime).IsGreaterThanOrEqualTo(TimeSpan.Zero);
    }

    [Test]
    public async Task PipelineSummary_ContainsModuleTimelines()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.ModuleTimelines).IsNotNull();
        await Assert.That(result.ModuleTimelines!.Count).IsEqualTo(2);
    }

    [Test]
    public async Task ModuleTimeline_ContainsModuleName()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.ModuleTimelines).IsNotNull();
        await Assert.That(result.ModuleTimelines!.Count).IsEqualTo(1);
        await Assert.That(result.ModuleTimelines![0].ModuleName).IsEqualTo("QuickModule1");
    }

    [Test]
    public async Task ModuleTimeline_ContainsTimingData()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.ModuleTimelines).IsNotNull();
        var timeline = result.ModuleTimelines![0];
        await Assert.That(timeline.StartTime).IsNotNull();
        await Assert.That(timeline.EndTime).IsNotNull();
        await Assert.That(timeline.ExecutionDuration).IsNotNull();
    }

    private static IEnumerable<Type> CreateModuleTypes(int count)
    {
        var typeArgument = typeof(int);
        for (var index = 0; index < count; index++)
        {
            typeArgument = typeof(MetricsModule<>).MakeGenericType(typeArgument);
            yield return typeArgument;
        }
    }

    private static void RecordCompletedModules(
        MetricsCollector collector,
        IEnumerator<Type> moduleTypes,
        DateTimeOffset now,
        int count,
        Status status)
    {
        for (var index = 0; index < count; index++)
        {
            var moduleType = GetNextModuleType(moduleTypes);
            collector.RecordModuleReady(moduleType, now, default, default);
            collector.RecordModuleCompleted(moduleType, now, status == Status.Successful, status == Status.Skipped, status);
        }
    }

    private static void RecordPendingModules(
        MetricsCollector collector,
        IEnumerator<Type> moduleTypes,
        DateTimeOffset now,
        int count)
    {
        for (var index = 0; index < count; index++)
        {
            collector.RecordModuleReady(GetNextModuleType(moduleTypes), now, default, default);
        }
    }

    private static Type GetNextModuleType(IEnumerator<Type> moduleTypes)
    {
        if (!moduleTypes.MoveNext())
        {
            throw new InvalidOperationException("Expected another module type");
        }

        return moduleTypes.Current;
    }
}
