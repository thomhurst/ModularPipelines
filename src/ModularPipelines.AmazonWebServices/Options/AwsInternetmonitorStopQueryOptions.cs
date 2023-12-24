using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internetmonitor", "stop-query")]
public record AwsInternetmonitorStopQueryOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--query-id")] string QueryId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}