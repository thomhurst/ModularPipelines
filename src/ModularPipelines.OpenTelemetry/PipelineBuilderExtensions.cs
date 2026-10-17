using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Tracing;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ModularPipelines.OpenTelemetry;

/// <summary>
/// Adds OpenTelemetry collection and export to a pipeline.
/// </summary>
public static class OpenTelemetryPipelineBuilderExtensions
{
    /// <summary>
    /// Registers Modular Pipelines activities and metrics with OpenTelemetry.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Optional exporter configuration.</param>
    /// <returns>The same pipeline builder.</returns>
    public static PipelineBuilder AddOpenTelemetry(
        this PipelineBuilder builder,
        Action<ModularPipelinesOpenTelemetryBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var pipelineName = builder.Environment.ApplicationName;
        var gitCommitSha = CiEnvironmentMetadata.FindCommitSha(Environment.GetEnvironmentVariable);
        var openTelemetryBuilder = builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resourceBuilder =>
            {
                resourceBuilder.AddService(pipelineName);
                if (gitCommitSha is not null)
                {
                    resourceBuilder.AddAttributes(
                    [
                        new KeyValuePair<string, object>("vcs.ref.head.revision", gitCommitSha),
                    ]);
                }
            })
            .WithTracing(tracing => tracing.AddSource(
                PipelineTelemetry.PipelineSourceName,
                PipelineTelemetry.ModuleSourceName,
                PipelineTelemetry.CommandSourceName))
            .WithMetrics(metrics => metrics.AddMeter(PipelineTelemetry.MeterName));

        builder.Services.TryAddSingleton<OpenTelemetryInitializer>();

        configure?.Invoke(new ModularPipelinesOpenTelemetryBuilder(openTelemetryBuilder));
        return builder;
    }
}
