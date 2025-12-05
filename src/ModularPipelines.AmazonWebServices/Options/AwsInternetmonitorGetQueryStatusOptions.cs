using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internetmonitor", "get-query-status")]
public record AwsInternetmonitorGetQueryStatusOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--query-id")] string QueryId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}