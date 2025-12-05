using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "delete-rum-metrics-destination")]
public record AwsRumDeleteRumMetricsDestinationOptions(
[property: CliOption("--app-monitor-name")] string AppMonitorName,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}