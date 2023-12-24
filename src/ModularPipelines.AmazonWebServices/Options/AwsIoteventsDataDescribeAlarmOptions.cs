using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents-data", "describe-alarm")]
public record AwsIoteventsDataDescribeAlarmOptions(
[property: CommandSwitch("--alarm-model-name")] string AlarmModelName
) : AwsOptions
{
    [CommandSwitch("--key-value")]
    public string? KeyValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}