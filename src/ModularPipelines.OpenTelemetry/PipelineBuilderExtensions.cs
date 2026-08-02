using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.OpenTelemetry;
using ModularPipelines.Tracing;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ModularPipelines.Extensions;

/// <summary>
/// Adds OpenTelemetry collection and export to a pipeline.
/// </summary>
public static class OpenTelemetryPipelineBuilderExtensions
{
    private static readonly string[] GitCommitEnvironmentVariables =
    [
        "GITHUB_SHA",
        "CI_COMMIT_SHA",
        "BUILD_SOURCEVERSION",
        "BITBUCKET_COMMIT",
        "CODEBUILD_RESOLVED_SOURCE_VERSION",
    ];

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
        var gitCommitSha = FindGitCommitSha();
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
                ModuleActivityTracing.PipelineSourceName,
                ModuleActivityTracing.ModuleSourceName,
                ModuleActivityTracing.CommandSourceName))
            .WithMetrics(metrics => metrics.AddMeter(ModuleActivityTracing.MeterName));

        configure?.Invoke(new ModularPipelinesOpenTelemetryBuilder(openTelemetryBuilder));
        return builder;
    }

    private static string? FindGitCommitSha()
    {
        return GitCommitEnvironmentVariables
            .Select(Environment.GetEnvironmentVariable)
            .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
    }
}
