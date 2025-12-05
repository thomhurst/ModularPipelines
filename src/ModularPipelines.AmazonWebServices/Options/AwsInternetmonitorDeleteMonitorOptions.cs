using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internetmonitor", "delete-monitor")]
public record AwsInternetmonitorDeleteMonitorOptions(
[property: CliOption("--monitor-name")] string MonitorName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}