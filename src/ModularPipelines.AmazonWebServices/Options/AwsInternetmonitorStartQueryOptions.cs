using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internetmonitor", "start-query")]
public record AwsInternetmonitorStartQueryOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--query-type")] string QueryType
) : AwsOptions
{
    [CliOption("--filter-parameters")]
    public string[]? FilterParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}