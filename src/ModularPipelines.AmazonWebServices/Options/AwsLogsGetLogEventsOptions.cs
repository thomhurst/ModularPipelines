using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "get-log-events")]
public record AwsLogsGetLogEventsOptions(
[property: CommandSwitch("--log-stream-name")] string LogStreamName
) : AwsOptions
{
    [CommandSwitch("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CommandSwitch("--log-group-identifier")]
    public string? LogGroupIdentifier { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}