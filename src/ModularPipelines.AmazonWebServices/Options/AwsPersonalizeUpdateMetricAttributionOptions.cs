using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "update-metric-attribution")]
public record AwsPersonalizeUpdateMetricAttributionOptions : AwsOptions
{
    [CliOption("--add-metrics")]
    public string[]? AddMetrics { get; set; }

    [CliOption("--remove-metrics")]
    public string[]? RemoveMetrics { get; set; }

    [CliOption("--metrics-output-config")]
    public string? MetricsOutputConfig { get; set; }

    [CliOption("--metric-attribution-arn")]
    public string? MetricAttributionArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}