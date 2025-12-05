using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internetmonitor", "stop-query")]
public record AwsInternetmonitorStopQueryOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--query-id")] string QueryId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}