using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "batch-delete-rum-metric-definitions")]
public record AwsRumBatchDeleteRumMetricDefinitionsOptions(
[property: CliOption("--app-monitor-name")] string AppMonitorName,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--metric-definition-ids")] string[] MetricDefinitionIds
) : AwsOptions
{
    [CliOption("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}