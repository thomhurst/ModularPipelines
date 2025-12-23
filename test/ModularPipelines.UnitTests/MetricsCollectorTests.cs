using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

[TUnit.Core.NotInParallel(nameof(MetricsCollectorTests))]
public class MetricsCollectorTests : TestBase
{
    public class QuickModule1 : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "Done";
        }
    }

    public class QuickModule2 : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "Done";
        }
    }

    public class QuickModule3 : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "Done";
        }
    }

    [Test]
    public async Task PipelineSummary_ContainsMetrics()
    {
        var result = await TestPipelineHostBuilder.Create()
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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.ParallelismFactor).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    public async Task PipelineMetrics_HasPeakConcurrency()
    {
        var result = await TestPipelineHostBuilder.Create()
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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<QuickModule1>()
            .AddModule<QuickModule2>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.AverageConcurrency).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    public async Task PipelineMetrics_HasEfficiency()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.Efficiency).IsGreaterThanOrEqualTo(0);
        await Assert.That(result.Metrics!.Efficiency).IsLessThanOrEqualTo(1);
    }

    [Test]
    public async Task PipelineMetrics_HasModuleCounts()
    {
        var result = await TestPipelineHostBuilder.Create()
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
    public async Task PipelineMetrics_HasTimingData()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.Metrics).IsNotNull();
        await Assert.That(result.Metrics!.WallClockDuration).IsGreaterThan(TimeSpan.Zero);
        await Assert.That(result.Metrics!.TotalModuleExecutionTime).IsGreaterThanOrEqualTo(TimeSpan.Zero);
    }

    [Test]
    public async Task PipelineSummary_ContainsModuleTimelines()
    {
        var result = await TestPipelineHostBuilder.Create()
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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.ModuleTimelines).IsNotNull();
        await Assert.That(result.ModuleTimelines!.Count).IsEqualTo(1);
        await Assert.That(result.ModuleTimelines![0].ModuleName).IsEqualTo("QuickModule1");
    }

    [Test]
    public async Task ModuleTimeline_ContainsTimingData()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<QuickModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.ModuleTimelines).IsNotNull();
        var timeline = result.ModuleTimelines![0];
        await Assert.That(timeline.StartTime).IsNotNull();
        await Assert.That(timeline.EndTime).IsNotNull();
        await Assert.That(timeline.ExecutionDuration).IsNotNull();
    }
}
