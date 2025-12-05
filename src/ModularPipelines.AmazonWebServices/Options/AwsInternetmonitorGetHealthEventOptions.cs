using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internetmonitor", "get-health-event")]
public record AwsInternetmonitorGetHealthEventOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--event-id")] string EventId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}