using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-log-events")]
public record AwsLogsPutLogEventsOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName,
[property: CommandSwitch("--log-stream-name")] string LogStreamName,
[property: CommandSwitch("--log-events")] string[] LogEvents
) : AwsOptions
{
    [CommandSwitch("--sequence-token")]
    public string? SequenceToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}