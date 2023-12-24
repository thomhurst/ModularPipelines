using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents", "describe-alarm-model")]
public record AwsIoteventsDescribeAlarmModelOptions(
[property: CommandSwitch("--alarm-model-name")] string AlarmModelName
) : AwsOptions
{
    [CommandSwitch("--alarm-model-version")]
    public string? AlarmModelVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}