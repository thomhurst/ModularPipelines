using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internetmonitor", "start-query")]
public record AwsInternetmonitorStartQueryOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--query-type")] string QueryType
) : AwsOptions
{
    [CommandSwitch("--filter-parameters")]
    public string[]? FilterParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}