using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.TestHelpers;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ModularPipelines.OpenTelemetry.UnitTests;

[TUnit.Core.NotInParallel]
public class ServiceResourceTests
{
    [Test]
    [Arguments(null, null, "pipeline-service", null)]
    [Arguments("", "", "pipeline-service", null)]
    [Arguments("  ", "  ", "pipeline-service", null)]
    [Arguments("environment-service", null, "environment-service", null)]
    [Arguments(null, "service.name=resource-service", "resource-service", null)]
    [Arguments("environment-service", "service.name=resource-service", "environment-service", null)]
    [Arguments(null, "deployment.environment.name=test", "pipeline-service", null)]
    [Arguments(null, "service.version=environment-version", "pipeline-service", "environment-version")]
    [Arguments("environment-service", "service.version=environment-version", "environment-service", "environment-version")]
    public async Task Service_Defaults_Respect_Environment_Overrides(
        string? serviceName, string? resourceAttributes, string expectedName, string? expectedVersion)
    {
        await WithEnvironment(serviceName, resourceAttributes, async () =>
        {
            var builder = TestPipelineBuilder.Create();
            builder.Environment.ApplicationName = "pipeline-service";
            builder.AddOpenTelemetry();
            using var services = builder.Services.BuildServiceProvider();

            var resources = new[]
            {
                services.GetRequiredService<TracerProvider>().GetResource(),
                services.GetRequiredService<MeterProvider>().GetResource(),
            };
            foreach (var resource in resources)
            {
                var attributes = resource.Attributes.ToDictionary();
                await Assert.That(attributes["service.name"]).IsEqualTo(expectedName);
                await Assert.That(attributes["service.version"])
                    .IsEqualTo(expectedVersion ?? System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version!.ToString());
                if (resourceAttributes == "deployment.environment.name=test")
                {
                    await Assert.That(attributes["deployment.environment.name"]).IsEqualTo("test");
                }
            }
        });
    }

    [Test]
    public async Task Explicit_Resource_Configuration_Can_Override_Service_Defaults_And_Environment()
    {
        await WithEnvironment("environment-service", "service.version=environment-version", async () =>
        {
            var builder = TestPipelineBuilder.Create();
            builder.AddOpenTelemetry(telemetry => telemetry.Builder.ConfigureResource(resource =>
                resource.AddService("configured-service", serviceVersion: "configured-version")));
            using var services = builder.Services.BuildServiceProvider();

            var resources = new[]
            {
                services.GetRequiredService<TracerProvider>().GetResource(),
                services.GetRequiredService<MeterProvider>().GetResource(),
            };
            foreach (var resource in resources)
            {
                var attributes = resource.Attributes.ToDictionary();
                await Assert.That(attributes["service.name"]).IsEqualTo("configured-service");
                await Assert.That(attributes["service.version"]).IsEqualTo("configured-version");
            }
        });
    }

    private static async Task WithEnvironment(string? serviceName, string? resourceAttributes, Func<Task> test)
    {
        var previousName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME");
        var previousAttributes = Environment.GetEnvironmentVariable("OTEL_RESOURCE_ATTRIBUTES");
        try
        {
            Environment.SetEnvironmentVariable("OTEL_SERVICE_NAME", serviceName);
            Environment.SetEnvironmentVariable("OTEL_RESOURCE_ATTRIBUTES", resourceAttributes);
            await test();
        }
        finally
        {
            Environment.SetEnvironmentVariable("OTEL_SERVICE_NAME", previousName);
            Environment.SetEnvironmentVariable("OTEL_RESOURCE_ATTRIBUTES", previousAttributes);
        }
    }
}
