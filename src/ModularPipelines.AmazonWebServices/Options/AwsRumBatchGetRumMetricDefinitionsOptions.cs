using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "batch-get-rum-metric-definitions")]
public record AwsRumBatchGetRumMetricDefinitionsOptions(
[property: CliOption("--app-monitor-name")] string AppMonitorName,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}