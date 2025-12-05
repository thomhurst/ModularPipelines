using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "batch-create-rum-metric-definitions")]
public record AwsRumBatchCreateRumMetricDefinitionsOptions(
[property: CliOption("--app-monitor-name")] string AppMonitorName,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--metric-definitions")] string[] MetricDefinitions
) : AwsOptions
{
    [CliOption("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}