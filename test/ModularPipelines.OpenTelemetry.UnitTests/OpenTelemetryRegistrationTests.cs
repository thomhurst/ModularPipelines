using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Tracing;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace ModularPipelines.OpenTelemetry.UnitTests;

public class OpenTelemetryRegistrationTests
{
    private sealed class TestModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task AddOpenTelemetry_Initializes_Trace_And_Meter_Providers()
    {
        var metricReader = new RecordingMetricReader();
        var builder = TestPipelineBuilder.Create();

        builder.AddOpenTelemetry();
        builder.Services.AddOpenTelemetry().WithMetrics(metrics => metrics.AddReader(metricReader));
        builder.AddModule<TestModule>();

        await using var pipeline = await builder.BuildAsync();

        using var activitySource = new ActivitySource(PipelineTelemetry.PipelineSourceName);
        using var activity = activitySource.StartActivity("test");
        using var meter = new Meter(PipelineTelemetry.MeterName);
        var counter = meter.CreateCounter<long>("test");

        await Assert.That(activity).IsNotNull();
        await Assert.That(counter.Enabled).IsTrue();
        await Assert.That(pipeline.Services.GetService<TracerProvider>()).IsNotNull();
        await Assert.That(pipeline.Services.GetService<MeterProvider>()).IsNotNull();
    }

    [Test]
    public async Task AddOpenTelemetry_ForceFlushes_Providers_Before_Disposal()
    {
        var activityProcessor = new RecordingActivityProcessor();
        var metricReader = new RecordingMetricReader();
        var builder = TestPipelineBuilder.Create();

        builder.AddOpenTelemetry();
        builder.Services.AddOpenTelemetry().WithTracing(tracing =>
            tracing.AddProcessor(activityProcessor));
        builder.Services.AddOpenTelemetry().WithMetrics(metrics => metrics.AddReader(metricReader));
        builder.AddModule<TestModule>();

        var pipeline = await builder.BuildAsync();
        await pipeline.DisposeAsync();

        await Assert.That(activityProcessor.ForceFlushCalled).IsTrue();
        await Assert.That(activityProcessor.FlushTimeoutMilliseconds).IsGreaterThan(0);
        await Assert.That(metricReader.CollectCalled).IsTrue();
        await Assert.That(metricReader.CollectTimeoutMilliseconds).IsGreaterThan(0);
    }

    [Test]
    public async Task AddOpenTelemetry_Returns_The_Pipeline_Builder()
    {
        var builder = TestPipelineBuilder.Create();

        var result = builder.AddOpenTelemetry();

        await Assert.That(result).IsSameReferenceAs(builder);
    }

    private sealed class RecordingActivityProcessor : BaseProcessor<Activity>
    {
        public bool ForceFlushCalled { get; private set; }

        public int FlushTimeoutMilliseconds { get; private set; }

        protected override bool OnForceFlush(int timeoutMilliseconds)
        {
            ForceFlushCalled = true;
            FlushTimeoutMilliseconds = timeoutMilliseconds;
            return true;
        }
    }

    private sealed class RecordingMetricReader : MetricReader
    {
        public bool CollectCalled { get; private set; }

        public int CollectTimeoutMilliseconds { get; private set; }

        protected override bool OnCollect(int timeoutMilliseconds)
        {
            CollectCalled = true;
            CollectTimeoutMilliseconds = timeoutMilliseconds;
            return true;
        }
    }
}
