using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
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
    public async Task AddOpenTelemetry_Registers_Trace_And_Meter_Providers()
    {
        var builder = TestPipelineHostBuilder.Create();

        builder.AddOpenTelemetry();
        builder.AddModule<TestModule>();

        await using var pipeline = await builder.BuildAsync();
        await Assert.That(pipeline.Services.GetService<TracerProvider>()).IsNotNull();
        await Assert.That(pipeline.Services.GetService<MeterProvider>()).IsNotNull();
    }

    [Test]
    public async Task AddOpenTelemetry_Returns_The_Pipeline_Builder()
    {
        var builder = TestPipelineHostBuilder.Create();

        var result = builder.AddOpenTelemetry();

        await Assert.That(result).IsSameReferenceAs(builder);
    }
}
