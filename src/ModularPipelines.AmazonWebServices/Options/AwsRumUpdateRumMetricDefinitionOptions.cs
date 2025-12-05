using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "update-rum-metric-definition")]
public record AwsRumUpdateRumMetricDefinitionOptions(
[property: CliOption("--app-monitor-name")] string AppMonitorName,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--metric-definition")] string MetricDefinition,
[property: CliOption("--metric-definition-id")] string MetricDefinitionId
) : AwsOptions
{
    [CliOption("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}